using System.Windows.Input;
using CustomDialog.Enums;
using CustomDialog.Service;
using Helpers.Commands;

namespace CustomDialog.Dialogs
{
    public class YesNoDialogViewModel : BaseDialogViewModel<DialogResults>
    {
        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public YesNoDialogViewModel(string title, string message)
            : base(title, message, string.Empty)
        {
            YesCommand = new GenericRelayCommand<IDialogWindow>(Yes);
            NoCommand = new GenericRelayCommand<IDialogWindow>(No);
        }

        //Window must be passed in param to close.
        private void Yes(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Yes);
        }

        private void No(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.No);
        }
    }
}