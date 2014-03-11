using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Degree
    {
        public int DegreeID { get; set; } // PK for Degree table
        [Required]
        public DateTime DegreeDate { get; set; } // DateTime degree received, also grad date if 'graduated'

        [ForeignKey("SchoolsAttended")]
        public virtual int SchoolsAttendedID { get; set; } // FK to SchoolsAttended table
        public virtual SchoolsAttended SchoolsAttended { get; set; }
        [ForeignKey("Major")]
        public virtual int MajorID { get; set; } // FK to Major table
        public virtual Major Major { get; set; }
        [ForeignKey("DegreeType")]
        public virtual int DegreeTypeID { get; set; } // FK to DegreeType table
        public virtual DegreeType DegreeType { get; set; }

    }
}