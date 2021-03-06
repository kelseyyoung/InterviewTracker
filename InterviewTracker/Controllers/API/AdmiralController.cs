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
    public class AdmiralController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Admiral
        [ActionName("GetAll")]
        public IEnumerable<Admiral> GetAdmirals()
        {
            var admiral = db.Admiral.Include(a => a.BioData).Include(a => a.Program);
            return admiral.AsEnumerable();
        }

        // GET api/Admiral/5
        [ActionName("Get")]
        public Admiral GetAdmiral(int id)
        {
            Admiral admiral = db.Admiral.Find(id);
            if (admiral == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return admiral;
        }

        // PUT api/Admiral/5
        [ActionName("Put")]
        public HttpResponseMessage PutAdmiral(int id, [FromUri] Admiral admiral)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != admiral.AdmiralID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(admiral).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, admiral);
        }

        // POST api/Admiral
        [ActionName("Post")]
        public HttpResponseMessage PostAdmiral([FromUri] Admiral admiral)
        {
            if (ModelState.IsValid)
            {
                db.Admiral.Add(admiral);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, admiral);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = admiral.AdmiralID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Admiral/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteAdmiral(int id)
        {
            Admiral admiral = db.Admiral.Find(id);
            if (admiral == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Admiral.Remove(admiral);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, admiral);
        }

        // TEST api/Admiral/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestAdmiral([FromUri] Admiral admiral)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, admiral);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Admiral> GetBy()
        {
            return db.Admiral.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}