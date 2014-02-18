using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class RD
    {
        public int RDID { get; set; }
        public string Type { get; set; } //TODO: Should this be an enum: RECLAMA, DEVOL?
        public string Reason { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
    }
}