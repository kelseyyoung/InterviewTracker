﻿using InterviewTracker.DAL;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InterviewTracker.Filters
{
    public class CustomAuthAttribute : ActionFilterAttribute
    {

        private readonly string[] _acceptedRoles = { };

        public CustomAuthAttribute()
        {

        }

        public CustomAuthAttribute(params string[] acceptedRoles)
        {
            _acceptedRoles = acceptedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InterviewTrackerContext db = new InterviewTrackerContext();
            User user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();
            if (user == null)
            {
                // Redirect to Unauth page
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary { { "controller", "Home" }, { "action", "Unauthorized" } });
                return;
            }
            if (_acceptedRoles.Length != 0 && !_acceptedRoles.Contains(user.UserGroup))
            {
                // Redirect to unauthorized
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary { { "controller", "Home" }, { "action", "Unauthorized" } });
            }
        }
    }
}