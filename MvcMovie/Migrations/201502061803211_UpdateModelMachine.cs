namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModelMachine : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MachineShopTables", "ItemNo", c => c.String());
            AlterColumn("dbo.MachineShopTables", "Operation", c => c.String());
            AlterColumn("dbo.MachineShopTables", "Operator", c => c.String());
            AlterColumn("dbo.MachineShopTables", "Qty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Hours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "ActualRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "StandardRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Setup", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Cleaning", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Down", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Other", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "NonconfParts", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MachineShopTables", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MachineShopTables", "Comments", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "NonconfParts", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Other", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Down", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Cleaning", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Setup", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Percent", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "StandardRate", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "ActualRate", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Hours", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Qty", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Operator", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "Operation", c => c.String(maxLength: 60));
            AlterColumn("dbo.MachineShopTables", "ItemNo", c => c.String(maxLength: 60));
        }
    }
}
