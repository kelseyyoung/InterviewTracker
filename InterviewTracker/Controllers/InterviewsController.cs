using InterviewTracker.DAL;
using InterviewTracker.Filters;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
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
            ViewBag.user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();

            Interview interview = db.Interview.Find(id);
            ViewBag.programsList = new string[] {"NR", "INST", "NPS", "PXO", "EDO", "ENLTECH", "NR1", "SUPPLY", "EOOW", "DOE"};
            List<string> biodataPrograms = new List<string>();
            foreach(Program p in interview.BioData.Programs) {
                biodataPrograms.Add(p.ProgramValue);
            }
            ViewBag.biodataPrograms = biodataPrograms;

            // If interviewer and this isn't your interview
            // Redirect to unauth
            if (ViewBag.user.UserGroup == "INTER" && ViewBag.user.UserID != interview.InterviewerID)
            {
                return RedirectToAction("Unauthorized", "Home");
            }

            ViewBag.interview = interview;

            // Set CurrentlyEditingID in interview
            interview.CurrentlyEditingID = ViewBag.user.UserID;
            db.Entry(interview).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Shouldn't happen
                // TODO: make global error page?
            }
            return View();
        }
    }
}
