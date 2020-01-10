using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models;

namespace Helpers.CategoriesHelpers
{
    public class CategoryExists
    {
        public bool CheckIfCategoryWithSameNameExists(string categoryName)
        {
            using (PQSContext db = new PQSContext())
            {
                if (db.Categories.FirstOrDefault(x => x.CategoryName.Equals(categoryName)) != null)
                    return true; // category with same name already exists

                return false; // category doesnt exist with the same name
            }
        }
    }
}
