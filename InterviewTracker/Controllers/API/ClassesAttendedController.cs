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
    public class ClassesAttendedController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/ClassesAttended
        [ActionName("GetAll")]
        public IEnumerable<ClassesAttended> GetClassesAttendeds()
        {
            var classesattended = db.ClassesAttended.Include(c => c.SchoolsAttended).Include(c => c.BioData).Include(c => c.Classes);
            return classesattended.AsEnumerable();
        }

        // GET api/ClassesAttended/5
        [ActionName("Get")]
        public ClassesAttended GetClassesAttended(int id)
        {
            ClassesAttended classesattended = db.ClassesAttended.Find(id);
            if (classesattended == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return classesattended;
        }

        // PUT api/ClassesAttended/5
        [ActionName("Put")]
        public HttpResponseMessage PutClassesAttended(int id, ClassesAttended classesattended)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != classesattended.ClassesAttendedID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(classesattended).State = EntityState.Modified;

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

        // POST api/ClassesAttended
        [ActionName("Post")]
        public HttpResponseMessage PostClassesAttended(ClassesAttended classesattended)
        {
            if (ModelState.IsValid)
            {
                db.ClassesAttended.Add(classesattended);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, classesattended);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = classesattended.ClassesAttendedID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/ClassesAttended/5
        [ActionName("Delete")]
        public HttpResponseMessage DeleteClassesAttended(int id)
        {
            ClassesAttended classesattended = db.ClassesAttended.Find(id);
            if (classesattended == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ClassesAttended.Remove(classesattended);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, classesattended);
        }

        // TEST api/ClassesAttended/test
        [ActionName("Test")]
        [HttpPost]
        public HttpResponseMessage TestClassesAttended([FromUri] ClassesAttended classesAttended)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, classesAttended);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [ActionName("GetBy")]
        [HttpGet]
        [Queryable]
        public IQueryable<ClassesAttended> GetBy()
        {
            return db.ClassesAttended.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}