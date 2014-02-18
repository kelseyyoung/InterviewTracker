using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SchoolsAttended
    {
        public int SchoolsAttendedID { get; set; } // PK For Schools Attended table
        public int YearStart { get; set; } // Year started school
        public int YearEnd { get; set; } // Year finished school
        public bool Graduated { get; set; } // T/F if graduated
        public string Comments { get; set; } // Comments about attendance

        [ForeignKey("BioData")]
        public int BioDataID { get; set; } // FK to BioData table
        [ForeignKey("School")]
        public int SchoolID { get; set; } // FK to School table
    }
}