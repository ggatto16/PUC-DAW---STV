namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotasCurso : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Notas", newName: "Nota");
            RenameTable(name: "dbo.Respostas", newName: "Resposta");
            CreateTable(
                "dbo.NotaCurso",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idcurso = c.Int(nullable: false),
                        Pontos = c.Int(nullable: false),
                        Comentario = c.String(),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idcurso })
                .ForeignKey("dbo.Curso", t => t.Idcurso, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.Idusuario, cascadeDelete: true)
                .Index(t => t.Idusuario)
                .Index(t => t.Idcurso);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaCurso", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.NotaCurso", "Idcurso", "dbo.Curso");
            DropIndex("dbo.NotaCurso", new[] { "Idcurso" });
            DropIndex("dbo.NotaCurso", new[] { "Idusuario" });
            DropTable("dbo.NotaCurso");
            RenameTable(name: "dbo.Resposta", newName: "Respostas");
            RenameTable(name: "dbo.Nota", newName: "Notas");
        }
    }
}
