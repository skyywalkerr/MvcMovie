using System;                       
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcMovie.Models
{
    public class MachineShopTable
    {      
            public int ID { get; set; } // PROPERTY ID

            [Display(Name = "Date")]
            [DataType(DataType.Date)]
            //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
            [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime Date { get; set; }

            [Display(Name = "Item No.")]
            //[StringLength(60, MinimumLength = 0)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public string ItemNo { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public string Operation { get; set; } // PROPERTY TITLE

            [Display(Name = "WO No.")]
            public string WorkOrderNo { get; set; }

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public string Operator { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            //[DisplayFormat(DataFormatString = "{00}", ApplyFormatInEditMode = true)]
            public int Qty { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float Hours { get; set; } // PROPERTY TITLE

            [Display(Name = "AR")]
            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float ActualRate { get; set; } // PROPERTY TITLE

            [Display(Name = "SR")]
            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float StandardRate { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            [DataType("Percent")]
            public float Percent { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float Setup { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float Cleaning { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float Down { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float Other { get; set; } // PROPERTY TITLE

            [Display(Name = "NP")]
            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public float NonconfParts { get; set; } // PROPERTY TITLE

            //[StringLength(60, MinimumLength = 3)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
            public string Comments { get; set; } // PROPERTY TITLE

            public string WorkCenter { get; set; }

            public string Machine { get; set; }

            public string Department { get; set; }

        }

        public class MachineShopDB : DbContext
        #region explenation
        //DbContext is a class from Entity framework responsible for data transaction with database

            //A DbContext instance represents a combination of the Unit Of Work        
        //and Repository patterns such that it can be used to query from a database and group 
        //together changes that will then be written back to the store as a unit. DbContext is conceptually similar to ObjectContext.

            // LINK : msdn.microsoft.com/en-us/library/system.data.entity.dbcontext%28v=vs.113%29.aspx

            // name of this class gonna be the same as database name but can be renamed as long as has still MDF SUFFIX
        // MovieDBContext - XML web.config file need to have a line for DB config, name of connection need to b named the same as name of this class
        #endregion
        {
            public DbSet<MachineShopTable> MainTableObj { get; set; }
            public DbSet<Machines> Machines { get; set; }

            #region explenation
            // this is a list/table of results from Movie database
            //DbSet is an class from entity framework, it's setting a connection with database

            //structure: DbSet<name_of_Model_class === name space>name_of_new_object {get; set;}

            //link : msdn.microsoft.com/en-us/library/gg696460%28v=vs.113%29.aspx
            #endregion
        }

        public class FilteringToolModel
        {
            //public string Department { get; set; }
            //public string WorkCenter { get; set; }
            public string CheckBox { get; set; }

        }

        //public class ViewModelClass
        //{
        //    IEnumerable<FilteringToolModel> FilteringToolView { get; set; }
        //    IEnumerable<MachineShopTable> MainTableView { get; set; }
        //}

        public class dateTimeTest
        {
            public DateTime todayTest { get; set; }
        }
}