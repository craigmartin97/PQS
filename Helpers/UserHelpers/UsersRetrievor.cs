using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using PQS.Models;
using Helpers.Interfaces;

namespace Helpers.UserHelpers
{
    /// <summary>
    /// The users retrievor is responsible for retrieiving users details
    /// </summary>
    public class UsersRetrievor : IRetrievor<User>
    {
        /// <summary>
        /// Get all users from the data store
        /// </summary>
        /// <returns>Returns collection of users</returns>
        public IList<User> GetAll()
        {
            using (PQSContext db = new PQSContext())
            {
                return db.Users.ToList();
            }
        }
    }
}
