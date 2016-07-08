namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class file2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arquivo",
                c => new
                    {
                        Idarquivo = c.Int(nullable: false),
                        Nomearquivo = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                        Tipo = c.String(),
                        Idmaterial = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Idarquivo)
                .ForeignKey("dbo.Material", t => t.Idarquivo)
                .Index(t => t.Idarquivo);
            
            AddColumn("dbo.Material", "Idarquivo", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Arquivo", "Idarquivo", "dbo.Material");
            DropIndex("dbo.Arquivo", new[] { "Idarquivo" });
            DropColumn("dbo.Material", "Idarquivo");
            DropTable("dbo.Arquivo");
        }
    }
}
