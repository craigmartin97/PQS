using PQS.Models;
using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Helpers.Interfaces;

namespace Helpers.ModulesHelpers
{
    public class ModulesRetrievor : IRetrievor<Module>, IAdvancedRetrievor<Categories, Module>
    {
        public IList<Module> GetAll()
        {
            using (PQSContext db = new PQSContext())
            {
                return db.Modules.Include(m => m.Category).OrderBy(x => x.AdminIdentifyingName).ToList();
            }
        }

        public IList<Module> GetFilteredList(Categories item)
        {
            using (PQSContext db = new PQSContext())
            {
                return db.Modules.Where(x => x.IsActive && x.Category.PK_Category_id == item.PK_Category_id).ToList();
            }
        }
    }
}
