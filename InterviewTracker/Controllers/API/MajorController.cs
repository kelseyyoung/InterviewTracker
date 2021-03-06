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
    public class MajorController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Major
        [ActionName("GetAll")]
        public IEnumerable<Major> GetMajors()
        {
            return db.Major.AsEnumerable();
        }

        // GET api/Major/5
        [ActionName("Get")]
        public Major GetMajor(int id)
        {
            Major major = db.Major.Find(id);
            if (major == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return major;
        }

        // PUT api/Major/5
        [ActionName("Put")]
        public HttpResponseMessage PutMajor(int id, [FromUri] Major major)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != major.MajorID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(major).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, major);
        }

        // POST api/Major
        [ActionName("Post")]
        public HttpResponseMessage PostMajor([FromUri] Major major)
        {
            if (ModelState.IsValid)
            {
                db.Major.Add(major);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, major);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = major.MajorID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Major/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteMajor(int id)
        {
            Major major = db.Major.Find(id);
            if (major == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Major.Remove(major);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, major);
        }

        // TEST api/Major/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestMajor([FromUri] Major major)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, major);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<Major> GetBy()
        {
            return db.Major.AsQueryable();
        }

        [ActionName("GetOrCreate")]
        [HttpPost]
        public HttpResponseMessage GetOrCreate(string MajorValue)
        {
            Major major = db.Major.Where(x => x.MajorValue == MajorValue).FirstOrDefault();
            if (major == null)
            {
                major = new Major { MajorValue = MajorValue };
                db.Major.Add(major);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, major);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, major);
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}