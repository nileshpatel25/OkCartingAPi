namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expense1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.expensedetails",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        expensetypeid = c.String(),
                        amount = c.Double(nullable: false),
                        chequeno = c.String(),
                        expensedate = c.DateTime(nullable: false),
                        remark = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.expensetypes",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        name = c.String(),
                        remark = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.expensetypes");
            DropTable("dbo.expensedetails");
        }
    }
}
