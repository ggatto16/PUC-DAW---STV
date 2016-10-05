namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Justificativa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alternativa", "Justificativa", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alternativa", "Justificativa");
        }
    }
}
