using CustomDialog.Dialogs;

namespace CustomDialog.Service
{
    public interface IDialogService
    {
        T OpenDialogService<T>(BaseDialogViewModel<T> dialogViewModel);
    }
}
