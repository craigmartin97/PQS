using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using PQS.Models;

namespace Helpers.PermissionLevelsHelper
{
    public class UserPermissionLevelRetrievor
    {
        public PermissionLevels GetPermissionLevel(Module module, User currentUser)
        {
            using (PQSContext db = new PQSContext())
            {
                UsersModules usersMod = db.UsersModules.FirstOrDefault(x => x.Module.PK_Module_Id == module.PK_Module_Id && x.User.PK_User_Id == currentUser.PK_User_Id);
                return usersMod != null ? usersMod.PermissionLevel : null;
            }
        }
    }
}
