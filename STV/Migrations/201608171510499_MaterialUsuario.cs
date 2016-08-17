namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaterialUsuario : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaterialUsuario",
                c => new
                    {
                        Idmaterial = c.Int(nullable: false),
                        Idusuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idmaterial, t.Idusuario })
                .ForeignKey("dbo.Material", t => t.Idmaterial)
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .Index(t => t.Idmaterial)
                .Index(t => t.Idusuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialUsuario", "Idusuario", "dbo.Usuario");
            DropForeignKey("dbo.MaterialUsuario", "Idmaterial", "dbo.Material");
            DropIndex("dbo.MaterialUsuario", new[] { "Idusuario" });
            DropIndex("dbo.MaterialUsuario", new[] { "Idmaterial" });
            DropTable("dbo.MaterialUsuario");
        }
    }
}
