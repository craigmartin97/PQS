using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomDialog.Service;
using PQS.Models.Models;

namespace Helpers.Interfaces
{
    public interface IUsername
    {
        string PromptUserToEnterUsername(User currentUser, IDialogService dialogService);

        void UpdateUsername(User currentUser, string userName);
    }
}
