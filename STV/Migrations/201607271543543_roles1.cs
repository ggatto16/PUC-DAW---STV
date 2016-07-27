namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Role");
        }
    }
}
