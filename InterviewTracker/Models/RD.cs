using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class RD
    {
        public int RDID { get; set; } // PK for RD table
        public string Type { get; set; } // Type of devolunteer, RECLAMA or DEVOL
        //TODO: Should this be an enum: RECLAMA, DEVOL?
        public string Reason { get; set; } // Reason for devolunteer
        public DateTime Date { get; set; } // Date of request or DEVOL status

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }
    }
}