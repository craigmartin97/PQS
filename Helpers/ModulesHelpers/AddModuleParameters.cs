using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using PQS.Models;

namespace Helpers.ModulesHelpers
{
    public class AddModuleParameters : IAdd<IList<ModuleParameters>>
    {
        public bool Add(IList<ModuleParameters> item)
        {
            using (PQSContext db = new PQSContext())
            {
                foreach (ModuleParameters param in item)
                {
                    db.ModuleParameters.Add(param);    
                }

                db.SaveChanges();
                return true;
            }
        }
    }
}
