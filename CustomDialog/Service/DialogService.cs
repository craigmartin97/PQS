using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomDialog.Dialogs;
using CustomDialog.Views;

namespace CustomDialog.Service
{
    public class DialogService : IDialogService
    {
        public T OpenDialogService<T>(BaseDialogViewModel<T> dialogViewModel)
        {
            IDialogWindow window = new DialogWindow();
            window.DataContext = dialogViewModel;
            window.ShowDialog();
            return dialogViewModel.DialogResult;
        }
    }
}
