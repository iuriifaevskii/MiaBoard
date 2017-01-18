namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDataSourceModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataSources", "CompanyAdmin_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DataSources", "CompanyAdmin_Id");
            AddForeignKey("dbo.DataSources", "CompanyAdmin_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataSources", "CompanyAdmin_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DataSources", new[] { "CompanyAdmin_Id" });
            DropColumn("dbo.DataSources", "CompanyAdmin_Id");
        }
    }
}
