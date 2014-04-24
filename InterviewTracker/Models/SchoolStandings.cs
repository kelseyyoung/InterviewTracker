using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class SchoolStandings
    {
        public int SchoolStandingsID { get; set; } // PK for School Standings table
        [Required(ErrorMessage="The Year of Record field is required")]
        public int? YearOfRecord { get; set; } // Year of this record
        [Required]
        [RegularExpression(@"^[0-4]\.[0-9]([0-9])?$", ErrorMessage = "GPA must be a valid decimal Grade Point Average")]
        public double? GPA { get; set; } // GPA
        [Required(ErrorMessage="The AOM value is required")]
        public int? AOMVal { get; set; } // Academic order of merit value
        [Required(ErrorMessage="The AOM Total is required")]
        public int? AOMTot { get; set; } // Academic order of merit total
        [Required(ErrorMessage="The OOM value is required")]
        public int? OOMVal { get; set; } // Overall order of merit value
        [Required(ErrorMessage="The OOM Total is required")]
        public int? OOMTot { get; set; } // Overall order of merit total
        [Required(ErrorMessage="The In Major Value is required")]
        public int? InMajorVal { get; set; } // Standing in major value
        [Required(ErrorMessage="The In Major Total is required")]
        public int? InMajorTot { get; set; } // Standing in major total

        [ForeignKey("SchoolsAttended")]
        public virtual int SchoolsAttendedID { get; set; } // FK to Schools Attended table
        public virtual SchoolsAttended SchoolsAttended { get; set; }
    }
}