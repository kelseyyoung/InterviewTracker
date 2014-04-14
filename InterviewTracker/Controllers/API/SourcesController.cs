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
    public class SourcesController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Sources
        [ActionName("GetAll")]
        public IEnumerable<Sources> GetSources()
        {
            return db.Sources.AsEnumerable();
        }

        // GET api/Sources/5
        [ActionName("Get")]
        public Sources GetSources(int id)
        {
            Sources sources = db.Sources.Find(id);
            if (sources == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return sources;
        }

        // PUT api/Sources/5
        [ActionName("Put")]
        public HttpResponseMessage PutSources(int id, [FromUri] Sources sources)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != sources.SourcesID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(sources).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, sources);
        }

        // POST api/Sources
        [ActionName("Post")]
        public HttpResponseMessage PostSources([FromUri] Sources sources)
        {
            if (ModelState.IsValid)
            {
                db.Sources.Add(sources);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, sources);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = sources.SourcesID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Sources/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteSources(int id)
        {
            Sources sources = db.Sources.Find(id);
            if (sources == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Sources.Remove(sources);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, sources);
        }

        // TEST api/Sources/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestSources([FromUri] Sources sources)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sources);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Sources> GetBy()
        {
            return db.Sources.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}