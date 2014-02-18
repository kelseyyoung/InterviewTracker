using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class FYGoals
    {
        public int FY { get; set; }
        public Source Source { get; set; }
        public int SUB { get; set; }
        public int SWO { get; set; }
        public int NR { get; set; }
        public int INST { get; set; }
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