using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Login
{
    /// <summary>
    /// CurrentUser class retrieves the current logged in user 
    /// on the device.
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Get the current logged in users name
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUser() { return System.Security.Principal.WindowsIdentity.GetCurrent().Name; }
    }
}
