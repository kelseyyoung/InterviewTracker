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
    public class SchoolController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/School
        public IEnumerable<School> GetSchools()
        {
            return db.School.AsEnumerable();
        }

        // GET api/School/5
        public School GetSchool(int id)
        {
            School school = db.School.Find(id);
            if (school == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return school;
        }

        // PUT api/School/5
        public HttpResponseMessage PutSchool(int id, School school)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != school.SchoolID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(school).State = EntityState.Modified;

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

        // POST api/School
        public HttpResponseMessage PostSchool(School school)
        {
            if (ModelState.IsValid)
            {
                db.School.Add(school);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, school);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = school.SchoolID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/School/5
        public HttpResponseMessage DeleteSchool(int id)
        {
            School school = db.School.Find(id);
            if (school == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.School.Remove(school);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, school);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}