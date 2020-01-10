namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveFlag1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Modules", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
