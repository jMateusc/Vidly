namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGenreTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Genres (Id,Name) VAlUES (1,'Action')");
            Sql("INSERT INTO Genres (Id,Name) VAlUES (2,'Comedy')");
            Sql("INSERT INTO Genres (Id,Name) VAlUES (3,'Thriller')");
            Sql("INSERT INTO Genres (Id,Name) VAlUES (4,'Drama')");
        }
        
        public override void Down()
        {
        }
    }
}
