namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.driveradvancesalaries",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        driverid = c.String(),
                        advancesalaryamt = c.Double(nullable: false),
                        advancesalarymonth = c.String(),
                        advancesalaryyear = c.String(),
                        advancesalarydate = c.DateTime(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.driverattendances",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        driverid = c.String(),
                        intime = c.DateTime(nullable: false),
                        outtime = c.DateTime(nullable: false),
                        status = c.Boolean(nullable: false),
                        dtattencanceDate = c.DateTime(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.driverattendances");
            DropTable("dbo.driveradvancesalaries");
        }
    }
}
