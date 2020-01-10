using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Data.Entity;

namespace Helpers.ModulesHelpers
{
    public class AllUsersModulesRetrievor : IAdvancedRetrievor<Module, UsersModules>
    {
        public IList<UsersModules> GetFilteredList(Module item)
        {
            using (PQSContext db = new PQSContext())
            {
                return db.UsersModules.Include(x => x.Module)
                                      .Include(x => x.PermissionLevel)
                                      .Include(x => x.User)
                                      .Include(x => x.Module.Category)
                                      .Where(x => x.Module.PK_Module_Id == item.PK_Module_Id).ToList();
            }
        }
    }
}
