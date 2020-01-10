using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using System.Diagnostics;

namespace Helpers.ModulesHelpers
{
    public class ModuleUpdator : ModuleChanger, IUpdator<Module>
    {
        public bool Update(Module item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    Module dbModule = db.Modules.Find(item.PK_Module_Id);
                    if (dbModule != null)
                    {
                        Categories category = db.Categories.Find(item.Category.PK_Category_id);
                        dbModule.Category = category;

                        dbModule.DisplayName = item.DisplayName;
                        dbModule.AdminIdentifyingName = item.AdminIdentifyingName;
                        dbModule.Description = item.Description;
                        dbModule.ExecutionPath = item.ExecutionPath;
                        dbModule.ExecutionDirectory = item.ExecutionDirectory;
                        dbModule.IconPath = item.IconPath;
                        dbModule.LastExecutionDate = item.LastExecutionDate;
                        dbModule.Order = item.Order;
                        dbModule.IsActive = item.IsActive;
                        dbModule.IsDisabled = item.IsDisabled;
                        dbModule.DisabledDate = GetDisabledDateTime(item.IsDisabled);
                        
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return false;
            }
        }

        public void UpdateLastExecutionTime(Module module)
        {
            using (PQSContext db = new PQSContext())
            {
                Module dbMod = db.Modules.Find(module.PK_Module_Id);
                dbMod.LastExecutionDate = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
