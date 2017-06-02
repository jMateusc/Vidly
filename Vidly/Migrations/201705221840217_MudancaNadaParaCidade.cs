namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MudancaNadaParaCidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Cidade", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Cidade");
        }
    }
}
