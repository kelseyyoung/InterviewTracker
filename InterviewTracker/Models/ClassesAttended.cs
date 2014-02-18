using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class ClassesAttended
    {
        public int ClassesAttendedID { get; set; } // PK for ClassesAttended table
        public int YearTaken { get; set; } // Year class was taken
        public string Grade { get; set; } // Grade received

        [ForeignKey("SchoolsAttended")]
        public int SchoolsAttendedID { get; set; } // FK to SchoolsAttended table
        [ForeignKey("BioData")]
        public int BioDataID { get; set; } // FK to BioData table
        [ForeignKey("Classes")]
        public int ClassesID { get; set; } // FK to Classes table
    }
}