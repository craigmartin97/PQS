using System.Windows.Input;
using CustomDialog.Enums;
using CustomDialog.Service;
using Helpers.Commands;

namespace CustomDialog.Dialogs
{
    public class AlertDialogViewModel : BaseDialogViewModel<DialogResults>
    {
        public ICommand OKCommand { get; private set; }

        public AlertDialogViewModel(string title, string message, string imagePath)
            : base(title, message, imagePath)
        {
            OKCommand = new GenericRelayCommand<IDialogWindow>(OK);
        }

        //Window must be passed in param to close.
        private void OK(IDialogWindow window)
        {
            CloseDialogWithResult(window, DialogResults.Undefined);
        }
    }
}