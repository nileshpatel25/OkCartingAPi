namespace CartingManagmentApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobworkid : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.jobworkdetails", name: "jobwork_id", newName: "jobworkid");
            RenameIndex(table: "dbo.jobworkdetails", name: "IX_jobwork_id", newName: "IX_jobworkid");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.jobworkdetails", name: "IX_jobworkid", newName: "IX_jobwork_id");
            RenameColumn(table: "dbo.jobworkdetails", name: "jobworkid", newName: "jobwork_id");
        }
    }
}
