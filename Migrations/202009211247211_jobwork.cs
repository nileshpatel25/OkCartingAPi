namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobwork : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.jobworks",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        customerid = c.String(),
                        paymenttype = c.String(),
                        createAt = c.DateTime(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.jobworkdetails", "jobwork_id", c => c.String(maxLength: 128));
            CreateIndex("dbo.jobworkdetails", "jobwork_id");
            AddForeignKey("dbo.jobworkdetails", "jobwork_id", "dbo.jobworks", "id");
            DropColumn("dbo.jobworkdetails", "userid");
            DropColumn("dbo.jobworkdetails", "customerid");
            DropColumn("dbo.jobworkdetails", "paymenttype");
        }
        
        public override void Down()
        {
            AddColumn("dbo.jobworkdetails", "paymenttype", c => c.String());
            AddColumn("dbo.jobworkdetails", "customerid", c => c.String());
            AddColumn("dbo.jobworkdetails", "userid", c => c.String());
            DropForeignKey("dbo.jobworkdetails", "jobwork_id", "dbo.jobworks");
            DropIndex("dbo.jobworkdetails", new[] { "jobwork_id" });
            DropColumn("dbo.jobworkdetails", "jobwork_id");
            DropTable("dbo.jobworks");
        }
    }
}
