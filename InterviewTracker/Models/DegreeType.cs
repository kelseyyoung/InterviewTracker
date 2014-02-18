using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DegreeType
    {
        public int DegreeTypeID { get; set; } // PK of DegreeType table
        public string DegreeType { get; set; } // Type of degree (Ph.D, Bachelors, etc)
    }
}