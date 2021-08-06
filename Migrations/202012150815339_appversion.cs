namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appversion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appversions",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        version = c.String(),
                        forceUpdate = c.Boolean(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        publishdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Appversions");
        }
    }
}
