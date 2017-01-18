namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldInDataSource : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DataSources", "CompanyAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DataSources", new[] { "CompanyAdmin_Id" });
            AddColumn("dbo.DataSources", "DatabaseName", c => c.String(maxLength: 255));
            DropColumn("dbo.DataSources", "CompanyId");
            DropColumn("dbo.DataSources", "CompanyAdmin_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataSources", "CompanyAdmin_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.DataSources", "CompanyId", c => c.Int(nullable: false));
            DropColumn("dbo.DataSources", "DatabaseName");
            CreateIndex("dbo.DataSources", "CompanyAdmin_Id");
            AddForeignKey("dbo.DataSources", "CompanyAdmin_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
