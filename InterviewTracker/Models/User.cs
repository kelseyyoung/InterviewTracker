using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class User
    {
        public int UserID { get; set; } // PK for User table
        [Required]
        public string LName { get; set; } // Last name
        [Required]
        public string Initials { get; set; } // Initials
        [Required]
        public string LoginID { get; set; } // NR Login ID
        [Required]
        public string Password { get; set; } // Password for application
        // TODO: this shouldn't be just a string
        [Required]
        public string Code { get; set; } // Division/section code
        // Quals
        public bool NR { get; set; } // Recommended for NR duty
        public bool INST { get; set; } // Recommended for Instructor duty
        public bool NPS { get; set; } // Recommended for Nuclear Power school duty
        public bool PXO { get; set; } // Recommended for Prospective XO
        public bool EDO { get; set; } // Recommended for Engineering Duty Officer
        public bool ENLTECH { get; set; } // Recommended for Enlisted Tech
        public bool NR1 { get; set; } // Recommended for NR-1 duty
        public bool SUPPLY { get; set; } // Recommended for NR duty (supply)
        public bool EOOW { get; set; } // Recommended for Engineering Officer of the Watch
        public bool DOE { get; set; } // Recommended for Field Office
        [Required]
        public string UserGroup { get; set; } // Type of user
    }

}

public enum UserGroup
{
    ADMIN = 1,
    COORD = 2,
    INTER = 3
}