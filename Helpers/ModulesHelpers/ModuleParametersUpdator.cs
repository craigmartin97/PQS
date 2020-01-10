using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Helpers.ModulesHelpers
{
    public class ModuleParametersUpdator : IUpdator<ObservableCollection<ModuleParameters>>
    {
        public bool Update(ObservableCollection<ModuleParameters> item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    if (item != null)
                    {
                        foreach (ModuleParameters param in item)
                        {
                            Debug.WriteLine("Updating params for " + param.Module.AdminIdentifyingName);
                            ModuleParameters paramExists = db.ModuleParameters.FirstOrDefault(x => x.PK_ModuleParameter_Id == param.PK_ModuleParameter_Id);

                            if (paramExists != null) // parameter already exists so update it
                            {
                                paramExists.Parameter = param.Parameter;
                                paramExists.Order = param.Order;
                            }
                            else if (paramExists == null) //add new param to db fo mod
                            {
                                Module dbMod = db.Modules.Find(param.Module.PK_Module_Id);
                                param.Module = dbMod;
                                db.ModuleParameters.Add(param);
                            }
                        }

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
