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
    public class DutyStationController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/DutyStation
        [ActionName("GetAll")]
        public IEnumerable<DutyStation> GetDutyStations()
        {
            var dutystation = db.DutyStation.Include(d => d.DutyHistory);
            return dutystation.AsEnumerable();
        }

        // GET api/DutyStation/5
        [ActionName("Get")]
        public DutyStation GetDutyStation(int id)
        {
            DutyStation dutystation = db.DutyStation.Find(id);
            if (dutystation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dutystation;
        }

        // PUT api/DutyStation/5
        [ActionName("Put")]
        public HttpResponseMessage PutDutyStation(int id, [FromUri] DutyStation dutystation)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dutystation.DutyStationID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(dutystation).State = EntityState.Modified;

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

        // POST api/DutyStation
        [ActionName("Post")]
        public HttpResponseMessage PostDutyStation([FromUri] DutyStation dutystation)
        {
            if (ModelState.IsValid)
            {
                db.DutyStation.Add(dutystation);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dutystation);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = dutystation.DutyStationID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/DutyStation/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteDutyStation(int id)
        {
            DutyStation dutystation = db.DutyStation.Find(id);
            if (dutystation == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.DutyStation.Remove(dutystation);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dutystation);
        }

        // TEST api/DutyStation/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestDutyStation([FromUri] DutyStation dutyStation)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, dutyStation);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<DutyStation> GetBy()
        {
            return db.DutyStation.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}