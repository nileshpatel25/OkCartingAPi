namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.driverattendances", "outtime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.driverattendances", "dtattencanceDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.driverattendances", "dtattencanceDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.driverattendances", "outtime", c => c.DateTime(nullable: false));
        }
    }
}
