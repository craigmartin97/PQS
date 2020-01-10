using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using PQS.Models.Models;
using PQS.Models;
using Helpers.ModulesHelpers;
using Helpers.Interfaces;
using Helpers.RegEdit;
using Helpers.PermissionLevelsHelper;

namespace Helpers.Executor
{
    public class ModuleExecutor
    {
        #region Propertys
        private readonly IAdd<ModuleExecutionRecords> executionRecords;

        /// <summary>
        /// Import dll needed to bring exe to foreground
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region Constructors
        public ModuleExecutor(IAdd<ModuleExecutionRecords> executionRecordsAdder)
        {
            executionRecords = executionRecordsAdder;
        }
        #endregion

        public void Execute(Module module, User currentUser)
        {
            try
            {
                bool pathExists = File.Exists(module.ExecutionPath);
                if (pathExists)
                {
                    UserPermissionLevelRetrievor permissionRetrievor = new UserPermissionLevelRetrievor();
                    PermissionLevels permLevel = permissionRetrievor.GetPermissionLevel(module, currentUser);
                    RegistryWriter writer = new RegistryWriter();
                    writer.WriteModulesKeyAndValues(module, permLevel);

                    Process process = new Process();
                    ProcessStartInfo startArgs = new ProcessStartInfo();

                    startArgs.FileName = module.ExecutionPath;
                    startArgs.Arguments = GetModuleParameters(module);
                    if (Directory.Exists(module.ExecutionDirectory))
                    {
                        startArgs.WorkingDirectory = module.ExecutionDirectory;
                    }
                    startArgs.UseShellExecute = false;
                    process.StartInfo = startArgs;
                    process.Start();

                    new ModuleUpdator().UpdateLastExecutionTime(module);
                    executionRecords.Add(new ModuleExecutionRecords(module, currentUser, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "  " + ex.InnerException);
            }
        }

        public string GetModuleParameters(Module module)
        {
            string args = null;

            IList<ModuleParameters> paramList = new ModuleParametersRetrievor().GetFilteredList(module);

            if ((paramList != null) && paramList.Count > 0)
            {
                foreach (ModuleParameters param in paramList)
                {
                    args += " " + param.Parameter;
                }
            }

            if (args != null)
                args.Trim();

            return args;
        }
    }
}
