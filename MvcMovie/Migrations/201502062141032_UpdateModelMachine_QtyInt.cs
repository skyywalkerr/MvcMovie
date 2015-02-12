namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelMachine_QtyInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MachineShopTables", "Qty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MachineShopTables", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
