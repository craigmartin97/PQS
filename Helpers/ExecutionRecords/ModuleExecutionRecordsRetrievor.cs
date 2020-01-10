using Helpers.Interfaces;
using PQS.Models;
using PQS.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Helpers.ExecutionRecords
{
    public class ModuleExecutionRecordsRetrievor : IRetrievor<ModuleExecutionRecords>
    {
        /// <summary>
        /// Get all the module execution records
        /// </summary>
        /// <returns></returns>
        public IList<ModuleExecutionRecords> GetAll()
        {
            using (PQSContext db = new PQSContext())
            {
                return db.ModuleExecutionRecords.Include(x => x.Module).Include(x => x.User).ToList();
            }
        }
    }
}
