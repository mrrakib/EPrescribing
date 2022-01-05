namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluationPeriodInDayMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "EvaluationPeriodInDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "EvaluationPeriodInDay");
        }
    }
}
