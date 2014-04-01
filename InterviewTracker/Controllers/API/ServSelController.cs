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
    public class ServSelController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/ServSel
        [ActionName("GetAll")]
        public IEnumerable<ServSel> GetServSels()
        {
            return db.ServSel.AsEnumerable();
        }

        // GET api/ServSel/5
        [ActionName("Get")]
        public ServSel GetServSel(int id)
        {
            ServSel servsel = db.ServSel.Find(id);
            if (servsel == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return servsel;
        }

        // PUT api/ServSel/5
        [ActionName("Put")]
        public HttpResponseMessage PutServSel(int id, [FromUri] ServSel servsel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != servsel.ServSelID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(servsel).State = EntityState.Modified;

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

        // POST api/ServSel
        [ActionName("Post")]
        public HttpResponseMessage PostServSel([FromUri] ServSel servsel)
        {
            if (ModelState.IsValid)
            {
                db.ServSel.Add(servsel);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, servsel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = servsel.ServSelID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/ServSel/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteServSel(int id)
        {
            ServSel servsel = db.ServSel.Find(id);
            if (servsel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ServSel.Remove(servsel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, servsel);
        }

        // TEST api/ServSel/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestServSel([FromUri] ServSel servSel)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, servSel);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<ServSel> GetBy()
        {
            return db.ServSel.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}