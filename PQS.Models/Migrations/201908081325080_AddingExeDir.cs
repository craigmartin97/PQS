namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingExeDir : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "ExecutionDirectory", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "ExecutionDirectory");
        }
    }
}
