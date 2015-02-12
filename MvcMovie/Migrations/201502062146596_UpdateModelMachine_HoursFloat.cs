namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelMachine_HoursFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MachineShopTables", "Hours", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MachineShopTables", "Hours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
