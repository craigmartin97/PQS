using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using System.Collections.ObjectModel;
using PQS.Models;
using System.Data.Entity;

namespace Helpers.PermissionLevelsHelper
{
    public class AvaliableUserPermissionLevels
    {
        public UserModuleRequests GetAvaliablePermissionLevelsForUser(UsersModules usersModule)
        {
            using(PQSContext db = new PQSContext())
            {
                return  db.UserModuleRequests
                    .Include(x => x.Module).Include(x => x.Module.Category).Include(x => x.PermissionLevel).Include(x => x.User)
                    .FirstOrDefault(x => (x.Module.PK_Module_Id == usersModule.Module.PK_Module_Id && x.User.PK_User_Id == usersModule.User.PK_User_Id)
                        && (x.IsComplete == null || x.IsComplete == false) && x.RequestCompletionDate == null);
            }
        }
    }
}
