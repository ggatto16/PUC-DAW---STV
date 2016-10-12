namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class i650 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.Curso", "Idusuario", "dbo.Usuario");
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            RenameColumn(table: "dbo.Curso", name: "Idusuario", newName: "IdusuarioInstrutor");
            RenameIndex(table: "dbo.Curso", name: "IX_Idusuario", newName: "IX_IdusuarioInstrutor");
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
            
            AddColumn("dbo.Alternativa", "Justificativa", c => c.String(maxLength: 1000));
            AddColumn("dbo.Questao", "Numero", c => c.Int());
            AddColumn("dbo.Atividade", "DataAbertura", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Atividade", "DataEncerramento", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Unidade", "Encerrada", c => c.Boolean(nullable: false));
            AddColumn("dbo.Curso", "DataInicial", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Curso", "Encerrado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Material", "URL", c => c.String(maxLength: 300));
            AddColumn("dbo.Arquivo", "Tamanho", c => c.Int());
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Questao", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Unidade", "Titulo", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int());
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Material", "Descricao", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Arquivo", "Nome", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento");
            AddForeignKey("dbo.Curso", "IdusuarioInstrutor", "dbo.Usuario", "Idusuario");
            DropColumn("dbo.Unidade", "Dtabertura");
            DropColumn("dbo.Unidade", "Status");
            DropColumn("dbo.Curso", "Dtinicial");
            DropColumn("dbo.Curso", "Dtfinal");
            DropColumn("dbo.Departamento", "Status");
            DropColumn("dbo.Usuario", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuario", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departamento", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Curso", "Dtfinal", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Curso", "Dtinicial", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Unidade", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Unidade", "Dtabertura", c => c.DateTime(storeType: "date"));
            DropForeignKey("dbo.Curso", "IdusuarioInstrutor", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropForeignKey("dbo.Nota", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioRole", "Idrole", "dbo.Role");
            DropForeignKey("dbo.UsuarioRole", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Resposta", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.Resposta", "Idquestao", "dbo.Questao");
            DropForeignKey("dbo.Resposta", "Idalternativa", "dbo.Alternativa");
            DropForeignKey("dbo.UsuarioMedalha", "Idmedalha", "dbo.Medalha");
            DropForeignKey("dbo.UsuarioMedalha", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.CursoUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.CursoUsuario", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.MaterialUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.MaterialUsuario", "Idmaterial", "dbo.Material");
            DropForeignKey("dbo.NotaCurso", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.NotaCurso", "Idcurso", "dbo.Curso");
            DropForeignKey("dbo.Nota", "Idatividade", "dbo.Atividade");
            DropIndex("dbo.UsuarioRole", new[] { "Idrole" });
            DropIndex("dbo.UsuarioRole", new[] { "Idusuario" });
            DropIndex("dbo.UsuarioMedalha", new[] { "Idmedalha" });
            DropIndex("dbo.UsuarioMedalha", new[] { "Idusuario" });
            DropIndex("dbo.CursoUsuario", new[] { "Idusuario" });
            DropIndex("dbo.CursoUsuario", new[] { "Idcurso" });
            DropIndex("dbo.MaterialUsuario", new[] { "Idusuario" });
            DropIndex("dbo.MaterialUsuario", new[] { "Idmaterial" });
            DropIndex("dbo.Resposta", new[] { "Idalternativa" });
            DropIndex("dbo.Resposta", new[] { "Idquestao" });
            DropIndex("dbo.Resposta", new[] { "Idusuario" });
            DropIndex("dbo.NotaCurso", new[] { "Idcurso" });
            DropIndex("dbo.NotaCurso", new[] { "Idusuario" });
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            DropIndex("dbo.Nota", new[] { "Idatividade" });
            DropIndex("dbo.Nota", new[] { "Idusuario" });
            AlterColumn("dbo.Arquivo", "Nome", c => c.String());
            AlterColumn("dbo.Material", "Descricao", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int(nullable: false));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(maxLength: 20));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(maxLength: 60));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(maxLength: 15));
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime());
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(maxLength: 50));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(maxLength: 120));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(maxLength: 30));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(maxLength: 60));
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidade", "Titulo", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String());
            AlterColumn("dbo.Questao", "Descricao", c => c.String());
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String());
            DropColumn("dbo.Arquivo", "Tamanho");
            DropColumn("dbo.Material", "URL");
            DropColumn("dbo.Curso", "Encerrado");
            DropColumn("dbo.Curso", "DataInicial");
            DropColumn("dbo.Unidade", "Encerrada");
            DropColumn("dbo.Unidade", "DataAbertura");
            DropColumn("dbo.Atividade", "DataEncerramento");
            DropColumn("dbo.Atividade", "DataAbertura");
            DropColumn("dbo.Questao", "Numero");
            DropColumn("dbo.Alternativa", "Justificativa");
            DropTable("dbo.UsuarioRole");
            DropTable("dbo.UsuarioMedalha");
            DropTable("dbo.CursoUsuario");
            DropTable("dbo.MaterialUsuario");
            DropTable("dbo.Role");
            DropTable("dbo.Resposta");
            DropTable("dbo.Medalha");
            DropTable("dbo.NotaCurso");
            DropTable("dbo.Nota");
            RenameIndex(table: "dbo.Curso", name: "IX_IdusuarioInstrutor", newName: "IX_Idusuario");
            RenameColumn(table: "dbo.Curso", name: "IdusuarioInstrutor", newName: "Idusuario");
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Curso", "Idusuario", "dbo.Usuario", "Idusuario", cascadeDelete: true);
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento", cascadeDelete: true);
        }
    }
}
