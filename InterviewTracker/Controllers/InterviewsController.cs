using InterviewTracker.DAL;
using InterviewTracker.Filters;
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

        [CustomAuth]
        public ActionResult Edit(int id)
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            var interview = db.Interview.Find(id);
            ViewBag.interview = interview;
            return View();
        }
    }
}
