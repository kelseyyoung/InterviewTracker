using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Classes
    {
        public int ClassesID { get; set; } // PK for classes table
        public string Subject { get; set; } // Class subject (MATH, HIST, etc)
        public string Code { get; set; } // Class code (140, 250, etc) TODO: should this be an int?
        public bool Technical { get; set; } // T/F if it's a technical class
    }
}