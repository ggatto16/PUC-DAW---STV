namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idmaterial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false));
            DropColumn("dbo.Arquivo", "Idarquivofsfsfs");
        }
    }
}
