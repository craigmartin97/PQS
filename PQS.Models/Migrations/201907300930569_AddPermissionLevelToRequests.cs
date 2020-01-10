namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissionLevelToRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserModuleRequests", "PermissionLevel_PK_PermissionLevel_Id", c => c.Int());
            CreateIndex("dbo.UserModuleRequests", "PermissionLevel_PK_PermissionLevel_Id");
            AddForeignKey("dbo.UserModuleRequests", "PermissionLevel_PK_PermissionLevel_Id", "dbo.PermissionLevels", "PK_PermissionLevel_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModuleRequests", "PermissionLevel_PK_PermissionLevel_Id", "dbo.PermissionLevels");
            DropIndex("dbo.UserModuleRequests", new[] { "PermissionLevel_PK_PermissionLevel_Id" });
            DropColumn("dbo.UserModuleRequests", "PermissionLevel_PK_PermissionLevel_Id");
        }
    }
}
