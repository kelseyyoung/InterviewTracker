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
        //TODO: QUALS
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