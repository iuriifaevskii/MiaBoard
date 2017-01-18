namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDataSourceModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        ServerName = c.String(),
                        CompanyId = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        ConnectionString = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataSources");
        }
    }
}
