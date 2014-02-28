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
    public class ProgramController : ApiController
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        // GET api/Program
        public IEnumerable<Program> GetPrograms()
        {
            return db.Program.AsEnumerable();
        }

        // GET api/Program/5
        public Program GetProgram(int id)
        {
            Program program = db.Program.Find(id);
            if (program == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return program;
        }

        // PUT api/Program/5
        public HttpResponseMessage PutProgram(int id, Program program)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != program.ProgramID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(program).State = EntityState.Modified;

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

        // POST api/Program
        public HttpResponseMessage PostProgram(Program program)
        {
            if (ModelState.IsValid)
            {
                db.Program.Add(program);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, program);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = program.ProgramID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Program/5
        public HttpResponseMessage DeleteProgram(int id)
        {
            Program program = db.Program.Find(id);
            if (program == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Program.Remove(program);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, program);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}