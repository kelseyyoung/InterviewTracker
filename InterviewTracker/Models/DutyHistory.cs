using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DutyHistory
    {
        public int DutyHistoryID { get; set; } // PK for Duty History table
        /* TODO: should branch be its own model to prevent duplicates? */
        public string Branch { get; set; } // Branch (Navy, USMC, etc)
        public string Rank { get; set; } // Rank (E1, O2, etc)
        public string Rating { get; set; } // Rating (ET, MM, etc)
        public bool NUC { get; set; } // T/F if applicant was prior NUC

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }

        public virtual ICollection<DutyStation> DutyStations { get; set; } // Collection of stations that make up this DutyHistory
    }
}