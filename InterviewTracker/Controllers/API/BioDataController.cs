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
    public class BioDataController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/BioData
        public IEnumerable<BioData> GetBioDatas()
        {
            var biodata = db.BioData.Include(b => b.Ethnicity).Include(b => b.Sources).Include(b => b.SubSources);
            return biodata.AsEnumerable();
        }

        // GET api/BioData/5
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
        public HttpResponseMessage PutBioData(int id, BioData biodata)
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
        public HttpResponseMessage PostBioData(BioData biodata)
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}