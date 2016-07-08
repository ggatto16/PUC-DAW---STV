namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class file4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Arquivo", "Tipo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "Tipo", c => c.String());
        }
    }
}
