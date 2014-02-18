using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewTracker.Models
{
    public class Admiral
    {
        public int AdmiralID { get; set; } // PK for Admiral table
        public bool Decision { get; set; } // T/F if accepted applicant
        public bool Accepted { get; set; } // T/F if applicant accepted
        public string Comments { get; set; } // Personal comments from admiral (not printed)
        public bool NP500 { get; set; } // T/F instructed to attend NP500
        public bool NSTC { get; set; } // T/F instructed to attend NSTC
        public bool SelfStudy { get; set; } // T/F instructed to do self study
        public bool PreSchool { get; set; } // T/F instructed to attend NPS Preschool
        public bool Letter { get; set; } // T/F if instructed to write letter to admiral
        public bool LetterReceived { get; set; } // T/F if letter was received
        public string AdmiralNotes { get; set; } // Printed notes from admiral
        public bool InviteBack { get; set; } // T/F if invited back for another interview
        public string SERVSEL { get; set; } // Community chosen by applicant for their contract

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
        [ForeignKey("Interview")]
        public int InterviewID { get; set; }

    }
}