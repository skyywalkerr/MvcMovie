namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_Machines_table_to_MainTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Machines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Department = c.String(),
                        WorkCenter = c.String(),
                        Machine = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Machines");
        }
    }
}
