using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using System.Collections.ObjectModel;
using PQS.ViewModels.Commands;
using System.Diagnostics;
using Helpers.Interfaces;
using Microsoft.Win32;
using System.IO;
using CustomDialog.Enums;

namespace PQS.ViewModels.ViewModels.Admin
{
    public abstract class ModuleManagementViewModel : ViewModelBase
    {
        #region Vars
        private readonly IRetrievor<Categories> categoriesRetrievor;
        private readonly IRetrievor<Module> moduleRetrievor;
        protected Module tempModule;
        #endregion

        #region Properties
        private ObservableCollection<Categories> allCategories;
        /// <summary>
        /// A collection of all avalialble categories that the user
        /// can choose from. 
        /// </summary>
        public ObservableCollection<Categories> AllCategories
        {
            get { return allCategories; }
            set
            {
                allCategories = value;
                OnPropertyChanged("AllCategories");
            }
        }

        private ObservableCollection<Module> allModules;
        /// <summary>
        /// Collection of all the avalialbe modules from the data source
        /// </summary>
        public ObservableCollection<Module> AllModules
        {
            get { return allModules; }
            set
            {
                allModules = value;
                OnPropertyChanged("AllModules");
            }
        }

        private ObservableCollection<ModuleParameters> moduleParameters = new ObservableCollection<ModuleParameters>();
        public ObservableCollection<ModuleParameters> ModuleParameters
        {
            get { return moduleParameters; }
            set
            {
                moduleParameters = value;
                OnPropertyChanged("ModuleParameters");
            }
        }

        private string deleteButtonContent;
        public string DeleteButtonContent
        {
            get { return deleteButtonContent; }
            set
            {
                deleteButtonContent = value;
                OnPropertyChanged("DeleteButtonContent");
            }
        }

        private string updateButtonContent;
        public string UpdateButtonContent
        {
            get { return updateButtonContent; }
            set
            {
                updateButtonContent = value;
                OnPropertyChanged("UpdateButtonContent");
            }
        }

        private string displayName;
        /// <summary>
        /// The display name of the application, this is what the user will see
        /// when executing applications from there respected groups
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        private string adminIdentifyingName;
        /// <summary>
        /// The admin id name is the name that the admins can uniquely distinguish the 
        /// applications from each other. I.e. Standard Conditions 51, Standard Conditions 53 etc
        /// </summary>
        public string AdminIdentifyingName
        {
            get { return adminIdentifyingName; }
            set
            {
                adminIdentifyingName = value;
                OnPropertyChanged("AdminIdentifyingName");
            }
        }

        private string description;
        /// <summary>
        /// A brief explination of the application
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        private string executionPath;
        /// <summary>
        /// The execution path of the application (exe)
        /// </summary>
        public string ExecutionPath
        {
            get { return executionPath; }
            set
            {
                executionPath = value;
                OnPropertyChanged("ExecutionPath");
            }
        }

        private string executionDirectory;
        /// <summary>
        /// The execution directory of the application (exe)
        /// </summary>
        public string ExecutionDirectory
        {
            get { return executionDirectory; }
            set
            {
                executionDirectory = value;
                OnPropertyChanged("ExecutionDirectory");
            }
        }

        private string logoPath;
        /// <summary>
        /// The logo/icon path for the application
        /// </summary>
        public string LogoPath
        {
            get { return logoPath; }
            set
            {
                logoPath = value;
                OnPropertyChanged("LogoPath");
            }
        }

        private Categories selectedCategory;
        /// <summary>
        /// The category that the user has selected from the combo box
        /// </summary>
        public Categories SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        private int? order;
        /// <summary>
        /// The order of the module, that will be displayed in the list
        /// </summary>
        public int? Order
        {
            get { return order; }
            set
            {
                order = value;
                OnPropertyChanged("Order");
            }
        }

        private bool isDisabled;
        /// <summary>
        /// Bool value to determine if the module has been disabled by the admins
        /// this will prevent the application from being executed by the users
        /// </summary>
        public bool IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                OnPropertyChanged("IsDisabled");
            }
        }

        private bool isActive;
        /// <summary>
        /// IsActive is a soft delete flag, keeps the module in the system but 
        /// hides it for all users
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged("IsActive");
            }
        }
        #endregion

        #region Constructors
        public ModuleManagementViewModel(IRetrievor<Categories> iRetrivor, IRetrievor<Module> iModuleRetrievor)
        {
            this.categoriesRetrievor = iRetrivor;
            this.moduleRetrievor = iModuleRetrievor;


            AllCategories = new ObservableCollection<Categories>(categoriesRetrievor.GetAll());
            AllModules = GetAllModules();
        }
        #endregion

        #region Commands
        public RelayCommand ModuleOperationCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("User is confirming a request to the modules");
                    ModuleOperation();
                });
            }
        }

        public RelayCommand DeleteOperationCommand
        {
            get
            {
                return new RelayCommand(param =>
                {
                    Debug.WriteLine("User is confirming request to the modules");
                    DeleteOperation();
                });
            }
        }

        public RelayCommand OpenFileDialogCommandForExecutionPath
        {
            get
            {
                return new RelayCommand(param =>
                {
                    try
                    {
                        OpenFileDialog dialog = new OpenFileDialog();
                        dialog.Filter = "Execution Files (*.exe)|*.exe|All files (*.*)|*.*";
                        if (dialog.ShowDialog() == true)
                        {
                            ExecutionPath = dialog.FileName;
                            if (ExecutionDirectory == null || ExecutionDirectory.Equals(""))
                            {
                                ExecutionDirectory = Path.GetDirectoryName(dialog.FileName);
                            }
                            else
                            {
                                string executionDir = Path.GetDirectoryName(dialog.FileName);
                                if (DisplayOptionDialog("Change Execution Directory?", "Do you want to change the execution directory to\n " + executionDir).Equals(DialogResults.Yes))
                                {
                                    ExecutionDirectory = executionDir;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public RelayCommand OpenFileDialogCommandForExecutionDirectory
        {
            get
            {
                return new RelayCommand(param =>
                {
                    try
                    {
                        System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            ExecutionDirectory = dialog.SelectedPath;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public RelayCommand OpenFileDialogCommandForLogoPath
        {
            get
            {
                return new RelayCommand(param =>
                {
                    try
                    {
                        OpenFileDialog dialog = new OpenFileDialog();
                        dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.ico) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.ico|All files (*.*)|*.*";
                        if (dialog.ShowDialog() == true)
                        {
                            LogoPath = dialog.FileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }
        #endregion

        #region Methods
        protected abstract void ModuleOperation();
        protected abstract void DeleteOperation();
        protected abstract Module CreateNewModule();

        protected ObservableCollection<Module> GetAllModules()
        {
            return new ObservableCollection<Module>(moduleRetrievor.GetAll());
        }

        protected virtual void ClearAllProperties()
        {
            DisplayName = null;
            AdminIdentifyingName = null;
            Description = null;
            ExecutionPath = null;
            ExecutionDirectory = null;
            LogoPath = null;
            SelectedCategory = null;
            Order = null;
            IsDisabled = false;
            ModuleParameters.Clear();
        }

        protected void SetParamsModule(Module module)
        {
            foreach (ModuleParameters moduleParam in ModuleParameters)
                moduleParam.Module = module;
        }

        protected bool CheckIfFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        protected bool CheckIfDirectoryExists(string filePath)
        {
            return Directory.Exists(filePath);
        }
        #endregion

    }
}
