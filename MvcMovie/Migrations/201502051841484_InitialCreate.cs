namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MachineShopTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ItemNo = c.String(maxLength: 60),
                        Operation = c.String(maxLength: 60),
                        Operator = c.String(maxLength: 60),
                        Qty = c.String(maxLength: 60),
                        Hours = c.String(maxLength: 60),
                        ActualRate = c.String(maxLength: 60),
                        StandardRate = c.String(maxLength: 60),
                        Percent = c.String(maxLength: 60),
                        Setup = c.String(maxLength: 60),
                        Cleaning = c.String(maxLength: 60),
                        Down = c.String(maxLength: 60),
                        Other = c.String(maxLength: 60),
                        NonconfParts = c.String(maxLength: 60),
                        Comments = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MachineShopTables");
        }
    }
}
