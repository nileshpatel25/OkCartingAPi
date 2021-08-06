namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.customerpayments", "createAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.customers", "createAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.drivers", "createAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.jobworkdetails", "createAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.vehicles", "createAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.drivers", "dateofjoining", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.drivers", "dateofjoining", c => c.DateTime(nullable: false));
            DropColumn("dbo.vehicles", "createAt");
            DropColumn("dbo.jobworkdetails", "createAt");
            DropColumn("dbo.drivers", "createAt");
            DropColumn("dbo.customers", "createAt");
            DropColumn("dbo.customerpayments", "createAt");
        }
    }
}
