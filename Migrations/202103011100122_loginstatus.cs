namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loginstatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoginStatus",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        registrationdate = c.DateTime(nullable: false),
                        subscriptionstartdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        subscriptionenddate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.driverattendances", "hour", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.driverattendances", "hour");
            DropTable("dbo.LoginStatus");
        }
    }
}
