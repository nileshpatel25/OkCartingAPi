namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class petrolpumpid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.fuelmasters", "petrolpumpid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.fuelmasters", "petrolpumpid");
        }
    }
}
