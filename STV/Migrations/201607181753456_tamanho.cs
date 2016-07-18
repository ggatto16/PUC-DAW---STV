namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tamanho : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "Tamanho", c => c.Int());
            DropColumn("dbo.Arquivo", "testeBlob");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "testeBlob", c => c.Binary());
            DropColumn("dbo.Arquivo", "Tamanho");
        }
    }
}
