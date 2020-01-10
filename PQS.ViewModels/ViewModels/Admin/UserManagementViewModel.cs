using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PQS.Models.Models;
using Helpers.Interfaces;
using PQS.ViewModels.Commands;
using System.Diagnostics;
using Helpers.ModulesHelpers;
using Helpers.Login;
using CustomDialog.Dialogs;
using CustomDialog.Enums;

namespace PQS.ViewModels.ViewModels.Admin
{
    /// <summary>
    /// User management view model is responsible for manageing interactions
    /// for editing users details
    /// </summary>
    public class UserManagementViewModel : EditUsersPermissions
    {
        #region Private vars
        private readonly IUpdator<User> updator;
        private readonly IDeletor<User> deletor;
        private readonly IRetrievor<User> retrievor;
        
        #endregion

        #region Public properties
        private ObservableCollection<User> allUsers;
        /// <summary>
        /// Collection of all users
        /// </summary>
        public ObservableCollection<User> AllUsers
        {
            get { return allUsers; }
            set
            {
                allUsers = value;
                OnPropertyChanged("AllUsers");
            }
        }

        private User selectedUser;
        /// <summary>
        /// The selected user, the user selected
        /// </summary>
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;

                AllUsersModules = filter.GetAllModulesWithUsersPopulated(selectedUser, moduleRetrievor, usersModulesRetrievor);
                filter.SetAllPermissionLevelOptions(AllUsersModules);

                OnPropertyChanged("SelectedUser");
            }
        }
        #endregion

        #region Constructors
        public UserManagementViewModel(IRetrievor<User> iRetrievor, IUpdator<User> updator, IDeletor<User> deletor
            , IAdvancedRetrievor<User, UsersModules> iUsersMods, IRetrievor<PermissionLevels> permissionLevelRetrievor, IRetrievor<Module> iModuleRetrievor)
            : base(permissionLevelRetrievor, iUsersMods, iModuleRetrievor)
        {
            this.updator = updator;
            this.deletor = deletor;
            this.retrievor = iRetrievor;

            AllUsers = GetAllUsers();
        }
        #endregion

        #region Commands
        public RelayCommand DeleteUserCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Deleing user button pressed");
                    CurrentUser currentLoggedInUser = new CurrentUser();

                    if (currentLoggedInUser.GetLoggedInUser().Equals(SelectedUser.WindowsId)) // can't delete self
                        DisplayAlert("Unable to delete user", "You can't delete your own account", CustomDialog.ImagePaths.errorImage);
                    else
                    {
                        DialogResults result = DisplayOptionDialog("Deleting " + SelectedUser.Username, "You are about to delete this user, are you sure?");
                        if (result == DialogResults.Yes)
                        {
                            bool deleted = deletor.Delete(SelectedUser);
                            if (deleted)
                            {
                                DisplayAlert("Sucessfully deleted", "The users was successfully deleted", CustomDialog.ImagePaths.successImage);
                                /**
                                 * Remove doesnt work as you need to refresh and create new obs coll in wpf mvvm
                                 */
                                AllUsers = GetAllUsers();
                            }
                            else DisplayAlert("Failed to delete", "The user was was not deleted, please try again", CustomDialog.ImagePaths.errorImage);
                        }
                    }
                });
            }
        }

        public RelayCommand UpdateUserCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Deleing user button pressed");
                    DialogResults result = DisplayOptionDialog("Confirm edit to " + SelectedUser.Username, "You are about to update the data for this user, are you sure?");
                    if (result == DialogResults.Yes)
                    {
                        bool userSuccess = updator.Update(SelectedUser);
                        bool moduleSuccess = UpdateUsersPermissions();

                        if (userSuccess && moduleSuccess)
                        {
                            DisplayAlert("Sucessfully updated", "The users details have successfully been updated", CustomDialog.ImagePaths.successImage);
                        }
                        else
                        {
                            DisplayAlert("Failed to update", "The users details have failed to be updated", CustomDialog.ImagePaths.errorImage);
                        }
                    }

                });
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Caller method to get all the users from the data store
        /// </summary>
        /// <returns>Returns new observable collection</returns>
        private ObservableCollection<User> GetAllUsers()
        {
            return new ObservableCollection<User>(retrievor.GetAll());
        }
        #endregion
    }
}
