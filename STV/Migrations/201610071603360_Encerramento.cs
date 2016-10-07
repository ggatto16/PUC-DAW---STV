namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Encerramento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Curso", "DataInicial", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Curso", "Encerrado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Unidade", "DataAbertura", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Unidade", "Encerrada", c => c.Boolean(nullable: false));
            DropColumn("dbo.Curso", "Dtinicial");
            DropColumn("dbo.Curso", "Dtfinal");
            DropColumn("dbo.Unidade", "Dtabertura");
            DropColumn("dbo.Unidade", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Unidade", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Unidade", "Dtabertura", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Curso", "Dtfinal", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Curso", "Dtinicial", c => c.DateTime(storeType: "date"));
            DropColumn("dbo.Unidade", "Encerrada");
            DropColumn("dbo.Unidade", "DataAbertura");
            DropColumn("dbo.Curso", "Encerrado");
            DropColumn("dbo.Curso", "DataInicial");
        }
    }
}
