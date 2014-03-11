using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class RD
    {
        public int RDID { get; set; } // PK for RD table
        [Required]
        public RDType? Type { get; set; } // Type of devolunteer, RECLAMA or DEVOL
        [Required]
        public DateTime Date { get; set; } // Date of request or DEVOL status
        [Required]
        public string Reason { get; set; } // Reason for devolunteer

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }
    }

}

public enum RDType
{
    DEVOL = 1,
    RECLAMA = 2
}