namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderPermLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PermissionLevels", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PermissionLevels", "Order");
        }
    }
}
