using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class UserContextInitializer : DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            var users = new List<User>()
            {
                new User() { LName = "young", Initials = "kj", LoginID = "youngkj", Password = "password", Code = "3E", UserGroup = UserGroup.COORD },
                new User() { LName = "byrnes", Initials = "da", LoginID = "byrnesda", Password = "password", Code = "3E", UserGroup = UserGroup.ADMIN },
            };
        }
    }
}