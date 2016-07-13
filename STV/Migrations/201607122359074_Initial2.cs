namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arquivo", "Nome", c => c.String());
            DropColumn("dbo.Arquivo", "Nomearquivo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arquivo", "Nomearquivo", c => c.String());
            DropColumn("dbo.Arquivo", "Nome");
        }
    }
}
