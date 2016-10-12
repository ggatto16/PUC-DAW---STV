namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class i650 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alternativa",
                c => new
                    {
                        Idalternativa = c.Int(nullable: false, identity: true),
                        Idquestao = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 1000),
                        Justificativa = c.String(maxLength: 1000),
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
                        Descricao = c.String(nullable: false, maxLength: 1000),
                        Numero = c.Int(),
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
                        Descricao = c.String(nullable: false, maxLength: 200),
                        Valor = c.Int(nullable: false),
                        DataAbertura = c.DateTime(nullable: false, storeType: "date"),
                        DataEncerramento = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Idatividade)
                .ForeignKey("dbo.Unidade", t => t.Idunidade, cascadeDelete: true)
                .Index(t => t.Idunidade);
            
            CreateTable(
                "dbo.Nota",
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
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Idusuario = c.Int(nullable: false, identity: true),
                        Cpf = c.String(nullable: false, maxLength: 15),
                        Nome = c.String(nullable: false, maxLength: 60),
                        Email = c.String(maxLength: 70),
                        Senha = c.String(nullable: false, maxLength: 500),
                        Iddepartamento = c.Int(),
                        Stamp = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Idusuario)
                .ForeignKey("dbo.Departamento", t => t.Iddepartamento)
                .Index(t => t.Cpf, unique: true)
                .Index(t => t.Iddepartamento);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        Idcurso = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 80),
                        DataInicial = c.DateTime(storeType: "date"),
                        Encerrado = c.Boolean(nullable: false),
                        IdusuarioInstrutor = c.Int(nullable: false),
                        Categoria = c.String(nullable: false, maxLength: 100),
                        Palavraschave = c.String(nullable: false, maxLength: 150),
                        Stamp = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Idcurso)
                .ForeignKey("dbo.Usuario", t => t.IdusuarioInstrutor)
                .Index(t => t.IdusuarioInstrutor);
            
            CreateTable(
                "dbo.Departamento",
                c => new
                    {
                        Iddepartamento = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100),
                        Stamp = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Iddepartamento);
            
            CreateTable(
                "dbo.NotaCurso",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idcurso = c.Int(nullable: false),
                        Pontos = c.Int(nullable: false),
                        Comentario = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idcurso })
                .ForeignKey("dbo.Curso", t => t.Idcurso, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.Idusuario, cascadeDelete: true)
                .Index(t => t.Idusuario)
                .Index(t => t.Idcurso);
            
            CreateTable(
                "dbo.Unidade",
                c => new
                    {
                        Idunidade = c.Int(nullable: false, identity: true),
                        Idcurso = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        DataAbertura = c.DateTime(storeType: "date"),
                        Encerrada = c.Boolean(nullable: false),
                        Stamp = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Idunidade)
                .ForeignKey("dbo.Curso", t => t.Idcurso, cascadeDelete: true)
                .Index(t => t.Idcurso);
            
            CreateTable(
                "dbo.Material",
                c => new
                    {
                        Idmaterial = c.Int(nullable: false, identity: true),
                        Idunidade = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 70),
                        Tipo = c.Int(nullable: false),
                        URL = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Idmaterial)
                .ForeignKey("dbo.Unidade", t => t.Idunidade, cascadeDelete: true)
                .Index(t => t.Idunidade);
            
            CreateTable(
                "dbo.Arquivo",
                c => new
                    {
                        Idmaterial = c.Int(nullable: false),
                        Blob = c.Binary(),
                        Nome = c.String(nullable: false, maxLength: 200),
                        ContentType = c.String(),
                        Tamanho = c.Int(),
                    })
                .PrimaryKey(t => t.Idmaterial)
                .ForeignKey("dbo.Material", t => t.Idmaterial, cascadeDelete: true)
                .Index(t => t.Idmaterial);
            
            CreateTable(
                "dbo.Medalha",
                c => new
                    {
                        Idmedalha = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.Idmedalha);
            
            CreateTable(
                "dbo.Resposta",
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
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Idrole = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Idrole);
            
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
            
            CreateTable(
                "dbo.MaterialUsuario",
                c => new
                    {
                        Idmaterial = c.Int(nullable: false),
                        Idusuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idmaterial, t.Idusuario })
                .ForeignKey("dbo.Material", t => t.Idmaterial)
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .Index(t => t.Idmaterial)
                .Index(t => t.Idusuario);
            
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
            
            CreateTable(
                "dbo.UsuarioRole",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idrole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idrole })
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .ForeignKey("dbo.Role", t => t.Idrole)
                .Index(t => t.Idusuario)
                .Index(t => t.Idrole);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alternativa", "Idquestao", "dbo.Questao");
            DropForeignKey("dbo.Questao", "Idatividade", "dbo.Atividade");
            DropForeignKey("dbo.Atividade", "Idunidade", "dbo.Unidade");
            DropForeignKey("dbo.Nota", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioRole", "Idrole", "dbo.Role");
            DropForeignKey("dbo.UsuarioRole", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Resposta", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Resposta", "Idquestao", "dbo.Questao");
            DropForeignKey("dbo.Resposta", "Idalternativa", "dbo.Alternativa");
            DropForeignKey("dbo.UsuarioMedalha", "Idmedalha", "dbo.Medalha");
            DropForeignKey("dbo.UsuarioMedalha", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.CursoUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.CursoUsuario", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.MaterialUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.MaterialUsuario", "Idmaterial", "dbo.Material");
            DropForeignKey("dbo.Material", "Idunidade", "dbo.Unidade");
            DropForeignKey("dbo.Arquivo", "Idmaterial", "dbo.Material");
            DropForeignKey("dbo.Unidade", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.NotaCurso", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.NotaCurso", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.Curso", "IdusuarioInstrutor", "dbo.Usuario");
            DropForeignKey("dbo.CursoDepartamento", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.CursoDepartamento", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.Nota", "Idatividade", "dbo.Atividade");
            DropForeignKey("dbo.Questao", "IdalternativaCorreta", "dbo.Alternativa");
            DropIndex("dbo.UsuarioRole", new[] { "Idrole" });
            DropIndex("dbo.UsuarioRole", new[] { "Idusuario" });
            DropIndex("dbo.UsuarioMedalha", new[] { "Idmedalha" });
            DropIndex("dbo.UsuarioMedalha", new[] { "Idusuario" });
            DropIndex("dbo.CursoUsuario", new[] { "Idusuario" });
            DropIndex("dbo.CursoUsuario", new[] { "Idcurso" });
            DropIndex("dbo.MaterialUsuario", new[] { "Idusuario" });
            DropIndex("dbo.MaterialUsuario", new[] { "Idmaterial" });
            DropIndex("dbo.CursoDepartamento", new[] { "Iddepartamento" });
            DropIndex("dbo.CursoDepartamento", new[] { "Idcurso" });
            DropIndex("dbo.Resposta", new[] { "Idalternativa" });
            DropIndex("dbo.Resposta", new[] { "Idquestao" });
            DropIndex("dbo.Resposta", new[] { "Idusuario" });
            DropIndex("dbo.Arquivo", new[] { "Idmaterial" });
            DropIndex("dbo.Material", new[] { "Idunidade" });
            DropIndex("dbo.Unidade", new[] { "Idcurso" });
            DropIndex("dbo.NotaCurso", new[] { "Idcurso" });
            DropIndex("dbo.NotaCurso", new[] { "Idusuario" });
            DropIndex("dbo.Curso", new[] { "IdusuarioInstrutor" });
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            DropIndex("dbo.Usuario", new[] { "Cpf" });
            DropIndex("dbo.Nota", new[] { "Idatividade" });
            DropIndex("dbo.Nota", new[] { "Idusuario" });
            DropIndex("dbo.Atividade", new[] { "Idunidade" });
            DropIndex("dbo.Questao", new[] { "IdalternativaCorreta" });
            DropIndex("dbo.Questao", new[] { "Idatividade" });
            DropIndex("dbo.Alternativa", new[] { "Idquestao" });
            DropTable("dbo.UsuarioRole");
            DropTable("dbo.UsuarioMedalha");
            DropTable("dbo.CursoUsuario");
            DropTable("dbo.MaterialUsuario");
            DropTable("dbo.CursoDepartamento");
            DropTable("dbo.Role");
            DropTable("dbo.Resposta");
            DropTable("dbo.Medalha");
            DropTable("dbo.Arquivo");
            DropTable("dbo.Material");
            DropTable("dbo.Unidade");
            DropTable("dbo.NotaCurso");
            DropTable("dbo.Departamento");
            DropTable("dbo.Curso");
            DropTable("dbo.Usuario");
            DropTable("dbo.Nota");
            DropTable("dbo.Atividade");
            DropTable("dbo.Questao");
            DropTable("dbo.Alternativa");
        }
    }
}
