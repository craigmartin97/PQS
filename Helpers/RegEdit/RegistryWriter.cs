using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using PQS.Models.Models;

namespace Helpers.RegEdit
{
    public class RegistryWriter
    {
        /// <summary>
        /// Check if the Dupint Teijin Films and PQS Keys exists
        /// </summary>
        public void CheckRegistryKeyExists(string checkSubKey)
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(checkSubKey);
            if (regKey == null)
                Registry.LocalMachine.CreateSubKey(checkSubKey);
        }

        /// <summary>
        /// Create the PQS Key and call the method to write the default values
        /// </summary>
        public void WritePQSKey()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Dupont Teijin Films\PQS");
            key.Close();
        }

        /// <summary>
        /// Write the loggedin status and the current users information to the regedit
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentUser"></param>
        public void WriteUserKeyAndValues(User currentUser)
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Dupont Teijin Films\PQS\User");
            key.SetValue("loggedin", "TRUE");
            key.SetValue("username", currentUser.Username);
            key.Close();
        }

        /// <summary>
        /// Write the modules key and the software keys and values to the regedit
        /// </summary>
        /// <param name="module"></param>
        public void WriteModulesKeyAndValues(Module module, PermissionLevels permLevel)
        {
            RegistryKey modulesKey = Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Dupont Teijin Films\PQS\Modules");

            RegistryKey softwareKey = Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Dupont Teijin Films\PQS\Modules\" + module.AdminIdentifyingName);
            softwareKey.SetValue("description", module.DisplayName);
            softwareKey.SetValue("name", module.AdminIdentifyingName);

            RegistryKey userKey = Registry.LocalMachine.CreateSubKey(@"Software\Wow6432Node\Dupont Teijin Films\PQS\User");

            string access = permLevel != null ? permLevel.Access.ToLower() : "read"; // if its null for some reason only allow the user to read, this would be unexpected and "Catch" case to prevent crash
            userKey.SetValue(module.AdminIdentifyingName, access);

            userKey.Close();
            softwareKey.Close();
            modulesKey.Close();


        }
    }
}
