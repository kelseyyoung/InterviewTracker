using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class ServSel
    {
        public int ServSelID { get; set; } // PK for ServSel table
        [Required]
        public string ServSelValue { get; set; } // ServSel value
    }
}