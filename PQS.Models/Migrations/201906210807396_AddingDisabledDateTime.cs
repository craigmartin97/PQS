namespace PQS.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingDisabledDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "DisabledDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "DisabledDate");
        }
    }
}
