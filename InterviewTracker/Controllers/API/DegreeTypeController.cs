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
    public class DegreeTypeController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/DegreeType
        public IEnumerable<DegreeType> GetDegreeTypes()
        {
            return db.DegreeType.AsEnumerable();
        }

        // GET api/DegreeType/5
        public DegreeType GetDegreeType(int id)
        {
            DegreeType degreetype = db.DegreeType.Find(id);
            if (degreetype == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return degreetype;
        }

        // PUT api/DegreeType/5
        public HttpResponseMessage PutDegreeType(int id, DegreeType degreetype)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != degreetype.DegreeTypeID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(degreetype).State = EntityState.Modified;

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

        // POST api/DegreeType
        public HttpResponseMessage PostDegreeType(DegreeType degreetype)
        {
            if (ModelState.IsValid)
            {
                db.DegreeType.Add(degreetype);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, degreetype);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = degreetype.DegreeTypeID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/DegreeType/5
        public HttpResponseMessage DeleteDegreeType(int id)
        {
            DegreeType degreetype = db.DegreeType.Find(id);
            if (degreetype == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.DegreeType.Remove(degreetype);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, degreetype);
        }

        // TEST api/DegreeType/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestDegreeType([FromUri] DegreeType degreeType)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, degreeType);
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