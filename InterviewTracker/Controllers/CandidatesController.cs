using InterviewTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class CandidatesController : Controller
    {

        private InterviewTrackerContext db = new InterviewTrackerContext();

        //
        // GET: /Candidates/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.ethnicities = db.Ethnicity.ToList();
            ViewBag.sources = db.Sources.ToList();
            ViewBag.subsources = db.SubSources.ToList();
            ViewBag.programs = db.Program.ToList();
            ViewBag.majors = db.Major.ToList();
            ViewBag.degreeTypes = db.DegreeType.ToList();
            ViewBag.schools = db.School.ToList();
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

    }
}
