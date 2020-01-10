using Helpers.Interfaces;
using PQS.Models;
using PQS.Models.Models;
using CustomDialog.Service;
using CustomDialog.Dialogs;
using CustomDialog.Enums;
using System;
using System.Diagnostics;

namespace Helpers.CategoriesHelpers
{
    public class CategoriesUpdator : IUpdator<Categories>
    {
        /// <summary>
        /// Updates an existing categorys details
        /// </summary>
        /// <param name="category">category object to find and update</param>
        /// <returns>Returns true if successfully updated the category</returns>
        public bool Update(Categories item)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    if (item != null)
                    {


                            // get the category from the database
                            Categories dbCategory = db.Categories.Find(item.PK_Category_id);
                            if (dbCategory != null)
                            {
                                dbCategory.CategoryName = item.CategoryName;
                                dbCategory.Order = item.Order;
                                db.SaveChanges();
                                return true;
                            }
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
    }
}