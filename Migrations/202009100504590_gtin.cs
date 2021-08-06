namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gtin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.customers", "gstin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.customers", "gstin");
        }
    }
}
