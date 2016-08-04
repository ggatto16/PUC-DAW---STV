namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Atividade", "Dtabertura", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Atividade", "Dtabertura", c => c.DateTime());
        }
    }
}
