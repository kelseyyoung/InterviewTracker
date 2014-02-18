using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Waivers
    {
        public int WaiversID { get; set; }
        public string Type { get; set; } //TODO: Should this be an enum: Age, Drug, Academic?
        public DateTime Date { get; set; }
        public string Comments { get; set; }

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
    }
}