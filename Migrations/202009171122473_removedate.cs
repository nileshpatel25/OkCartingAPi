namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.driverattendances", "dtattencanceDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.driverattendances", "intime");
            DropColumn("dbo.driverattendances", "outtime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.driverattendances", "outtime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.driverattendances", "intime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.driverattendances", "dtattencanceDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
