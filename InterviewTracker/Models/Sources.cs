using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Sources
    {
        public int SourcesID { get; set; } // PK for Source table
        [Required(ErrorMessage="The Source field is required")]
        public string SourcesValue { get; set; } // Source
    }
}