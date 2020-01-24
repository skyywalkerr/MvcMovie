namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetiredFloat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "RetiredFloat", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "RetiredFloat");
        }
    }
}
