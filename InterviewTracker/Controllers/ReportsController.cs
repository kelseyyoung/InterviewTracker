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

        public void generateSchoolReport(int id)
        {
            var school = db.School.Find(id);
            string year = DateTime.Today.Year.ToString();
            string fileName = "SchoolSuccessReport_" + school.SchoolValue + "_" + year + ".docx";

            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportStart.html"));

            //Compile and generate report
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false);

        }
        public void generateCandidateReport(int id)
        {
            var bioData = db.BioData.Find(id);

            string fileName = "CandidateReport_" + bioData.FName + " " + bioData.LName + ".docx";
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportStart.html"));
            string nonApplicable = "N/A";

            //Fill in header section with appropriate information
            reportBody = reportBody.Replace("name", bioData.LName + ", " + bioData.FName + " " + bioData.MName + " " + bioData.Suffix);
            reportBody = reportBody.Replace("dateOfBirth", bioData.DOB.ToString().Substring(0, bioData.DOB.ToString().IndexOf(" ")));
            reportBody = reportBody.Replace("source", bioData.Sources.SourcesValue);
            reportBody = reportBody.Replace("fyg", bioData.FYG.Value.ToString());

            string programString = "";
            foreach (Program program in bioData.Programs.ToList())
            {
                if (!programString.Equals("")) programString = programString + ",";
                programString = programString + program.ProgramValue;
            }
            reportBody = reportBody.Replace("program", programString);

            //Add school section
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableStart.html"));
            string row;
            string schoolInfo;
            string transcript = "";
            foreach (SchoolsAttended school in bioData.SchoolsAttended.ToList())
            {
                row = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportSchoolRow.html"));
                int numYears;
                if (school.YearStart != null && school.YearEnd != null)
                    numYears = school.YearEnd.Value - school.YearStart.Value;
                else
                    numYears = 1;
                schoolInfo = school.School.SchoolValue + " (" + numYears.ToString() + " year" + ")";
                if (numYears > 1) schoolInfo = schoolInfo.Replace("year", "years");

                row = row.Replace("schoolInfo", schoolInfo);
                transcript = System.IO.File.ReadAllText(Server.MapPath("~/Templates/TranscriptTableStart.html"));
                transcript = transcript.Replace("schoolInfo", schoolInfo);
                string classRow;
                bool foundTechClass;
                for (int i = 0; i < numYears; ++i)
                {
                    transcript = transcript + System.IO.File.ReadAllText(Server.MapPath("~/Templates/TranscriptTableRowHeader.html"));
                    transcript = transcript.Replace("yearAttended", "Year " + (i + 1) + ": " + (school.YearStart + i).Value.ToString() + " - " + (school.YearStart + i + 1).Value.ToString());

                    foundTechClass = false;
                    foreach (ClassesAttended ca in school.ClassesAttended.ToList())
                    {
                        if (ca.YearTaken == (i + 1) && ca.Classes.Technical == true)
                        {
                            foundTechClass = true;
                            classRow = System.IO.File.ReadAllText(Server.MapPath("~/Templates/TranscriptTableRow.html"));
                            classRow = classRow.Replace("subject", ca.Classes.Subject);
                            classRow = classRow.Replace("code", ca.Classes.Code);
                            classRow = classRow.Replace("name", ca.Classes.Name);
                            transcript = transcript + classRow;
                        }
                    }
                    if (!foundTechClass)
                    {
                        classRow = "<tr><td><i>No technical classes found for this year</i></td></tr>";
                        transcript = transcript + classRow;
                    }
                }
                transcript = transcript + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));

                string degreeInfo = "";
                if (school.Graduated == true)
                {
                    foreach (Degree degree in school.Degrees.ToList())
                    {
                        if (!degreeInfo.Equals("")) degreeInfo = degreeInfo + "; ";
                        degreeInfo = degreeInfo + degree.DegreeType.DegreeTypeValue + " " + degree.Major.MajorValue;
                    }
                    row = row.Replace("degreeInfo", degreeInfo);
                }
                reportBody = reportBody + row;


            }
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));

            //Add Duty History section
            string dhEntry;
            string grade;
            foreach (DutyHistory dh in bioData.DutyHistories.ToList())
            {
                dhEntry = System.IO.File.ReadAllText(Server.MapPath("~/Templates/DutyHistoryTableStart.html"));

                if (dh.NUC == true)
                    dhEntry = dhEntry.Replace("rank", dh.Rank + " (NUC)");
                else
                    dhEntry = dhEntry.Replace("rank", dh.Rank);

                foreach (DutyStation ds in dh.DutyStations.ToList())
                {
                    row = System.IO.File.ReadAllText(Server.MapPath("~/Templates/DutyStationRow.html"));
                    row = row.Replace("date", ds.ReportDate.ToShortDateString() + " - " + ds.DepartDate.ToShortDateString());
                    row = row.Replace("station", "Duty Station " + ds.DutyStationID.ToString());
                    row = row.Replace("duties", "<br>" + ds.Duties);

                    grade = "";
                    if (ds.RankInRateVal != null && ds.RankInRateTot != null)
                        grade = grade + ds.RankInRateVal.Value.ToString() + "/" + ds.RankInRateTot.Value.ToString() + " (in-rate)";
                    if (ds.RankOverallVal != null && ds.RankOverallTot != null)
                    {
                        if (!grade.Equals("")) grade = grade + "<br>";
                        grade = grade + ds.RankOverallVal.Value.ToString() + "/" + ds.RankOverallTot.Value.ToString() + " (overall)";
                    }
                    if (ds.GPA != null)
                    {
                        if (!grade.Equals("")) grade = grade + "<br>";
                        grade = grade + "GPA: " + ds.GPA.Value.ToString();
                    }
                    row = row.Replace("grade", grade);

                    dhEntry = dhEntry + row;
                }

                dhEntry = dhEntry + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
                reportBody = reportBody + dhEntry;

            }

            //Tack on the previously constructed transcript
            reportBody = reportBody + transcript;

            //Compile and generate report
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false);
        }

        public void generateInterviewResults(String date)
        {
            date = date.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
            System.Diagnostics.Debug.WriteLine(date);
            DateTime dt = DateTime.ParseExact(date, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string fileName = "InterviewResults_" + dt.ToShortDateString().Replace("/", "-") + ".docx";
            System.Diagnostics.Debug.WriteLine(fileName);

            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/InterviewResultsTableStart.html"));

            reportBody = reportBody.Replace("date", dt.ToLongDateString());

            string row;
            //TO DO: only put the relevant interviews into list, not all of them
            foreach (Interview interview in db.Interview.ToList())
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
                foreach (SchoolsAttended sa in bioData.SchoolsAttended.ToList())
                {
                    if (!schoolInfo.Equals(""))
                    {
                        schoolInfo = schoolInfo + ";\n";
                    }
                    schoolInfo = schoolInfo + sa.School.SchoolValue;

                    foreach(Degree d in sa.Degrees.ToList())
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

                string resultsString = "";
                if (interview.NR != null)
                {
                    if (interview.NR.Value) resultsString = resultsString + "Yes-NR DUTY";
                    else resultsString = resultsString + "No-NR DUTY";
                }
                if (interview.INST != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.INST.Value) resultsString = resultsString + "Yes-INST";
                    else resultsString = resultsString + "No-INST";
                }
                if (interview.NPS != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.NPS.Value) resultsString = resultsString + "Yes-NPS";
                    else resultsString = resultsString + "No-NPS";
                }
                if (interview.PXO != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.PXO.Value) resultsString = resultsString + "Yes-PXO";
                    else resultsString = resultsString + "No-PXO";
                }
                if (interview.EDO != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.EDO.Value) resultsString = resultsString + "Yes-EDO";
                    else resultsString = resultsString + "No-EDO";
                }
                if (interview.ENLTECH != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.ENLTECH.Value) resultsString = resultsString + "Yes-ENLTECH";
                    else resultsString = resultsString + "No-ENLTECH";
                }
                if (interview.NR1 != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.NR1.Value) resultsString = resultsString + "Yes-NR1";
                    else resultsString = resultsString + "No-NR1";
                }
                if (interview.SUPPLY != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.SUPPLY.Value) resultsString = resultsString + "Yes-SUPPLY";
                    else resultsString = resultsString + "No-SUPPLY";
                }
                if (interview.EOOW != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.EOOW.Value) resultsString = resultsString + "Yes-EOOW";
                    else resultsString = resultsString + "No-EOOW";
                }
                if (interview.DOE != null)
                {
                    if (!resultsString.Equals("")) resultsString = resultsString + "; ";
                    if (interview.DOE.Value) resultsString = resultsString + "Yes-DOE";
                    else resultsString = resultsString + "No-DOE";
                }
                row = row.Replace("results", resultsString);

                reportBody = reportBody + row;
            }
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false);
        }

        public void generateFYReport(String year)
        {
            string fileName = year + "_FYReport.docx";
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FYReportTable.html"));

            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, true);
        }

        public void generateSATACTReport(String startFYG, String endFYG)
        {
            int start;
            int end;
            try
            {
                start = Convert.ToInt32(startFYG);
                end = Convert.ToInt32(endFYG);
            }
            catch (Exception e)
            {
                end = DateTime.Today.Year;
                start = end;
                endFYG = end.ToString();
                startFYG = endFYG;
            }

            string fileName = "SAT-ACT Scores FYG " + startFYG + "-" + endFYG;
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = "<p><b>SAT/ACT Scores from FYG " + startFYG + " to FYG " + endFYG + "</b></p> <table style=\"width:600px\">";
            footer = "</table>" + footer;
            string nonApplicable = "";

            string nextEntry;
            foreach (BioData bioData in db.BioData.Where(data => data.FYG.Value >= start && data.FYG <= end))
            {
                nextEntry = System.IO.File.ReadAllText(Server.MapPath("~/Templates/SAT-ACTTable.html"));
                nextEntry = nextEntry.Replace("ssn", bioData.SSN);
                nextEntry = nextEntry.Replace("name", bioData.LName + ", " + bioData.FName);
                nextEntry = nextEntry.Replace("fyg", bioData.FYG.Value.ToString());

                if (bioData.SATM != null)
                    nextEntry = nextEntry.Replace("satm", bioData.SATM.Value.ToString());
                else
                    nextEntry = nextEntry.Replace("satm", nonApplicable);
                if (bioData.SATV != null)
                    nextEntry = nextEntry.Replace("satv", bioData.SATV.Value.ToString());
                else
                    nextEntry = nextEntry.Replace("satv", nonApplicable);
                if (bioData.ACTM != null)
                    nextEntry = nextEntry.Replace("actm", bioData.ACTM.Value.ToString());
                else
                    nextEntry = nextEntry.Replace("actm", nonApplicable);
                if (bioData.ACTV != null)
                    nextEntry = nextEntry.Replace("actv", bioData.ACTV.Value.ToString());
                else
                    nextEntry = nextEntry.Replace("actv", nonApplicable);

                reportBody = reportBody + nextEntry;
            }
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false);
        }

        public void generateReport(String filename, String html, bool landscape)
        {
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
                        //to process an image you must provide a base 

                        //converter.BaseImageUrl = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority);

                        Body body = mainPart.Document.Body;

                        if (landscape)
                        {
                            SectionProperties properties = new SectionProperties();
                            PageSize pageSize = new PageSize()
                            {
                                Width = (UInt32Value)15840U,
                                Height = (UInt32Value)12240U,
                                Orient = PageOrientationValues.Landscape
                            };
                            properties.Append(pageSize);
                            body.Append(properties);

                            SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                            ParagraphProperties paragraphProperties = new ParagraphProperties();
                            Paragraph paragraph = new Paragraph();

                            paragraphProperties.Append(spacing);
                            paragraph.Append(paragraphProperties);
                            body.Append(paragraph);
                         }

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
