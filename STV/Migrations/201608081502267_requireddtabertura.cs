namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requireddtabertura : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Unidade", "Dtabertura", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Unidade", "Dtabertura", c => c.DateTime(storeType: "date"));
        }
    }
}
