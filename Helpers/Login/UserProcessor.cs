using PQS.Models;
using PQS.Models.Models;
using System;
using Helpers.Interfaces;
using System.Diagnostics;

namespace Helpers.Login
{
    /// <summary>
    /// UserProcessor holds logic to retrieve, add, update and delete users
    /// from the database
    /// </summary>
    public class UserProcessor : IUserProcessor
    {
        /// <summary>
        /// Checks if the user already exists in the database.
        /// If the user already has an account the user object is returned.
        /// Otherwise a new default user object is created and returned as the current user
        /// </summary>
        public User GetUser()
        {
            User currentUser = new ValidateUser().CheckIfUserExists();
            return currentUser ?? AddDefaultUserProfile();
        }

        /// <summary>
        /// Creates a new user object with the default information
        /// and submits the user to the database.
        /// The default information is the creationdate, accessdate, windows id and 
        /// the account is defauled to a normal account.
        /// </summary>
        /// <returns>Returns the newly created user</returns>
        public User AddDefaultUserProfile()
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    User newUser = new User()
                    {
                        LastAccessDate = DateTime.Now,
                        AccountCreationDate = DateTime.Now,
                        WindowsId = new CurrentUser().GetLoggedInUser(),
                        IsAdmin = false
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return newUser;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return null;
            }
        }

        /// <summary>
        /// Update the current users last accessed date
        /// </summary>
        /// <param name="user">current accessing in user</param>
        public void UpdateUsersLastAccessDate(User user)
        {
            using (PQSContext db = new PQSContext())
            {
                User dbUser = db.Users.Find(user.PK_User_Id);

                if (dbUser != null)
                {
                    dbUser.LastAccessDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }
}
