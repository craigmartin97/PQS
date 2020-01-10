using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomDialog.Service
{
    public interface IDialogWindow
    {
        bool? DialogResult { get; set; }
        object DataContext { get; set; }

        bool? ShowDialog();
    }
}
