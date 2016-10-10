namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracoes_09_10_2016 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            AddColumn("dbo.Atividade", "DataAbertura", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Atividade", "DataEncerramento", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Alternativa", "Justificativa", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Questao", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int());
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.NotaCurso", "Comentario", c => c.String(maxLength: 500));
            AlterColumn("dbo.Unidade", "Titulo", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Material", "Descricao", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Material", "URL", c => c.String(maxLength: 300));
            AlterColumn("dbo.Arquivo", "Nome", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento");
            DropColumn("dbo.Atividade", "Dtabertura");
            DropColumn("dbo.Atividade", "Dtencerramento");
            DropColumn("dbo.Usuario", "Status");
            DropColumn("dbo.Departamento", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departamento", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Usuario", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Atividade", "Dtencerramento", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Atividade", "Dtabertura", c => c.DateTime(storeType: "date"));
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            AlterColumn("dbo.Arquivo", "Nome", c => c.String());
            AlterColumn("dbo.Material", "URL", c => c.String());
            AlterColumn("dbo.Material", "Descricao", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Unidade", "Titulo", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.NotaCurso", "Comentario", c => c.String());
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime());
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(maxLength: 50));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(maxLength: 120));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(maxLength: 30));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(maxLength: 60));
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int(nullable: false));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(maxLength: 500));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(maxLength: 60));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(maxLength: 15));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String());
            AlterColumn("dbo.Questao", "Descricao", c => c.String());
            AlterColumn("dbo.Alternativa", "Justificativa", c => c.String());
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String());
            DropColumn("dbo.Atividade", "DataEncerramento");
            DropColumn("dbo.Atividade", "DataAbertura");
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento", cascadeDelete: true);
        }
    }
}
