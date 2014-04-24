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
    public class FYGoalsController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/FYGoals
        [ActionName("GetAll")]
        public IEnumerable<FYGoals> GetFYGoals()
        {
            return db.FYGoals.AsEnumerable();
        }

        // GET api/FYGoals/5
        [ActionName("Get")]
        public FYGoals GetFYGoals(int id)
        {
            FYGoals fygoals = db.FYGoals.Find(id);
            if (fygoals == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return fygoals;
        }

        // PUT api/FYGoals/5
        [ActionName("Put")]
        public HttpResponseMessage PutFYGoals(int id, [FromUri] FYGoals fygoals)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != fygoals.FYID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(fygoals).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, fygoals);
        }

        // POST api/FYGoals
        [ActionName("Post")]
        public HttpResponseMessage PostFYGoals([FromUri] FYGoals fygoals)
        {
            if (ModelState.IsValid)
            {
                db.FYGoals.Add(fygoals);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, fygoals);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = fygoals.FYID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/FYGoals/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteFYGoals(int id)
        {
            FYGoals fygoals = db.FYGoals.Find(id);
            if (fygoals == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.FYGoals.Remove(fygoals);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, fygoals);
        }

        // TEST api/FYGoals/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestFYGoals([FromUri] FYGoals fygoals)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fygoals);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<FYGoals> GetBy()
        {
            return db.FYGoals.AsQueryable();
        }

        [ActionName("GetByYearAndSource")]
        [HttpGet]
        public FYGoals GetByYearAndSource(int FY, string Source)
        {
            return db.FYGoals.Where(y => y.FY == FY).Where(x => x.Source == Source).FirstOrDefault();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}