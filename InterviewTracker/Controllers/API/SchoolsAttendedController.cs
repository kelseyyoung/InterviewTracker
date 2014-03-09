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
    public class SchoolsAttendedController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/SchoolsAttended
        public IEnumerable<SchoolsAttended> GetSchoolsAttendeds()
        {
            var schoolsattended = db.SchoolsAttended.Include(s => s.BioData).Include(s => s.School);
            return schoolsattended.AsEnumerable();
        }

        // GET api/SchoolsAttended/5
        public SchoolsAttended GetSchoolsAttended(int id)
        {
            SchoolsAttended schoolsattended = db.SchoolsAttended.Find(id);
            if (schoolsattended == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return schoolsattended;
        }

        // PUT api/SchoolsAttended/5
        public HttpResponseMessage PutSchoolsAttended(int id, SchoolsAttended schoolsattended)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != schoolsattended.SchoolsAttendedID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(schoolsattended).State = EntityState.Modified;

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

        // POST api/SchoolsAttended
        public HttpResponseMessage PostSchoolsAttended(SchoolsAttended schoolsattended)
        {
            if (ModelState.IsValid)
            {
                db.SchoolsAttended.Add(schoolsattended);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, schoolsattended);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = schoolsattended.SchoolsAttendedID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SchoolsAttended/5
        public HttpResponseMessage DeleteSchoolsAttended(int id)
        {
            SchoolsAttended schoolsattended = db.SchoolsAttended.Find(id);
            if (schoolsattended == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SchoolsAttended.Remove(schoolsattended);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, schoolsattended);
        }

        // TEST api/SchoolsAttended/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestSchoolsAttended([FromUri] SchoolsAttended schoolsAttended)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, schoolsAttended);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}