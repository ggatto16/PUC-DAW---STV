namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Arquivo", name: "Id", newName: "Idmaterial");
            RenameIndex(table: "dbo.Arquivo", name: "IX_Id", newName: "IX_Idmaterial");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Arquivo", name: "IX_Idmaterial", newName: "IX_Id");
            RenameColumn(table: "dbo.Arquivo", name: "Idmaterial", newName: "Id");
        }
    }
}
