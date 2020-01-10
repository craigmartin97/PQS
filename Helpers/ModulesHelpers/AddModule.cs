using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using CustomDialog.Dialogs;
using CustomDialog.Service;
using System.Diagnostics;

namespace Helpers.ModulesHelpers
{
    public class AddModule : ModuleChanger, IAdd<Module>
    {
        /// <summary>
        /// Add a new module to the data store
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(Module item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    item.Category = db.Categories.Find(item.Category.PK_Category_id);
                    item.DisabledDate = GetDisabledDateTime(item.IsDisabled);
                    db.Modules.Add(item);
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
