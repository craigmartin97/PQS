using System.Collections.Generic;
using System.Linq;
using Helpers.Interfaces;
using Helpers.ModulesHelpers;
using PQS.Models;
using PQS.Models.Models;

namespace Helpers.CategoriesHelpers
{
    /// <summary>
    /// Categories Processor handles the logic for categories
    /// </summary>
    public class CategoriesRetrievor : IRetrievor<Categories>
    {
        /// <summary>
        /// Gets all the categories from the database table Categories.
        /// </summary>
        /// <returns>Returns a list of categories</returns>
        public IList<Categories> GetAll()
        {
            using (PQSContext db = new PQSContext())
                return db.Categories.OrderBy(x => x.Order).ThenBy(x => x.CategoryName).ToList();
        }
    }
}