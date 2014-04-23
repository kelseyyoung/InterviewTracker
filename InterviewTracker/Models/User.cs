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
        [Required(ErrorMessage="The Login ID field is required")]
        public string LoginID { get; set; } // NR Login ID
        //[Required]
        //public string Password { get; set; } // Password for application
        [Required]
        public string Code { get; set; } // Division/section code
        // Quals
        [Required(ErrorMessage="The NR qual is required")]
        public bool? NR { get; set; } // Can interview for NR duty
        [Required(ErrorMessage="The INST qual is required")]
        public bool? INST { get; set; } // Can interview for Instructor duty
        [Required(ErrorMessage="The NPS qual is required")]
        public bool? NPS { get; set; } // Can interview for Nuclear Power school duty
        [Required(ErrorMessage="The PXO qual is required")]
        public bool? PXO { get; set; } // Can interview for Prospective XO
        [Required(ErrorMessage="The EDO qual is required")]
        public bool? EDO { get; set; } // Can interview for Engineering Duty Officer
        [Required(ErrorMessage="The ENLTECH qual is required")]
        public bool? ENLTECH { get; set; } // Can interview for Enlisted Tech
        [Required(ErrorMessage="The NR1 qual is required")]
        public bool? NR1 { get; set; } // Can interview for NR-1 duty
        [Required(ErrorMessage="The SUPPLY qual is required")]
        public bool? SUPPLY { get; set; } // Can interview for NR duty (supply)
        [Required(ErrorMessage="The EOOW qual is required")]
        public bool? EOOW { get; set; } // Can interview for Engineering Officer of the Watch
        [Required(ErrorMessage="The DOE qual is required")]
        public bool? DOE { get; set; } // Can interview for Field Office
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