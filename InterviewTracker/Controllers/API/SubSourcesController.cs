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
    public class SubSourcesController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/SubSources
        public IEnumerable<SubSources> GetSubSources()
        {
            return db.SubSources.AsEnumerable();
        }

        // GET api/SubSources/5
        public SubSources GetSubSources(int id)
        {
            SubSources subsources = db.SubSources.Find(id);
            if (subsources == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return subsources;
        }

        // PUT api/SubSources/5
        public HttpResponseMessage PutSubSources(int id, SubSources subsources)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != subsources.SubSourcesID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(subsources).State = EntityState.Modified;

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

        // POST api/SubSources
        public HttpResponseMessage PostSubSources(SubSources subsources)
        {
            if (ModelState.IsValid)
            {
                db.SubSources.Add(subsources);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, subsources);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = subsources.SubSourcesID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SubSources/5
        public HttpResponseMessage DeleteSubSources(int id)
        {
            SubSources subsources = db.SubSources.Find(id);
            if (subsources == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SubSources.Remove(subsources);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, subsources);
        }

        // TEST api/SubSources/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestSubSources([FromUri] SubSources subsources)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, subsources);
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