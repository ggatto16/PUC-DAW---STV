namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Atividade", "Dtencerramento", c => c.DateTime(storeType: "date"));
            DropColumn("dbo.Usuario", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuario", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Atividade", "Dtencerramento", c => c.DateTime());
        }
    }
}
