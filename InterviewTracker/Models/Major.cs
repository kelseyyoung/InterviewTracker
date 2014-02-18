using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Major
    {
        public int MajorID { get; set; } // PK for Major table
        public string Major { get; set; } // Major type (Physics, Engineering, etc)
    }
}