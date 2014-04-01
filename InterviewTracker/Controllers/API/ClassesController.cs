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
    public class ClassesController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Classes
        [ActionName("GetAll")]
        public IEnumerable<Classes> GetClasses()
        {
            return db.Classes.AsEnumerable();
        }

        // GET api/Classes/5
        [ActionName("Get")]
        public Classes GetClasses(int id)
        {
            Classes classes = db.Classes.Find(id);
            if (classes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return classes;
        }

        // PUT api/Classes/5
        [ActionName("Put")]
        public HttpResponseMessage PutClasses(int id, [FromUri] Classes classes)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != classes.ClassesID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(classes).State = EntityState.Modified;

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

        // POST api/Classes
        [ActionName("Post")]
        public HttpResponseMessage PostClasses([FromUri] Classes classes)
        {
            if (ModelState.IsValid)
            {
                db.Classes.Add(classes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, classes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = classes.ClassesID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Classes/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteClasses(int id)
        {
            Classes classes = db.Classes.Find(id);
            if (classes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Classes.Remove(classes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, classes);
        }

        // TEST api/Classes/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestClasses([FromUri] Classes classes)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, classes);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Classes> GetBy()
        {
            return db.Classes.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}