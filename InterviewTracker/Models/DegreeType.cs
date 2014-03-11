using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DegreeType
    {
        public int DegreeTypeID { get; set; } // PK of DegreeType table
        [Required]
        public string DegreeTypeValue { get; set; } // Type of degree (Ph.D, Bachelors, etc)
    }
}