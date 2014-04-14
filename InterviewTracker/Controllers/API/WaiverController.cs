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
    public class WaiverController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Waiver
        [ActionName("GetAll")]
        public IEnumerable<Waiver> GetWaivers()
        {
            var waiver = db.Waiver.Include(w => w.BioData);
            return waiver.AsEnumerable();
        }

        // GET api/Waiver/5
        [ActionName("Get")]
        public Waiver GetWaiver(int id)
        {
            Waiver waiver = db.Waiver.Find(id);
            if (waiver == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return waiver;
        }

        // PUT api/Waiver/5
        [ActionName("Put")]
        public HttpResponseMessage PutWaiver(int id, [FromUri] Waiver waiver)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != waiver.WaiverID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(waiver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, waiver);
        }

        // POST api/Waiver
        [ActionName("Post")]
        public HttpResponseMessage PostWaiver([FromUri] Waiver waiver)
        {
            if (ModelState.IsValid)
            {
                db.Waiver.Add(waiver);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, waiver);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = waiver.WaiverID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Waiver/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteWaiver(int id)
        {
            Waiver waiver = db.Waiver.Find(id);
            if (waiver == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Waiver.Remove(waiver);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, waiver);
        }

        // TEST api/Waiver/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestWaiver([FromUri] Waiver waiver)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, waiver);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Waiver> GetBy()
        {
            return db.Waiver.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}