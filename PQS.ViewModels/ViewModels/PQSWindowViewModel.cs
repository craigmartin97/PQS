using CustomDialog.Service;
using Helpers;
using Helpers.Interfaces;
using Helpers.ObserverFavourites;
using Helpers.RegEdit;
using Helpers.UserHelpers;
using PQS.Models.Models;
using PQS.ViewModels.Commands;
using PQS.ViewModels.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using Helpers.UserRequests;
using Helpers.ModulesHelpers;
using Helpers.PermissionLevelsHelper;

namespace PQS.ViewModels.ViewModels
{
    public class PQSWindowViewModel : ViewModelBase, ITitleUpdator, IUpdator<User>
    {
        #region Private vars
        private readonly IAdvancedRetrievor<User, UsersModules> userModuleRetrievor;
        private readonly IAdvancedRetrievor<IList<UsersModules>, Categories> categoriesRetrievor;
        private readonly IAdvancedRetrievor<User, UsersModules> userModules;
        private readonly IUserProcessor iUserProcessor;

        private int requestSoftwareCounter = 0;
        private ObservableCollection<PermissionLevels> allPermissionLevels;
        private DispatcherTimer timerRequests = new DispatcherTimer();
        #endregion

        #region Properties

        private ObservableCollection<Module> allModules;
        public ObservableCollection<Module> AllModules
        {
            get { return allModules; }
            set
            {
                allModules = value;
                OnPropertyChanged("AllModules");
            }
        }

        private ObservableCollection<Categories> navigationCategories;
        /// <summary>
        /// Categories that the user can see on the side navigation bar.
        /// Only categories with modules that the user has access to are displayed.
        /// </summary>
        public ObservableCollection<Categories> NavigationCategories
        {
            get { return navigationCategories; }
            set
            {
                navigationCategories = value;
                OnPropertyChanged("NavigationCategories");
            }
        }

        private ObservableCollection<UserModuleRequests> userRequests;
        public ObservableCollection<UserModuleRequests> UserRequests
        {
            get { return userRequests; }
            set
            {
                userRequests = value;
                OnPropertyChanged("UserRequests");
            }
        }

        private UserModuleRequests _selectedRequest;
        public UserModuleRequests SelectedRequest
        {
            get { return _selectedRequest; }
            set
            {
                _selectedRequest = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        private bool adminCategoryVisibility;
        /// <summary>
        /// Visibility of the admin category visibility navigation button.
        /// Only admin users of the PQS can see the navigation button.
        /// </summary>
        public bool AdminCategoryVisibility
        {
            get { return adminCategoryVisibility; }
            set
            {
                adminCategoryVisibility = value;
                OnPropertyChanged("AdminCategoryVisibility");
            }
        }

        private bool favouritesCategoryVisibility;
        public bool FavouritesCategoryVisibility
        {
            get { return favouritesCategoryVisibility; }
            set
            {
                favouritesCategoryVisibility = value;
                OnPropertyChanged("FavouritesCategoryVisibility");
            }
        }

        private bool _requestsAlertButtonVisibility = false;
        public bool RequestsAlertButtonVisibility
        {
            get { return _requestsAlertButtonVisibility; }
            set
            {
                _requestsAlertButtonVisibility = value;
                OnPropertyChanged("RequestsAlertButtonVisibility");
            }
        }

        private bool _isCheckedToggle = false;
        public bool IsCheckedToggle
        {
            get { return _isCheckedToggle; }
            set
            {
                _isCheckedToggle = value;

                if (value == true)
                {
                    timerRequests.Stop();
                    Debug.WriteLine("Stop timer");
                }
                else
                {
                    timerRequests.Start();
                    CheckForRequests(); // now check, incase there has been new requests
                    Debug.WriteLine("Started Timer");
                } 
                // stop timer

                OnPropertyChanged("IsCheckedToggle");
            }
        }

        private int _requestAlerts;
        public int RequestAlerts
        {
            get { return _requestAlerts; }
            set
            {
                _requestAlerts = value;
                OnPropertyChanged("RequestAlerts");
            }
        }

        private string subModuleTitle;
        /// <summary>
        /// Title of the Sub Module, which module is current selected text.
        /// </summary>
        public string SubModuleTitle
        {
            get { return subModuleTitle; }
            set
            {
                subModuleTitle = value;
                OnPropertyChanged("SubModuleTitle");
            }
        }

        private DateTime currentDateTime = DateTime.Now;
        public DateTime CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
                OnPropertyChanged("CurrentDateTime");
            }
        }

