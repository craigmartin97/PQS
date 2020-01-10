using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomDialog.Service;

namespace CustomDialog.Dialogs
{
    public abstract class BaseDialogViewModel<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public T DialogResult { get; set; }

        public string ImagePath { get; set; }

        public bool CloseButtonVisibility { get; set; }

        public BaseDialogViewModel() : this(string.Empty, string.Empty, string.Empty, true) { }
        public BaseDialogViewModel(string title) : this(title, string.Empty, string.Empty, true) { }

        public BaseDialogViewModel(string title, string message, string imagePath, bool closeBtnVisibility = true)
        {
            Title = title;
            Message = message;
            ImagePath = imagePath;
            CloseButtonVisibility = closeBtnVisibility;
        }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;
            dialog.DialogResult = (result == null) ? false : true;
        }
    }
}
