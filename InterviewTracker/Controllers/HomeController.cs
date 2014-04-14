using InterviewTracker.DAL;
using InterviewTracker.Filters;
using InterviewTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InterviewTracker.Controllers
{
    public class HomeController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;
            ViewBag.candidates = db.BioData.ToList();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form["username"];
            User u = db.User.Where(x => x.LoginID == username).FirstOrDefault();
            if (u == null)
            {
                // User does not exist
                return RedirectToAction("Unauthorized", "Home");
            }
            // User exists in DB
            // Create Ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
             u.LoginID,
             DateTime.Now,
             DateTime.Now.AddMinutes(10), //TODO: set to actual minutes?
             false,
             u.UserGroup);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult Logout()
        {
            // Create Logout Ticket
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
             "Logout",
             DateTime.Now,
             DateTime.Now.AddMinutes(5), //TODO: set to actual minutes?
             false,
             "");
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
            return View();
        }
    }
}
