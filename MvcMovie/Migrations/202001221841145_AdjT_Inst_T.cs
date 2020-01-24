namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjT_Inst_T : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "ADJ_T", c => c.Single(nullable: false));
            AddColumn("dbo.MachineShopTables", "INS_T", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "INS_T");
            DropColumn("dbo.MachineShopTables", "ADJ_T");
        }
    }
}
