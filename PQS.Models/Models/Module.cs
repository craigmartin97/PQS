using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace PQS.Models.Models
{
    public class Module : ICloneable, IDataErrorInfo
    {
        [Key]
        public int PK_Module_Id { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Required]
        public string AdminIdentifyingName { get; set; }

        [Required]
        public string ExecutionPath { get; set; }

        [Required]
        public string ExecutionDirectory { get; set; }

        [Required]
        public string IconPath { get; set; }

        public DateTime? LastExecutionDate { get; set; }

        [Required]
        public virtual Categories Category { get; set; }

        [Required]
        public int? Order { get; set; }

        public bool IsActive { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime? DisabledDate { get; set; }

        //public virtual ICollection<User> Users { get; set; }


        #region IDataErrorInfo
        [NotMapped]
        public string Error
        {
            get { return "This field can't be empty"; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("DisplayName"))
                {
                    if (string.IsNullOrWhiteSpace(DisplayName))
                        return Error;
                }
                else if (columnName.Equals("AdminIdentifyingName"))
                {
                    if (string.IsNullOrWhiteSpace(AdminIdentifyingName))
                        return Error;
                }
                else if (columnName.Equals("ExecutionPath"))
                {
                    if (string.IsNullOrWhiteSpace(ExecutionPath))
                        return Error;
                    else if (!CheckFilePath(ExecutionPath))
                        return "The given path does not exist";
                }
                else if (columnName.Equals("IconPath"))
                {
                    if (string.IsNullOrWhiteSpace(IconPath))
                        return Error;
                    else if (!CheckFilePath(IconPath))
                        return "The given path does not exist";
                }
                else if (columnName.Equals("Order"))
                {
                    if (Order == null)
                        return Error;
                }
                else if (columnName.Equals("Category"))
                {
                    if (Category == null)
                        return Error;
                }

                return null;
            }
        }
        #endregion

        #region Constructors
         public Module()
        {

        }

        public Module(bool isActive)
        {
            IsActive = isActive;
        }
        #endregion

        #region Clones
        public object Clone()
        {
            return new Module()
            {
                PK_Module_Id = PK_Module_Id,
                DisplayName = DisplayName,
                AdminIdentifyingName = AdminIdentifyingName,
                Description = Description,
                ExecutionPath = ExecutionPath,
                IconPath = IconPath,
                Category = Category,
                LastExecutionDate = LastExecutionDate,
                Order = Order,
                IsActive = IsActive,
                IsDisabled = IsDisabled,
                DisabledDate = DisabledDate
            };
        }
        #endregion

        private bool CheckFilePath(string path)
        {
            return File.Exists(path);
        }
    }
}