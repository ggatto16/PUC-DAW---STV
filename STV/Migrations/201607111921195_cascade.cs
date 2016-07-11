namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Arquivo", "Idmaterial", "dbo.Material");
            AddForeignKey("dbo.Arquivo", "Idmaterial", "dbo.Material", "Idmaterial", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Arquivo", "Idmaterial", "dbo.Material");
            AddForeignKey("dbo.Arquivo", "Idmaterial", "dbo.Material", "Idmaterial");
        }
    }
}
