namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsFavourite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersModules", "IsFavourite", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersModules", "IsFavourite");
        }
    }
}
