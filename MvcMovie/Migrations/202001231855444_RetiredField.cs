namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetiredField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "Retired", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "Retired");
        }
    }
}
