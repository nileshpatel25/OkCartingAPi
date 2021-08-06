namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoiceno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.jobworks", "invoiceno", c => c.Int(nullable: false));
            AddColumn("dbo.jobworks", "status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.jobworks", "status");
            DropColumn("dbo.jobworks", "invoiceno");
        }
    }
}
