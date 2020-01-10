using PQS.Models;
using PQS.Models.Models;
using System.Linq;

namespace Helpers.Login
{
    /// <summary>
    /// Validates the users details
    /// </summary>
    public class ValidateUser
    {
        /// <summary>
        /// Check if the user already exists in the 
        /// database.
        /// </summary>
        public User CheckIfUserExists()
        {
            using (PQSContext db = new PQSContext())
            {
                CurrentUser currentUser = new CurrentUser();
                string userName = currentUser.GetLoggedInUser();
                return db.Users.FirstOrDefault(x => x.WindowsId.Equals(userName));
            }
        }
    }
}
