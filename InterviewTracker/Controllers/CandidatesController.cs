using InterviewTracker.DAL;
using InterviewTracker.Filters;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InterviewTracker.Controllers
{
    public class CandidatesController : Controller
    {

        private InterviewTrackerContext db = new InterviewTrackerContext();

        //
        // GET: /Candidates/

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            ViewBag.ethnicities = db.Ethnicity.ToList();
            ViewBag.sources = db.Sources.ToList();
            ViewBag.subsources = db.SubSources.ToList();
            ViewBag.programs = db.Program.ToList();
            return View();
        }

        [CustomAuth]
        public ActionResult Success()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            return View();
        }

        [CustomAuth("COORD", "ADMIN")]
        public ActionResult Create()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            ViewBag.ethnicities = db.Ethnicity.ToList();
            ViewBag.sources = db.Sources.ToList();
            ViewBag.subsources = db.SubSources.ToList();
            ViewBag.programs = db.Program.ToList();
            ViewBag.majors = db.Major.ToList();
            ViewBag.degreeTypes = db.DegreeType.ToList();
            ViewBag.schools = db.School.ToList();
            return View();
        }

        private JsonResult getDHSelectValues(ICollection<DutyHistory> DHList)
        {
            var retValue = DHList.Select(
                x => new
                {
                    id = x.DutyHistoryID,
                    branchValue = x.Branch,
                    NUCVAlue = x.NUC
                }).ToArray();
            return Json(retValue);
        }

        private JsonResult getSchoolSelectValues(ICollection<SchoolsAttended> SchoolList)
        {
            var retValue = SchoolList.Select(
                x => new
                {
                    id = x.SchoolsAttendedID,
                    major = x.Degrees.First().Major.MajorValue,
                    degreeType = x.Degrees.First().DegreeType.DegreeTypeValue,
                    graduated = x.Graduated
                }).ToArray();
            return Json(retValue);
        }

        private JsonResult getProgramSelectValues(ICollection<Program> programList)
        {
            var retValue = programList.Select(
                x => new
                {
                    value = x.ProgramValue,
                }).ToArray();
            return Json(retValue);
        }

        [CustomAuth]
        public ActionResult View(int id)
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            ViewBag.bioData = db.BioData.Find(id);

            ViewBag.ethnicities = db.Ethnicity.ToList();
            ViewBag.sources = db.Sources.ToList();
            ViewBag.subsources = db.SubSources.ToList();
            ViewBag.programs = db.Program.ToList();
            ViewBag.majors = db.Major.ToList();
            ViewBag.degreeTypes = db.DegreeType.ToList();
            ViewBag.schools = db.School.ToList();
            ViewBag.dutyStations = db.DutyStation.ToList();

            ViewBag.schoolsAttended = ViewBag.bioData.SchoolsAttended;

            ViewBag.dutyHistories = ViewBag.bioData.DutyHistories;
            ViewBag.classesAttended = ViewBag.bioData.ClassesAttended;
            ViewBag.RDs = ViewBag.bioData.RDs;
            ViewBag.waivers = ViewBag.bioData.Waivers;
            ViewBag.screens = ViewBag.bioData.Screens;

            return View();
        }

        [CustomAuth("ADMIN", "COORD")]
        public ActionResult Edit(int id)
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

            ViewBag.bioData = db.BioData.Find(id);     

            ViewBag.ethnicities = db.Ethnicity.ToList();
            ViewBag.sources = db.Sources.ToList();
            ViewBag.subsources = db.SubSources.ToList();
            ViewBag.programs = db.Program.ToList();
            ViewBag.majors = db.Major.ToList();
            ViewBag.degreeTypes = db.DegreeType.ToList();
            ViewBag.schools = db.School.ToList();
            ViewBag.dutyStations = db.DutyStation.ToList();
           
            ViewBag.schoolsAttended = ViewBag.bioData.SchoolsAttended;
            ViewBag.programsToSelect = ViewBag.bioData.Programs;

            if (ViewBag.bioData.SubSources != null)
                ViewBag.subsourcePreload = ViewBag.bioData.SubSources;
            else
                ViewBag.subsourcePreload = db.SubSources.First();

            ViewBag.dutyHistories = ViewBag.bioData.DutyHistories;
            ViewBag.classesAttended = ViewBag.bioData.ClassesAttended;
            ViewBag.RDs = ViewBag.bioData.RDs;
            ViewBag.waivers = ViewBag.bioData.Waivers;
            ViewBag.screens = ViewBag.bioData.Screens;

            ViewBag.schoolCount = ViewBag.schoolsAttended.Count;
            int dsCount = 0;
            foreach (var dh in ViewBag.dutyHistories)
            {
                dsCount += dh.DutyStations.Count;
            }
            ViewBag.dutyStationsCount = dsCount;
            ViewBag.waiversCount = ViewBag.waivers.Count;
            ViewBag.screensCount = ViewBag.screens.Count;
            ViewBag.rdCount = ViewBag.RDs.Count;

            var DHselects = getDHSelectValues(ViewBag.dutyHistories);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewBag.dhSelectValues = serializer.Serialize(DHselects.Data);

            var schoolSelects = getSchoolSelectValues(ViewBag.schoolsAttended);
            JavaScriptSerializer serializer2 = new JavaScriptSerializer();
            ViewBag.schoolSelectValues = serializer2.Serialize(schoolSelects.Data);

            var programSelects = getProgramSelectValues(ViewBag.programsToSelect);
            ViewBag.programSelectValues = serializer.Serialize(programSelects.Data);

            return View();
        }

    }
}
