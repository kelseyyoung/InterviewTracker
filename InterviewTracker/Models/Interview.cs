using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Interview
    {
        public int InterviewID { get; set; } // PK for Interview table
        [Required]
        public DateTime? Date { get; set; } // Date of interview
        [Required]
        public string Status { get; set; } // Status of interview
        [Required]
        public string Location { get; set; }
        public string Comments { get; set; } // Comments from interviewer
        public string EditedComments { get; set; } // Edited comments from interview coordinator
        [Required]
        public DateTime? StartTime { get; set; } // Start time
        //TODO: How to make these just Time?
        [Required]
        public DateTime? EndTime { get; set; } // End time
        //TODO: How to make these just Time?
        [Required]
        public int? Duration { get; set; } // Duration in minutes
        public DateTime? EditTime { get; set; } // Last time interviewer's comments were edited
        public bool? NR { get; set; } // Recommended for NR duty
        public bool? INST { get; set; } // Recommended for Instructor duty
        public bool? NPS { get; set; } // Recommended for Nuclear Power school duty
        public bool? PXO { get; set; } // Recommended for Prospective XO
        public bool? EDO { get; set; } // Recommended for Engineering Duty Officer
        public bool? ENLTECH { get; set; } // Recommended for Enlisted Tech
        public bool? NR1 { get; set; } // Recommended for NR-1 duty
        public bool? SUPPLY { get; set; } // Recommended for NR duty (supply)
        public bool? EOOW { get; set; } // Recommended for Engineering Officer of the Watch
        public bool? DOE { get; set; } // Recommended for Field Office

        [ForeignKey("CurrentlyEditingUser")]
        public virtual int? CurrentlyEditingID { get; set; } // FK to User who is currently editing the comments
        public virtual User CurrentlyEditingUser { get; set; } // Can be null
        [ForeignKey("InterviewerUser")]
        public virtual int InterviewerID { get; set; } // FK to User who was the interviewer
        public virtual User InterviewerUser { get; set; }
        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to applicant
        public virtual BioData BioData { get; set; }

    }
}

public enum Status
{
    Scheduled = 1,
    Entered = 2,
    Edited = 3,
    Final = 4
}