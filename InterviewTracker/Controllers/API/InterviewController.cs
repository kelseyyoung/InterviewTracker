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
    public class InterviewController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Interview
        public IEnumerable<Interview> GetInterviews()
        {
            var interview = db.Interview.Include(i => i.CurrentlyEditingUser).Include(i => i.InterviewerUser).Include(i => i.BioData);
            return interview.AsEnumerable();
        }

        // GET api/Interview/5
        public Interview GetInterview(int id)
        {
            Interview interview = db.Interview.Find(id);
            if (interview == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return interview;
        }

        // PUT api/Interview/5
        public HttpResponseMessage PutInterview(int id, Interview interview)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != interview.InterviewID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(interview).State = EntityState.Modified;

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

        // POST api/Interview
        public HttpResponseMessage PostInterview(Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Interview.Add(interview);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, interview);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = interview.InterviewID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Interview/5
        public HttpResponseMessage DeleteInterview(int id)
        {
            Interview interview = db.Interview.Find(id);
            if (interview == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Interview.Remove(interview);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, interview);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}