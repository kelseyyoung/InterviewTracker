using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Screens
    {
        public int ScreenID { get; set; }
        public List<Program> AppliedFor { get; set; } //TODO: change
        public string Screener { get; set; }
        public DateTime ScreenDate { get; set; }
        public Status NRStatus { get; set; }
        public Status INSTStatus { get; set; }
        public Status NPSStatus { get; set; }
        public string Notes { get; set; }

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
    }

    public enum Status
    {
        Yes = 1,
        No = 2,
        Pending = 3,
        Blank = 4,
        Forward = 5,
        Maybe = 6
    }
}