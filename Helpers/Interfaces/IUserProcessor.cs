using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;

namespace Helpers.Interfaces
{
    public interface IUserProcessor
    {
        User GetUser();
        User AddDefaultUserProfile();
        void UpdateUsersLastAccessDate(User user);
    }
}
