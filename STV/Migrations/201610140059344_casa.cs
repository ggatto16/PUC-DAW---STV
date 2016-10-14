namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class casa : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Usuario", "Cpf", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuario", new[] { "Cpf" });
        }
    }
}
