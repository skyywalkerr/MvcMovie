namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelMachine_AllDecimalsToFloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MachineShopTables", "ActualRate", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "StandardRate", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "Percent", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "Setup", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "Cleaning", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "Down", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "Other", c => c.Single(nullable: false));
            AlterColumn("dbo.MachineShopTables", "NonconfParts", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MachineShopTables", "NonconfParts", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Other", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Down", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Cleaning", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Setup", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "StandardRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "ActualRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
