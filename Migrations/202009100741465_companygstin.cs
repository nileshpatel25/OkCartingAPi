namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companygstin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "gstin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "gstin");
        }
    }
}
