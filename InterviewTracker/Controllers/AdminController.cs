using InterviewTracker.DAL;
using InterviewTracker.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class AdminController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        [CustomAuth("COORD", "ADMIN")]
        public ActionResult AddUser()
        {
            return View();
        }

        [CustomAuth("COORD", "ADMIN")]
        public ActionResult EditFYGoals()
        {
            ViewBag.sources = db.Sources.ToList();
            return View();
        }
    }
}
