using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcMovie.Models
{
    public class Machines
    {
        public int ID { get; set; } // PROPERTY ID

        [Display(Name = "Department")]        
        public string Department{ get; set; }

        [Display(Name = "Work Center")]
        //[StringLength(60, MinimumLength = 0)] // VALIDATION - Attribute that determine : Maximum length 60, minimum 3
        public string WorkCenter { get; set; } // PROPERTY TITLE

        [Display(Name = "Machine")]
        public string Machine { get; set; } // PROPERTY TITLE
    }
}