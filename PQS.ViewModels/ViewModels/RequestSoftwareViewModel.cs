using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PQS.ViewModels.Commands;
using Helpers.Interfaces;
using PQS.Models.Models;
using Helpers.UserRequests;
using Helpers.PermissionLevelsHelper;

namespace PQS.ViewModels.ViewModels
{
    public class RequestSoftwareViewModel : ViewModelBase
    {
        #region Vars
        private readonly IAdvancedRetrievor<User, UserModuleRequests> userModulesRetriever;
        private readonly User currentUser;
        #endregion

        #region Properties

        private ObservableCollection<UserModuleRequests> _userRequestModules;
        /// <summary>
        /// Collection of all modules from the database
        /// </summary>
        public ObservableCollection<UserModuleRequests> UserRequestModules
        {
            get { return _userRequestModules; }
            set
            {
                _userRequestModules = value;
                OnPropertyChanged("UserRequestModules");
            }
        }
        #endregion

        #region Constructors
        public RequestSoftwareViewModel(IAdvancedRetrievor<User, UserModuleRequests> userModulesRetriever, User currentUser)
        {
            this.userModulesRetriever = userModulesRetriever;
            this.currentUser = currentUser;

            DisplayAllModules();
            GetAllPermissionLevels();
        }
        #endregion

        #region Commands

        public RelayCommand RequestAccessCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Requesting access command pressed");
                    if (param is UserModuleRequests && currentUser != null)
                    {
                        UserModuleRequests userModuleRequest = param as UserModuleRequests;

                        if (userModuleRequest.RequestAlreadyMade) // delete
                        {
                            Debug.WriteLine("Delete request for " + userModuleRequest.Module.AdminIdentifyingName);
                            new DeleteUserModuleRequest().Delete(userModuleRequest);

                            userModuleRequest.RequestAlreadyMade = false;
                        }
                        else // request access
                        {
                            Debug.WriteLine("Requesting access for " + userModuleRequest.Module.AdminIdentifyingName);

                            userModuleRequest.RequestDate = DateTime.Now;

                            if (userModuleRequest.PermissionLevel != null)
                            {
                                new AddUserModuleRequest().Add(userModuleRequest);
                                userModuleRequest.RequestAlreadyMade = true;
                            }
                            else
                                DisplayAlert("Select Permission Level", "You must select a permission level below", CustomDialog.ImagePaths.alertImage);
                        }
                    }
                });
            }
        }

        #endregion

        #region Methods
        private void DisplayAllModules()
        {
            UserRequestModules = new ObservableCollection<UserModuleRequests>(userModulesRetriever.GetFilteredList(currentUser));
        }

        private void GetAllPermissionLevels()
        {
            if (UserRequestModules != null)
            {
                foreach (UserModuleRequests request in UserRequestModules)
                {
                    request.PermissionLevels = new ObservableCollection<PermissionLevels>(new PermissionLevelsRetrievor().GetAll());
                    request.PermissionLevel = request.PermissionLevels.FirstOrDefault(x => x.Order == request.PermissionLevels.Min(y => y.Order));
                }
            }
        }
        #endregion
    }
}
