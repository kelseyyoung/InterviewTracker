using InterviewTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class InterviewsController : Controller
    {

        private InterviewTrackerContext db = new InterviewTrackerContext();

        public ActionResult Edit(int id)
        {
            ViewBag.interview = db.Interview.Find(id);
            ViewBag.otherInterviews = "";
            return View();
        }
    }
}
