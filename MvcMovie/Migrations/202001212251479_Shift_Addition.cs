namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shift_Addition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "Shift", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "Shift");
        }
    }
}
