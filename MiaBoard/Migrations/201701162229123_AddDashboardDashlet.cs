namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDashboardDashlet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dashboards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dashlets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        DashboardId = c.Int(nullable: false),
                        DataSourceId = c.Int(nullable: false),
                        Sql = c.String(),
                        Styles = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dashboards", t => t.DashboardId, cascadeDelete: true)
                .ForeignKey("dbo.DataSources", t => t.DataSourceId, cascadeDelete: true)
                .Index(t => t.DashboardId)
                .Index(t => t.DataSourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dashlets", "DataSourceId", "dbo.DataSources");
            DropForeignKey("dbo.Dashlets", "DashboardId", "dbo.Dashboards");
            DropIndex("dbo.Dashlets", new[] { "DataSourceId" });
            DropIndex("dbo.Dashlets", new[] { "DashboardId" });
            DropTable("dbo.Dashlets");
            DropTable("dbo.Dashboards");
        }
    }
}
