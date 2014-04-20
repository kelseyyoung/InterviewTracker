using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SubSources
    {
        public int SubSourcesID { get; set; } // PK for SubSource table
        [Required(ErrorMessage="The SubSource field is required")]
        public string SubSourcesValue { get; set; } // SubSource
    }
}