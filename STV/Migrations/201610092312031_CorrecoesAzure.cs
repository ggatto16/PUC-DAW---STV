namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecoesAzure : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Questao", "Descricao", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Arquivo", "Nome", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arquivo", "Nome", c => c.String());
            AlterColumn("dbo.Unidade", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Departamento", "Stamp", c => c.DateTime());
            AlterColumn("dbo.Departamento", "Descricao", c => c.String(maxLength: 50));
            AlterColumn("dbo.Curso", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Curso", "Palavraschave", c => c.String(maxLength: 120));
            AlterColumn("dbo.Curso", "Categoria", c => c.String(maxLength: 30));
            AlterColumn("dbo.Curso", "Titulo", c => c.String(maxLength: 60));
            AlterColumn("dbo.Usuario", "Stamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Usuario", "Senha", c => c.String(maxLength: 500));
            AlterColumn("dbo.Usuario", "Nome", c => c.String(maxLength: 60));
            AlterColumn("dbo.Usuario", "Cpf", c => c.String(maxLength: 15));
            AlterColumn("dbo.Atividade", "Descricao", c => c.String());
            AlterColumn("dbo.Questao", "Descricao", c => c.String());
            AlterColumn("dbo.Alternativa", "Descricao", c => c.String());
        }
    }
}
