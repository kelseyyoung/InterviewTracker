using InterviewTracker.DAL;
using InterviewTracker.Filters;
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

            var interview = db.Interview.Find(id);
            ViewBag.interview = interview;

            // Set CurrentlyEditingID in interview
            interview.CurrentlyEditingID = ViewBag.userModel.UserID;
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
