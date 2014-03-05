using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Major
    {
        public int MajorID { get; set; } // PK for Major table
        [Required]
        public string MajorValue { get; set; } // Major type (Physics, Engineering, etc)
    }
}