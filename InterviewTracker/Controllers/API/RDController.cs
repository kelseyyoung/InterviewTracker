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
        [ActionName("GetAll")]
        public IEnumerable<RD> GetRDs()
        {
            var rd = db.RD.Include(r => r.BioData);
            //var rd = db.RD;
            return rd.AsEnumerable();
        }

        // GET api/RD/5
        [ActionName("Get")]
        public RD GetRD(int id)
        {
            RD rd = db.RD.Find(id);
            //RD rd = db.RD.Where(x => x.RDID == id).First();
            if (rd == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return rd;
        }

        // PUT api/RD/5
        [ActionName("Put")]
        public HttpResponseMessage PutRD(int id, [FromUri] RD rd)
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

            return Request.CreateResponse(HttpStatusCode.OK, rd);
        }

        // POST api/RD
        [ActionName("Post")]
        public HttpResponseMessage PostRD([FromUri] RD rd)
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
        [ActionName("Delete")]
        public HttpResponseMessage DeleteRD(int id)
        {
            RD rd = db.RD.Find(id);
            //RD rd = db.RD.Where(x => x.RDID == id).First();
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

        // TEST api/RD/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestRD([FromUri] RD rd)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rd);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<RD> GetBy()
        {
            return db.RD.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}