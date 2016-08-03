namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealizadoOKOK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notas", "Realizado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Atividade", "Realizado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Atividade", "Realizado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Notas", "Realizado");
        }
    }
}
