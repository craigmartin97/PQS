using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using PQS.Models.Models;

namespace Helpers.RegEdit
{
    public class RegistryRemover
    {
        /// <summary>
        /// Remove the PQS Key and all child keys
        /// </summary>
        public void RemoveAllKeys()
        {
            Registry.LocalMachine.DeleteSubKeyTree(@"Software\Wow6432Node\Dupont Teijin Films\PQS");
        }
    }
}