        private string applicationVersion = "V" + new GetApplicationVersion().GetVersion();
        public string ApplicationVersion
        {
            get { return applicationVersion; }
            set
            {
                applicationVersion = value;
                OnPropertyChanged("ApplicationVersion");
            }
        }
        #endregion

        #region Constructors
        public PQSWindowViewModel(IAdvancedRetrievor<User, UsersModules> userModules,
            IAdvancedRetrievor<IList<UsersModules>, Categories> categoriesRetrievor, IUserProcessor iUserProcessor,
            User currentUser = null)
        {
            this.userModuleRetrievor = userModules;
            this.userModules = userModules;
            this.categoriesRetrievor = categoriesRetrievor;
            this.iUserProcessor = iUserProcessor;

            CurrentUser = currentUser != null ? currentUser : iUserProcessor.GetUser();

            AdminCategoryVisibility = false;
            FavouritesCategoryVisibility = false;

            allPermissionLevels = GetPermissionLevels();
            CheckForRequests();
            

            RegistryWriter writer = new RegistryWriter();

            // check the sub key exists
            writer.CheckRegistryKeyExists(@"Software\Wow6432Node\Dupont Teijin Films");

            writer.WritePQSKey();
            writer.WriteUserKeyAndValues(CurrentUser);

            Subject.CurrentUser = CurrentUser;

            iUserProcessor.UpdateUsersLastAccessDate(CurrentUser); // update users last access date

            Update(CurrentUser); // update nav options
            DisplayCategory();

            StartTimerToGetRequests();
            StartTimerToGetLatestTime();
        }
        #endregion

        #region Commands

