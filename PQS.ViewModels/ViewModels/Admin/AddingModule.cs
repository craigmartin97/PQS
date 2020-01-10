using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using PQS.ViewModels.Properties;
using PQS.ViewModels.Commands;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using Helpers.ModulesHelpers;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class AddingModule : ModuleManagementViewModel, IDataErrorInfo
    {
        #region Vars
        private readonly IAdd<Module> addModule;
        private readonly IAdd<IList<ModuleParameters>> addParams;
        #endregion

        #region Constructors
        public AddingModule(IRetrievor<Categories> iRetrivor, IRetrievor<Module> iModuleRetrievor, IAdd<Module> adder, IAdd<IList<ModuleParameters>> addParams)
            : base(iRetrivor, iModuleRetrievor)
        {
            addModule = adder;
            this.addParams = addParams;

            IsActive = true; // make the module active by default

            UpdateButtonContent = "Add Module";
            DeleteButtonContent = "Clear All";
        }
        #endregion

        #region Override methods
        protected override void ModuleOperation()
        {
            tempModule = CreateNewModule();
            
            bool success = addModule.Add(tempModule);

            SetParamsModule(tempModule);
            bool successParams = addParams.Add(ModuleParameters);

            if (success && successParams)
            {
                DisplayAlert("Successfully added", "The module " + tempModule.DisplayName + " has successfully been submitted",
                    CustomDialog.ImagePaths.successImage);
            }
            else
                DisplayAlert("Failed to submit", "The data failed to submit", CustomDialog.ImagePaths.errorImage);

            ClearAllProperties();
        }

        protected override void DeleteOperation()
        {
            ClearAllProperties();
        }

        protected override Module CreateNewModule()
        {
            return new Module()
            {
                DisplayName = DisplayName,
                AdminIdentifyingName = AdminIdentifyingName,
                Description = Description,
                ExecutionPath = ExecutionPath,
                ExecutionDirectory = ExecutionDirectory,
                IconPath = LogoPath,
                IsActive = IsActive,
                IsDisabled = IsDisabled,
                Order = Order,
                Category = SelectedCategory
            };
        }
        #endregion

        #region Internal methods
        protected override void ClearAllProperties()
        {
            base.ClearAllProperties();
            IsActive = true;
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
                if (columnName.Equals("DisplayName"))
                {
                    if (string.IsNullOrWhiteSpace(DisplayName))
                        return Error;
                    else if ((SelectedCategory != null) && new ModuleExists().CheckIfModuleAlreadyExistsInCategory(SelectedCategory.PK_Category_id, DisplayName))
                    {
                        return "A module with the same display name already exists in category " + SelectedCategory.CategoryName;
                    }
                }
                else if (columnName.Equals("AdminIdentifyingName"))
                {
                    if (string.IsNullOrWhiteSpace(AdminIdentifyingName))
                        return Error;
                    else if (new ModuleExists().CheckIfModuleWithAdminIdNameAlreadyExists(AdminIdentifyingName))
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
                    else if (!CheckIfFileExists(LogoPath))
                        return "The file path does not exist";
                }
                else if (columnName.Equals("SelectedCategory"))
                {
                    if (SelectedCategory == null)
                        return "You must select a category";
                    OnPropertyChanged("DisplayName"); // category selected, now check that the display name doesnt already exist in this category
                }
                else if (columnName.Equals("Order"))
                {
                    if (Order == null)
                        return Error;
                }


                return null;
            }
        }
        #endregion
    }
}
