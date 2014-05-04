using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class ClassesAttended
    {
        public int ClassesAttendedID { get; set; } // PK for ClassesAttended table
        [Required(ErrorMessage="The Year Taken field is required")]
        public int? YearTaken { get; set; } // Year class was taken in int (year 1, 2, 3, etc)
        [Required]
        public string Grade { get; set; } // Grade received

        [ForeignKey("SchoolsAttended")]
        public virtual int SchoolsAttendedID { get; set; } // FK to SchoolsAttended table
        public virtual SchoolsAttended SchoolsAttended { get; set; }
        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; }
        public virtual BioData BioData { get; set; }
        [ForeignKey("Classes")]
        public virtual int ClassesID { get; set; } // FK to Classes table
        public virtual Classes Classes { get; set; }
    }
}