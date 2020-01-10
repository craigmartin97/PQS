using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using System.Collections.ObjectModel;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using CustomDialog.Enums;
using Helpers.ModulesHelpers;
using System.IO;
using System.ComponentModel;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class EditingModule : ModuleManagementViewModel, IDataErrorInfo
    {
        #region Vars
        private readonly IUpdator<Module> moduleUpdator;
        private readonly IAdvancedRetrievor<Module, ModuleParameters> parametersRetrievor;
        private readonly IUpdator<ObservableCollection<ModuleParameters>> moduleParameterUpdator;
        private readonly IDeletor<Module> deleteModule;
        private readonly IDeletor<Module> moduleParamsDeletor;
        #endregion

        #region Properties
        private Module selectedModule;
        public Module SelectedModule
        {
            get { return selectedModule; }
            set
            {
                if (value != null)
                {
                    selectedModule = value;
                    ModuleParameters = new ObservableCollection<ModuleParameters>(parametersRetrievor.GetFilteredList(selectedModule));
                    PopulateAllProperties();
                    OnPropertyChanged("SelectedModule");
                }
            }
        }
        #endregion

        #region Constructors
        public EditingModule(IRetrievor<Categories> iRetrivor, IRetrievor<Module> iModuleRetrievor,
            IUpdator<Module> updator, IAdvancedRetrievor<Module, ModuleParameters> iModuleParams, IUpdator<ObservableCollection<ModuleParameters>> moduleParams
            , IDeletor<Module> delete, IDeletor<Module> moduleParamsDeletor)
            : base(iRetrivor, iModuleRetrievor)
        {
            moduleUpdator = updator;
            parametersRetrievor = iModuleParams;
            moduleParameterUpdator = moduleParams;
            deleteModule = delete;
            this.moduleParamsDeletor = moduleParamsDeletor;

            UpdateButtonContent = "Update Module";
            DeleteButtonContent = "Delete Module";
        }
        #endregion

        #region Methods
        protected override void ModuleOperation()
        {
            tempModule = CreateNewModule();
            SetParamsModule(tempModule);

            if (tempModule != null)
            {
                bool updateSuccess = moduleUpdator.Update(tempModule);
                bool modParamsDelSuccess = moduleParamsDeletor.Delete(tempModule); // delete all params for module, bit of a quick cheat, v v v lazy
                bool moduleSuccess = moduleParameterUpdator.Update(ModuleParameters);

                if (updateSuccess && moduleSuccess && modParamsDelSuccess)
                {
                    DisplayAlert("Successfully updated",
                        "The module " + tempModule.DisplayName + " has been successfully updated",
                        CustomDialog.ImagePaths.successImage);
                    ClearAllProperties(); // clear all from display
                }
                else
                {
                    DisplayAlert("Failed to update", "The module " + tempModule.DisplayName + " has failed to update",
                        CustomDialog.ImagePaths.errorImage);
                }
            }
        }

        protected override void DeleteOperation()
        {
            if (SelectedModule != null)
            {
                DialogResults result = DisplayOptionDialog("Delete Module", "Are you sure you want to delete " + SelectedModule.AdminIdentifyingName);
                if (result == DialogResults.Yes)
                {
                    bool sucess = deleteModule.Delete(SelectedModule);
                    if (sucess)
                    {
                        DisplayAlert("Module Deleted", "The module " + SelectedModule.DisplayName + " has been deleted from the PQS", CustomDialog.ImagePaths.successImage);
                        ClearAllProperties(); // clear all from display
                    }
                }
            }
        }

        protected override Module CreateNewModule()
        {
            return new Module()
            {
                PK_Module_Id = SelectedModule.PK_Module_Id,
                DisplayName = DisplayName,
                AdminIdentifyingName = AdminIdentifyingName,
                Description = Description,
                ExecutionDirectory = ExecutionDirectory,
                ExecutionPath = ExecutionPath,
                Category = SelectedCategory,
                IconPath = LogoPath,
                IsDisabled = IsDisabled,
                IsActive = IsActive,
                Order = Order
            };
        }

        /// <summary>
        /// Clear all the properties values
        /// </summary>
        protected override void ClearAllProperties()
        {
            base.ClearAllProperties();
            SelectedModule = null;
            IsActive = false;

            AllModules = GetAllModules(); //refresh collection
        }

        private void PopulateAllProperties()
        {
            if (selectedModule != null)
            {
                DisplayName = SelectedModule.DisplayName;
                AdminIdentifyingName = SelectedModule.AdminIdentifyingName;
                Description = SelectedModule.Description;
                ExecutionPath = SelectedModule.ExecutionPath;
                ExecutionDirectory = SelectedModule.ExecutionDirectory;
                LogoPath = SelectedModule.IconPath;
                IsDisabled = SelectedModule.IsDisabled;
                IsActive = SelectedModule.IsActive;
                SelectedCategory = SelectedModule.Category;
                Order = SelectedModule.Order;
            }
        }
        #endregion

        #region IData
        public string Error
        {
            get
            {
                return "This field cannot be null or empty";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (selectedModule != null)
                {
                    if (columnName.Equals("DisplayName"))
                    {
                        if (string.IsNullOrWhiteSpace(DisplayName))
                            return Error;
                    }
                    else if (columnName.Equals("AdminIdentifyingName"))
                    {
                        if (string.IsNullOrWhiteSpace(AdminIdentifyingName))
                            return Error;
                        else if (new ModuleExists().CheckIfModuleWithAdminIdNameAlreadyExistsAndNotSelected(SelectedModule.PK_Module_Id, AdminIdentifyingName))
                            return "A module with the same name already exists";
                    }
                    else if (columnName.Equals("ExecutionPath"))
                    {
                        if (string.IsNullOrWhiteSpace(ExecutionPath))
                            return Error;
                        else if (!CheckIfFileExists(ExecutionPath))
                            return "The file path does not exist";
                    }
                    else if (columnName.Equals("ExecutionDirectory"))
                    {
                        if (string.IsNullOrWhiteSpace(ExecutionDirectory))
                            return Error;
                        else if (!CheckIfDirectoryExists(ExecutionDirectory))
                            return "The file directory does not exist";
                    }
                    else if (columnName.Equals("LogoPath"))
                    {
                        if (string.IsNullOrWhiteSpace(LogoPath))
                            return Error;
                    }
                    else if (columnName.Equals("SelectedCategory"))
                    {
                        if (SelectedCategory == null)
                            return "You must select a category";
                    }
                    else if (columnName.Equals("Order"))
                    {
                        if (Order == null)
                            return Error;
                    }
                }
                return null;
            }
        }
        #endregion
    }
}