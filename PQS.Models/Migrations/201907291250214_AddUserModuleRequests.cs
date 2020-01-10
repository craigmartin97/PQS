namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserModuleRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserModuleRequests",
                c => new
                    {
                        PK_UserModuleRequest_Id = c.Int(nullable: false, identity: true),
                        RequestDate = c.DateTime(),
                        RequestCompletionDate = c.DateTime(),
                        IsComplete = c.Boolean(),
                        RequestAccepted = c.Boolean(),
                        Module_PK_Module_Id = c.Int(),
                        User_PK_User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PK_UserModuleRequest_Id)
                .ForeignKey("dbo.Modules", t => t.Module_PK_Module_Id)
                .ForeignKey("dbo.Users", t => t.User_PK_User_Id)
                .Index(t => t.Module_PK_Module_Id)
                .Index(t => t.User_PK_User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModuleRequests", "User_PK_User_Id", "dbo.Users");
            DropForeignKey("dbo.UserModuleRequests", "Module_PK_Module_Id", "dbo.Modules");
            DropIndex("dbo.UserModuleRequests", new[] { "User_PK_User_Id" });
            DropIndex("dbo.UserModuleRequests", new[] { "Module_PK_Module_Id" });
            DropTable("dbo.UserModuleRequests");
        }
    }
}
