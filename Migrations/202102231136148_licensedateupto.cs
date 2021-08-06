namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class licensedateupto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.drivers", "licensevalidupto", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.drivers", "licensevalidupto", c => c.String());
        }
    }
}
