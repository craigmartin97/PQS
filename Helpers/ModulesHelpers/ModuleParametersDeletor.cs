using Helpers.Interfaces;
using PQS.Models;
using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Helpers.ModulesHelpers
{
    public class ModuleParametersDeletor : IDeletor<Module>
    {
        /// <summary>
        /// Delete all the modules parameters
        /// </summary>
        /// <param name="item">the given module</param>
        /// <returns>Returns true if successfully deleted the parameters</returns>
        public bool Delete(Module item)
        {
            /**
             * This class is used to delete the modules parameters
             * If the user deletes a parameter for the module it would have been tricky
             * to find, so i decided to just delete and readd them all again.
             * Maybe this isnt the best idea or the best way this could be done but it works fine.
             * An alternative would be to retrieve all the modules parameters and check them against the primary key of the oen in the 
             * list if its not found then delete from db.
             * 
             */
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    IEnumerable<ModuleParameters> modulesParams = db.ModuleParameters.Where(x => x.Module.PK_Module_Id == item.PK_Module_Id);
                    db.ModuleParameters.RemoveRange(modulesParams);
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
