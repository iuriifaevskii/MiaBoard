namespace MiaBoard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedApplicationUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.AspNetUsers", "MidleName", c => c.String(maxLength: 255));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false, defaultValueSql: "1"));
            AddColumn("dbo.AspNetUsers", "DateRegistration", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateHired", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "ContactNo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ContactNo");
            DropColumn("dbo.AspNetUsers", "DateHired");
            DropColumn("dbo.AspNetUsers", "DateRegistration");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "MidleName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
