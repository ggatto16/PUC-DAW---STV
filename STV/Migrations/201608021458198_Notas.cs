namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idatividade = c.Int(nullable: false),
                        Pontos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idatividade })
                .ForeignKey("dbo.Atividade", t => t.Idatividade, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.Idusuario, cascadeDelete: true)
                .Index(t => t.Idusuario)
                .Index(t => t.Idatividade);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notas", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Notas", "Idatividade", "dbo.Atividade");
            DropIndex("dbo.Notas", new[] { "Idatividade" });
            DropIndex("dbo.Notas", new[] { "Idusuario" });
            DropTable("dbo.Notas");
        }
    }
}
