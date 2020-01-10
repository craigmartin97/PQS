using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using CustomDialog.Enums;
using System.Collections.ObjectModel;
using PQS.ViewModels.Commands;

namespace PQS.ViewModels.ViewModels
{
    /// <summary>
    /// Base View Model that all ViewModels inherit from
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Properties
        protected bool IsValidating { get; set; }

        private ViewModelBase currentView { get; set; }
        /// <summary>
        /// Stores the current ViewModel being displayed inside the ContentControl
        /// </summary>
        public ViewModelBase CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        protected static User _currentUser;

        private User currentUser;
        /// <summary>
        /// Holds the current user who is using the PQS system
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                _currentUser = value; // set a static to use throughout all classes
                OnPropertyChanged("CurrentUser");
            }
        }

        //private bool isOperationButtonEnabled = true;
        //public bool IsOperationButtonEnabled
        //{
        //    get { return isOperationButtonEnabled; }
        //    set
        //    {
        //        isOperationButtonEnabled = value;
        //        OnPropertyChanged("IsOperationButtonEnabled");
        //    }
        //}

        #endregion

        #region Notify Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region methods
        public void DisplayAlert(string title, string message, string imagePath)
        {
            DialogService dialogService = new DialogService();
            AlertDialogViewModel alert = new AlertDialogViewModel(title, message, imagePath);
            dialogService.OpenDialogService(alert);
        }

        public DialogResults DisplayOptionDialog(string title, string message)
        {
            DialogService dialogService = new DialogService();
            YesNoDialogViewModel optionDialog = new YesNoDialogViewModel(title, message);
            DialogResults result = dialogService.OpenDialogService(optionDialog);
            return result;
        }

        #endregion
    }
}