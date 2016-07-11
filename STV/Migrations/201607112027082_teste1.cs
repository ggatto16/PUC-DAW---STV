namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Arquivo");
            AddColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Arquivo", "Idarquivo");
            DropColumn("dbo.Arquivo", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "Content", c => c.Binary());
            DropPrimaryKey("dbo.Arquivo");
            DropColumn("dbo.Arquivo", "Idarquivo");
            AddPrimaryKey("dbo.Arquivo", "Idmaterial");
        }
    }
}
