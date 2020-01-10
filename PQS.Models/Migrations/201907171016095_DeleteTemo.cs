namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTemo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Temp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Temp", c => c.Boolean(nullable: false));
        }
    }
}
