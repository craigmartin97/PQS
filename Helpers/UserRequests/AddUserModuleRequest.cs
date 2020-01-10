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
    public class AddUserModuleRequest : IAdd<UserModuleRequests>
    {
        public bool Add(UserModuleRequests item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    if (item.Module != null && item.Module.Category != null && item.User != null)
                    {
                        item.Module = db.Modules.Find(item.Module.PK_Module_Id);
                        item.Module.Category = db.Categories.Find(item.Module.Category.PK_Category_id);
                        item.User = db.Users.Find(item.User.PK_User_Id);
                        item.PermissionLevel = db.PermissionLevels.Find(item.PermissionLevel.PK_PermissionLevel_Id);

                        db.UserModuleRequests.Add(item);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}