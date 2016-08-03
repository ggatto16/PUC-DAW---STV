namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRealizado : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notas", "Realizado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notas", "Realizado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
