using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using System.Collections.ObjectModel;
using Helpers.Interfaces;
using PQS.ViewModels.Commands;
using System.Diagnostics;
using CustomDialog.Enums;
using Helpers.ModulesHelpers;

namespace PQS.ViewModels.ViewModels.Admin
{
    /// <summary>
    /// The ViewModel for the UsersList screen
    /// </summary>
    public class UsersListViewModel : EditUsersPermissions
    {
        #region Vars
        private readonly IRetrievor<Module> moduleRetrievor;
        private readonly IAdvancedRetrievor<Module, UsersModules> usersModulesRetrievor;
        #endregion

        #region Propeties
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

        private Module selectedModule;
        public Module SelectedModule
        {
            get { return selectedModule; }
            set
            {
                selectedModule = value;
                AllUsersModules = new ObservableCollection<UsersModules>(usersModulesRetrievor.GetFilteredList(SelectedModule));
                OnPropertyChanged("SelectedModule");
            }
        }
        #endregion

        #region Constructors
        public UsersListViewModel(IRetrievor<Module> moduleRetrievor, IAdvancedRetrievor<Module,
            UsersModules> usersModulesRetrievor, IRetrievor<PermissionLevels> parametersRetrievor)
            : base(parametersRetrievor, new UsersModulesProcessor(), moduleRetrievor) // TODO: temp added null here
        {
            this.moduleRetrievor = moduleRetrievor;
            this.usersModulesRetrievor = usersModulesRetrievor;
            AllModules = new ObservableCollection<Module>(moduleRetrievor.GetAll());
        }
        #endregion

        #region Commands
        public RelayCommand UpdatePermissionsCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Deleing user button pressed");
                    DialogResults result = DisplayOptionDialog("Confirm change of permissions", "You are about to update all the above users permissions are you sure?");
                    if (result == DialogResults.Yes)
                    {
                        bool moduleSuccess = UpdateUsersPermissions();

                        if (moduleSuccess)
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
    }
}
