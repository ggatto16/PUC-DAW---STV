namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Tema", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Tema");
        }
    }
}
