using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
    public class Admiral
    {
        public int AdmiralID { get; set; } // PK for Admiral table
        [Required]
        public bool? Decision { get; set; } // T/F if accepted applicant
        [Required]
        public bool? Accepted { get; set; } // T/F if applicant accepted
        public string Comments { get; set; } // Personal comments from admiral (not printed)
        [Required]
        public bool? NP500 { get; set; } // T/F instructed to attend NP500
        [Required]
        public bool? NSTC { get; set; } // T/F instructed to attend NSTC
        [Required]
        public bool? SelfStudy { get; set; } // T/F instructed to do self study
        [Required]
        public bool? PreSchool { get; set; } // T/F instructed to attend NPS Preschool
        [Required]
        public bool? Letter { get; set; } // T/F if instructed to write letter to admiral
        [Required]
        public bool? LetterReceived { get; set; } // T/F if letter was received
        public string AdmiralNotes { get; set; } // Printed notes from admiral
        [Required]
        public bool? InviteBack { get; set; } // T/F if invited back for another interview
        //[Required]
        //public string SERVSEL { get; set; } // Community chosen by applicant for their contract
        [Required]
        public DateTime Date { get; set; } //Date candidate was reviewed

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; }
        public virtual BioData BioData { get; set; }
        [ForeignKey("Program")]
        public virtual int ProgramID { get; set; }
        public virtual Program Program { get; set; }

    }
}