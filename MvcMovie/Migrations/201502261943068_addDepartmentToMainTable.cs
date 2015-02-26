namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDepartmentToMainTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MachineShopTables", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MachineShopTables", "Department");
        }
    }
}
