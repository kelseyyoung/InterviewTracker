using InterviewTracker.DAL;
using InterviewTracker.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace InterviewTracker.Controllers
{
    public class AdminController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        [CustomAuth("COORD", "ADMIN")]
        public ActionResult AddUser()
        {
            int[] temp = new int[5];
            int length = temp[8];
            ViewBag.user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();

            ViewBag.users = db.User.ToList().OrderBy(x => x.LoginID);
            return View();
        }

        [CustomAuth("COORD", "ADMIN")]
        public ActionResult EditFYGoals()
        {
            ViewBag.user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();

            ViewBag.fys = db.FYGoals.OrderByDescending(x => x.FY).Select(
                x => x.FY).Distinct();
            return View();
        }
    }
}
