namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SenhaDigitada : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuario", "Senha", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuario", "Senha", c => c.String(maxLength: 20));
        }
    }
}
