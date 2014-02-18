using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class School
    {
        public int SchoolID { get; set; } // PK for School table
        public string School { get; set; } // School name (all caps to avoid duplicates)
    }
}