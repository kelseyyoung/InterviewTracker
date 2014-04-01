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
    public class InterviewController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Interview
        [ActionName("GetAll")]
        public IEnumerable<Interview> GetInterviews()
        {
            var interview = db.Interview.Include(i => i.CurrentlyEditingUser).Include(i => i.InterviewerUser).Include(i => i.BioData);
            return interview.AsEnumerable();
        }

        // GET api/Interview/5
        [ActionName("Get")]
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
        [ActionName("Put")]
        public HttpResponseMessage PutInterview(int id, [FromUri] Interview interview)
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
        [ActionName("Post")]
        public HttpResponseMessage PostInterview([FromUri] Interview interview)
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
        [ActionName("Delete")]
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
        // TEST api/Interview/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestInterview([FromUri] Interview interview)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, interview);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Interview> GetBy()
        {
            return db.Interview.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}