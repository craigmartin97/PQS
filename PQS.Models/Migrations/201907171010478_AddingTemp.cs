namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTemp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Temp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Temp");
        }
    }
}
