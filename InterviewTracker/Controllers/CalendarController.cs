using InterviewTracker.DAL;
using InterviewTracker.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class CalendarController : Controller
    {

        private InterviewTrackerContext db = new InterviewTrackerContext();

        //
        // GET: /Calendar/

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;
            CustomPrincipal u = ViewBag.currUser;
            ViewBag.userModel = db.User.Where(x => x.LoginID == u.LoginID).FirstOrDefault();

            ViewBag.candidates = db.BioData.ToList();
            ViewBag.interviewers = db.User.ToList();
            ViewBag.startTime = 6;
            ViewBag.endTime = 18;
            return View();
        }

    }
}
