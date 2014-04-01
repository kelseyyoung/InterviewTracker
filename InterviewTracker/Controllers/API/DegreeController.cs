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
    public class DegreeController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Degree
        [ActionName("GetAll")]
        public IEnumerable<Degree> GetDegrees()
        {
            var degree = db.Degree.Include(d => d.SchoolsAttended).Include(d => d.Major).Include(d => d.DegreeType);
            return degree.AsEnumerable();
        }

        // GET api/Degree/5
        [ActionName("Get")]
        public Degree GetDegree(int id)
        {
            Degree degree = db.Degree.Find(id);
            if (degree == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return degree;
        }

        // PUT api/Degree/5
        [ActionName("Put")]
        public HttpResponseMessage PutDegree(int id, [FromUri] Degree degree)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != degree.DegreeID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(degree).State = EntityState.Modified;

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

        // POST api/Degree
        [ActionName("Post")]
        public HttpResponseMessage PostDegree([FromUri] Degree degree)
        {
            if (ModelState.IsValid)
            {
                db.Degree.Add(degree);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, degree);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = degree.DegreeID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Degree/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteDegree(int id)
        {
            Degree degree = db.Degree.Find(id);
            if (degree == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Degree.Remove(degree);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, degree);
        }

        // TEST api/Degree/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestDegree([FromUri] Degree degree)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, degree);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Degree> GetBy()
        {
            return db.Degree.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}