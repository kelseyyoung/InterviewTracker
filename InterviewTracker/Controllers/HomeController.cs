using InterviewTracker.DAL;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class HomeController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        public ActionResult Index()
        {
            ViewBag.candidates = db.BioData.ToList();
            return View();
        }
    }
}
