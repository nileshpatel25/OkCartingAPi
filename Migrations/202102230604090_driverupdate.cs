namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class driverupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.drivers", "licensevalidupto", c => c.String());
            AddColumn("dbo.drivers", "driverimage", c => c.String());
            AddColumn("dbo.drivers", "licenseimage", c => c.String());
            AddColumn("dbo.drivers", "adharcardimage", c => c.String());
            AddColumn("dbo.drivers", "hireon", c => c.String());
            AddColumn("dbo.AspNetUsers", "latitude", c => c.String());
            AddColumn("dbo.AspNetUsers", "logitude", c => c.String());
            AddColumn("dbo.AspNetUsers", "pushTokenId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "pushTokenId");
            DropColumn("dbo.AspNetUsers", "logitude");
            DropColumn("dbo.AspNetUsers", "latitude");
            DropColumn("dbo.drivers", "hireon");
            DropColumn("dbo.drivers", "adharcardimage");
            DropColumn("dbo.drivers", "licenseimage");
            DropColumn("dbo.drivers", "driverimage");
            DropColumn("dbo.drivers", "licensevalidupto");
        }
    }
}
