namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuelmaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.fuelmasters",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        vehicleid = c.String(),
                        driverid = c.String(),
                        liter = c.Double(nullable: false),
                        totalamount = c.Double(nullable: false),
                        fueldate = c.DateTime(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.fuelmasters");
        }
    }
}
