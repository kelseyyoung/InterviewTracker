using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class User
    {
        public int UserID { get; set; } // PK for User table
        public string LName { get; set; } // Last name
        public string Initials { get; set; } // Initials
        public string LoginID { get; set; } // NR Login ID
        public string Password { get; set; } // Password for application
        public string Code { get; set; } // Division/section code
        //TODO: QUALS
        public UserGroup UserGroup { get; set; } // Type of user
    }

}

public enum UserGroup
{
    ADMIN,
    COORD,
    INTER
}