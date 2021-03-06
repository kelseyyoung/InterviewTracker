﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class FYGoals
    {
        [Key]
        public int FYID { get; set; } // PK for FYGoals table
        [Required]
        public int FY { get; set; } // FYG this entry belongs to
        [Required]
        public string Source { get; set; } // Commissioning source
        [Required]
        public int? SUB { get; set; } // Submarine goal
        [Required]
        public int? SUBF { get; set; } // Submarine Female goal
        [Required]
        public int? SWO { get; set; } // Surface goal
        [Required]
        public int? NR { get; set; } // NR Duty goal
        [Required]
        public int? INST { get; set; } // Instructor Duty goal
    }
}

public enum FYSource
{
    USNA = 1,
    NROTC = 2,
    NUPOC = 3,
    STA21 = 4,
    OTHER = 5
}