namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Usuario", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuario", "Role", c => c.Int(nullable: false));
        }
    }
}
