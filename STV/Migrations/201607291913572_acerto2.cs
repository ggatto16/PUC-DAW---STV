namespace STV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acerto2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Curso", "Dtinicial", c => c.DateTime());
            AlterColumn("dbo.Curso", "Dtfinal", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Curso", "Dtfinal", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Curso", "Dtinicial", c => c.DateTime(storeType: "date"));
        }
    }
}
