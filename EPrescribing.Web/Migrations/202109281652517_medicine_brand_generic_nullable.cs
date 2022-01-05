namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class medicine_brand_generic_nullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Medicines", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Medicines", "GenericId", "dbo.Generics");
            DropIndex("dbo.Medicines", new[] { "GenericId" });
            DropIndex("dbo.Medicines", new[] { "BrandId" });
            AlterColumn("dbo.Medicines", "GenericId", c => c.Int());
            AlterColumn("dbo.Medicines", "BrandId", c => c.Int());
            CreateIndex("dbo.Medicines", "GenericId");
            CreateIndex("dbo.Medicines", "BrandId");
            AddForeignKey("dbo.Medicines", "BrandId", "dbo.Brands", "Id");
            AddForeignKey("dbo.Medicines", "GenericId", "dbo.Generics", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Medicines", "GenericId", "dbo.Generics");
            DropForeignKey("dbo.Medicines", "BrandId", "dbo.Brands");
            DropIndex("dbo.Medicines", new[] { "BrandId" });
            DropIndex("dbo.Medicines", new[] { "GenericId" });
            AlterColumn("dbo.Medicines", "BrandId", c => c.Int(nullable: false));
            AlterColumn("dbo.Medicines", "GenericId", c => c.Int(nullable: false));
            CreateIndex("dbo.Medicines", "BrandId");
            CreateIndex("dbo.Medicines", "GenericId");
            AddForeignKey("dbo.Medicines", "GenericId", "dbo.Generics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Medicines", "BrandId", "dbo.Brands", "Id", cascadeDelete: true);
        }
    }
}
