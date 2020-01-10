using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.ModulesHelpers
{
    public class ModuleDeletor : IDeletor<Module>
    {
        public bool Delete(Module item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    Module dbMod = db.Modules.Find(item.PK_Module_Id);
                    if (dbMod != null)
                    {
                        db.ModuleParameters.RemoveRange(db.ModuleParameters.Where(x => x.Module.PK_Module_Id == dbMod.PK_Module_Id));
                        db.ModuleExecutionRecords.RemoveRange(db.ModuleExecutionRecords.Where(x => x.Module.PK_Module_Id == dbMod.PK_Module_Id));
                        db.UserModuleRequests.RemoveRange(db.UserModuleRequests.Where(x => x.Module.PK_Module_Id == dbMod.PK_Module_Id));

                        db.Modules.Remove(dbMod);
                        db.SaveChanges();
                        return true;
                    }
                }

                return false;
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return false;
            }

        }
    }
}
