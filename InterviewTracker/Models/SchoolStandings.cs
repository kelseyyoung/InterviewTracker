using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SchoolStandings
    {
        public int SchoolStandingsID { get; set; } // PK for School Standings table
        public int YearOfRecord { get; set; } // Year of this record
        public float GPA { get; set; } // GPA
        public int AOMVal { get; set; } // Academic order of merit value
        public int AOMTot { get; set; } // Academic order of merit total
        public int OOMVal { get; set; } // Overall order of merit value
        public int OOMTot { get; set; } // Overall order of merit total
        public int InMajorVal { get; set; } // Standing in major value
        public int InMajorTot { get; set; } // Standing in major total

        [ForeignKey("SchoolsAttended")]
        public int SchoolsAttendedID { get; set; } // FK to Schools Attended table
    }
}