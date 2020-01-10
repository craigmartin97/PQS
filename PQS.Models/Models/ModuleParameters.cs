using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PQS.Models.Models
{
    public class ModuleParameters
    {
        [Key]
        public int PK_ModuleParameter_Id { get; set; }

        public string Parameter { get; set; }

        public virtual Module Module { get; set; }

        public int Order { get; set; }

        public ModuleParameters()
        {

        }
    }
}
