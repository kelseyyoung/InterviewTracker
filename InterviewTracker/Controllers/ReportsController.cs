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
using InterviewTracker.Filters;
using System.Web.Helpers;

namespace InterviewTracker.Controllers
{

    public class ReportsController : Controller
    {
        private InterviewTrackerContext db = new InterviewTrackerContext();

        //Temporary variables just to create working solution off sample; Plan to update or remove later
        protected System.Web.UI.WebControls.Label lblError;
        protected System.Web.UI.WebControls.Label lblFeedback;

        public string pageBreakParagraph = "<p>%$%lineBreak%$%</p>";
        //
        // GET: /Reports/

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.user = db.User.Where(x => x.LoginID == System.Environment.UserName).FirstOrDefault();
            ViewBag.schools = db.School.OrderBy(x=> x.SchoolValue).ToList();
            ViewBag.candidates = db.BioData.OrderBy(x => x.LName).ThenBy(y => y.FName).ToList();

            return View();
        }

        public void generateSchoolReport(int id)
        {
            var school = db.School.Find(id);
            string year = DateTime.Today.Year.ToString();
            string fileName = "SchoolSuccessReport_" + school.SchoolValue + "_" + year + ".docx";

            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/SchoolReport.html"));

            header = header.Replace("<title></title>", "<style>ul.a {list-style-type:square;}ul.b {list-style-type:circle;}</style>");

            reportBody = reportBody.Replace("schoolTitle", school.SchoolValue);
            reportBody = reportBody.Replace("dateOfReport", DateTime.Today.ToString("dd MMMM yyyy"));

            //Filter down to only candidates from this school who have completed the interview process
            var alumniCandidates = db.Admiral.Where(x => x.BioData.SchoolsAttended.Any(a => a.SchoolID == id));

            int numCandidatesInterviewed = alumniCandidates.Count();
            reportBody = reportBody.Replace("numCandidatesInterviewed", numCandidatesInterviewed.ToString());

            //Get counts for each source for all interviewed candidates
            int nupocCount = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "NUPOC").Count();
            int nrotcCount = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "NRTOC").Count();
            int sta21nCount = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "STA21N").Count();

            if (nupocCount > 0)
                reportBody = reportBody.Replace("nupocCount", nupocCount.ToString());
            else
                reportBody = reportBody.Replace("<li>NUPOC:&#9; nupocCount</li>", "");

            if (nrotcCount > 0)
                reportBody = reportBody.Replace("nrotcCount", nrotcCount.ToString());
            else
                reportBody = reportBody.Replace("<li>NROTC:&#9; nrotcCount</li>", "");

            if (sta21nCount > 0)
                reportBody = reportBody.Replace("sta21nCount", sta21nCount.ToString());
            else
                reportBody = reportBody.Replace("<li>STA21N:&#9; sta21nCount</li>", "");

            //Filter down to only accepted candidates
            alumniCandidates = alumniCandidates.Where(b => b.Decision == true && b.Accepted == true);

            //Filter into lists for each source
            var nupocAlumni = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "NUPOC");
            var nrotcAlumni = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "NRTOC");
            var sta21nAlumni = alumniCandidates.Where(y => y.BioData.Sources.SourcesValue == "STA21N");

            //Calculate number of candidates selected for each source
            int allSelectedCount = alumniCandidates.Count();
            nupocCount = nupocAlumni.Count();
            nrotcCount = nrotcAlumni.Count();
            sta21nCount = sta21nAlumni.Count();

            reportBody = reportBody.Replace("numCandidatesSelected", allSelectedCount.ToString());

            float percentage;
            if (numCandidatesInterviewed > 0)
                percentage = ((float)(allSelectedCount) / (float)(numCandidatesInterviewed)) * 100;
            else
                percentage = 0;

            reportBody = reportBody.Replace("selectedPercent", percentage.ToString("0.00") + "%");

            //Fill total candidates selected for each source
            reportBody = reportBody.Replace("numNupoc", nupocCount.ToString());
            reportBody = reportBody.Replace("numNrotc", nrotcCount.ToString());
            reportBody = reportBody.Replace("numSta21n", sta21nCount.ToString());

            string[] programs = { "SUB", "SWO", "INST", "NR" };
            string currentProgram;
            int nupoc, nrotc, sta21n;
            int nupocTotal = 0, nrotcTotal = 0, sta21nTotal = 0;

            for (int i = 0; i < programs.Length; ++i)
            {
                currentProgram = programs[i];
                nupoc = nupocAlumni.Where(b => b.Program.ProgramValue == currentProgram).Count();
                nrotc = nrotcAlumni.Where(b => b.Program.ProgramValue == currentProgram).Count();
                sta21n = nrotcAlumni.Where(b => b.Program.ProgramValue == currentProgram).Count();

                reportBody = reportBody.Replace("nupoc" + currentProgram, nupoc.ToString());
                reportBody = reportBody.Replace("nrotc" + currentProgram, nrotc.ToString());
                reportBody = reportBody.Replace("sta21n" + currentProgram, sta21n.ToString());

                nupocTotal += nupoc;
                nrotcTotal += nrotc;
                sta21nTotal += sta21n;
            }

            if (nupocTotal < nupocCount)
                reportBody = reportBody.Replace("nupocNo", (nupocCount - nupocTotal).ToString());
            else
                reportBody = reportBody.Replace("<li>No Contract:&#9; nupocNo</li>", "");

            if (nrotcTotal < nrotcCount)
                reportBody = reportBody.Replace("nrotcNo", (nrotcCount - nrotcTotal).ToString());
            else
                reportBody = reportBody.Replace("<li>No Contract:&#9; nrotcNo</li>", "");

            if (sta21nTotal < sta21nCount)
                reportBody = reportBody.Replace("sta21nNo", (sta21nCount - sta21nTotal).ToString());
            else
                reportBody = reportBody.Replace("<li>No Contract:&#9; sta21nNo</li>", "");

            //Compile and generate report
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);

        }

        public void generateCandidateReport(int id)
        {
            int[] idArray = new int[1] { id };
            var bioData = db.BioData.Find(id);
            string fileName = bioData.LName + "_" + bioData.FName + "_CandidatePacket" + ".docx";
            generateCandidateReports(idArray, fileName);
        }

        public void generateCandidateReports(int[] ids, string fileName)
        {  
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = "";
            string nonApplicable = "N/A";

            for (int index = 0; index < ids.Length; ++index)
            {
                int id = ids[index];

                var bioData = db.BioData.Find(id);
                reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportStart.html"));
               
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

            //Add Interview Section
            string nextSection, results;
            //Only look at interviews that have been conducted and edited
            foreach (Interview interview in bioData.Interviews.Where(x => x.Status == Status.Edited.ToString() || x.Status == Status.Final.ToString()))
            {
                nextSection = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportInterview.html"));
                nextSection = nextSection.Replace("interviewer", interview.InterviewerUser.LoginID.ToString());
                nextSection = nextSection.Replace("comments", interview.EditedComments);
                nextSection = nextSection.Replace("timeElapsed", interview.Duration.ToString());

                    results = generateResultsString(interview);

                nextSection = nextSection.Replace("results", results);
                reportBody = reportBody + nextSection;
            }

            reportBody = reportBody + "<p>%$%lineBreak%$%</p>";
            }
            //Compile and generate report
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
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

            var year = dt.Year;
            var month = dt.Month;
            var day = dt.Day;
            foreach (Admiral interview in db.Admiral.Where(x=> x.Date.Year == year && x.Date.Month == month && x.Date.Day == day).ToList())
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

                    foreach (Degree d in sa.Degrees.ToList())
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
                /*
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
                }*/
                bool found = false;
                foreach(Program program in bioData.Programs)
                {
                    if(resultsString != "")
                    {
                        resultsString = resultsString + "; ";
                    }
                    if(program.ProgramValue == interview.Program.ProgramValue)
                    {
                        resultsString = resultsString + "Yes-" + program.ProgramValue;
                        found = true;
                    }
                    else
                    {
                        resultsString = resultsString + "No-" + program.ProgramValue;
                    }
                }
                if(!found)
                {
                    if (resultsString != "")
                    {
                        resultsString = resultsString + "; ";
                    }
                    resultsString = resultsString + "Yes-" + interview.Program.ProgramValue;
                }
                row = row.Replace("results", resultsString);
                
                reportBody = reportBody + row;
            }
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
        }


        public void generateFYReport(String year, String dateString, Boolean byFY)
        {
            DateTime date;
            try
            {
                dateString = dateString.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
                date = DateTime.ParseExact(dateString, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
                date = DateTime.Today;
            }

            try
            {
                if (date.Year.ToString() != year)
                {
                    if(!byFY)
                    {
                        date = new DateTime(Int32.Parse(year), 12, 31); //Decemeber 31st of FYG year
                    }
                    else if (date.Month < 10 || date.Year != (Int32.Parse(year) - 1))
                    {
                        date = new DateTime(Int32.Parse(year), 9, 30); //September 30th of FYG year
                    }
                }
            }
            catch 
            { 
                //don't worry about it, just move on
            }

            string fileName = "FY" + year + "_OfficerAccessionsReport.docx";
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));

            header = header + System.IO.File.ReadAllText(Server.MapPath("~/Templates/titleTemplate.html"));
            string dateToDisplay = date.ToString("dd MMMM yyyy");
            header = header.Replace("theTitle", "FY" + year + " NUCLEAR OFFICER ACCENSIONS<br>" + dateToDisplay);

            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FYReportTable.html"));
            string nrTable = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FYReportNRTable.html"));
            string instTable = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FYReportInstTable.html"));
            Boolean done = false;

            //If doing a CY report, shuffle months around appropriately
            if (!byFY)
            {
                fileName = date.Year.ToString() + "_CYOfficerAccessionsReport_forFYG" + year + ".docx";
                reportBody = FYMonthsToCYMonths(reportBody);
                nrTable = FYMonthsToCYMonths(nrTable);
                instTable = FYMonthsToCYMonths(instTable);
            }

            string subTable = reportBody.Replace("title", "SUBMARINE ACCESSIONS");
            string surfTable = reportBody.Replace("title", "SURFACE WAREFARE OFFICER ACCESSIONS");
            reportBody = reportBody.Replace("title", "OVERALL ACCESSIONS");

            nrTable = nrTable.Replace("title", "NR ENGINEER ACCESSIONS");
            instTable = instTable.Replace("title", "INSTRUCTOR ACCESSIONS");

            int totalGoal;
            int otherTotalGoal = 0, otherSubGoal = 0, otherSurfGoal = 0, otherNRGoal = 0, otherInstGoal = 0;

            int subTotalGoal = 0, surfTotalGoal = 0, nrTotalGoal = 0, instTotalGoal = 0, overallTotalGoal = 0;
            var intYear = Int32.Parse(year);
            foreach (FYGoals fyGoal in db.FYGoals.Where(goal => goal.FY == intYear))
            {
                if (fyGoal.Source.Equals(FYSource.USNA.ToString()))
                {
                    totalGoal = fyGoal.SUB.Value + fyGoal.SWO.Value + fyGoal.NR.Value + fyGoal.INST.Value;
                    reportBody = reportBody.Replace("usna goal", totalGoal.ToString());
                    subTable = subTable.Replace("usna goal", fyGoal.SUB.Value.ToString());
                    surfTable = surfTable.Replace("usna goal", fyGoal.SWO.Value.ToString());
                    nrTable = nrTable.Replace("usna goal", fyGoal.NR.Value.ToString());
                    instTable = instTable.Replace("usna goal", fyGoal.INST.Value.ToString());
                }
                else if (fyGoal.Source.Equals(FYSource.NROTC.ToString()))
                {
                    totalGoal = fyGoal.SUB.Value + fyGoal.SWO.Value + fyGoal.NR.Value + fyGoal.INST.Value;
                    reportBody = reportBody.Replace("nrotc goal", totalGoal.ToString());
                    subTable = subTable.Replace("nrotc goal", fyGoal.SUB.Value.ToString());
                    surfTable = surfTable.Replace("nrotc goal", fyGoal.SWO.Value.ToString());
                    nrTable = nrTable.Replace("nrotc goal", fyGoal.NR.Value.ToString());
                    instTable = instTable.Replace("nrotc goal", fyGoal.INST.Value.ToString());
                }
                else if (fyGoal.Source.Equals(FYSource.NUPOC.ToString()))
                {
                    totalGoal = fyGoal.SUB.Value + fyGoal.SWO.Value + fyGoal.NR.Value + fyGoal.INST.Value;
                    reportBody = reportBody.Replace("nupoc goal", totalGoal.ToString());
                    subTable = subTable.Replace("nupoc goal", fyGoal.SUB.Value.ToString());
                    surfTable = surfTable.Replace("nupoc goal", fyGoal.SWO.Value.ToString());
                    nrTable = nrTable.Replace("nupoc goal", fyGoal.NR.Value.ToString());
                    instTable = instTable.Replace("nupoc goal", fyGoal.INST.Value.ToString());
                }
                else if (fyGoal.Source.Equals(FYSource.STA21.ToString()))
                {
                    totalGoal = fyGoal.SUB.Value + fyGoal.SWO.Value + fyGoal.NR.Value + fyGoal.INST.Value;
                    reportBody = reportBody.Replace("sta goal", totalGoal.ToString());
                    subTable = subTable.Replace("sta goal", fyGoal.SUB.Value.ToString());
                    surfTable = surfTable.Replace("sta goal", fyGoal.SWO.Value.ToString());
                    nrTable = nrTable.Replace("sta goal", fyGoal.NR.Value.ToString());
                    instTable = instTable.Replace("sta goal", fyGoal.INST.Value.ToString());
                }
                else
                {
                    totalGoal = fyGoal.SUB.Value + fyGoal.SWO.Value + fyGoal.NR.Value + fyGoal.INST.Value;
                    otherTotalGoal += totalGoal;
                    otherSubGoal += fyGoal.SUB.Value;
                    otherSurfGoal += fyGoal.SWO.Value;
                    otherNRGoal += fyGoal.NR.Value;
                    otherInstGoal += fyGoal.INST.Value;
                }

                //Sum up the total goals, regardless of source, for each table
                overallTotalGoal += totalGoal;
                subTotalGoal += fyGoal.SUB.Value;
                surfTotalGoal += fyGoal.SWO.Value;
                nrTotalGoal += fyGoal.NR.Value;
                instTotalGoal += fyGoal.INST.Value;
            }

            //Fill in calculated "other" source goals on each table
            reportBody = reportBody.Replace("other goal", otherTotalGoal.ToString());
            subTable = subTable.Replace("other goal", otherSubGoal.ToString());
            surfTable = surfTable.Replace("other goal", otherSurfGoal.ToString());
            nrTable = nrTable.Replace("other goal", otherNRGoal.ToString());
            instTable = instTable.Replace("other goal", otherInstGoal.ToString());

            //Fill in calculated total goals for each table
            reportBody = reportBody.Replace("total goal", overallTotalGoal.ToString());
            subTable = subTable.Replace("total goal", subTotalGoal.ToString());
            surfTable = surfTable.Replace("total goal", surfTotalGoal.ToString());
            nrTable = nrTable.Replace("total goal", nrTotalGoal.ToString());
            instTable = instTable.Replace("total goal", instTotalGoal.ToString());

            //Get the list of all offered and accepted Admiral entries
            var allAccepted = db.Admiral.Where(x => x.Decision == true && x.Accepted == true);
            //Filter down to the only candidates of the relevant FYG
            var FYGAccepted = allAccepted.Where(accepted => accepted.BioData.FYG == intYear);
            //Filter down to interviews that have occurred before or on the date specified
            var acceptedCandidates = FYGAccepted.Where(y => y.Date.CompareTo(date) <= 0);

            IQueryable<Admiral> monthlyList;
            IQueryable<Admiral> sourceList;
            string[] mainSources = { "USNA", "NROTC", "NUPOC", "STA21N" }; //The list of sources that get their own row in table
            int totalsIndex = mainSources.Length + 1;

            int[,] cumulativeSourceCounts = new int[5, mainSources.Length + 2];

            int month;
            int yearOfMonth;

            if (byFY)
            {
                month = 10; //OCT
            }
            else
            {
                month = 1; //JAN
            }

            if (date.Month >= 10 || !byFY)
            {
                yearOfMonth = date.Year;
            }
            else
            {
                yearOfMonth = date.Year - 1;
            }

            var priorCandidates = acceptedCandidates.Where(x =>
                (x.Date.Month < month
                && x.Date.Year == yearOfMonth)
                || x.Date.Year < yearOfMonth);

            //Calculate total prior candidates for each table
            reportBody = reportBody.Replace("total" + " " + "prior", priorCandidates.Count().ToString());
            subTable = subTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.Program.ProgramValue == "SUB").Count().ToString());
            surfTable = surfTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.Program.ProgramValue == "SWO").Count().ToString());
            nrTable = nrTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.Program.ProgramValue == "NR").Count().ToString());
            instTable = instTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.Program.ProgramValue == "INST").Count().ToString());

            //Loop for sorting prior candidates by source
            for (int j = 0; j < mainSources.Length + 1; ++j)
            {
                string currentSource;
                //If we are looking for source "other"...
                if (j == mainSources.Length)
                {
                    sourceList = priorCandidates;
                    //Filter out every "main source" until only "other" sources remain
                    sourceList = sourceList.Where(y => !mainSources.Contains(y.BioData.Sources.SourcesValue));
                }
                else //Otherwise, just get the list of appropriate source value
                {
                    currentSource = mainSources[j];
                    sourceList = priorCandidates.Where(y => y.BioData.Sources.SourcesValue == currentSource);
                }

                var subList = sourceList.Where(z => z.Program.ProgramValue == "SUB");
                var surfList = sourceList.Where(z => z.Program.ProgramValue == "SWO");
                var nrList = sourceList.Where(z => z.Program.ProgramValue == "NR");
                var instrList = sourceList.Where(z => z.Program.ProgramValue == "INST");

                if (j < mainSources.Length) //looking at one the main sources
                {
                    reportBody = reportBody.Replace(mainSources[j].ToLower() + " " + "prior", sourceList.Count().ToString());
                    subTable = subTable.Replace(mainSources[j].ToLower() + " " + "prior", subList.Count().ToString());
                    surfTable = surfTable.Replace(mainSources[j].ToLower() + " " + "prior", surfList.Count().ToString());
                    nrTable = nrTable.Replace(mainSources[j].ToLower() + " " + "prior", nrList.Count().ToString());
                    instTable = instTable.Replace(mainSources[j].ToLower() + " " + "prior", instrList.Count().ToString());
                }
                else if(j == mainSources.Length)
                {
                    reportBody = reportBody.Replace("other" + " " + "prior", sourceList.Count().ToString());
                    subTable = subTable.Replace("other" + " " + "prior", subList.Count().ToString());
                    surfTable = surfTable.Replace("other" + " " + "prior", surfList.Count().ToString());
                    nrTable = nrTable.Replace("other" + " " + "prior", nrList.Count().ToString());
                    instTable = instTable.Replace("other" + " " + "prior", instrList.Count().ToString());
                }
                //Initialize cumulative values with prior values
                cumulativeSourceCounts[0, j] += sourceList.Count(); //table index 0 is for overall
                cumulativeSourceCounts[1, j] += subList.Count(); //table index 1 is for submarine
                cumulativeSourceCounts[2, j] += surfList.Count(); //table index 2 is for surface
                cumulativeSourceCounts[3, j] += nrList.Count(); //table index 3 is for nr engineering
                cumulativeSourceCounts[4, j] += instrList.Count(); //table index 4 is for instructor
            }

            //Loop for looking at each month of current year (either fiscal or calender)
            for (int i = 0; i < 12; ++i)
            {
                //Get the list of accepted candidates from each month
                monthlyList = acceptedCandidates.Where(x => x.Date.Month == (month) && x.Date.Year == yearOfMonth);

                //Clear out totals
                cumulativeSourceCounts[0, totalsIndex] = 0;
                cumulativeSourceCounts[1, totalsIndex] = 0;
                cumulativeSourceCounts[2, totalsIndex] = 0;
                cumulativeSourceCounts[3, totalsIndex] = 0;
                cumulativeSourceCounts[4, totalsIndex] = 0;

                //Iterate through the "main" sources (sources that get their own row in the tables)
                for (int j = 0; j < mainSources.Length + 1; ++j)
                {
                    string currentSource;
                    //If we are looking for source "other"...
                    if (j == mainSources.Length)
                    {
                        sourceList = monthlyList;
                        //Filter out every "main source" until only "other" sources remain
                        sourceList = sourceList.Where(y => !mainSources.Contains(y.BioData.Sources.SourcesValue));
                    }
                    else //Otherwise, just get the list of appropriate source value
                    {
                        currentSource = mainSources[j];
                        sourceList = monthlyList.Where(y => y.BioData.Sources.SourcesValue == currentSource);
                    }

                    //TO DO: sub/surf sub categories
                    var subList = sourceList.Where(z => z.Program.ProgramValue == "SUB");
                    var surfList = sourceList.Where(z => z.Program.ProgramValue == "SWO");
                    var nrList = sourceList.Where(z => z.Program.ProgramValue == "NR");
                    var instrList = sourceList.Where(z => z.Program.ProgramValue == "INST");

                    if ((yearOfMonth == date.Year) && (month <= date.Month) || (yearOfMonth < date.Year))
                    {
                        cumulativeSourceCounts[0, j] += sourceList.Count(); //table index 0 is for overall
                        cumulativeSourceCounts[1, j] += subList.Count(); //table index 1 is for submarine
                        cumulativeSourceCounts[2, j] += surfList.Count(); //table index 2 is for surface
                        cumulativeSourceCounts[3, j] += nrList.Count(); //table index 3 is for nr engineering
                        cumulativeSourceCounts[4, j] += instrList.Count(); //table index 4 is for instructor
                        
                        cumulativeSourceCounts[0, totalsIndex] += cumulativeSourceCounts[0, j]; //overall
                        cumulativeSourceCounts[1, totalsIndex] += cumulativeSourceCounts[1, j]; //submarine
                        cumulativeSourceCounts[2, totalsIndex] += cumulativeSourceCounts[2, j]; //surface
                        cumulativeSourceCounts[3, totalsIndex] += cumulativeSourceCounts[3, j]; //nr engineering
                        cumulativeSourceCounts[4, totalsIndex] += cumulativeSourceCounts[4, j]; //instructor
                    }
                    else //This month hasn't happened yet (either in reality, or according to specified report date), so fill in 0's
                    {
                        //Before zeroing, fill in "currently selected" column
                        if (j < mainSources.Length)
                        {
                            reportBody = reportBody.Replace(mainSources[j].ToLower() + " current", cumulativeSourceCounts[0, j].ToString());
                            subTable = subTable.Replace(mainSources[j].ToLower() + " current", cumulativeSourceCounts[1, j].ToString());
                            surfTable = surfTable.Replace(mainSources[j].ToLower() + " current", cumulativeSourceCounts[2, j].ToString());
                            nrTable = nrTable.Replace(mainSources[j].ToLower() + " current", cumulativeSourceCounts[3, j].ToString());
                            instTable = instTable.Replace(mainSources[j].ToLower() + " current", cumulativeSourceCounts[4, j].ToString());
                        }
                        else
                        {
                            reportBody = reportBody.Replace("other current", cumulativeSourceCounts[0, j].ToString());
                            subTable = subTable.Replace("other current", cumulativeSourceCounts[1, j].ToString());
                            surfTable = surfTable.Replace("other current", cumulativeSourceCounts[2, j].ToString());
                            nrTable = nrTable.Replace("other current", cumulativeSourceCounts[3, j].ToString());
                            instTable = instTable.Replace("other current", cumulativeSourceCounts[4, j].ToString());
                        }

                        cumulativeSourceCounts[0, j] = 0; //overall
                        cumulativeSourceCounts[1, j] = 0; //submarine
                        cumulativeSourceCounts[2, j] = 0; //surface
                        cumulativeSourceCounts[3, j] = 0; //nr engineering
                        cumulativeSourceCounts[4, j] = 0; //instructor
                    }

                    //Fill values into the report tables
                    if (j < mainSources.Length) //looking at one the main sources
                    {
                        reportBody = reportBody.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[0, j].ToString());
                        subTable = subTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[1, j].ToString());
                        surfTable = surfTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[2, j].ToString());
                        nrTable = nrTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[3, j].ToString());
                        instTable = instTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[4, j].ToString());
                    }
                    else if (j == mainSources.Length) //"other" source
                    {
                        int nrOther = 0, instOther = 0;
                        for(int k = 0; k < mainSources.Length; ++k)
                        {
                            if(mainSources[k] != "NUPOC")
                            {
                                instOther += cumulativeSourceCounts[4, k];
                                if(mainSources[k] != "NROTC")
                                {
                                    nrOther += cumulativeSourceCounts[3, k];
                                }
                            }
                        }
                        nrOther += cumulativeSourceCounts[3, j];
                        instOther += cumulativeSourceCounts[4, j];

                        if (!done)
                        {
                            if (nrOther == 0)
                                nrTable = removeOtherSection(nrTable);
                            if (instOther == 0)
                                instTable = removeOtherSection(instTable);
                            done = true;
                        }
                            
                        reportBody = reportBody.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[0, j].ToString());
                        subTable = subTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[1, j].ToString());
                        surfTable = surfTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[2, j].ToString());
                        nrTable = nrTable.Replace("other" + " " + (i + 1) + "_", nrOther.ToString());
                        instTable = instTable.Replace("other" + " " + (i + 1) + "_", instOther.ToString());
                    }
                }
                reportBody = reportBody.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[0, totalsIndex].ToString());
                subTable = subTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[1, totalsIndex].ToString());
                surfTable = surfTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[2, totalsIndex].ToString());
                nrTable = nrTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[3, totalsIndex].ToString());
                instTable = instTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[4, totalsIndex].ToString());

                //increment month
                month++;
                //Loop around from Dec to Jan of FY when appropriate
                if (month > 12)
                {
                    month = 1;
                    yearOfMonth = yearOfMonth + 1;
                }
                if(!((yearOfMonth == date.Year) && (month <= date.Month) || (yearOfMonth < date.Year)))
                {
                    float overallPercent = 0, subPercent = 0, surfPercent = 0, nrPercent = 0, instPercent = 0;
                    if (overallTotalGoal > 0)
                        overallPercent = (float)(cumulativeSourceCounts[0, totalsIndex] / (float)(overallTotalGoal));
                    if (subTotalGoal > 0)
                        subPercent = ((float)(cumulativeSourceCounts[1, totalsIndex]) / (float)(subTotalGoal));
                    if (surfTotalGoal > 0)
                        surfPercent = ((float)(cumulativeSourceCounts[2, totalsIndex]) / (float)(surfTotalGoal));
                    if (nrTotalGoal > 0)
                        nrPercent = ((float)(cumulativeSourceCounts[3, totalsIndex]) / (float)(nrTotalGoal));
                    if (instTotalGoal > 0)
                        instPercent = ((float)(cumulativeSourceCounts[4, totalsIndex]) / (float)(instTotalGoal));

                    reportBody = reportBody.Replace("total current", cumulativeSourceCounts[0, totalsIndex].ToString());
                    subTable = subTable.Replace("total current", cumulativeSourceCounts[1, totalsIndex].ToString());
                    surfTable = surfTable.Replace("total current", cumulativeSourceCounts[2, totalsIndex].ToString());
                    nrTable = nrTable.Replace("total current", cumulativeSourceCounts[3, totalsIndex].ToString());
                    instTable = instTable.Replace("total current", cumulativeSourceCounts[4, totalsIndex].ToString());

                    reportBody = reportBody.Replace("overall percent", overallPercent.ToString("0.00") + "%");
                    subTable = subTable.Replace("overall percent", subPercent.ToString("0.00") + "%");
                    surfTable = surfTable.Replace("overall percent", surfPercent.ToString("0.00") + "%");
                    nrTable = nrTable.Replace("overall percent", nrPercent.ToString("0.00") + "%");
                    instTable = instTable.Replace("overall percent", instPercent.ToString("0.00") + "%");
                }
            }

            reportBody = reportBody + subTable + surfTable + instTable + nrTable;
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, true, true);
        }

        public void generateAlphaReport(String date, Boolean chrons)
        {
            date = date.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
            System.Diagnostics.Debug.WriteLine(date);
            DateTime dt = DateTime.ParseExact(date, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string fileName;
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody;

            string rowTemplate;
            if (!chrons)
            {
                reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/AlphaReportStart.html"));
                rowTemplate = "~/Templates/AlphaReportRow.html";
                fileName = "AlphaReport_" + dt.ToShortDateString() + ".docx";
            }
            else
            {
                reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/ChronReportStart.html"));
                rowTemplate = "~/Templates/ChronReportRow.html";
                fileName = "ChronsReport" + dt.ToShortDateString() + ".docx";
            }

            reportBody = reportBody.Replace("date", dt.ToLongDateString());

            string row, university, major, gradDate;
            var year = dt.Year;
            var month = dt.Month;
            var day = dt.Day;
            foreach(Interview interview in db.Interview.Where(x => x.Date.Value.Year == year 
                          && x.Date.Value.Month == month 
                          && x.Date.Value.Day == day).GroupBy(y => y.BioDataID).Select(z => z.FirstOrDefault()).ToList())
            {
                BioData bioData = interview.BioData;
                row = System.IO.File.ReadAllText(Server.MapPath(rowTemplate));
                row = row.Replace("lastName", bioData.LName);
                row = row.Replace("ex", bioData.Suffix);
                row = row.Replace("firstName", bioData.FName);
                row = row.Replace("source", bioData.Sources.SourcesValue);
                row = row.Replace("fyg", bioData.FYG.Value.ToString());
                
                if (bioData.SATM != null)
                    row = row.Replace("satm", bioData.SATM.Value.ToString());
                else
                    row = row.Replace("satm", "NA");

                if (bioData.SATV != null)
                    row = row.Replace("satv", bioData.SATV.Value.ToString());
                else
                    row = row.Replace("satv", "NA");

                university = "";
                major = "";
                gradDate = "";
                foreach(SchoolsAttended school in bioData.SchoolsAttended.ToList())
                {
                    if(university != "" || major != "" || gradDate != "")
                    {
                        university = university + ";<br>";
                        major = major + ";<br>";
                        gradDate = gradDate + ";<br>";
                    }
                    university = university + school.School.SchoolValue;

                    Degree degree = school.Degrees.First();
                    major = major + degree.Major.MajorValue.ToString();
                    gradDate = gradDate + degree.DegreeDate.ToShortDateString();
                }
                row = row.Replace("university", university);
                row = row.Replace("major", major);
                row = row.Replace("gradDate", gradDate);

                reportBody = reportBody + row;
            }
            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
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
            footer = "</table><img src='__chart__' />" + footer;
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
            reportBody = reportBody.Replace("__chart__", generateChart());
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
        }

        public void generateLabels(String date)
        {
            date = date.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
            System.Diagnostics.Debug.WriteLine(date);
            DateTime dt = DateTime.ParseExact(date, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string fileName = "Labels_" + dt.ToShortDateString() + ".docx";
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableStart.html"));

            var year = dt.Year;
            var month = dt.Month;
            var day = dt.Day;

            string row;
            int index = 0;
            foreach(Interview interview in db.Interview.Where(x => x.Date.Value.Year == year 
                        && x.Date.Value.Month == month 
                        && x.Date.Value.Day == day).GroupBy(y => y.BioDataID).Select(z => z.FirstOrDefault()).ToList())
            {
                if(index % 2 == 0)
                {
                    reportBody = reportBody + "<tr>";
                }
                BioData bioData = interview.BioData;
                row = System.IO.File.ReadAllText(Server.MapPath("~/Templates/LabelRow.html"));
                row = row.Replace("firstName", bioData.FName);
                row = row.Replace("lastName", bioData.LName);
                row = row.Replace("middleName", bioData.MName);
                row = row.Replace("ssn", bioData.SSN);
                row = row.Replace("date", interview.Date.Value.ToShortDateString());
                row = row.Replace("source", bioData.Sources.SourcesValue);
                

                reportBody = reportBody + row;
                if(index % 2 == 1)
                {
                   reportBody = reportBody + "</tr>";
                }
                index++;
            }
            if (index % 2 == 1)
            {
                reportBody = reportBody + "</tr>";
            }

            reportBody = reportBody + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
        }

        public void generateBioIDCard (int id)
        {
            BioData bioData = db.BioData.Find(id);
            string fileName = bioData.LName + "_" + bioData.FName + "_bioCard" + ".docx";
            int[] idArray = new int[1] { id };
            generateBioIDCards(idArray, fileName);
        }

        public void generateBioIDCards(int[] ids, string fileName)
        {
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));
            string reportBody = "";

            string newTable, interviewRow;
            for (int i = 0; i < ids.Length; ++i)
            {
                int id = ids[i];
                BioData bioData = db.BioData.Find(id);
                var interviews = db.Interview.Where(x => x.BioDataID == id);

                newTable = System.IO.File.ReadAllText(Server.MapPath("~/Templates/BioCard.html"));
                newTable = newTable.Replace("lastName", bioData.LName);
                newTable = newTable.Replace("firstName", bioData.FName);
                if (bioData.MName != "" && bioData.MName != null)
                    newTable = newTable.Replace("middleName", bioData.MName);
                else
                    newTable = newTable.Replace(", middleName", "");
                newTable = newTable.Replace("ssn", bioData.SSN);
                newTable = newTable.Replace("fyg", bioData.FYG.Value.ToString());
                newTable = newTable.Replace("source", bioData.Sources.SourcesValue);
                newTable = newTable.Replace("dateOfBirth", bioData.DOB.Value.ToShortDateString());

                string schoolInfo = "", majorInfo = "", majors = "";
                foreach (SchoolsAttended school in bioData.SchoolsAttended.ToList())
                {
                    if (schoolInfo != "")
                        schoolInfo = schoolInfo + "<br>";
                    if (majorInfo != "")
                        majorInfo = majorInfo + "<br>";

                    majors = "";
                    foreach(Degree d in school.Degrees.ToList())
                    {
                        if(majors != "")
                        {
                            majors = majors + "; ";
                        }
                        majors = majors + d.Major.MajorValue + " (" + d.DegreeType.DegreeTypeValue + ")";
                    }
                    majorInfo = majorInfo + majors;
 
                    int numYears;
                    if (school.YearStart != null && school.YearEnd != null)
                    {
                        numYears = school.YearEnd.Value - school.YearStart.Value;
                    }
                    else
                    {
                        numYears = 0;
                    }

                    if(numYears == 0)
                        schoolInfo = school.School.SchoolValue + " (" + "< 1" + " year" + ")";
                    else
                        schoolInfo = school.School.SchoolValue + " (" + numYears.ToString() + " year" + ")";
                    
                    if (numYears > 1) schoolInfo = schoolInfo.Replace("year", "years");
                }
                newTable = newTable.Replace("school", schoolInfo);
                newTable = newTable.Replace("major", majorInfo);

                DutyHistory mostRecentDH = null;
                DateTime mostRecentDate = new DateTime(1, 1, 1);
                foreach(DutyHistory dh in bioData.DutyHistories.ToList())
                {
                    foreach(DutyStation ds in dh.DutyStations.ToList())
                    {
                        if(mostRecentDH == null)
                        {
                            mostRecentDH = dh;
                            mostRecentDate = ds.ReportDate;
                        }
                        else if(ds.ReportDate.CompareTo(mostRecentDate) > 0)
                        {
                            mostRecentDH = dh;
                            mostRecentDate = ds.ReportDate;
                        }

                    }
                }
                newTable = newTable.Replace("rank", mostRecentDH.Rank);

                string program = "";
                foreach(Program prog in bioData.Programs.ToList())
                {
                    if(program != "")
                    {
                        program = program + ", ";
                    }
                    program = program + prog.ProgramValue;
                }
                newTable = newTable.Replace("program", program);

                newTable = newTable + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableStart.html"));
                int interviewIndex = 1, seenIndex = 0;
                Interview[] seenInterviews;
                if (bioData.Interviews.Count() > 0)
                    seenInterviews = new Interview[bioData.Interviews.Count()];
                else
                    seenInterviews = new Interview[10]; //10 is arbitrary... there shouldn't be any

                DateTime currentDate;
                string results;
                foreach(Interview interview in bioData.Interviews)
                {
                    if(!seenInterviews.Contains(interview))
                    {
                        currentDate = interview.Date.Value;
                        interviewRow = System.IO.File.ReadAllText(Server.MapPath("~/Templates/BioCardInterviewRow.html"));
                        interviewRow = interviewRow.Replace("index", interviewIndex.ToString());
                        interviewRow = interviewRow.Replace("date", currentDate.ToShortDateString());
                        results = "";
                        foreach (Interview inter in bioData.Interviews.Where(z => z.Date.Value == currentDate).OrderBy(a => a.EndTime).ToList())
                        {
                            if (results != "")
                            {
                                results = results + "<br>";
                            }
                            results = results + inter.InterviewerUser.LoginID + ": ";
                            results = results + generateResultsString(inter);
                            seenInterviews[seenIndex] = inter;
                            seenIndex++;
                        }
                        interviewRow = interviewRow.Replace("decisions", results);
                        interviewIndex++;
                        newTable = newTable + interviewRow;
                    }
                }

                newTable = newTable + System.IO.File.ReadAllText(Server.MapPath("~/Templates/tableEnd.html"));
                reportBody = reportBody + newTable;
                reportBody = reportBody + pageBreakParagraph;
            }

            string reportHtml = header + reportBody + footer;
            generateReport(fileName, reportHtml, false, false);
        }

        public void generateBiosByDateHelper(string date, Boolean bioCards, Boolean packets)
        {
            date = date.Substring(0, "DDD MMM dd yyyy 00:00:00".Length);
            System.Diagnostics.Debug.WriteLine(date);
            DateTime dt = DateTime.ParseExact(date, "ddd MMM d yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            var year = dt.Year;
            var month = dt.Month;
            var day = dt.Day;
            IQueryable<Interview> interviews = db.Interview.Where(x => x.Date.Value.Year == year && x.Date.Value.Month == month && x.Date.Value.Day == day);
            List<Interview> uniqueInterviews = interviews.GroupBy(y => y.BioDataID).Select(z => z.FirstOrDefault()).ToList();
            if (uniqueInterviews.Count() > 0)
            {
                int[] bioIDs = new int[uniqueInterviews.Count()];

                for (int i = 0; i < uniqueInterviews.Count(); ++i)
                {
                    bioIDs[i] = uniqueInterviews.ElementAt(i).BioDataID;
                }
                string fileName;
                if (bioCards)
                {
                    fileName = "BioCards_" + DateTime.Today.ToShortDateString() + ".docx";
                    generateBioIDCards(bioIDs, fileName);
                }
                if (packets)
                {
                    fileName = "CandidatePackets_" + dt.ToShortDateString() + ".docx";
                    generateCandidateReports(bioIDs, fileName);
                }
            }
        }

        private string generateChart()
        {
            var myChart = new Chart(width: 600, height: 400)
                .AddTitle("Chart Title")
                .AddSeries(
                    name: "Employee",
                    xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                    yValues: new[] { "2", "6", "4", "5", "3" })
                .GetBytes("png");
            var chartString = "data:image/png;base64," + Convert.ToBase64String(myChart);
            return chartString;
        }

        public void generateReport(String filename, String html, bool landscape, bool tablePageBreaks)
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

                            PageMargin pageMargin = new PageMargin() 
                            { 
                                Top = 720, 
                                Right = (UInt32Value)1008U, 
                                Bottom = 720, 
                                Left = (UInt32Value)1008U, 
                                Header = (UInt32Value)720U, 
                                Footer = (UInt32Value)720U, 
                                Gutter = (UInt32Value)0U };
                            properties.Append(pageMargin);
                            

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

                            if (landscape)
                            {
                                ParagraphProperties paragraphProperties1 = new ParagraphProperties(
                                  new ParagraphStyleId() { Val = "No Spacing" },
                                  new SpacingBetweenLines() { After = "0" }
                                 );
                                paragraphs[i].PrependChild(paragraphProperties1);

                            }
                            body.Append(paragraphs[i]);
                        }

                        /*foreach(Paragraph P in mainPart.Document.Descendants<Paragraph>())
                        {
                            if (P.LocalName == "pageBreak")
                            {
                                P.InsertAfterSelf
                                        (new Paragraph(
                                            new Run(
                                                new Break() { Type = BreakValues.Page })));
                            }
                        }*/

                        //I have no idea why it doesnt work when you try to use pageBreakParagraph... but it doesnt... so redeclare this same string here
                        string lineBreakCharacter = "%$%lineBreak%$%"; 

                        List<Paragraph> pageBreakMarkers = new List<Paragraph>();
                        var lastP = mainPart.Document.Descendants<Paragraph>().LastOrDefault();
                        foreach (Paragraph P in mainPart.Document.Descendants<Paragraph>())
                        {   
                            foreach (Run R in P.Descendants<Run>())
                            {
                                if (R.Descendants<Text>()
                                    .Where(T => T.Text == lineBreakCharacter).Count() > 0)
                                {
                                    if (P != lastP)
                                    {
                                        P.InsertAfterSelf
                                            (new Paragraph(
                                                new Run(
                                                    new Break() { Type = BreakValues.Page })));
                                    }
                                    pageBreakMarkers.Add(P);
                                }
                            }
                        }
                        foreach(Paragraph P in pageBreakMarkers)
                        {
                            P.Remove();
                        }

                        if (tablePageBreaks)
                        {
                        var tables = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Table>();
                            
                        var last = tables.LastOrDefault();
                        foreach (DocumentFormat.OpenXml.Wordprocessing.Table table in tables)
                        {
                            if (table != last)
                            {
                                table.InsertAfterSelf
                                    (new Paragraph(
                                        new Run(
                                            new Break() { Type = BreakValues.Page })));
                            }
                        }
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

        static void AppendPageBreaks(WordprocessingDocument myDoc)
        {
            MainDocumentPart mainPart = myDoc.MainDocumentPart;
            var tables = myDoc.MainDocumentPart.Document
                .Body
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Table>();
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table table in tables)
            {
                table.InsertAfterSelf
                    (new Paragraph(
                        new Run(
                            new Break() { Type = BreakValues.Page })));
            }
        }

        public string FYMonthsToCYMonths(string table)
        {
            string reportTable = table;
            reportTable = reportTable.Replace("Oct", "octOld");
            reportTable = reportTable.Replace("Nov", "novOld");
            reportTable = reportTable.Replace("Dec", "decOld");

            reportTable = reportTable.Replace("Sep", "Dec");
            reportTable = reportTable.Replace("Aug", "Nov");
            reportTable = reportTable.Replace("Jul", "Oct");
            reportTable = reportTable.Replace("Jun", "Sep");
            reportTable = reportTable.Replace("May", "Aug");
            reportTable = reportTable.Replace("Apr", "Jul");
            reportTable = reportTable.Replace("Mar", "Jun");
            reportTable = reportTable.Replace("Feb", "May");
            reportTable = reportTable.Replace("Jan", "Apr");
            reportTable = reportTable.Replace("decOld", "Mar");
            reportTable = reportTable.Replace("novOld", "Feb");
            reportTable = reportTable.Replace("octOld", "Jan");
            return reportTable;
        }

        public string removeOtherSection(string section)
        {
            string beginMarker = "<!--begin other section-->";
            string endMarker = "<!--end other section-->";

            int start = section.IndexOf(beginMarker);
            int end = section.IndexOf(endMarker);
            int length = section.Length - end;

            string newSection = section;
           
            if(start > 0 && end > 0)
                newSection = section.Substring(0, start) + section.Substring(end, length);
            
            return newSection;
        }

        public string generateResultsString(Interview interview)
        {
            string results = "";
            //Check for NR results
            if (interview.NR == true)
                results = results + "NR - YES";
            else if (interview.NR == false)
                results = results + "NR - NO";
            //Check for INST results
            if (interview.INST == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "INST - YES";
            }
            else if (interview.INST == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "INST - NO";
            }
            //Check for NPS results
            if (interview.NPS == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "NPS - YES";
            }
            else if (interview.NPS == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "NPS - NO";
            }
            //Check for PXO results
            if (interview.PXO == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "PXO - YES";
            }
            else if (interview.PXO == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "PXO - NO";
            }
            //Check for EDO results
            if (interview.EDO == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "EDO - YES";
            }
            else if (interview.EDO == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "EDO - NO";
            }
            //Check for ENLTECH results
            if (interview.ENLTECH == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "ENLTECH - YES";
            }
            else if (interview.ENLTECH == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "ENLTECH - NO";
            }
            //Check for NR1 results
            if (interview.NR1 == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "NR1 - YES";
            }
            else if (interview.NR1 == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "NR1 - NO";
            }
            //Check for SUPPLY results
            if (interview.SUPPLY == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "SUPPLY - YES";
            }
            else if (interview.SUPPLY == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "SUPPLY - NO";
            }
            //Check for EOOW results
            if (interview.EOOW == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "EOOW - YES";
            }
            else if (interview.EOOW == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "EOOW - NO";
            }
            //Check for DOE results
            if (interview.DOE == true)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "DOE - YES";
            }
            else if (interview.DOE == false)
            {
                if (!results.Equals("")) results = results + ", ";
                results = results + "DOE - NO";
            }

            return results;
        }

    }
}
