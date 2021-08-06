namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.drivers", "dateofjoining", c => c.DateTime(nullable: false));
            AddColumn("dbo.drivers", "dateofresinging", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.drivers", "active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.drivers", "active");
            DropColumn("dbo.drivers", "dateofresinging");
            DropColumn("dbo.drivers", "dateofjoining");
        }
    }
}
