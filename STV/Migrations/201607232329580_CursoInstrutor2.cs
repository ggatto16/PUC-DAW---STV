namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CursoInstrutor2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Curso", name: "Idusuario", newName: "IdusuarioInstrutor");
            RenameIndex(table: "dbo.Curso", name: "IX_Idusuario", newName: "IX_IdusuarioInstrutor");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Curso", name: "IX_IdusuarioInstrutor", newName: "IX_Idusuario");
            RenameColumn(table: "dbo.Curso", name: "IdusuarioInstrutor", newName: "Idusuario");
        }
    }
}
