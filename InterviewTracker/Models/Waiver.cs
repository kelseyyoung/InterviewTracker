using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Waiver
    {
        public int WaiverID { get; set; } // PK for Waiver table
        public string Type { get; set; } // Type of Waiver
        //TODO: Should this be an enum: Age, Drug, Academic?
        public DateTime Date { get; set; } // Date of waiver issue
        public string Comments { get; set; } // Comments on Waiver

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }
    }
}