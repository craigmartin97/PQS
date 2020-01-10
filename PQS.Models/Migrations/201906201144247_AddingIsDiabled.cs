namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsDiabled : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "IsDisabled", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "IsDisabled");
        }
    }
}
