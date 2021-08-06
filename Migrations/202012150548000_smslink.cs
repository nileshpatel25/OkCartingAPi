namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smslink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SMSLinks",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        code = c.String(),
                        link = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SMSLinks");
        }
    }
}
