﻿using System;
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
    public class SchoolStandingsController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/SchoolStandings
        [ActionName("GetAll")]
        public IEnumerable<SchoolStandings> GetSchoolStandings()
        {
            var schoolstandings = db.SchoolStandings.Include(s => s.SchoolsAttended);
            return schoolstandings.AsEnumerable();
        }

        // GET api/SchoolStandings/5
        [ActionName("Get")]
        public SchoolStandings GetSchoolStandings(int id)
        {
            SchoolStandings schoolstandings = db.SchoolStandings.Find(id);
            if (schoolstandings == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return schoolstandings;
        }

        // PUT api/SchoolStandings/5
        [ActionName("Put")]
        public HttpResponseMessage PutSchoolStandings(int id, [FromUri] SchoolStandings schoolstandings)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != schoolstandings.SchoolStandingsID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(schoolstandings).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, schoolstandings);
        }

        // POST api/SchoolStandings
        [ActionName("Post")]
        public HttpResponseMessage PostSchoolStandings([FromUri] SchoolStandings schoolstandings)
        {
            if (ModelState.IsValid)
            {
                db.SchoolStandings.Add(schoolstandings);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, schoolstandings);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = schoolstandings.SchoolStandingsID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SchoolStandings/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteSchoolStandings(int id)
        {
            SchoolStandings schoolstandings = db.SchoolStandings.Find(id);
            if (schoolstandings == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SchoolStandings.Remove(schoolstandings);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, schoolstandings);
        }

        // TEST api/SchoolStandings/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestSchoolStandings([FromUri] SchoolStandings schoolStandings)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, schoolStandings);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<SchoolStandings> GetBy()
        {
            return db.SchoolStandings.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}