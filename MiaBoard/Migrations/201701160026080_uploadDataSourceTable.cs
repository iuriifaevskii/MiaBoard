namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadDataSourceTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataSources", "Type", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.DataSources", "ServerName", c => c.String(maxLength: 255));
            AlterColumn("dbo.DataSources", "Username", c => c.String(maxLength: 255));
            AlterColumn("dbo.DataSources", "Password", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataSources", "Password", c => c.String());
            AlterColumn("dbo.DataSources", "Username", c => c.String());
            AlterColumn("dbo.DataSources", "ServerName", c => c.String());
            AlterColumn("dbo.DataSources", "Type", c => c.String());
        }
    }
}
