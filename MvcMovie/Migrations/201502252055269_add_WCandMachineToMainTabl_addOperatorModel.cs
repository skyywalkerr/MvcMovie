namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WCandMachineToMainTabl_addOperatorModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "WorkCenter", c => c.String());
            AddColumn("dbo.MachineShopTables", "Machine", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "Machine");
            DropColumn("dbo.MachineShopTables", "WorkCenter");
        }
    }
}
