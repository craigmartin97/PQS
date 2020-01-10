namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingParametersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleParameters",
                c => new
                    {
                        PK_ModuleParameter_Id = c.Int(nullable: false, identity: true),
                        Parameter = c.String(),
                        Order = c.Int(nullable: false),
                        Module_PK_Module_Id = c.Int(),
                    })
                .PrimaryKey(t => t.PK_ModuleParameter_Id)
                .ForeignKey("dbo.Modules", t => t.Module_PK_Module_Id)
                .Index(t => t.Module_PK_Module_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleParameters", "Module_PK_Module_Id", "dbo.Modules");
            DropIndex("dbo.ModuleParameters", new[] { "Module_PK_Module_Id" });
            DropTable("dbo.ModuleParameters");
        }
    }
}
