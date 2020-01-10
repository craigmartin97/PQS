using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.ExecutionRecords
{
    public class AddModuleExecutionRecords : IAdd<ModuleExecutionRecords>
    {
        public bool Add(ModuleExecutionRecords item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    item.Module = db.Modules.Find(item.Module.PK_Module_Id);
                    item.User = db.Users.Find(item.User.PK_User_Id);

                    db.ModuleExecutionRecords.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return false;
            }

        }
    }
}
