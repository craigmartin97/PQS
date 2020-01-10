namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "IsActive");
        }
    }
}
