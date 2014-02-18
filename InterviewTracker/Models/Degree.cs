using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Degree
    {
        public int DegreeID { get; set; } // PK for Degree table
        public DateTime DegreeDate { get; set; } // DateTime degree received, also grad date if 'graduated'

        [ForeignKey("SchoolsAttended")]
        public int SchoolsAttendedID { get; set; } // FK to SchoolsAttended table
        [ForeignKey("Major")]
        public int MajorID { get; set; } // FK to Major table
        [ForeignKey("DegreeType")]
        public int DegreeTypeID { get; set; } // FK to DegreeType table

    }
}