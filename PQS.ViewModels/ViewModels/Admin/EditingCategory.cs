using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomDialog.Dialogs;
using Helpers.Interfaces;
using CustomDialog.Service;
using PQS.Models.Models;
using CustomDialog.Enums;
using System.ComponentModel;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class EditingCategory : CategoriesManagementViewModel, IDataErrorInfo
    {
        #region Vars
        private readonly IUpdator<Categories> categoriesUpdator;
        #endregion

        #region Properties
        private Categories selectedCategory;
        /// <summary>
        /// The current selected category by the user
        /// </summary>
        public Categories SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (value != null)
                {
                    selectedCategory = value;
                    tempCategory = (Categories)selectedCategory.Clone();

                    CategoryName = value.CategoryName;
                    Order = value.Order;

                    OnPropertyChanged("SelectedCategory");
                }
            }
        }
        #endregion

        #region Constructors
        public EditingCategory(IRetrievor<Categories> retrievor, IUpdator<Categories> categoriesUpdator) //IDialogService dialogService, 
            : base(retrievor) //dialogService, 
        {
            this.categoriesUpdator = categoriesUpdator;
        }
        #endregion

        #region Methods
        protected override void CategoriesOperation()
        {
            tempCategory = CreateNewCategory();

            if (SelectedCategory != null && tempCategory != null)
            {
                if (ValidateUserEntries())
                {
                    DialogResults result = DisplayOptionDialog("Confirm edit to " + SelectedCategory.CategoryName,
                        "You are about to update the data for this category, are you sure?");

                    if (result == DialogResults.Yes)
                    {
                        bool success = categoriesUpdator.Update(tempCategory);
                        SetAllCategories(); // update categories in list.
                        ClearCategoryData();

                        if (success) DisplayAlert("Successfully edited category", "The category " + SelectedCategory.CategoryName + " has successfully been updated", CustomDialog.ImagePaths.successImage);
                        else DisplayAlert("Failed to edit", "The category " + SelectedCategory.CategoryName + " has failed to update", CustomDialog.ImagePaths.errorImage);
                    }

                }
                else DisplayAlert("Enter data", "You must add data to all fields", CustomDialog.ImagePaths.errorImage);
            }
            else DisplayAlert("Select Category", "You must select category", CustomDialog.ImagePaths.errorImage);
        }

        /// <summary>
        /// Clear all data related to categories
        /// </summary>
        private void ClearCategoryData()
        {
            SelectedCategory = null;
            tempCategory = null;
            CategoryName = null;
            Order = null;
        }
        #endregion

        /// <summary>
        /// Create a new category object with the name and order the user typed in
        /// and with the SelectedCategories primary key
        /// </summary>
        /// <returns></returns>
        protected override Categories CreateNewCategory()
        {
            return new Categories()
            {
                PK_Category_id = SelectedCategory.PK_Category_id,
                CategoryName = CategoryName,
                Order = Order
            };
        }

        public string Error
        {
            get { return "This field cannot be null or empty"; }
        }

        public string this[string columnName]
        {
            get
            {
                if (SelectedCategory != null) // only validate once a selection is made
                {
                    if (columnName.Equals("CategoryName"))
                    {
                        if (string.IsNullOrWhiteSpace(CategoryName))
                            return Error;
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
    }
}
