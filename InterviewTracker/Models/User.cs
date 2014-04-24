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
        [Required(ErrorMessage="The Last Name field is required")]
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
        public bool NR { get; set; } // Can interview for NR duty
        public bool INST { get; set; } // Can interview for Instructor duty
        public bool NPS { get; set; } // Can interview for Nuclear Power school duty
        public bool PXO { get; set; } // Can interview for Prospective XO
        public bool EDO { get; set; } // Can interview for Engineering Duty Officer
        public bool ENLTECH { get; set; } // Can interview for Enlisted Tech
        public bool NR1 { get; set; } // Can interview for NR-1 duty
        public bool SUPPLY { get; set; } // Can interview for NR duty (supply)
        public bool EOOW { get; set; } // Can interview for Engineering Officer of the Watch
        public bool DOE { get; set; } // Can interview for Field Office
        [Required(ErrorMessage="The User Group field is required")]
        public string UserGroup { get; set; } // Type of user
    }

}

public enum UserGroup
{
    ADMIN = 1,
    COORD = 2,
    INTER = 3
}