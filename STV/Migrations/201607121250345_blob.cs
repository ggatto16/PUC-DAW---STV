namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "File", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Arquivo", "File");
        }
    }
}
