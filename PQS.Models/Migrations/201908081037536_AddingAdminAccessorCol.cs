namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAdminAccessorCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersModules", "AdminAccessor_PK_User_Id", c => c.Int());
            CreateIndex("dbo.UsersModules", "AdminAccessor_PK_User_Id");
            AddForeignKey("dbo.UsersModules", "AdminAccessor_PK_User_Id", "dbo.Users", "PK_User_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersModules", "AdminAccessor_PK_User_Id", "dbo.Users");
            DropIndex("dbo.UsersModules", new[] { "AdminAccessor_PK_User_Id" });
            DropColumn("dbo.UsersModules", "AdminAccessor_PK_User_Id");
        }
    }
}
