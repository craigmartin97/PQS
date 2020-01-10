using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using PQS.Models;
using System.Data.Entity;

namespace Helpers.UserRequests
{
    public class AllUsersRequests : IRetrievor<UserModuleRequests>
    {

        public IList<UserModuleRequests> GetAll()
        {
            using (PQSContext db = new PQSContext())
            {
                return db.UserModuleRequests.Include(x => x.Module).Include(x => x.Module.Category).Include(x => x.User).Include(x => x.PermissionLevel)
                    .Where(x => x.IsComplete == null || !(bool)x.IsComplete).ToList();
            }
        }
    }
}
