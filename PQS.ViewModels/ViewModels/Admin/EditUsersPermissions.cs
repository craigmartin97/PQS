using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PQS.Models.Models;
using Helpers.Interfaces;
using PQS.ViewModels.Commands;
using Helpers.ModulesHelpers;
using Helpers.PermissionLevelsHelper;
using Helpers.UserRequests;

namespace PQS.ViewModels.ViewModels.Admin
{
    public abstract class EditUsersPermissions : ViewModelBase
    {
        #region Vars
        //private readonly IUpdator<ObservableCollection<UsersModules>> usersModulesUpdator;
        protected readonly FilterUsersModules filter = new FilterUsersModules();
        protected readonly IAdvancedRetrievor<User, UsersModules> usersModulesRetrievor;
        protected readonly IRetrievor<Module> moduleRetrievor;
        #endregion

        #region Properties
        private ObservableCollection<UsersModules> allUsersModules = new ObservableCollection<UsersModules>();
        /// <summary>
        /// Collection of all the users modules
        /// </summary>
        public ObservableCollection<UsersModules> AllUsersModules
        {
            get { return allUsersModules; }
            set
            {
                allUsersModules = value;
                OnPropertyChanged("AllUsersModules");
            }
        }

        private ObservableCollection<PermissionLevels> allPermissionLevels;
        public ObservableCollection<PermissionLevels> AllPermissionLevels
        {
            get { return allPermissionLevels; }
            set
            {
                allPermissionLevels = value;
                OnPropertyChanged("AllPermissionLevels");
            }
        }

        private UsersModules selectedUsersModule;
        public UsersModules SelectedUsersModule
        {
            get { return selectedUsersModule; }
            set
            {
                selectedUsersModule = value;
                OnPropertyChanged("SelectedUsersModule");
            }
        }
        #endregion

        #region
        public EditUsersPermissions(IRetrievor<PermissionLevels> permissionLevelRetrievor, IAdvancedRetrievor<User, UsersModules> iUsersMods
            , IRetrievor<Module> iModuleRetrievor)
        {
            this.usersModulesRetrievor = iUsersMods;
            this.moduleRetrievor = iModuleRetrievor;
            //this.usersModulesUpdator = iUsersModsUpdator;
            AllPermissionLevels = new ObservableCollection<PermissionLevels>(permissionLevelRetrievor.GetAll());
        }
        #endregion

        #region
        /// <summary>
        /// Command changes the current rows permission level
        /// </summary>
        public RelayCommand UpdatePermissionCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if (SelectedUsersModule != null)
                    {
                        /**
                         * Wrong check, needed to be param is GrantAccessPermissionLevels not PermissionLevels
                         * second check below will do functionality for PermissionLevels
                         * 
                         * i dislike this type checking! below!!!
                         */
                        if (param is GrantAccessPermissionLevels) // respond to user request
                        {
                            GrantAccessPermissionLevels permLevel = param as GrantAccessPermissionLevels;
                            CompleteUserRequest completeRequest = new CompleteUserRequest();

                            
                            if (permLevel.IsAccessRequest)
                                completeRequest.CompleteRequest(SelectedUsersModule);
                            else
                                completeRequest.RejectUsersRequest(SelectedUsersModule);

                            AllUsersModules = filter.GetAllModulesWithUsersPopulated(SelectedUsersModule.User, moduleRetrievor, usersModulesRetrievor);
                            filter.SetAllPermissionLevelOptions(AllUsersModules);
                        }
                        else if (param is PermissionLevels) // give access
                        {
                            SelectedUsersModule.PermissionLevel = param as PermissionLevels;
                        }
                        else if (param == null) // remove permission
                            SelectedUsersModule.PermissionLevel = null;
                    }

                });
            }
        }
        #endregion

        #region Methods
        protected bool UpdateUsersPermissions()
        {
            if ((_currentUser != null) && _currentUser.IsAdmin) // double check editor is admin, when they must be really
                return new UsersModulesUpdator().UpdateUsersModules(AllUsersModules, _currentUser); // usersModulesUpdator.Update(AllUsersModules);

            return false;
        }
        #endregion
    }
}
