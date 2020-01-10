using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models;

namespace Helpers.ModulesHelpers
{
    public class ModuleExists
    {

        public bool CheckIfModuleWithAdminIdNameAlreadyExists(string adminIdName)
        {
            return CheckIfModuleWithAdminIdNameAlreadyExistsAndNotSelected(null, adminIdName);
        }

        public bool CheckIfModuleWithAdminIdNameAlreadyExistsAndNotSelected(int? selectedModuleId, string adminIdName)
        {
            using (PQSContext db = new PQSContext())
            {
                if (db.Modules.FirstOrDefault(x => (selectedModuleId != null? x.PK_Module_Id != selectedModuleId : true) && x.AdminIdentifyingName.Equals(adminIdName)) != null) // the same admin id name already exists
                    return true;

                return false; // the admin id name doesnt exists yet.
            }
        }

        public bool CheckIfModuleAlreadyExistsInCategory(int categoryId, string displayName)
        {
            using(PQSContext db = new PQSContext())
            {
                if (db.Modules.FirstOrDefault(x => x.Category.PK_Category_id == categoryId && x.DisplayName.Equals(displayName)) != null)
                    return true; // display name already exists in this category

                return false; // doesnt exist yet
            }
        }
    }
}
