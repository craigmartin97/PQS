using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PQS.Models.Models
{
    public class UsersModules : ICloneable
    {
        [Key]
        public int PK_UsersModules_Id { get; set; }

        [Required]
        public virtual Module Module { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual PermissionLevels PermissionLevel { get; set; }

        public bool IsFavourite { get; set; }

        /// <summary>
        /// The admin user who gave the user access writes to the module
        /// </summary>
        public virtual User AdminAccessor { get; set; }

        /// <summary>
        /// A collection of permission levels for users module.
        /// Allows users to select options. These options can change for each module.
        /// </summary>
        [NotMapped]
        public ObservableCollection<PermissionLevels> PermissionLevelOptions { get; set; }

        /// <summary>
        /// Bool to indicate if the user has requested access to the software
        /// Needed for view binding.
        /// </summary>
        [NotMapped]
        public bool RequestAlreadyMadePermissionLevelOptions { get; set; }

        public object Clone()
        {
            return new UsersModules()
            {
                PK_UsersModules_Id = PK_UsersModules_Id,
                Module = Module,
                User = User,
                PermissionLevel = PermissionLevel,
                IsFavourite = IsFavourite
            };
        }
    }
}
