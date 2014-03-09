﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    //TODO: are we keeping track of screens anymore?
    public class Screen
    {
        public int ScreenID { get; set; } // PK for Screen table
        public virtual ICollection<Program> ProgramsAppliedFor { get; set; } // List of programs applicant applied for
        // TODO: I think this is currently unused
        public string Screener { get; set; } // Name of screener
        // TODO: should this be a User? Fname and Lname?
        public string Location { get; set; } // Location screened
        // TODO: should this be a source?
        public DateTime ScreenDate { get; set; } // Date of screening
        public ScreenStatus NRStatus { get; set; } // Status of NR
        public ScreenStatus INSTStatus { get; set; } // Status of INST
        public ScreenStatus NPSStatus { get; set; } // Status of NPS
        public string Notes { get; set; } // Notes on screen

        [ForeignKey("BioData")]
        public virtual int BioDataID { get; set; } // FK to BioData table
        public virtual BioData BioData { get; set; }
    }
}

public enum ScreenStatus
{
    Blank = 1,
    Yes = 2,
    No = 3,
    Pending = 4,
    Forward = 5,
    Maybe = 6
}