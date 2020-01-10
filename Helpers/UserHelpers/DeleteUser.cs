using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using Helpers.Login;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using CustomDialog.Enums;
using System.Diagnostics;

namespace Helpers.UserHelpers
{
    public class DeleteUser : IDeletor<User>
    {
        public bool Delete(User item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    User dbUser = db.Users.Find(item.PK_User_Id);
                    if (dbUser != null)
                    {
                        db.ModuleExecutionRecords.RemoveRange(db.ModuleExecutionRecords.Where(x => x.User.PK_User_Id == dbUser.PK_User_Id));
                        db.Users.Remove(dbUser);
                        db.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return false;
            }
        }
    }
}
