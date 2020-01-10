using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.ModulesHelpers
{
    public class UpdateFavourite : IUpdator<UsersModules>
    {

        public bool Update(UsersModules item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    UsersModules module = db.UsersModules.Find(item.PK_UsersModules_Id);
                    if (module != null)
                    {
                        module.IsFavourite = item.IsFavourite;
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
