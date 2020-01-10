using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Helpers
{
    public class GetApplicationVersion
    {
        /// <summary>
        /// Get the calling applications current version number
        /// </summary>
        /// <returns>Returns a number that is the current version i.e. 1.0.0.0</returns>
        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
