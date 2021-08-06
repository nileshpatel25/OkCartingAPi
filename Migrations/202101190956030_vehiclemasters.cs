namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vehiclemasters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.vehiclemasters",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        vehiclename = c.String(),
                        createAt = c.DateTime(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.drivers", "fulldayhour", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.drivers", "fulldayhour");
            DropTable("dbo.vehiclemasters");
        }
    }
}
