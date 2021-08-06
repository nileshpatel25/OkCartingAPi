namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class petrolpump : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.petrolpumps",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        name = c.String(),
                        address = c.String(),
                        ownername = c.String(),
                        contactno = c.String(),
                        otherconatcno = c.String(),
                        gst = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.fuelmasters", "paymenttype", c => c.String());
            AddColumn("dbo.fuelmasters", "rate", c => c.Double(nullable: false));
            AddColumn("dbo.fuelmasters", "receipt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.fuelmasters", "receipt");
            DropColumn("dbo.fuelmasters", "rate");
            DropColumn("dbo.fuelmasters", "paymenttype");
            DropTable("dbo.petrolpumps");
        }
    }
}
