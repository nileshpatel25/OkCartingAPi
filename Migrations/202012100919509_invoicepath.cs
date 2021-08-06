namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoicepath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.jobworks", "invoicepath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.jobworks", "invoicepath");
        }
    }
}
