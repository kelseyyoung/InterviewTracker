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
        public IEnumerable<Degree> GetDegrees()
        {
            var degree = db.Degree.Include(d => d.SchoolsAttended).Include(d => d.Major).Include(d => d.DegreeType);
            return degree.AsEnumerable();
        }

        // GET api/Degree/5
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
        public HttpResponseMessage PutDegree(int id, Degree degree)
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
        public HttpResponseMessage PostDegree(Degree degree)
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}