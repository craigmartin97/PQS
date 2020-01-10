using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PQS.Models.Models
{
    public class UsersStats
    {
        public string Title { get; set; }
        public ObservableCollection<ModuleExecutionRecords> ModuleExecutionRecords { get; set; }
        public int NumOfExecutions { get; set; }
    }
}
