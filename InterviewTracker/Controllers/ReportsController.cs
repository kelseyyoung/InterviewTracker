using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;
using InterviewTracker.Models;
using InterviewTracker.DAL;

namespace InterviewTracker.Controllers
{

    public class ReportsController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        //Temporary variables just to create working solution off sample; Plan to update or remove later
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.Label lblFeedback;

        //
        // GET: /Reports/

        public ActionResult Index()
        {
            return View();
        }

        public void generateCandidateReport(/*int bioID*/)
        {
            var bioData = db.BioData.Find(1);
            //var bioData = db.BioData.Where(x => x.ID == 1).First();
           
            string fileName = Server.MapPath("~/CandidateReport_" + bioData.FName + " " + bioData.LName + ".docx");
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReport.html"));
            string nonApplicable = "N/A";

            reportBody = reportBody.Replace("name", bioData.FName + " " + bioData.MName + " " + bioData.LName + " " + bioData.Suffix);
            reportBody = reportBody.Replace("dateOfBirth", bioData.DOB.ToString().Substring(0, bioData.DOB.ToString().IndexOf(" ")));
            reportBody = reportBody.Replace("id", bioData.ID.ToString());
            reportBody = reportBody.Replace("sex", bioData.Sex.ToString());
            reportBody = reportBody.Replace("ssn", bioData.SSN);
            reportBody = reportBody.Replace("ethnicity", bioData.Ethnicity.EthnicityValue);
            reportBody = reportBody.Replace("fyg", bioData.FYG.ToString());
            reportBody = reportBody.Replace("source", bioData.Sources.SourcesValue);
            
            if(bioData.SubSources != null)
                reportBody = reportBody.Replace("subSource", bioData.SubSources.SubSourcesValue);
            else
                reportBody = reportBody.Replace("subSource", nonApplicable);

            if(bioData.ACTM != null)
                reportBody = reportBody.Replace("actm", bioData.ACTM.Value.ToString());
            else
                reportBody = reportBody.Replace("actm", nonApplicable);

            if(bioData.ACTV != null)
                reportBody = reportBody.Replace("actv", bioData.ACTV.Value.ToString());
            else
                reportBody = reportBody.Replace("actv", nonApplicable);

            if(bioData.SATM != null)
                reportBody = reportBody.Replace("satm", bioData.SATM.Value.ToString());
            else
                reportBody = reportBody.Replace("satm", nonApplicable);
            
            if(bioData.SATV != null)
                reportBody = reportBody.Replace("satv", bioData.SATV.Value.ToString());
            else
                reportBody = reportBody.Replace("satv", nonApplicable);

            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml);
        }

        public void generateInterviewResults(String date)
        {
            date = date.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
            System.Diagnostics.Debug.WriteLine(date);
            DateTime dt = DateTime.ParseExact(date, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string fileName = Server.MapPath("~/InterviewResults_" + dt.ToShortDateString().Replace("/", "-")  + ".docx");
            System.Diagnostics.Debug.WriteLine(fileName);

            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/InterviewResultsTableStart.html"));

            string row;
            //TO DO: only put the relevant interviews into list, not all of them
            foreach(Interview interview in db.Interview.ToList())
            {
                BioData bioData = interview.BioData;
                row = System.IO.File.ReadAllText(Server.MapPath("~/Templates/InterviewResultsRow.html"));
                row = row.Replace("source", bioData.Sources.SourcesValue);
                row = row.Replace("last name", bioData.LName);
                row = row.Replace("ex", bioData.Suffix);
                row = row.Replace("first name", bioData.FName);

                //How should this report deal with multiple schools/degrees?
                string schoolInfo = "";
                string degreeInfo = "";
                foreach(SchoolsAttended sa in bioData.SchoolsAttended.ToList())
                {
                    if(!schoolInfo.Equals(""))
                    {
                        schoolInfo = schoolInfo + ";\n";
                    }
                    schoolInfo = schoolInfo + sa.School.SchoolValue;
                    foreach(Degree d in sa.Degree.ToList())
                    {
                        if (!degreeInfo.Equals(""))
                        {
                            degreeInfo = degreeInfo + ";\n";
                        }
                        degreeInfo = degreeInfo + d.Major.MajorValue + "(" + d.DegreeType.DegreeTypeValue + ")";
                    }
                }

                row = row.Replace("university", schoolInfo);
                row = row.Replace("cla", bioData.FYG.Value.ToString());
                row = row.Replace("major", degreeInfo);
                //Where should we pull "results" from? 
                row = row.Replace("results", "IDK");

                reportBody = reportBody + row;
            }
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml);
        }

        public void generateFYReport(String year)
        {
            string fileName = Server.MapPath("~/"+ year + "FYReport" + ".docx");
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = "<h1>Test h1</h1><p>Look at this, I can write paragraphs and shit " + year + "</p>";
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml);
        }

        public void generateReport(String filename, String html)
        {
            try
            {
                if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);

                using (MemoryStream generatedDocument = new MemoryStream())
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new Body()).Save(mainPart);
                        }

                        HtmlConverter converter = new HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        var paragraphs = converter.Parse(html);
                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            body.Append(paragraphs[i]);
                        }

                        mainPart.Document.Save();
                    }
                    byte[] bytesInStream = generatedDocument.ToArray();
                    System.IO.File.WriteAllBytes(filename, bytesInStream);
                    //System.IO.File.WriteAllBytes(filename, generatedDocument.ToArray());
                }

                //This will open word. comment if not needed
                System.Diagnostics.Process.Start(filename);

                //TODO: if you want to stream the word document, instead of saving the file first, use another implementation

               // lblError.Visible = false;
               // lblFeedback.Visible = true;
            }
            catch (Exception ex)
            {
               // lblError.Text = "Error: " + ex.Message + " (see exception details)";
               // lblError.Visible = true;
               // lblFeedback.Visible = false;
            }
            try
            {
                string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

                using (MemoryStream generatedDocument = new MemoryStream())
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new Body()).Save(mainPart);
                        }

                        HtmlConverter converter = new HtmlConverter(mainPart);

                        //http://html2openxml.codeplex.com/wikipage?title=ImageProcessing&referringTitle=Documentation
                        //to process an image you must provide a base url
                        //converter.BaseImageUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority);

                        Body body = mainPart.Document.Body;

                        var paragraphs = converter.Parse(html);
                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            body.Append(paragraphs[i]);
                        }

                        mainPart.Document.Save();
                    }

                    byte[] bytesInStream = generatedDocument.ToArray(); // simpler way of converting to array
                    generatedDocument.Close();

                    Response.Clear();
                    Response.ContentType = contentType;
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                    //this will generate problems
                    Response.BinaryWrite(bytesInStream);
                    try
                    {
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        //Response.End(); generates an exception. if you don't use it, you get some errors when Word opens the file...
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