        /// <summary>
        /// Changes the active ContentControl DataType to the
        /// users active modules for the category
        /// </summary>
        public RelayCommand ModulesNavigationCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if (param != null && param is Categories)
                    {
                        Categories category = param as Categories;
                        Debug.WriteLine("Nav Button Pressed " + category.CategoryName);
                        SubModuleTitle = category.CategoryName;
                        CurrentView = new DefaultUsersModulesViewModel(userModuleRetrievor, category, CurrentUser, this); //new UsersModulesViewModel(userModuleRetrievor, category, CurrentUser, this);
                    }
                });
            }
        }

        /// <summary>
        /// Changes the active screen to the Admin Area.
        /// </summary>
        public RelayCommand AdminCategoryNavigationCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    CurrentView = new AdminViewModel(this, this);
                });
            }
        }

        public RelayCommand FavouritesNavigationCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if ((param != null) && param is string)
                    {
                        SubModuleTitle = param.ToString();
                        CurrentView = new FavouritesViewModel(userModuleRetrievor, CurrentUser, this);
                    }

                });
            }
        }

        public RelayCommand RefreshNavigationOptions
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Pressed Refresh Button");
                    CurrentUser = iUserProcessor.GetUser(); // retrieve refreshed current user to check if admin changed.
                    Update(CurrentUser);
                });
            }
        }

        /// <summary>
        /// A command to change the active view to the Request software view
        /// </summary>
        public RelayCommand RequestSoftwareCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Executing request software command");
                    UpdateTitle("Request Software");
                    CurrentView = new RequestSoftwareViewModel(new UserModuleRequestsRetriever(), CurrentUser);
                });
            }
        }

        public RelayCommand AcceptSoftwareRequestCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Giving the user the new access level");
                    if (param is UserModuleRequests)
                    {

                        UserModuleRequests request = param as UserModuleRequests;

                        if (request.PermissionLevel.IsEditorPermission) // additional check required.
                        {
                            CustomDialog.Enums.DialogResults filledInRequestSheet = DisplayOptionDialog("Completed Lotus Notes Request?", "Have you or the user completed the lotus notes editor access request sheet?");
                            if (filledInRequestSheet == CustomDialog.Enums.DialogResults.Yes)
                            {
                                CompleteRequest(request);
                            }
                        }
                        else
                        {
                            CompleteRequest(request);
                        }
                    }
                });
            }
        }

        public RelayCommand RejectSoftwareRequestCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Rejecting the users request");
                    if (param is UserModuleRequests)
                    {
                        UserModuleRequests request = param as UserModuleRequests;
                        bool success = new CompleteUserRequest().RejectUsersRequest(request);

                        if (success)
                        {
                            UserRequests.Remove(request);
                            SetUserRequestsVisibility();
                            ClosePopupIfNoItems();
                            GetTotalUserRequests();
                        }
                    }
                });
            }
        }
        #endregion

        #region Implementations
        public void UpdateTitle(string title)
        {
            SubModuleTitle = title;
        }

        public bool Update(User item)
        {
            IList<UsersModules> usersMods = userModules.GetFilteredList(item);
            NavigationCategories = new ObservableCollection<Categories>(categoriesRetrievor.GetFilteredList(usersMods));

            AdminCategoryVisibility = item.IsAdmin;
            FavouritesCategoryVisibility = usersMods.Any(x => x.IsFavourite);

            if (!FavouritesCategoryVisibility)
                ModulesNavigationCommand.Execute(NavigationCategories.FirstOrDefault());

            return true;
        }

        /// <summary>
        /// Checks if the user has any favourite categorys.
        /// If the user has favourites they are shown there favourites list otherwise
        /// they are shown the first categories options.
        /// If the user has no category to choose from they are shown blank screen
        /// </summary>
        private void DisplayCategory()
        {
            // was last in favs go back to first category
            if (!FavouritesCategoryVisibility)
                ModulesNavigationCommand.Execute(NavigationCategories.FirstOrDefault());
            else FavouritesNavigationCommand.Execute("Favourites");
        }
        #endregion

        #region Timer to update current time
        /// <summary>
        /// Starts a dispatch timer to get the current date and time each second
        /// </summary>
        private void StartTimerToGetLatestTime()
        {
            var dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += DispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 1);
            dispatchTimer.Start();
        }

        private void StartTimerToGetRequests()
        {
            timerRequests.Tick += RequestTimer;
            timerRequests.Interval = new TimeSpan(0, 0, 1); // when going live change this to 30, when testing changed to 1 for quickness
            timerRequests.Start();
        }

        /// <summary>
        /// Does the work to get the current date and time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatchTimer_Tick(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        /// <summary>
        /// Does the work to get the current date and time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestTimer(object sender, EventArgs e)
        {
            CheckForRequests();
        }

        private ObservableCollection<UserModuleRequests> GetUserRequests()
        {
            ObservableCollection<UserModuleRequests> allRequests = new ObservableCollection<UserModuleRequests>(new AllUsersRequests().GetAll());

            foreach (UserModuleRequests request in allRequests) // set the list of permission levels avaliable for the request
            {
                request.PermissionLevels = allPermissionLevels;
                // reselect for drop down on display
                request.PermissionLevel = request.PermissionLevels.FirstOrDefault(x => x.PK_PermissionLevel_Id == request.PermissionLevel.PK_PermissionLevel_Id);
            }


            return allRequests;
        }

        private ObservableCollection<PermissionLevels> GetPermissionLevels()
        {
            return new ObservableCollection<PermissionLevels>(new PermissionLevelsRetrievor().GetAll());
        }

        private void CheckForRequests()
        {
            UserRequests = GetUserRequests(); // do on load, get all user requsts
            SetUserRequestsVisibility(); // change the visibility of the notification bell
            GetTotalUserRequests(); // get count of requests
        }

        private void GetTotalUserRequests()
        {
            if (UserRequests != null)
                RequestAlerts = UserRequests.Count;
        }

        private void SetUserRequestsVisibility()
        {
            if (UserRequests != null)
            {
                RequestsAlertButtonVisibility = UserRequests.Count > 0;
            }
                
        }

        private void ClosePopupIfNoItems()
        {
            if (UserRequests != null)
                IsCheckedToggle = UserRequests.Count > 0;
        }

        private void CompleteRequest(UserModuleRequests request)
        {
            bool success = new CompleteUserRequest().CompleteRequest(request);

            if (success)
            {
                UserRequests = GetUserRequests();
                SetUserRequestsVisibility();
                ClosePopupIfNoItems();
                GetTotalUserRequests();
            }
            else DisplayAlert("Failed to give access", "The user has suspected deleted the request in the past 30 seconds. If these persists, there is another issue", CustomDialog.ImagePaths.alertImage);
        }
        #endregion
    }
}