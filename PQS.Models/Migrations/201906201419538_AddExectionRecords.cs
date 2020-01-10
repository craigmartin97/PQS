namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExectionRecords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleExecutionRecords",
                c => new
                    {
                        PK_Execution_Id = c.Int(nullable: false, identity: true),
                        ExecutionDate = c.DateTime(nullable: false),
                        Module_PK_Module_Id = c.Int(),
                        User_PK_User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PK_Execution_Id)
                .ForeignKey("dbo.Modules", t => t.Module_PK_Module_Id)
                .ForeignKey("dbo.Users", t => t.User_PK_User_Id)
                .Index(t => t.Module_PK_Module_Id)
                .Index(t => t.User_PK_User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleExecutionRecords", "User_PK_User_Id", "dbo.Users");
            DropForeignKey("dbo.ModuleExecutionRecords", "Module_PK_Module_Id", "dbo.Modules");
            DropIndex("dbo.ModuleExecutionRecords", new[] { "User_PK_User_Id" });
            DropIndex("dbo.ModuleExecutionRecords", new[] { "Module_PK_Module_Id" });
            DropTable("dbo.ModuleExecutionRecords");
        }
    }
}
