using CustomDialog.Dialogs;
using CustomDialog.Enums;
using CustomDialog.Service;
using Helpers.Interfaces;
using PQS.Models;
using PQS.Models.Models;
using System;
using System.Diagnostics;

namespace Helpers.UserHelpers
{
    public class UserUpdator : IUpdator<User>
    {

        public bool Update(User item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    User dbUser = db.Users.Find(item.PK_User_Id);
                    if (dbUser != null)
                    {
                        dbUser.IsAdmin = item.IsAdmin;
                        dbUser.Username = item.Username;
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
