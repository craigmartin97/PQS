using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PQS.Models.Models
{
    public class User : IDataErrorInfo
    {
        [Key]
        public int PK_User_Id { get; set; }

        [Required]
        public string WindowsId { get; set; }

        public string Username { get; set; }

        [Required]
        public DateTime AccountCreationDate { get; set; }

        [Required]
        public DateTime LastAccessDate { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        //public virtual ICollection<Module> Modules { get; set; }

        public string Error
        {
            get { return "This field cannot be empty"; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("Username"))
                {
                    if (string.IsNullOrWhiteSpace(Username))
                        return Error;
                }

                return null;
            }
        }
    }
}