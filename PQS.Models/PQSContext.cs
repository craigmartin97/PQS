using PQS.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PQS.Models.Properties;

namespace PQS.Models
{
    public class PQSContext : DbContext
    {

        public PQSContext() : base(Settings.Default.ConnString) 
        {

        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<PermissionLevels> PermissionLevels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UsersModules> UsersModules { get; set; }
        public DbSet<ModuleParameters> ModuleParameters { get; set; }
        public DbSet<ModuleExecutionRecords> ModuleExecutionRecords { get; set; }
        public DbSet<UserModuleRequests> UserModuleRequests { get; set; }
    }
}
