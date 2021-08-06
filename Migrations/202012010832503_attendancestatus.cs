namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendancestatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.driverattendances", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.driverattendances", "status", c => c.Boolean(nullable: false));
        }
    }
}
