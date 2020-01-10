namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        PK_Category_id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_Category_id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        PK_Module_Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(nullable: false),
                        Description = c.String(),
                        AdminIdentifyingName = c.String(nullable: false),
                        ExecutionPath = c.String(nullable: false),
                        IconPath = c.String(nullable: false),
                        LastExecutionDate = c.DateTime(),
                        Order = c.Int(nullable: false),
                        Category_PK_Category_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_Module_Id)
                .ForeignKey("dbo.Categories", t => t.Category_PK_Category_id, cascadeDelete: true)
                .Index(t => t.Category_PK_Category_id);
            
            CreateTable(
                "dbo.PermissionLevels",
                c => new
                    {
                        PK_PermissionLevel_Id = c.Int(nullable: false, identity: true),
                        Access = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PK_PermissionLevel_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        PK_User_Id = c.Int(nullable: false, identity: true),
                        WindowsId = c.String(nullable: false),
                        Username = c.String(),
                        AccountCreationDate = c.DateTime(nullable: false),
                        LastAccessDate = c.DateTime(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PK_User_Id);
            
            CreateTable(
                "dbo.UsersModules",
                c => new
                    {
                        PK_UsersModules_Id = c.Int(nullable: false, identity: true),
                        Module_PK_Module_Id = c.Int(nullable: false),
                        PermissionLevel_PK_PermissionLevel_Id = c.Int(nullable: false),
                        User_PK_User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PK_UsersModules_Id)
                .ForeignKey("dbo.Modules", t => t.Module_PK_Module_Id, cascadeDelete: true)
                .ForeignKey("dbo.PermissionLevels", t => t.PermissionLevel_PK_PermissionLevel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_PK_User_Id, cascadeDelete: true)
                .Index(t => t.Module_PK_Module_Id)
                .Index(t => t.PermissionLevel_PK_PermissionLevel_Id)
                .Index(t => t.User_PK_User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersModules", "User_PK_User_Id", "dbo.Users");
            DropForeignKey("dbo.UsersModules", "PermissionLevel_PK_PermissionLevel_Id", "dbo.PermissionLevels");
            DropForeignKey("dbo.UsersModules", "Module_PK_Module_Id", "dbo.Modules");
            DropForeignKey("dbo.Modules", "Category_PK_Category_id", "dbo.Categories");
            DropIndex("dbo.UsersModules", new[] { "User_PK_User_Id" });
            DropIndex("dbo.UsersModules", new[] { "PermissionLevel_PK_PermissionLevel_Id" });
            DropIndex("dbo.UsersModules", new[] { "Module_PK_Module_Id" });
            DropIndex("dbo.Modules", new[] { "Category_PK_Category_id" });
            DropTable("dbo.UsersModules");
            DropTable("dbo.Users");
            DropTable("dbo.PermissionLevels");
            DropTable("dbo.Modules");
            DropTable("dbo.Categories");
        }
    }
}
