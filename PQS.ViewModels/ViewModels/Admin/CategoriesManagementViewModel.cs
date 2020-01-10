using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using CustomDialog.Service;
using System.Collections.ObjectModel;
using PQS.Models.Models;
using PQS.ViewModels.Commands;
using System.ComponentModel;

namespace PQS.ViewModels.ViewModels.Admin
{
    public abstract class CategoriesManagementViewModel : ViewModelBase
    {
        #region Private vars
        //protected readonly IDialogService dialogService;
        protected readonly IRetrievor<Categories> retrievor;

        protected Categories tempCategory;
        #endregion

        #region Properties
        private ObservableCollection<Categories> allCategories;
        /// <summary>
        /// Collection of all the categories avaliable to the user from the database
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

        private string categoryName;
        /// <summary>
        /// The name of the category the user has entered
        /// </summary>
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

        private int? order;
        /// <summary>
        /// The order of the category the category will appear in the side menu and in combo boxes
        /// inline with other categories.
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
        #endregion

        #region Constructors
        public CategoriesManagementViewModel(IRetrievor<Categories> iRetrievor) //IDialogService iDialogService, 
        {
            //dialogService = iDialogService;
            retrievor = iRetrievor;
            SetAllCategories();
        }
        #endregion

        #region Commands
        /// <summary>
        /// The category operation command allows the user to 
        /// either edit a category that has been selected or add a 
        /// new category to the database.
        /// </summary>
        public RelayCommand CategoryOperationCommand { get { return new RelayCommand(param => { CategoriesOperation(); }); } }
        #endregion

        #region Override Methods
        protected abstract Categories CreateNewCategory();
        #endregion

        #region Internal Helpers
        /// <summary>
        /// The method for the operation when the user presses the operation button
        /// to edit or add a category.
        /// </summary>
        protected abstract void CategoriesOperation();

        /// <summary>
        /// Get all the valid categories that the user can engage with
        /// </summary>
        protected void SetAllCategories()
        {
            AllCategories = new ObservableCollection<Categories>(retrievor.GetAll());
        }

        /// <summary>
        /// Checks if the user has entered a category name and order value
        /// </summary>
        /// <returns>Returns bool result if the user has entered the correct data</returns>
        protected bool ValidateUserEntries()
        {
            return !string.IsNullOrWhiteSpace(tempCategory.CategoryName) && tempCategory.Order != null;
        }
        #endregion
    }
}
