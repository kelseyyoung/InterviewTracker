﻿using System;
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
    public class EthnicityController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Ethnicity
        [ActionName("GetAll")]
        public IEnumerable<Ethnicity> GetEthnicities()
        {
            return db.Ethnicity.AsEnumerable();
        }

        // GET api/Ethnicity/5
        [ActionName("Get")]
        public Ethnicity GetEthnicity(int id)
        {
            Ethnicity ethnicity = db.Ethnicity.Find(id);
            if (ethnicity == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return ethnicity;
        }

        // PUT api/Ethnicity/5
        [ActionName("Put")]
        public HttpResponseMessage PutEthnicity(int id, [FromUri] Ethnicity ethnicity)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != ethnicity.EthnicityID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(ethnicity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ethnicity);
        }

        // POST api/Ethnicity
        [ActionName("Post")]
        public HttpResponseMessage PostEthnicity([FromUri] Ethnicity ethnicity)
        {
            if (ModelState.IsValid)
            {
                db.Ethnicity.Add(ethnicity);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, ethnicity);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = ethnicity.EthnicityID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Ethnicity/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteEthnicity(int id)
        {
            Ethnicity ethnicity = db.Ethnicity.Find(id);
            if (ethnicity == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Ethnicity.Remove(ethnicity);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ethnicity);
        }

        // TEST api/Ethnicity/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestEthnicity([FromUri] Ethnicity ethnicity)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ethnicity);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Ethnicity> GetBy()
        {
            return db.Ethnicity.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}