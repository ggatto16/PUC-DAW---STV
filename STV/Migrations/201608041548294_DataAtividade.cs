namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAtividade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atividade", "Dtabertura", c => c.DateTime());
            AddColumn("dbo.Atividade", "Dtencerramento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Atividade", "Dtencerramento");
            DropColumn("dbo.Atividade", "Dtabertura");
        }
    }
}
