using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Classes
    {
        public int ClassesID { get; set; } // PK for classes table
        [Required]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage="Subject must be all capital letters")]
        public string Subject { get; set; } // Class subject (MATH, HIST, etc)
        [Required]
        public string Name { get; set; } // Name of class
        [Required]
        public string Code { get; set; } // Class code (140, 250A, etc)
        [Required]
        public bool? Technical { get; set; } // T/F if it's a technical class
    }
}