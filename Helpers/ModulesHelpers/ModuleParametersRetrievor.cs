using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using System.Data.Entity;
using PQS.Models.Models;
using PQS.Models;

namespace Helpers.ModulesHelpers
{
    public class ModuleParametersRetrievor : IAdvancedRetrievor<Module, ModuleParameters>
    {
        public IList<ModuleParameters> GetFilteredList(Module item)
        {
            using (PQSContext db = new PQSContext())
            {
                return db.ModuleParameters
                    .Include(x => x.Module)
                    .Include(x => x.Module.Category)
                    .Where(x => x.Module.PK_Module_Id == item.PK_Module_Id)
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x.Parameter)
                    .ToList();
            }
        }
    }
}
