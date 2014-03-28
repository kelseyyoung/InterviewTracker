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
    public class DutyHistoryController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/DutyHistory
        [ActionName("GetAll")]
        public IEnumerable<DutyHistory> GetDutyHistories()
        {
            var dutyhistory = db.DutyHistory.Include(d => d.BioData);
            return dutyhistory.AsEnumerable();
        }

        // GET api/DutyHistory/5
        [ActionName("Get")]
        public DutyHistory GetDutyHistory(int id)
        {
            DutyHistory dutyhistory = db.DutyHistory.Find(id);
            if (dutyhistory == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dutyhistory;
        }

        // PUT api/DutyHistory/5
        [ActionName("Put")]
        public HttpResponseMessage PutDutyHistory(int id, DutyHistory dutyhistory)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dutyhistory.DutyHistoryID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(dutyhistory).State = EntityState.Modified;

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

        // POST api/DutyHistory
        [ActionName("Post")]
        public HttpResponseMessage PostDutyHistory(DutyHistory dutyhistory)
        {
            if (ModelState.IsValid)
            {
                db.DutyHistory.Add(dutyhistory);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dutyhistory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = dutyhistory.DutyHistoryID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/DutyHistory/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteDutyHistory(int id)
        {
            DutyHistory dutyhistory = db.DutyHistory.Find(id);
            if (dutyhistory == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.DutyHistory.Remove(dutyhistory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dutyhistory);
        }

        // TEST api/DutyHistory/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestDutyHistory([FromUri] DutyHistory dutyHistory)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dutyHistory);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<DutyHistory> GetBy()
        {
            return db.DutyHistory.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}