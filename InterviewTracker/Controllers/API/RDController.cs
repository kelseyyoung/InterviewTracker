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
    public class RDController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/RD
        public IEnumerable<RD> GetRDs()
        {
            var rd = db.RD.Include(r => r.BioData);
            return rd.AsEnumerable();
        }

        // GET api/RD/5
        public RD GetRD(int id)
        {
            RD rd = db.RD.Find(id);
            if (rd == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return rd;
        }

        // PUT api/RD/5
        public HttpResponseMessage PutRD(int id, RD rd)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != rd.RDID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(rd).State = EntityState.Modified;

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

        // POST api/RD
        public HttpResponseMessage PostRD(RD rd)
        {
            if (ModelState.IsValid)
            {
                db.RD.Add(rd);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, rd);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = rd.RDID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/RD/5
        public HttpResponseMessage DeleteRD(int id)
        {
            RD rd = db.RD.Find(id);
            if (rd == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.RD.Remove(rd);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, rd);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}