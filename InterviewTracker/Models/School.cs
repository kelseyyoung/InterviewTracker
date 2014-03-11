using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class School
    {
        public int SchoolID { get; set; } // PK for School table
        [Required]
        public string SchoolValue { get; set; } // School name (all caps to avoid duplicates)
    }
}