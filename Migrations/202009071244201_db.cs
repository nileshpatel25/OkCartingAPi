namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.customerpayments",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        customerid = c.String(),
                        jobworkid = c.String(),
                        paymenttype = c.String(),
                        paymentby = c.String(),
                        chequeno = c.String(),
                        paymentdate = c.DateTime(nullable: false),
                        amount = c.Double(nullable: false),
                        remark = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.customers",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        name = c.String(),
                        mobileno = c.String(),
                        othermobileno = c.String(),
                        address = c.String(),
                        address2 = c.String(),
                        landmark = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.drivers",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        vehicleid = c.String(),
                        name = c.String(),
                        mobileno = c.String(),
                        othermobileno = c.String(),
                        address = c.String(),
                        address2 = c.String(),
                        landmark = c.String(),
                        adharcardno = c.String(),
                        licenseno = c.String(),
                        perdaysalary = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.jobworkdetails",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        customerid = c.String(),
                        vehicleid = c.String(),
                        hour = c.Double(nullable: false),
                        perhourrate = c.Double(nullable: false),
                        totalamount = c.Double(nullable: false),
                        workdate = c.DateTime(nullable: false),
                        discrition = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.vehicles",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        vehiclename = c.String(),
                        vehiclenumber = c.String(),
                        perhourrate = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.vehicles");
            DropTable("dbo.jobworkdetails");
            DropTable("dbo.drivers");
            DropTable("dbo.customers");
            DropTable("dbo.customerpayments");
        }
    }
}
