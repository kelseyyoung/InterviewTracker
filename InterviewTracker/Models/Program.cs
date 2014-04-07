using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Program
    {
        public int ProgramID { get; set; } // PK for Program table
        [Required]
        public string ProgramValue { get; set; } // Type of program
        [JsonIgnore]
        public virtual ICollection<BioData> BioDatas { get; set; }
        [JsonIgnore]
        public virtual ICollection<Screen> Screens { get; set; }
    }
}