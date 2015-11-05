namespace GameStore.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Gamedates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "PublicationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Games", "IncomeDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "IncomeDate");
            DropColumn("dbo.Games", "PublicationDate");
        }
    }
}
