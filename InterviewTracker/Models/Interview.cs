using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Interview
    {
        public int InterviewID { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public string Comments { get; set; }
        public string EditedComments { get; set; }
        public DateTime StartTime { get; set; } //TODO: How to make these just Time?
        public DateTime EndTime { get; set; } //TODO: How to make these just Time?
        public int Duration { get; set; }
        public DateTime EditTime { get; set; }
        public bool NR { get; set; }
        public bool INST { get; set; }
        public bool NPS { get; set; }
        public bool PXO { get; set; }
        public bool EDO { get; set; }
        public bool ENLTECH { get; set; }
        public bool NR1 { get; set; }
        public bool Supply { get; set; }
        public bool EOOW { get; set; }
        public bool DOE { get; set; }

        [ForeignKey("Users")]
        public int CurrentlyEditingID { get; set; }
        [ForeignKey("Users")]
        public int InterviewerID { get; set; }
        [ForeignKey("BioData")]
        public int BioDataID { get; set; }

    }

    public enum Status {
        Scheduled = 1,
        Entered = 2,
        Edited = 3,
        Final = 4
    }
}