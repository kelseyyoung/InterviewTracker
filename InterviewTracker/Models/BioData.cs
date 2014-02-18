using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class BioData
    {
        public int ID { get; set; }
        public int SSN { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string? Suffix { get; set; }
        public DateTime DOB { get; set; }
        public Sex Sex { get; set; }
        public List<Program> Programs { get; set; } //TODO: change
        public int UnitIDID { get; set; }
        public virtual UnitID UnitID { get; set; } //TODO: no unit ID
        public int FYG { get; set; }
        public int ACTM { get; set; }
        public int ACTV { get; set; }
        public int SATM { get; set; }
        public int SATV { get; set; }

        [ForeignKey("Ethnicity")]
        public int EthnicityID { get; set; }
        [ForeignKey("Source")]
        public int SourceID { get; set; }
        [ForeignKey("SubSource")]
        public int SubSourceID { get; set; }
    }

    public enum Sex
    {
        M = 1,
        F = 2
    }
}