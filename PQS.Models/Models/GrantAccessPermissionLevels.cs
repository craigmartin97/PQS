using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PQS.Models.Models
{
    [NotMapped]
    public class GrantAccessPermissionLevels : PermissionLevels
    {
        public bool IsAccessRequest { get; set; }

        public string AccessDescription { get; set; }

        public PermissionLevels RequestedAccess { get; set; }

        public bool HasRequested { get; set; }
    }
}
