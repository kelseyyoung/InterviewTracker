using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SchoolStandings
    {
        public int StandingsID { get; set; }
        public int YearOfRecord { get; set; }
        public float GPA { get; set; }
        public int AOMVal { get; set; }
        public int AOMTot { get; set; }
        public int OOMVal { get; set; }
        public int OOMTot { get; set; }
        public int InMajorVal { get; set; }
        public int InMajorTot { get; set; }

        [ForeignKey("SchoolsAttended")]
        public int AttendedID { get; set; }
    }
}