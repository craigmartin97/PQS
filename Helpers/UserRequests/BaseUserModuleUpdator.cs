using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using PQS.Models;

namespace Helpers.UserRequests
{
    public class BaseUserModuleUpdator
    {
        protected PermissionLevels GetPermissionLevel(PQSContext db, UsersModules module)
        {
            return module.PermissionLevel != null ? db.PermissionLevels.Find(module.PermissionLevel.PK_PermissionLevel_Id) : null;
        }

        protected Categories GetCategory(PQSContext db, UsersModules module)
        {
            return module.Module.Category != null ? db.Categories.Find(module.Module.Category.PK_Category_id) : null;
        }
    }
}
