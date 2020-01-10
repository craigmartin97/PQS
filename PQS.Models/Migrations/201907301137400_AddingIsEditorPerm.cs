namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsEditorPerm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PermissionLevels", "IsEditorPermission", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PermissionLevels", "IsEditorPermission");
        }
    }
}
