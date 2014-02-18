using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewTracker.Models
{
    public class Admiral
    {
        public int AdmiralID { get; set; }
        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
        [ForeignKey("Interview")]
        public int InterviewID { get; set; }
        public bool Decision { get; set; }
        public bool Accepted { get; set; }
        public string Comments { get; set; }
        public bool NP500 { get; set; }
        public bool NSTC { get; set; }
        public bool SelfStudy { get; set; }
        public bool PreSchool { get; set; }
        public bool Letter { get; set; }
        public bool LetterReceived { get; set; }
        public string AdminNotes { get; set; }
        public bool InviteBack { get; set; }
        public string SERVSEL { get; set; }

    }
}