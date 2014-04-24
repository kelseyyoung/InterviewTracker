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

        [CustomAuth]
        public ActionResult Index()
        {
            ViewBag.currUser = System.Web.HttpContext.Current.User;

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

            //Add Interview Section
            string nextSection, results;
            //Only look at interviews that have been conducted and edited
            foreach (Interview interview in bioData.Interviews.Where(x => x.Status == Status.Edited.ToString() || x.Status == Status.Final.ToString()))
            {
                nextSection = System.IO.File.ReadAllText(Server.MapPath("~/Templates/CandidateReportInterview.html"));
                nextSection = nextSection.Replace("interviewer", "Interviewer" + interview.InterviewerID.ToString());
                nextSection = nextSection.Replace("comments", interview.EditedComments);
                nextSection = nextSection.Replace("timeElapsed", interview.Duration.ToString());

                results = "";
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

                nextSection = nextSection.Replace("results", results);
                reportBody = reportBody + nextSection;
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
            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Tegeneratemplates/InterviewResultsTableStart.html"));

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
    
            string fileName = year + "_FYReport.docx"; //TO DO: add pull/latest interview date to title?
            string header = System.IO.File.ReadAllText(Server.MapPath("~/Templates/header.html"));
            string footer = System.IO.File.ReadAllText(Server.MapPath("~/Templates/footer.html"));

            string statusFinal = Status.Final.ToString();
            IQueryable<Interview> completedInterviews = db.Interview.Where(x => (x.Date.Value.CompareTo(date)) <= 0 && (x.Status == statusFinal));
            Interview mostRecentInterview;
            if (completedInterviews.Count() > 0)
            {
                mostRecentInterview = completedInterviews.OrderByDescending(z => z.Date).FirstOrDefault();
                date = (DateTime)(mostRecentInterview.Date);
            }
     

            header = header + System.IO.File.ReadAllText(Server.MapPath("~/Templates/titleTemplate.html"));
            string dateToDisplay = date.ToString("dd MMMM yyyy");
            header = header.Replace("theTitle", "FY" + year + " NUCLEAR OFFICER ACCENSIONS<br>" + dateToDisplay);

            string reportBody = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FYReportTable.html"));

            //If doing a CY report, shuffle months around appropriately
            if(!byFY)
            {
                fileName = date.Year.ToString() + "_CYReport_forFYG"+ year +".docx"; //TO DO: is that a good title?
                reportBody = reportBody.Replace("Oct", "octOld");
                reportBody = reportBody.Replace("Nov", "novOld");
                reportBody = reportBody.Replace("Dec", "decOld");

                reportBody = reportBody.Replace("Sep", "Dec");
                reportBody = reportBody.Replace("Aug", "Nov");
                reportBody = reportBody.Replace("Jul", "Oct");
                reportBody = reportBody.Replace("Jun", "Sep");
                reportBody = reportBody.Replace("May", "Aug");
                reportBody = reportBody.Replace("Apr", "Jul");
                reportBody = reportBody.Replace("Mar", "Jun");
                reportBody = reportBody.Replace("Feb", "May");
                reportBody = reportBody.Replace("Jan", "Apr");
                reportBody = reportBody.Replace("decOld", "Mar");
                reportBody = reportBody.Replace("novOld", "Feb");
                reportBody = reportBody.Replace("octOld", "Jan");

            }

            string subTable = reportBody.Replace("title", "SUBMARINE ACCESSIONS");
            string surfTable = reportBody.Replace("title", "SURFACE WAREFARE OFFICER ACCESSIONS");
            string nrTable = reportBody.Replace("title", "NR ENGINEER ACCESSIONS");
            string instTable = reportBody.Replace("title", "INSTRUCTOR ACCESSIONS");
            reportBody = reportBody.Replace("title", "OVERALL ACCESSIONS");

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
            var acceptedCandidates = FYGAccepted.Where(y => y.BioData.Interviews.OrderByDescending(z => z.Date).FirstOrDefault().Date.Value.CompareTo(date) <= 0);

            IQueryable<Admiral> monthlyList;
            IQueryable<Admiral> sourceList;
            string[] mainSources = { "USNA", "NROTC", "NUPOC", "STA21N" }; //The list of sources that get their own row in table
            int totalsIndex = mainSources.Length + 1;

            int[,] cumulativeSourceCounts = new int[5, mainSources.Length + 2];

            int month;
            int yearOfMonth;

            if(byFY)
            {
                month = 10; //OCT
            }
            else
            {
                month = 1; //JAN
            }

            if(date.Month >= 10 || !byFY)
            {
                yearOfMonth = date.Year;
            }
            else
            {
                yearOfMonth = date.Year - 1;
            }

            var priorCandidates = acceptedCandidates.Where(x =>
                (x.BioData.Interviews.OrderByDescending(y => y.Date).FirstOrDefault().Date.Value.Month < month
                && x.BioData.Interviews.OrderByDescending(y => y.Date).FirstOrDefault().Date.Value.Year == yearOfMonth)
                || x.BioData.Interviews.OrderByDescending(y => y.Date).FirstOrDefault().Date.Value.Year < yearOfMonth);

            //Calculate total prior candidates for each table
            reportBody = reportBody.Replace("total" + " " + "prior", priorCandidates.Count().ToString());
            //TO DO: sub/surf sub categories
            subTable = subTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.BioData.Interviews.OrderByDescending(z => z.Date).FirstOrDefault().NPS == true).Count().ToString());
            surfTable = surfTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.BioData.Interviews.OrderByDescending(z => z.Date).FirstOrDefault().NPS == true).Count().ToString());
            nrTable = nrTable.Replace("total" + " " + "prior", priorCandidates.Where(x => x.BioData.Interviews.OrderByDescending(z => z.Date).FirstOrDefault().NR == true).Count().ToString());
            instTable = instTable.Replace("totol" + " " + "prior", priorCandidates.Where(x => x.BioData.Interviews.OrderByDescending(z => z.Date).FirstOrDefault().INST == true).Count().ToString());

            //Loop for sorting prior candidates by source
            for (int j = 0; j < mainSources.Length + 1; ++j)
            {
                string currentSource;
                //If we are looking for source "other"...
                if (j == mainSources.Length)
                {
                    sourceList = priorCandidates;
                    //Filter out every "main source" until only "other" sources remain
                    for (int l = 0; l < mainSources.Length; ++l)
                    {
                        currentSource = mainSources[l];
                        sourceList = sourceList.Where(y => y.BioData.Sources.SourcesValue != currentSource);
                    }
                }
                else //Otherwise, just get the list of appropriate source value
                {
                    currentSource = mainSources[j];
                    sourceList = priorCandidates.Where(y => y.BioData.Sources.SourcesValue == currentSource);
                }

                //TO DO: sub/surf sub categories 
                var subList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NPS == true);
                var surfList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NPS == true);
                var nrList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NR == true);
                var instrList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().INST == true);

                if (j < mainSources.Length) //looking at one the main sources
                {
                    reportBody = reportBody.Replace(mainSources[j].ToLower() + " " + "prior", sourceList.Count().ToString());
                    subTable = subTable.Replace(mainSources[j].ToLower() + " " + "prior", subList.Count().ToString());
                    surfTable = surfTable.Replace(mainSources[j].ToLower() + " " + "prior", surfList.Count().ToString());
                    nrTable = nrTable.Replace(mainSources[j].ToLower() + " " + "prior", nrList.Count().ToString());
                    instTable = instTable.Replace(mainSources[j].ToLower() + " " + "prior", instrList.Count().ToString());
                }
                else
                {
                    reportBody = reportBody.Replace("other" + " " + "prior", sourceList.Count().ToString());
                    subTable = subTable.Replace("other" + " " + "prior", subList.Count().ToString());
                    surfTable = surfTable.Replace("other" + " " + "prior", surfList.Count().ToString());
                    nrTable = nrTable.Replace("other" + " " + "prior", nrList.Count().ToString());
                    instTable = instTable.Replace("other" + " " + "prior", instrList.Count().ToString());
                }
            }

            //Loop for looking at each month of current year (either fiscal or calender)
            for (int i = 0; i < 12; ++i)
            {
                //Loop around from Dec to Jan of FY when appropriate
                if(month > 12)
                {
                    month = 1;
                    yearOfMonth = yearOfMonth + 1;
                }
                //Get the list of accepted candidates from each month
                monthlyList = acceptedCandidates.Where(x => x.BioData.Interviews.OrderByDescending(y => y.Date).FirstOrDefault().Date.Value.Month == (month) && x.BioData.Interviews.OrderByDescending(y => y.Date).FirstOrDefault().Date.Value.Year == yearOfMonth);

                //Clear out totals
                cumulativeSourceCounts[0, totalsIndex] = 0;
                cumulativeSourceCounts[1, totalsIndex] = 0;
                cumulativeSourceCounts[2, totalsIndex] = 0;
                cumulativeSourceCounts[3, totalsIndex] = 0;
                cumulativeSourceCounts[4, totalsIndex] = 0;

                //Iterate through the "main" sources (sources that get their own row in the tables)
                for (int j = 0; j < mainSources.Length+1; ++j)
                {
                    string currentSource;
                    //If we are looking for source "other"...
                    if (j == mainSources.Length)
                    {
                        sourceList = monthlyList;
                        //Filter out every "main source" until only "other" sources remain
                        for (int l = 0; l < mainSources.Length; ++l)
                        {
                            currentSource = mainSources[l];
                            sourceList = sourceList.Where(y => y.BioData.Sources.SourcesValue != currentSource);
                        }
                    }
                    else //Otherwise, just get the list of appropriate source value
                    {
                        currentSource = mainSources[j];
                        sourceList = monthlyList.Where(y => y.BioData.Sources.SourcesValue == currentSource);
                    }

                    //TO DO: sub/surf sub categories
                    var subList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NPS == true);
                    var surfList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NPS == true);
                    var nrList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().NR == true);
                    var instrList = sourceList.Where(z => z.BioData.Interviews.OrderByDescending(a => a.Date).FirstOrDefault().INST == true);

                    cumulativeSourceCounts[0, j] += sourceList.Count(); //table index 0 is for overall
                    cumulativeSourceCounts[1, j] += subList.Count(); //table index 1 is for submarine
                    cumulativeSourceCounts[2, j] += surfList.Count(); //table index 2 is for surface
                    cumulativeSourceCounts[3, j] += nrList.Count(); //table index 3 is for nr engineering
                    cumulativeSourceCounts[4, j] += instrList.Count(); //table index 4 is for instructor

                    //Fill values into the report tables
                    if (j < mainSources.Length) //looking at one the main sources
                    {
                        reportBody = reportBody.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[0, j].ToString());
                        subTable = subTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[1, j].ToString());
                        surfTable = surfTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[2, j].ToString());
                        nrTable = nrTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[3, j].ToString());
                        instTable = instTable.Replace(mainSources[j].ToLower() + " " + (i + 1) + "_", cumulativeSourceCounts[4, j].ToString());
                    }
                    else if(j == mainSources.Length) //"other" source
                    {
                        reportBody = reportBody.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[0, j].ToString());
                        subTable = subTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[1, j].ToString());
                        surfTable = surfTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[2, j].ToString());
                        nrTable = nrTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[3, j].ToString());
                        instTable = instTable.Replace("other" + " " + (i + 1) + "_", cumulativeSourceCounts[4, j].ToString());
                    }

                    //Add up totals for each table by summing over each source (totals stored in second index mainSources.Length + 1)
                    cumulativeSourceCounts[0, totalsIndex] += cumulativeSourceCounts[0, j]; //overall
                    cumulativeSourceCounts[1, totalsIndex] += cumulativeSourceCounts[1, j]; //submarine
                    cumulativeSourceCounts[2, totalsIndex] += cumulativeSourceCounts[2, j]; //surface
                    cumulativeSourceCounts[3, totalsIndex] += cumulativeSourceCounts[3, j]; //nr engineering
                    cumulativeSourceCounts[4, totalsIndex] += cumulativeSourceCounts[4, j]; //instructor
                    
                }
                reportBody = reportBody.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[0, totalsIndex].ToString());
                subTable = subTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[1, totalsIndex].ToString());
                surfTable = surfTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[2, totalsIndex].ToString());
                nrTable = nrTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[3, totalsIndex].ToString());
                instTable = instTable.Replace("total" + " " + (i + 1) + "_", cumulativeSourceCounts[4, totalsIndex].ToString());
               
                month++;
            }

            reportBody = reportBody + subTable + surfTable + instTable + nrTable;
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
                            
                            if(landscape)
                            {
                                ParagraphProperties paragraphProperties1 = new ParagraphProperties(
                                  new ParagraphStyleId() { Val = "No Spacing" },
                                  new SpacingBetweenLines() { After = "0" }
                                 );
                                paragraphs[i].PrependChild(paragraphProperties1);
                                
                            }
                            body.Append(paragraphs[i]);
                        }

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
                    ( new Paragraph(
                        new Run(
                            new Break() { Type = BreakValues.Page })));
            }
        }
    }
}
