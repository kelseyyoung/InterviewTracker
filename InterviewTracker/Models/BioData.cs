using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class BioData
    {
        [Key]
        public int ID { get; set; } // PK for BioData table
        public int SSN { get; set; } // Social Security #
        public string LName { get; set; } // Last Name
        public string FName { get; set; } // First Name
        public string MName { get; set; } // Middle Name
        public string? Suffix { get; set; } // Suffix (optional)
        public DateTime DOB { get; set; } // Date of Birth
        public Sex Sex { get; set; } // Sex (Options: M, F)
        public List<Program> Programs { get; set; } // List of programs being applied for
        public int? UnitID { get; set; } // Unit ID (optional)
        public int FYG { get; set; } // Fiscal Year Group
        public int? ACTM { get; set; } // ACT Math Score
        public int? ACTV { get; set; } // ACT Verbal Score
        public int? SATM { get; set; } // SAT Math Score
        public int? SATV { get; set; } // SAT Verbal Score

        [ForeignKey("Ethnicity")]
        public int EthnicityID { get; set; } // FK to Ethnicity table
        [ForeignKey("Source")]
        public int SourceID { get; set; } // FK to Source table
        [ForeignKey("SubSource")]
        public int? SubSourceID { get; set; } // FK to SubSource table (optional)

        /*
         * Collections of data auto populated when a row contains this BioID FK
         */
        public virtual ICollection<SchoolsAttended> SchoolsAttended { get; set; } // Collection of schools attended
        public virtual ICollection<ClassesAttended> ClassesAttended { get; set; } // Collection of classes attended
        public virtual ICollection<DutyHistory> DutyHistories { get; set; } // Collection of duty histories
        public virtual ICollection<Interview> Interviews { get; set; } // Collection of interviews
        public virtual ICollection<RD> RDs { get; set; } // Collection of RDs
        public virtual ICollection<Screen> Screens { get; set; } // Collection of screens
        public virtual ICollection<Waiver> Waivers { get; set; } // Collection of waivers
    }

    public enum Sex
    {
        M = 1,
        F = 2
    }
}