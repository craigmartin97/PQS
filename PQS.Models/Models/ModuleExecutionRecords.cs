using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PQS.Models.Models
{
    /// <summary>
    /// Module execution records is to see which users executed which applications
    /// on what dates
    /// 
    /// I decoupled this from Modules and User objects so Users and Modules could be deleted 
    /// without deleting ExecutionRecords
    /// </summary>
    public class ModuleExecutionRecords
    {
        [Key]
        public int PK_Execution_Id { get; set; }

        public virtual Module Module { get; set; }

        public virtual User User { get; set; }

        //public string DisplayName { get; set; }

        //public string AdminIdentifyingName { get; set; }

        //public string WindowsId { get; set; }

        //public string Username { get; set; }

        public DateTime ExecutionDate { get; set; }

        [NotMapped]
        public int ExecutionCounter { get; set; }

        #region Constructors
        //string displayName, string adminIdName, string windowsId, string username, DateTime executionDate
        public ModuleExecutionRecords(Module module, User user, DateTime executionDate)
        {
            //DisplayName = displayName;
            //AdminIdentifyingName = adminIdName;
            //WindowsId = windowsId;
            //Username = username;
            Module = module;
            User = user;
            ExecutionDate = executionDate;
        }

        public ModuleExecutionRecords()
        {

        }
        #endregion
    }
}
