namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_WorkOrderNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "WorkOrderNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "WorkOrderNo");
        }
    }
}
