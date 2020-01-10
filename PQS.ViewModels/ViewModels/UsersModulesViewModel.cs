using CustomDialog.Dialogs;
using CustomDialog.Service;
using Helpers.ExecutionRecords;
using Helpers.Executor;
using Helpers.Interfaces;
using Helpers.ModulesHelpers;
using Helpers.ObserverFavourites;
using PQS.Models.Models;
using PQS.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace PQS.ViewModels.ViewModels
{
    public abstract class UsersModulesViewModel : ViewModelBase
    {
        #region Vars
        protected readonly User currentUser;
        protected readonly Categories category;
        protected readonly IAdvancedRetrievor<User, UsersModules> userModules;
        protected readonly IUpdator<User> updateNav;
        #endregion

        #region Properties
        private ObservableCollection<UsersModules> modules;
        public ObservableCollection<UsersModules> Modules
        {
            get { return modules; }
            set
            {
                modules = value;
                OnPropertyChanged("Modules");
            }
        }

        private UsersModules selectedModule;
        /// <summary>
        /// The module that the user has selected from the list
        /// </summary>
        public UsersModules SelectedModule
        {
            get { return selectedModule; }
            set
            {
                Debug.WriteLine("Selected a module, executing");
                selectedModule = value;
                OnPropertyChanged("SelectedModule");

            }
        }

        #endregion

        #region Constructors

        public UsersModulesViewModel(IAdvancedRetrievor<User, UsersModules> userModules, Categories category, User currentUser, IUpdator<User> updateNav)
        {
            if (category != null && currentUser != null)
            {
                this.currentUser = currentUser;
                this.category = category;
                this.userModules = userModules;
                this.updateNav = updateNav;

                DisplayModulesCollection();
                ValidateIconPathForEachModule();
            }
        }

        public UsersModulesViewModel(IAdvancedRetrievor<User, UsersModules> userModules, User currentUser, IUpdator<User> updateNav)
        {
            if (currentUser != null)
            {
                this.currentUser = currentUser;
                this.userModules = userModules;
                this.updateNav = updateNav;

                DisplayModulesCollection();
                ValidateIconPathForEachModule();

                Subject.FavouriteModules = Modules; // tell the context menu in bottom corner
            }
        }

        #endregion

        #region Commands
        public RelayCommand AddToFavouritesCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    if ((param != null) && param is UsersModules)
                    {
                        UsersModules usersMod = param as UsersModules;
                        Debug.WriteLine("Adding module to favourites");

                        if (usersMod != null)
                        {
                            // if favoutire make it NOT a favoutire else make it a favourite
                            usersMod.IsFavourite = (usersMod.IsFavourite) ? false : true;
                            bool success = new UpdateFavourite().Update(usersMod);
                            if (success) updateNav.Update(currentUser);

                            DisplayModulesCollection();
                            ValidateIconPathForEachModule();

                            Subject.FavouriteModules = Modules; // tell the context menu in bottom corner
                        }
                    }
                });
            }
        }

        private DispatcherTimer timer = new DispatcherTimer();
        public RelayCommand ExecuteModule
        {
            get
            {
                return new RelayCommand(param =>
                {
                    // IMPORTANT : always use breakpoints when editing any executions incase of getting stuck in a never ending execution loop
                    if (SelectedModule != null)
                    {
                        /**
                         * this is to prevent multi click executions. It tried other ways, such as try finally and CanExecute on
                         * the command but they failed to work
                         * 
                         * ALTERNATIVE: create new thread?
                         */

                        timer.Tick += (sender, args) => Execute();
                        timer.Interval = new System.TimeSpan(0, 0, 1);
                        timer.Start();
                    }
                });
            }
        }

        public RelayCommand ClearAll
        {
            get
            {
                return new RelayCommand(param =>
                {
                    DisplayModulesCollection();
                });
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check to see if the IconPath provided will return an icon
        /// if not use the default error image as the logo
        /// </summary>
        /// <returns>Returns an icon path to be display for the module</returns>
        protected void ValidateIconPathForEachModule()
        {
            if (Modules != null)
                foreach (UsersModules mod in Modules)
                    mod.Module.IconPath = File.Exists(mod.Module.IconPath) ? mod.Module.IconPath : CustomDialog.ImagePaths.errorImage;
        }

        /// <summary>
        /// Checks if the module is disabled by the admins, if not the module is then
        /// executed. Otherwise, a message is displayed explaining that the module is disabled.
        /// and the date and time it was disabled.
        /// </summary>
        private void Execute() // before editing use break points to ensure no never ending loop
        {
            try
            {
                timer.Stop(); // only execute timer once.
                if (SelectedModule != null)
                {
                    if (!SelectedModule.Module.IsDisabled)
                    {
                        Debug.WriteLine("Beginning execution of " + SelectedModule.Module.DisplayName + " " + SelectedModule.Module.Category.CategoryName);
                        new ModuleExecutor(new AddModuleExecutionRecords()).Execute(SelectedModule.Module, currentUser);
                    }

                    else
                    {
                        string message = "This module has been disabled by the administrators";
                        message += (SelectedModule.Module.DisabledDate != null) ? " at " + SelectedModule.Module.DisabledDate : null;
                        DisplayAlert("Module Disabled", message, CustomDialog.ImagePaths.alertImage);
                    }
                }
            }
            finally
            {
                ClearAll.Execute(null);
            }
        }

        /// <summary>
        /// Get the modules for the category and display them to the user
        /// </summary>
        protected abstract void DisplayModulesCollection();
        #endregion
    }
}
