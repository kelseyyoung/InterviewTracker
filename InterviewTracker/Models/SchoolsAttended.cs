using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SchoolsAttended
    {
        public int AttendedID { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public bool Graduated { get; set; }
        public string Comments { get; set; }

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
    }
}