using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;

namespace Helpers.PermissionLevelsHelper
{
    public class PermissionLevelsRetrievor : IRetrievor<PermissionLevels>
    {
        public IList<PermissionLevels> GetAll()
        {
            using (PQSContext db = new PQSContext())
            {
                return db.PermissionLevels.OrderBy(x => x.Order).ToList();
            }
        }
    }
}
