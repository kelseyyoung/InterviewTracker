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
        public IEnumerable<Waiver> GetWaivers()
        {
            var waiver = db.Waiver.Include(w => w.BioData);
            return waiver.AsEnumerable();
        }

        // GET api/Waiver/5
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
        public HttpResponseMessage PutWaiver(int id, Waiver waiver)
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

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Waiver
        public HttpResponseMessage PostWaiver(Waiver waiver)
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}