using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using InterviewTracker.Models;
using InterviewTracker.DAL;

namespace InterviewTracker.Controllers.API
{
    public class ScreenController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Screen
        [ActionName("GetAll")]
        public IEnumerable<Screen> GetScreens()
        {
            var screen = db.Screen.Include(s => s.BioData);
            return screen.AsEnumerable();
        }

        // GET api/Screen/5
        [ActionName("Get")]
        public Screen GetScreen(int id)
        {
            Screen screen = db.Screen.Find(id);
            if (screen == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return screen;
        }

        // PUT api/Screen/5
        [ActionName("Put")]
        public HttpResponseMessage PutScreen(int id, Screen screen)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != screen.ScreenID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(screen).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Screen
        [ActionName("Post")]
        public HttpResponseMessage PostScreen(Screen screen)
        {
            if (ModelState.IsValid)
            {
                db.Screen.Add(screen);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, screen);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = screen.ScreenID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Screen/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteScreen(int id)
        {
            Screen screen = db.Screen.Find(id);
            if (screen == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Screen.Remove(screen);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, screen);
        }

        // TEST api/Screen/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestScreen([FromUri] Screen screen)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, screen);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Screen> GetBy()
        {
            return db.Screen.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}