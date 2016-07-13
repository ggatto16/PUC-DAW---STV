namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alternativa",
                c => new
                    {
                        Idalternativa = c.Int(nullable: false, identity: true),
                        Idquestao = c.Int(nullable: false),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Idalternativa)
                .ForeignKey("dbo.Questao", t => t.Idquestao, cascadeDelete: true)
                .Index(t => t.Idquestao);
            
            CreateTable(
                "dbo.Questao",
                c => new
                    {
                        Idquestao = c.Int(nullable: false, identity: true),
                        Idatividade = c.Int(nullable: false),
                        IdalternativaCorreta = c.Int(),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Idquestao)
                .ForeignKey("dbo.Alternativa", t => t.IdalternativaCorreta)
                .ForeignKey("dbo.Atividade", t => t.Idatividade, cascadeDelete: true)
                .Index(t => t.Idatividade)
                .Index(t => t.IdalternativaCorreta);
            
            CreateTable(
                "dbo.Atividade",
                c => new
                    {
                        Idatividade = c.Int(nullable: false, identity: true),
                        Idunidade = c.Int(nullable: false),
                        Descricao = c.String(),
                        Valor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Idatividade)
                .ForeignKey("dbo.Unidade", t => t.Idunidade, cascadeDelete: true)
                .Index(t => t.Idunidade);
            
            CreateTable(
                "dbo.Unidade",
                c => new
                    {
                        Idunidade = c.Int(nullable: false, identity: true),
                        Idcurso = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 60),
                        Dtabertura = c.DateTime(storeType: "date"),
                        Status = c.Boolean(nullable: false),
                        Stamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Idunidade)
                .ForeignKey("dbo.Curso", t => t.Idcurso, cascadeDelete: true)
                .Index(t => t.Idcurso);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        Idcurso = c.Int(nullable: false, identity: true),
                        Titulo = c.String(maxLength: 60),
                        Dtinicial = c.DateTime(storeType: "date"),
                        Dtfinal = c.DateTime(storeType: "date"),
                        Idusuario = c.Int(nullable: false),
                        Categoria = c.String(maxLength: 30),
                        Palavraschave = c.String(maxLength: 120),
                        Stamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Idcurso)
                .ForeignKey("dbo.Usuario", t => t.Idusuario, cascadeDelete: true)
                .Index(t => t.Idusuario);
            
            CreateTable(
                "dbo.Departamento",
                c => new
                    {
                        Iddepartamento = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 50),
                        Status = c.Boolean(nullable: false),
                        Stamp = c.DateTime(),
                    })
                .PrimaryKey(t => t.Iddepartamento);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Idusuario = c.Int(nullable: false, identity: true),
                        Cpf = c.String(maxLength: 15),
                        Nome = c.String(maxLength: 60),
                        Email = c.String(maxLength: 70),
                        Senha = c.String(maxLength: 20),
                        Iddepartamento = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Stamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Idusuario)
                .ForeignKey("dbo.Departamento", t => t.Iddepartamento, cascadeDelete: true)
                .Index(t => t.Iddepartamento);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        Idmaterial = c.Int(nullable: false, identity: true),
                        Idunidade = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40),
                        Tipo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Idmaterial)
                .ForeignKey("dbo.Unidade", t => t.Idunidade, cascadeDelete: true)
                .Index(t => t.Idunidade);
            
            CreateTable(
                "dbo.Arquivo",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Blob = c.Binary(),
                        Nomearquivo = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Material", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.CursoDepartamento",
                c => new
                    {
                        Idcurso = c.Int(nullable: false),
                        Iddepartamento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idcurso, t.Iddepartamento })
                .ForeignKey("dbo.Curso", t => t.Idcurso)
                .ForeignKey("dbo.Departamento", t => t.Iddepartamento)
                .Index(t => t.Idcurso)
                .Index(t => t.Iddepartamento);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Arquivo", "Id", "dbo.Material");
            DropForeignKey("dbo.Alternativa", "Idquestao", "dbo.Questao");
            DropForeignKey("dbo.Questao", "Idatividade", "dbo.Atividade");
            DropForeignKey("dbo.Atividade", "Idunidade", "dbo.Unidade");
            DropForeignKey("dbo.Material", "Idunidade", "dbo.Unidade");
            DropForeignKey("dbo.Unidade", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.Curso", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.CursoDepartamento", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.CursoDepartamento", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.Questao", "IdalternativaCorreta", "dbo.Alternativa");
            DropIndex("dbo.CursoDepartamento", new[] { "Iddepartamento" });
            DropIndex("dbo.CursoDepartamento", new[] { "Idcurso" });
            DropIndex("dbo.Arquivo", new[] { "Id" });
            DropIndex("dbo.Material", new[] { "Idunidade" });
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            DropIndex("dbo.Curso", new[] { "Idusuario" });
            DropIndex("dbo.Unidade", new[] { "Idcurso" });
            DropIndex("dbo.Atividade", new[] { "Idunidade" });
            DropIndex("dbo.Questao", new[] { "IdalternativaCorreta" });
            DropIndex("dbo.Questao", new[] { "Idatividade" });
            DropIndex("dbo.Alternativa", new[] { "Idquestao" });
            DropTable("dbo.CursoDepartamento");
            DropTable("dbo.Arquivo");
            DropTable("dbo.Material");
            DropTable("dbo.Usuario");
            DropTable("dbo.Departamento");
            DropTable("dbo.Curso");
            DropTable("dbo.Unidade");
            DropTable("dbo.Atividade");
            DropTable("dbo.Questao");
            DropTable("dbo.Alternativa");
        }
    }
}
