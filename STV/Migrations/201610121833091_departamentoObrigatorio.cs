namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departamentoObrigatorio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int(nullable: false));
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento");
            DropIndex("dbo.Usuario", new[] { "Iddepartamento" });
            AlterColumn("dbo.Usuario", "Iddepartamento", c => c.Int());
            CreateIndex("dbo.Usuario", "Iddepartamento");
            AddForeignKey("dbo.Usuario", "Iddepartamento", "dbo.Departamento", "Iddepartamento");
        }
    }
}
