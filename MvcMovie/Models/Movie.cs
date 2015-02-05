using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcMovie.Models
{
    //EACH INSTANCE OF MOVIE OBJECT WILL CORRESPOND TO A ROW WITHIN A DATABASE

    // EACH PROPERTY OF MOVIE CLASS WILLMAP TO A COLUMN IN IN THE TABLE

    // reminder :  property is a field/variable of a class

    public class Movie
    {
        public int ID{get;set;} // PROPERTY ID

        [StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
        public string Title{get;set;} // PROPERTY TITLE

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")] //VALIDATION - RegularExpression(string pattern) - data input must meet this expectations
        [Required] // VALIDATION - specify that this data field is required 
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1,1000000)] //specify minimum and maximum value for field
        [DataType(DataType.Currency)] //determine type of data
        public decimal Price { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(5)]
        public string Rating { get; set; } // new field
    }

    public class MovieDBContext : DbContext 
        //DbContext is a class from Entity framework responsible for data transaction with database
        
        //A DbContext instance represents a combination of the Unit Of Work        
        //and Repository patterns such that it can be used to query from a database and group 
        //together changes that will then be written back to the store as a unit. DbContext is conceptually similar to ObjectContext.

        // LINK : msdn.microsoft.com/en-us/library/system.data.entity.dbcontext%28v=vs.113%29.aspx

        // name of this class gonna be the same as database name but can be renamed as long as has still MDF SUFFIX
        // MovieDBContext - XML web.config file need to have a line for DB config, name of connection need to b named the same as name of this class

    {
        public DbSet<Movie> Movies { get; set; } 
        // this is a list/table of results from Movie database
        //DbSet is an class from entity framework, it's setting a connection with database
        //structure: DbSet<name_of_Model_class>name_of_new_object {get; set;}
        //link : msdn.microsoft.com/en-us/library/gg696460%28v=vs.113%29.aspx
    }
    
}