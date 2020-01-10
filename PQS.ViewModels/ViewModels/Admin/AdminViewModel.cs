using System.Diagnostics;
using CustomDialog.Service;
using Helpers.CategoriesHelpers;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.ViewModels.Commands;
using Helpers.UserHelpers;
using Helpers.ModulesHelpers;
using Helpers.PermissionLevelsHelper;
using Helpers.ExecutionRecords;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class AdminViewModel : ViewModelBase
    {
        #region Private vars
        private readonly ITitleUpdator titleUpdator;
        private readonly IRetrievor<Categories> categoriesRetrievor = new CategoriesRetrievor();
        private readonly IRetrievor<Module> modulesRetrievor = new ModulesRetrievor();
        private readonly IDialogService dialogService = new DialogService();
        #endregion

        #region Properties
        private ViewModelBase adminViewModel;
        public ViewModelBase AdminCurrentView
        {
            get { return adminViewModel; }
            set
            {
                adminViewModel = value;
                OnPropertyChanged("AdminCurrentView");
            }
        }
        #endregion

        #region Constructors
        public AdminViewModel(ITitleUpdator titleUpdator, IUpdator<User> updateNav)
        {
            this.titleUpdator = titleUpdator;
            UserCommand.Execute(null);
        }
        #endregion

        #region Commands
        public RelayCommand UserCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("users " + CurrentUser);
                    titleUpdator.UpdateTitle("Users");
                    AdminCurrentView = new UserManagementViewModel(new UsersRetrievor(), new UserUpdator()
                        , new DeleteUser(), new UsersModulesProcessor(), new PermissionLevelsRetrievor(), new ModulesRetrievor());
                });
            }
        }

        public RelayCommand EditModuleCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("modules");
                    titleUpdator.UpdateTitle("Edit Module");
                    AdminCurrentView = new EditingModule(categoriesRetrievor, modulesRetrievor, new ModuleUpdator(), new ModuleParametersRetrievor()
                        , new ModuleParametersUpdator(), new ModuleDeletor(), new ModuleParametersDeletor());
                });
            }
        }

        public RelayCommand AddModuleCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("modules");
                    titleUpdator.UpdateTitle("Add Module");
                    AdminCurrentView = new AddingModule(categoriesRetrievor, modulesRetrievor, new AddModule(), new AddModuleParameters());
                });
            }
        }

        public RelayCommand EditingCategoryCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("editing");
                    titleUpdator.UpdateTitle("Edit Category");
                    AdminCurrentView = new EditingCategory(categoriesRetrievor, new CategoriesUpdator()); //dialogService, 
                });
            }
        }

        public RelayCommand AddingCategoryCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("editing");
                    titleUpdator.UpdateTitle("Add Category");
                    AdminCurrentView = new AddingCategory(categoriesRetrievor, new AddCategory()); //dialogService, 
                });
            }
        }

        /// <summary>
        /// Command changes the current selected category to the Users List
        /// </summary>
        public RelayCommand UsersListCategoryCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    PrintDebug("editing");
                    titleUpdator.UpdateTitle("Users List");
                    AdminCurrentView = new UsersListViewModel(modulesRetrievor, new AllUsersModulesRetrievor(),
                        new PermissionLevelsRetrievor());
                });
            }
        }

        /// <summary>
        /// Command to change the current selected category to the Usage stats 
        /// </summary>
        public RelayCommand UsageCategoryCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("Opening the usage stats");
                    titleUpdator.UpdateTitle("Usage Statistics");
                    AdminCurrentView = new UsageStatisticsViewModel(new ModuleExecutionRecordsRetrievor(), new ModulesRetrievor());
                });
            }
        }


        #endregion

        #region Methods
        private void PrintDebug(string name)
        {
            string print = "Changing to " + name + " category screen";
            Debug.Write(print);
        }
        #endregion
    }
}
