using PQS.Models;
using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Helpers.Interfaces;

namespace Helpers.ModulesHelpers
{
    /// <summary>
    /// Users module processor is responsible for manipulating the
    /// users modules for the database
    /// </summary>
    public class UsersModulesProcessor : IAdvancedRetrievor<User, UsersModules>
    {

        /// <summary>
        /// Get all the modules for the current user
        /// </summary>
        /// <param name="user">current users id</param>
        /// <returns>Returns list of modules for the user</returns>
        public IList<UsersModules> GetFilteredList(User item)
        {
            using (PQSContext db = new PQSContext())
            {
                return db.UsersModules
                    .Include(x => x.Module)
                    .Include(x => x.Module.Category)
                    .Include(x => x.PermissionLevel)
                    .Include(x => x.User)
                    .Where(x => x.User.PK_User_Id == item.PK_User_Id).ToList();// null;//db.Modules.Include(m => m.Category).Where(x => x.Users.Any(u => u.PK_User_Id == user.PK_User_Id)).ToList();
            }
        }
    }
}