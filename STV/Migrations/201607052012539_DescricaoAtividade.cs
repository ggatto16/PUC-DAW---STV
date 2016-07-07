namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescricaoAtividade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atividade", "Descricao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Atividade", "Descricao");
        }
    }
}
