namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToObjectsExeRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", c => c.Int());
            AddColumn("dbo.ModuleExecutionRecords", "User_PK_User_Id", c => c.Int());
            CreateIndex("dbo.ModuleExecutionRecords", "Module_PK_Module_Id");
            CreateIndex("dbo.ModuleExecutionRecords", "User_PK_User_Id");
            AddForeignKey("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", "dbo.Modules", "PK_Module_Id");
            AddForeignKey("dbo.ModuleExecutionRecords", "User_PK_User_Id", "dbo.Users", "PK_User_Id");
            DropColumn("dbo.ModuleExecutionRecords", "DisplayName");
            DropColumn("dbo.ModuleExecutionRecords", "AdminIdentifyingName");
            DropColumn("dbo.ModuleExecutionRecords", "WindowsId");
            DropColumn("dbo.ModuleExecutionRecords", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ModuleExecutionRecords", "Username", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "WindowsId", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "AdminIdentifyingName", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "DisplayName", c => c.String());
            DropForeignKey("dbo.ModuleExecutionRecords", "User_PK_User_Id", "dbo.Users");
            DropForeignKey("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", "dbo.Modules");
            DropIndex("dbo.ModuleExecutionRecords", new[] { "User_PK_User_Id" });
            DropIndex("dbo.ModuleExecutionRecords", new[] { "Module_PK_Module_Id" });
            DropColumn("dbo.ModuleExecutionRecords", "User_PK_User_Id");
            DropColumn("dbo.ModuleExecutionRecords", "Module_PK_Module_Id");
        }
    }
}
