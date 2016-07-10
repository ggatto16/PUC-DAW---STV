namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Arquivo", "Idmaterial");
            RenameColumn(table: "dbo.Arquivo", name: "Idarquivo", newName: "Idmaterial");
            RenameIndex(table: "dbo.Arquivo", name: "IX_Idarquivo", newName: "IX_Idmaterial");
            DropColumn("dbo.Material", "Idarquivo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Material", "Idarquivo", c => c.Int());
            RenameIndex(table: "dbo.Arquivo", name: "IX_Idmaterial", newName: "IX_Idarquivo");
            RenameColumn(table: "dbo.Arquivo", name: "Idmaterial", newName: "Idarquivo");
            AddColumn("dbo.Arquivo", "Idmaterial", c => c.Int(nullable: false));
        }
    }
}
