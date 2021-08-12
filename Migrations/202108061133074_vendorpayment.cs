namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorpayment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.vendorpayments",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        vendorid = c.String(),
                        purchaseid = c.String(),
                        paymenttype = c.String(),
                        paymentby = c.String(),
                        chequeno = c.String(),
                        paymentdate = c.DateTime(nullable: false),
                        createAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        amount = c.Double(nullable: false),
                        remark = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.vendorpayments");
        }
    }
}
