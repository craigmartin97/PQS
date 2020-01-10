using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.UserRequests
{
    public class DeleteUserModuleRequest : IDeletor<UserModuleRequests>
    {
        public bool Delete(UserModuleRequests item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    UserModuleRequests dbUserModRequest = db.UserModuleRequests.Find(item.PK_UserModuleRequest_Id);

                    if (dbUserModRequest != null)
                    {
                        db.UserModuleRequests.Remove(dbUserModRequest);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

        }
    }
}
