using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.CategoriesHelpers
{
    public class AddCategory : IAdd<Categories>
    {
        public bool Add(Categories item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    db.Categories.Add(item);
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
