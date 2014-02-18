using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string LName { get; set; }
        public string Initials { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        //TODO: QUALS
        public UserGroup UserGroup { get; set; }
    }

    public enum UserGroup {
        ADMIN = 1,
        COORD = 2,
        INTER = 3
    }
}