namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class URLMaterial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Material", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Material", "URL");
        }
    }
}
