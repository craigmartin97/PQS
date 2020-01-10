using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PQS.Models.Models
{
    public class PermissionLevels
    {
        [Key]
        public int PK_PermissionLevel_Id { get; set; }

        [Required]
        public string Access { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public bool IsEditorPermission { get; set; }
    }
}