namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioMedalha : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Medalha",
                c => new
                    {
                        Idmedalha = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Idmedalha);
            
            CreateTable(
                "dbo.UsuarioMedalha",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idmedalha = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idmedalha })
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .ForeignKey("dbo.Medalha", t => t.Idmedalha)
                .Index(t => t.Idusuario)
                .Index(t => t.Idmedalha);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioMedalha", "Idmedalha", "dbo.Medalha");
            DropForeignKey("dbo.UsuarioMedalha", "Idusuario", "dbo.Usuario");
            DropIndex("dbo.UsuarioMedalha", new[] { "Idmedalha" });
            DropIndex("dbo.UsuarioMedalha", new[] { "Idusuario" });
            DropTable("dbo.UsuarioMedalha");
            DropTable("dbo.Medalha");
        }
    }
}
