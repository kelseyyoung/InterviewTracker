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
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }
        [ForeignKey("School")]
        public virtual int SchoolID { get; set; } // FK to School table
        public virtual School School { get; set; }

        public virtual ICollection<SchoolStandings> SchoolStandings { get; set; } // Associated SchoolStandings
        public virtual ICollection<ClassesAttended> ClassesAttended { get; set; } //The classes attended at this school
        public virtual ICollection<Degree> Degree { get; set; } //Associated degree with this school attendance
    }
}