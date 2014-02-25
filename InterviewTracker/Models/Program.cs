using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Program
    {
        public int ProgramID { get; set; } // PK for Program table
        public string ProgramValue { get; set; } // Type of program
        public virtual ICollection<BioData> BioDatas { get; set; }
        public virtual ICollection<Screen> Screens { get; set; }
    }
}