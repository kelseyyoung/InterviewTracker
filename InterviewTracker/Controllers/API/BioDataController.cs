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
using System.Web.Http.ModelBinding;

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

            return Request.CreateResponse(HttpStatusCode.OK, biodata);
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

            // Remove all entities with foreign keys first
            
            // Screen
            db.Screen.Where(x => x.BioDataID == id).ToList().ForEach(y => db.Screen.Remove(y));
            // Waiver
            db.Waiver.Where(x => x.BioDataID == id).ToList().ForEach(y => db.Waiver.Remove(y));
            // RD
            db.RD.Where(x => x.BioDataID == id).ToList().ForEach(y => db.RD.Remove(y));
            // Admiral
            db.Admiral.Where(x => x.BioDataID == id).ToList().ForEach(y => db.Admiral.Remove(y));
            // ClassesAttended
            db.ClassesAttended.Where(x => x.BioDataID == id).ToList().ForEach(y => db.ClassesAttended.Remove(y));
            // Duty History and Duty Station
            List<DutyHistory> dhs = db.DutyHistory.Where(x => x.BioDataID == id).ToList();
            foreach (var dh in dhs)
            {
                db.DutyStation.Where(x => x.DutyHistoryID == dh.DutyHistoryID).ToList().ForEach(y => db.DutyStation.Remove(y));
                db.DutyHistory.Remove(dh);
            }
            // Interview
            db.Interview.Where(x => x.BioDataID == id).ToList().ForEach(y => db.Interview.Remove(y));
            // SchoolsAttended, Degree, SchoolStandings
            List<SchoolsAttended> sas = db.SchoolsAttended.Where(x => x.BioDataID == id).ToList();
            foreach (var sa in sas)
            {
                db.Degree.Where(x => x.SchoolsAttendedID == sa.SchoolsAttendedID).ToList().ForEach(y => db.Degree.Remove(y));
                db.SchoolStandings.Where(x => x.SchoolsAttendedID == sa.SchoolsAttendedID).ToList().ForEach(y => db.SchoolStandings.Remove(y));
                db.SchoolsAttended.Remove(sa);
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

        [ActionName("SetPrograms")]
        [HttpPost]
        public HttpResponseMessage SetPrograms(int id, [ModelBinder] List<string> BiodataPrograms)
        {
            BioData biodata = db.BioData.Find(id);

            if (biodata == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // Clear out programs
            biodata.Programs.Clear();

            // Add new programs
            foreach (string pid in BiodataPrograms)
            {
                Program p = db.Program.Find(Convert.ToInt32(pid));
                biodata.Programs.Add(p);
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

            return Request.CreateResponse(HttpStatusCode.Created, biodata);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}