namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CursoUsuario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CursoUsuario",
                c => new
                    {
                        Idcurso = c.Int(nullable: false),
                        Idusuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idcurso, t.Idusuario })
                .ForeignKey("dbo.Curso", t => t.Idcurso)
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .Index(t => t.Idcurso)
                .Index(t => t.Idusuario);
            
            AddColumn("dbo.Usuario", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CursoUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.CursoUsuario", "Idcurso", "dbo.Curso");
            DropIndex("dbo.CursoUsuario", new[] { "Idusuario" });
            DropIndex("dbo.CursoUsuario", new[] { "Idcurso" });
            DropColumn("dbo.Usuario", "Discriminator");
            DropTable("dbo.CursoUsuario");
        }
    }
}
