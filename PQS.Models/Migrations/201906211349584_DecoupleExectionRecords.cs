namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecoupleExectionRecords : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", "dbo.Modules");
            DropForeignKey("dbo.ModuleExecutionRecords", "User_PK_User_Id", "dbo.Users");
            DropIndex("dbo.ModuleExecutionRecords", new[] { "Module_PK_Module_Id" });
            DropIndex("dbo.ModuleExecutionRecords", new[] { "User_PK_User_Id" });
            AddColumn("dbo.ModuleExecutionRecords", "DisplayName", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "AdminIdentifyingName", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "WindowsId", c => c.String());
            AddColumn("dbo.ModuleExecutionRecords", "Username", c => c.String());
            DropColumn("dbo.ModuleExecutionRecords", "Module_PK_Module_Id");
            DropColumn("dbo.ModuleExecutionRecords", "User_PK_User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ModuleExecutionRecords", "User_PK_User_Id", c => c.Int());
            AddColumn("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", c => c.Int());
            DropColumn("dbo.ModuleExecutionRecords", "Username");
            DropColumn("dbo.ModuleExecutionRecords", "WindowsId");
            DropColumn("dbo.ModuleExecutionRecords", "AdminIdentifyingName");
            DropColumn("dbo.ModuleExecutionRecords", "DisplayName");
            CreateIndex("dbo.ModuleExecutionRecords", "User_PK_User_Id");
            CreateIndex("dbo.ModuleExecutionRecords", "Module_PK_Module_Id");
            AddForeignKey("dbo.ModuleExecutionRecords", "User_PK_User_Id", "dbo.Users", "PK_User_Id");
            AddForeignKey("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", "dbo.Modules", "PK_Module_Id");
        }
    }
}
