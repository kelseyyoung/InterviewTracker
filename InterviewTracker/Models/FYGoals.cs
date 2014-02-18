using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class FYGoals
    {
        [Key]
        public int FY { get; set; } // PK for FYGoals table
        public Source Source { get; set; } // Commissioning source
        public int SUB { get; set; } // Submarine goal
        public int SWO { get; set; } // Surface goal
        public int NR { get; set; } // NR Duty goal
        public int INST { get; set; } // Instructor Duty goal
    }

    public enum Source
    {
        USNA = 1,
        NROTC = 2,
        NUPOC = 3,
        STA21 = 4,
        OTHER = 5
    }
}