using InterviewTracker.DAL;
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

        public ActionResult Index()
        {
            ViewBag.candidates = db.BioData.ToList();
            ViewBag.interviewers = db.User.ToList();
            ViewBag.startTime = 0;
            ViewBag.endTime = 23;
            return View();
        }

    }
}
