using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Screen
    {
        public int ScreenID { get; set; } // PK for Screen table
        public List<Program> AppliedFor { get; set; } // List of programs applicant applied for
        public string Screener { get; set; } // Name of screener
        // TODO: should this be a User? Fname and Lname?
        public DateTime ScreenDate { get; set; } // Date of screening
        public ScreenStatus NRStatus { get; set; } // Status of NR
        public ScreenStatus INSTStatus { get; set; } // Status of INST
        public ScreenStatus NPSStatus { get; set; } // Status of NPS
        public string Notes { get; set; } // Notes on screen

        [ForeignKey("BioData")]
        public int BioDataID { get; set; } // FK to BioData table
    }
}

public enum ScreenStatus
{
    Yes = 1,
    No = 2,
    Pending = 3,
    Blank = 4,
    Forward = 5,
    Maybe = 6
}