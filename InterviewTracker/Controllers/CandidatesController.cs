using InterviewTracker.DAL;
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
                    major = x.Degree.First().Major.MajorValue,
                    degreeType = x.Degree.First().DegreeType.DegreeTypeValue,
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

        public ActionResult Edit()
        {
            ViewBag.bioData = db.BioData.Find(2);

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
