namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.jobworkdetails", "driverid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.jobworkdetails", "driverid");
        }
    }
}
