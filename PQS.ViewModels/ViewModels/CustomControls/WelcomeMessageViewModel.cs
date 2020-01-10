using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CustomDialog.Commands;
using System.Windows.Input;
using CustomDialog.Dialogs;
using CustomDialog.Service;

namespace PQS.ViewModels.ViewModels.CustomControls
{
    public class WelcomeMessageViewModel : BaseDialogViewModel<string>, INotifyPropertyChanged, IDataErrorInfo
    {
        private string userName;
        /// <summary>
        /// The text that the user enters as there UserName
        /// </summary>
        public string Username
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("Username");
            }
        }

        public ICommand CreateAccountCommand { get; private set; }

        public WelcomeMessageViewModel(string title)
            : base(title, string.Empty, string.Empty, false)
        {
            CreateAccountCommand = new GenericRelayCommand<IDialogWindow>(CreateAccount);
        }

        //Window must be passed in param to close.
        private void CreateAccount(IDialogWindow window)
        {
            if (!string.IsNullOrWhiteSpace(Username))
                CloseDialogWithResult(window, Username);
        }

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

        #region IDataErrorInfo
        public string Error
        {
            get { return "You must enter a username"; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("Username"))
                    if (string.IsNullOrWhiteSpace(Username))
                        return Error;

                return string.Empty;
            }
        }

        #endregion
    }
}
