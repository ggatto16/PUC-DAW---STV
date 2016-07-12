namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blob1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Arquivo");
            DropIndex("dbo.Arquivo", new[] { "Idmaterial" });
            DropColumn("dbo.Arquivo", "Idarquivo");
            RenameColumn(table: "dbo.Arquivo", name: "Idmaterial", newName: "Idarquivo");
            
            AddColumn("dbo.Arquivo", "Blob", c => c.Binary());
            AlterColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Arquivo", "Idarquivo");
            CreateIndex("dbo.Arquivo", "Idarquivo");
            DropColumn("dbo.Arquivo", "File");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "File", c => c.Binary());
            DropIndex("dbo.Arquivo", new[] { "Idarquivo" });
            DropPrimaryKey("dbo.Arquivo");
            AlterColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Arquivo", "Blob");
            AddPrimaryKey("dbo.Arquivo", "Idarquivo");
            RenameColumn(table: "dbo.Arquivo", name: "Idarquivo", newName: "Idmaterial");
            AddColumn("dbo.Arquivo", "Idarquivo", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Arquivo", "Idmaterial");
        }
    }
}
