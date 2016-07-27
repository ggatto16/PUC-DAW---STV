namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Idrole = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Idrole);
            
            CreateTable(
                "dbo.UsuarioRole",
                c => new
                    {
                        Idusuario = c.Int(nullable: false),
                        Idrole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Idusuario, t.Idrole })
                .ForeignKey("dbo.Usuario", t => t.Idusuario)
                .ForeignKey("dbo.Role", t => t.Idrole)
                .Index(t => t.Idusuario)
                .Index(t => t.Idrole);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioRole", "Idrole", "dbo.Role");
            DropForeignKey("dbo.UsuarioRole", "Idusuario", "dbo.Usuario");
            DropIndex("dbo.UsuarioRole", new[] { "Idrole" });
            DropIndex("dbo.UsuarioRole", new[] { "Idusuario" });
            DropTable("dbo.UsuarioRole");
            DropTable("dbo.Role");
        }
    }
}
