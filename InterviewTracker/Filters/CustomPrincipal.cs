using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;

namespace InterviewTracker.Filters
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return this.Role == role;
        }

        public bool IsInRole(string[] roles)
        {
            if (roles.Length == 0)
            {
                return true;
            }
            return roles.Contains(this.Role);
        }

        public CustomPrincipal(string loginID, string role)
        {
            this.LoginID = loginID;
            this.Role = role;
            this.Identity = new GenericIdentity("");
        }

        public string LoginID { get; set; }
        public string Role { get; set; }
    }
}