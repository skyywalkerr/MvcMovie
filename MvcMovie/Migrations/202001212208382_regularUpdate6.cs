namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class regularUpdate6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MachineShopTables", "Shift");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MachineShopTables", "Shift", c => c.Int(nullable: false));
        }
    }
}
