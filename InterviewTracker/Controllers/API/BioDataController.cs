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
    public class BioDataController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/BioData
        [ActionName("GetAll")]
        public IEnumerable<BioData> GetBioDatas()
        {
            var biodata = db.BioData.Include(b => b.Ethnicity).Include(b => b.Sources).Include(b => b.SubSources);
            return biodata.AsEnumerable();
        }

        // GET api/BioData/5
        [ActionName("Get")]
        public BioData GetBioData(int id)
        {
            BioData biodata = db.BioData.Find(id);
            if (biodata == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return biodata;
        }

        // PUT api/BioData/5
        [ActionName("Put")]
        public HttpResponseMessage PutBioData(int id, [FromUri] BioData biodata)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != biodata.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(biodata).State = EntityState.Modified;

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

        // POST api/BioData
        [ActionName("Post")]
        public HttpResponseMessage PostBioData([FromUri] BioData biodata)
        {
            if (ModelState.IsValid)
            {
                db.BioData.Add(biodata);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, biodata);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = biodata.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/BioData/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteBioData(int id)
        {
            BioData biodata = db.BioData.Find(id);
            if (biodata == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.BioData.Remove(biodata);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, biodata);
        }

        // TEST api/BioData/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestBioData([FromUri] BioData biodata)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, biodata);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<BioData> GetBy()
        {
            return db.BioData.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}