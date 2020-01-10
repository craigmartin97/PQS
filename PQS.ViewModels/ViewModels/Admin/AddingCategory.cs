using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Helpers.Interfaces;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using PQS.Models.Models;
using Helpers.CategoriesHelpers;

namespace PQS.ViewModels.ViewModels.Admin
{
    public class AddingCategory : CategoriesManagementViewModel, IDataErrorInfo
    {
        #region Vars
        private readonly IAdd<Categories> categoryAdder;
        #endregion

        #region Constructors
        public AddingCategory(IRetrievor<Categories> retrievor, IAdd<Categories> adder) //IDialogService dialogService, 
            : base(retrievor) //dialogService, 
        {
            categoryAdder = adder;
        }
        #endregion

        #region Methods
        protected override void CategoriesOperation()
        {
            tempCategory = CreateNewCategory();

            if (ValidateUserEntries()) // new cateogty
            {
                bool checkIfCatExists = AllCategories.Any(x => x.CategoryName.Equals(tempCategory.CategoryName));
                if (!checkIfCatExists)
                {
                    bool success = categoryAdder.Add(new Categories(tempCategory.CategoryName, tempCategory.Order));
                    if (success)
                    {
                        DisplayAlert("Successfully Submitted",
                        "The category " + tempCategory.CategoryName + " has been saved successfully", CustomDialog.ImagePaths.successImage);
                    }
                    else
                    {
                        DisplayAlert("Failed to add category",
                        "The category " + tempCategory.CategoryName + " has failed to submit", CustomDialog.ImagePaths.errorImage);
                    }
                }
            }
        }

        protected override Categories CreateNewCategory()
        {
            return new Categories()
            {
                CategoryName = CategoryName,
                Order = Order
            };
        }
        #endregion

        public string Error
        {
            get { return "This field cannot be null or empty"; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("CategoryName"))
                {
                    if (string.IsNullOrWhiteSpace(CategoryName))
                        return Error;
                    else if (new CategoryExists().CheckIfCategoryWithSameNameExists(CategoryName)) // category exists
                    {
                        return "The category name already exists";
                    }
                }
                else if (columnName.Equals("Order"))
                {
                    if (Order == null)
                        return Error;
                }


                return null;
            }
        }
    }
}
