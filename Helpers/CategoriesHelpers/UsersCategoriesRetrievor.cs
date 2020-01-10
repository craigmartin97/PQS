using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using Helpers.ModulesHelpers;

namespace Helpers.CategoriesHelpers
{
    public class UsersCategoriesRetrievor : IAdvancedRetrievor<IList<UsersModules>, Categories>
    {
        public IList<Categories> GetFilteredList(IList<UsersModules> item)
        {
            using (PQSContext db = new PQSContext())
            {
                return item.GroupBy(x => x.Module.Category.CategoryName)
                           .Select(x => x.First().Module.Category)
                           .OrderBy(x => x.Order)
                           .ToList();
            }
        }
    }
}
