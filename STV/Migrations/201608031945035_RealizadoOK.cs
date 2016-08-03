namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealizadoOK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Atividade", "Realizado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Atividade", "Realizado");
        }
    }
}
