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
        public string Suffix { get; set; } // Suffix (optional) **Don't need ?, is already nullable
        public DateTime DOB { get; set; } // Date of Birth
        public Sex Sex { get; set; } // Sex (Options: M, F)
        public virtual ICollection<Program> Programs { get; set; } // List of programs being applied for
        public int? UnitID { get; set; } // Unit ID (optional)
        public int FYG { get; set; } // Fiscal Year Group
        public int? ACTM { get; set; } // ACT Math Score (optional)
        public int? ACTV { get; set; } // ACT Verbal Score (optional)
        public int? SATM { get; set; } // SAT Math Score (optional)
        public int? SATV { get; set; } // SAT Verbal Score (optional)

        [ForeignKey("Ethnicity")]
        public virtual int EthnicityID { get; set; } // FK to Ethnicity table
        public virtual Ethnicity Ethnicity { get; set; }
        [ForeignKey("Sources")]
        public virtual int SourcesID { get; set; } // FK to Source table
        public virtual Sources Sources { get; set; }
        [ForeignKey("SubSources")]
        public virtual int? SubSourcesID { get; set; } // FK to SubSource table (optional)
        public virtual SubSources SubSources { get; set; }

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

}

public enum Sex
{
    M = 1,
    F = 2
}