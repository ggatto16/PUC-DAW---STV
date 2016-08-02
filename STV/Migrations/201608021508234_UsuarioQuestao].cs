namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioQuestao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Respostas",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idquestao = c.Int(nullable: false),
                        Idalternativa = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idquestao })
                .ForeignKey("dbo.Alternativa", t => t.Idalternativa, cascadeDelete: true)
                .ForeignKey("dbo.Questao", t => t.Idquestao)
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .Index(t => t.Idusuario)
                .Index(t => t.Idquestao)
                .Index(t => t.Idalternativa);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Respostas", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Respostas", "Idquestao", "dbo.Questao");
            DropForeignKey("dbo.Respostas", "Idalternativa", "dbo.Alternativa");
            DropIndex("dbo.Respostas", new[] { "Idalternativa" });
            DropIndex("dbo.Respostas", new[] { "Idquestao" });
            DropIndex("dbo.Respostas", new[] { "Idusuario" });
            DropTable("dbo.Respostas");
        }
    }
}
