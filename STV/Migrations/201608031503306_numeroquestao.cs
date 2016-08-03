namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numeroquestao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questao", "Numero", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questao", "Numero");
        }
    }
}
