using InterviewTracker.DAL;
using InterviewTracker.Filters;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InterviewTracker.Controllers
{
    public class HomeController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

    }
}
