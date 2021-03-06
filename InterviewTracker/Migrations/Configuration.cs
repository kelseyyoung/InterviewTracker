namespace InterviewTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using InterviewTracker.Models;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<InterviewTracker.DAL.InterviewTrackerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InterviewTracker.DAL.InterviewTrackerContext context)
        {
            // Initial DB data

            /**** Lookup Table Data ****/

            // Ethnicity

            Ethnicity CAUC = new Ethnicity { EthnicityValue = "CAUC" };
            Ethnicity BLACK = new Ethnicity { EthnicityValue = "BLACK" };
            Ethnicity HISP = new Ethnicity { EthnicityValue = "HISP" };
            Ethnicity API = new Ethnicity { EthnicityValue = "API" };
            Ethnicity OTHER = new Ethnicity { EthnicityValue = "OTHER" };

            context.Ethnicity.AddOrUpdate(i => i.EthnicityID,
                CAUC,
                BLACK,
                HISP,
                API,
                OTHER
            );

            // Sources
            Sources USNA = new Sources { SourcesValue = "USNA" };
            Sources NROTC = new Sources { SourcesValue = "NROTC" };
            Sources NUPOC = new Sources { SourcesValue = "NUPOC" };
            Sources STA21N = new Sources { SourcesValue = "STA21N" };
            Sources FLEET = new Sources { SourcesValue = "FLEET" };
            Sources LDO = new Sources { SourcesValue = "LDO" };
            Sources CWO = new Sources { SourcesValue = "CWO" };

            context.Sources.AddOrUpdate(i => i.SourcesID,
                USNA,
                NROTC,
                NUPOC,
                STA21N,
                FLEET,
                LDO,
                CWO
            );

            // SubSources
            SubSources ECP = new SubSources { SubSourcesValue = "ECP" };
            SubSources MECP = new SubSources { SubSourcesValue = "MECP" };
            SubSources STA = new SubSources { SubSourcesValue = "STA" };
            SubSources STA21 = new SubSources { SubSourcesValue = "STA21" };

            context.SubSources.AddOrUpdate(i => i.SubSourcesID,
                ECP,
                MECP,
                STA,
                STA21
            );

            // Program
            Program NR = new Program { ProgramValue = "NR" };
            Program INST = new Program { ProgramValue = "INST" };
            Program NPS = new Program { ProgramValue = "NPS" };
            Program SWO = new Program { ProgramValue = "SWO" };
            Program SUB = new Program { ProgramValue = "SUB" };
            Program PXO = new Program { ProgramValue = "PXO" };
            Program EDO = new Program { ProgramValue = "EDO" };
            Program NR1 = new Program { ProgramValue = "NR1" };
            Program ENLTECH = new Program { ProgramValue = "ENLTECH" };
            Program SUPPLY = new Program { ProgramValue = "SUPPLY" };
            Program DOE = new Program { ProgramValue = "DOE" };
            Program EOOW = new Program { ProgramValue = "EOOW" };

            context.Program.AddOrUpdate(i => i.ProgramID,
                NR,
                INST,
                NPS,
                SWO,
                SUB,
                PXO,
                EDO,
                NR1,
                ENLTECH,
                SUPPLY,
                DOE,
                EOOW
            );

            // DegreeType
            DegreeType BS = new DegreeType { DegreeTypeValue = "BS" };
            DegreeType BA = new DegreeType { DegreeTypeValue = "BA" };
            DegreeType BSAST = new DegreeType { DegreeTypeValue = "BSAST" };
            DegreeType MA = new DegreeType { DegreeTypeValue = "MA" };
            DegreeType MS = new DegreeType { DegreeTypeValue = "MS" };
            DegreeType MBA = new DegreeType { DegreeTypeValue = "MBA" };
            DegreeType MPA = new DegreeType { DegreeTypeValue = "MPA" };
            DegreeType ME = new DegreeType { DegreeTypeValue = "ME" };
            DegreeType NE = new DegreeType { DegreeTypeValue = "NE" };
            DegreeType PhD = new DegreeType { DegreeTypeValue = "PhD" };
            DegreeType AA = new DegreeType { DegreeTypeValue = "AA" };
            DegreeType AS = new DegreeType { DegreeTypeValue = "AS" };

            context.DegreeType.AddOrUpdate(i => i.DegreeTypeID,
                BS,
                BA,
                BSAST,
                MA,
                MS,
                MBA,
                MPA,
                ME,
                NE,
                PhD,
                AA,
                AS
            );

            // Major
            Major MATH = new Major { MajorValue = "MATH" };
            Major PHYSICS = new Major { MajorValue = "PHYSICS" };
            Major BIOLOGY = new Major { MajorValue = "BIOLOGY" };
            Major CS = new Major { MajorValue = "COMPUTER SCIENCE" };

            context.Major.AddOrUpdate(i => i.MajorID,
                MATH,
                BIOLOGY,
                PHYSICS,
                CS
            );

            // School
            School UofA = new School { SchoolValue = "UNIVERSITY OF ARIZONA" };
            School ASU = new School { SchoolValue = "ARIZONA STATE UNIVERSITY" };
            School Stanford = new School { SchoolValue = "STANFORD" };
            School Texas = new School { SchoolValue = "UNIVERSITY OF TEXAS" };

            context.School.AddOrUpdate(i => i.SchoolID,
                UofA,
                ASU,
                Stanford,
                Texas
            );

            // ServSel
            // TODO: what is a ServSel
            /*
            ServSel servsel = new ServSel { ServSelValue = "What is this" };

            context.ServSel.AddOrUpdate(i => i.ServSelID,
                servsel
            );
            */

            /**** Normal Table Data ****/

            // BioData
            /*
            List<Program> KelseyPrograms = new List<Program> { NR, INST };
            BioData Kelsey = new BioData
                {
                    SSN = "123-45-6789",
                    LName = "Young",
                    MName = "Jeanne",
                    FName = "Kelsey",
                    DOB = DateTime.Parse("1992-4-16"),
                    Sex = Sex.F.ToString(),
                    Programs = KelseyPrograms,
                    FYG = 2014,
                    Ethnicity = CAUC,
                    Sources = NUPOC
                };
            List<Program> JohnPrograms = new List<Program> { NR, ENLTECH, DOE };
            BioData John = new BioData
                {
                    SSN = "223-45-6789",
                    LName = "Doe",
                    MName = "Edward",
                    FName = "John",
                    DOB = DateTime.Parse("1990-8-12"),
                    Sex = Sex.M.ToString(),
                    Programs = JohnPrograms,
                    FYG = 2013,
                    Ethnicity = BLACK,
                    Sources = NROTC,
                    SubSources = ECP
                };
            List<Program> StevePrograms = new List<Program> { SUB, PXO };
            BioData Steve = new BioData
                {
                    SSN = "323-45-6789",
                    LName = "Aardvark",
                    MName = "Hank",
                    FName = "Steve",
                    DOB = DateTime.Parse("1994-12-25"),
                    Sex = Sex.M.ToString(),
                    Programs = StevePrograms,
                    FYG = 2012,
                    Ethnicity = API,
                    Sources = CWO
                };

            context.BioData.AddOrUpdate(i => i.ID,
                Kelsey,
                John,
                Steve
            );
            */
            // Users
            User Admin = new User
            {
                LName = "admin",
                Initials = "aa",
                LoginID = "adminaa",
                Code = "08A",
                NR = true,
                INST = false,
                NPS = true,
                PXO = false,
                EDO = true,
                ENLTECH = false,
                NR1 = true,
                SUPPLY = false,
                EOOW = false,
                DOE = false,
                UserGroup = UserGroup.ADMIN.ToString()
            };
            User Coord = new User
            {
                LName = "coord",
                Initials = "bb",
                LoginID = "coordbb",
                Code = "08B",
                NR = false,
                INST = false,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = false,
                NR1 = true,
                SUPPLY = false,
                EOOW = true,
                DOE = false,
                UserGroup = UserGroup.COORD.ToString()
            };
            // 3 Different interviewers
            User Interviewer = new User
            {
                LName = "interviewer",
                Initials = "cc",
                LoginID = "interviewercc",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.INTER.ToString()
            };
            User Interviewer2 = new User
            {
                LName = "interviewer2",
                Initials = "cc",
                LoginID = "interviewer2cc",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.INTER.ToString()
            };
            User Interviewer3 = new User
            {
                LName = "interviewer3",
                Initials = "cc",
                LoginID = "interviewer3cc",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.INTER.ToString()
            };

            User KelseyUser = new User
            {
                LName = "Young",
                Initials = "KY",
                LoginID = "Kelsey",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.ADMIN.ToString()
            };

            User DewayneUser = new User
            {
                LName = "Byrnes",
                Initials = "DB",
                LoginID = "DWByrnes",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.ADMIN.ToString()
            };

            User AdministratorUser = new User
            {
                LName = "Admin",
                Initials = "AA",
                LoginID = "Administrator",
                Code = "08C",
                NR = true,
                INST = true,
                NPS = true,
                PXO = true,
                EDO = true,
                ENLTECH = true,
                NR1 = true,
                SUPPLY = true,
                EOOW = true,
                DOE = true,
                UserGroup = UserGroup.ADMIN.ToString()
            };

            context.User.AddOrUpdate(i => i.UserID,
                Admin,
                Coord,
                Interviewer,
                Interviewer2,
                Interviewer3,
                KelseyUser,
                DewayneUser,
                AdministratorUser
            );

            // Classes
            Classes MATH109 = new Classes
            {
                Subject = "MATH",
                Code = "109",
                Name = "Algebra",
                Technical = true
            };
            Classes ENGL266 = new Classes
            {
                Subject = "ENGL",
                Code = "266",
                Name = "Technical Writing",
                Technical = false
            };
            Classes PHYS418 = new Classes
            {
                Subject = "PHYS",
                Code = "418",
                Name = "Static Physics",
                Technical = true
            };

            context.Classes.AddOrUpdate(i => i.ClassesID,
                MATH109,
                ENGL266,
                PHYS418
            );
            /*
            // Duty History
            DutyHistory KelseyDutyHistory = new DutyHistory
            {
                Branch = "Navy",
                Rank = "E1",
                Rating = "MM",
                NUC = false,
                BioData = Kelsey
            };
            DutyHistory JohnDutyHistory = new DutyHistory
            {
                Branch = "USMC",
                Rank = "O2",
                Rating = "ET",
                NUC = true,
                BioData = John
            };
            DutyHistory SteveDutyHistory = new DutyHistory
            {
                Branch = "Navy",
                Rank = "O2",
                Rating = "ET",
                NUC = false,
                BioData = Steve
            };

            context.DutyHistory.AddOrUpdate(i => i.DutyHistoryID,
                KelseyDutyHistory,
                JohnDutyHistory,
                SteveDutyHistory
            );
            
            // Duty Station
            DutyStation KelseyDutyStation = new DutyStation
            {
                Name = "KelseyDutyStation",
                ReportDate = DateTime.Parse("2008-1-1"),
                DepartDate = DateTime.Parse("2011-1-1"),
                Duties = "Was an officer",
                GPA = 3.5,
                RankOverallVal = 20,
                RankOverallTot = 200,
                RankInRateVal = 100,
                RankInRateTot = 200,
                Notes = "Was an excellent officer",
                DutyHistory = KelseyDutyHistory
            };
            DutyStation JohnDutyStation = new DutyStation
            {
                Name = "JohnDutyStation",
                ReportDate = DateTime.Parse("2008-1-1"),
                DepartDate = DateTime.Parse("2009-12-31"),
                Duties = "Cooked meals",
                GPA = 2.4,
                RankOverallVal = 100,
                RankOverallTot = 300,
                RankInRateVal = 200,
                RankInRateTot = 300,
                Notes = "Not a great cook",
                DutyHistory = JohnDutyHistory
            };
            DutyStation SteveDutyStation = new DutyStation
            {
                Name = "SteveDutyStation",
                ReportDate = DateTime.Parse("2011-1-1"),
                DepartDate = DateTime.Parse("2013-1-1"),
                Duties = "Corporal duties",
                GPA = 3.94,
                RankOverallVal = 2,
                RankOverallTot = 100,
                RankInRateVal = 3,
                RankInRateTot = 80,
                Notes = "One of the smartest",
                DutyHistory = SteveDutyHistory
            };

            context.DutyStation.AddOrUpdate(i => i.DutyStationID,
                KelseyDutyStation,
                JohnDutyStation,
                SteveDutyStation
            );
            */

            // FY Goals
            FYGoals FY2012 = new FYGoals
            {
                FY = 2012,
                Source = FYSource.NROTC.ToString(),
                SUB = 30,
                SUBF = 20,
                SWO = 25,
                NR = 10,
                INST = 50
            };
            FYGoals FY2013 = new FYGoals
            {
                FY = 2013,
                Source = FYSource.NUPOC.ToString(),
                SUB = 10,
                SUBF = 5,
                SWO = 30,
                NR = 13,
                INST = 77
            };
            FYGoals FY2014 = new FYGoals
            {
                FY = 2014,
                Source = FYSource.USNA.ToString(),
                SUB = 11,
                SUBF = 3,
                SWO = 84,
                NR = 44,
                INST = 29
            };

            context.FYGoals.AddOrUpdate(i => i.FYID,
                FY2012,
                FY2013,
                FY2014
            );
            /*
            // SchoolsAttended
            SchoolsAttended KelseySchoolsAttended = new SchoolsAttended
            {
                YearStart = 2009,
                YearEnd = 2013,
                Graduated = true,
                Comments = "Great student",
                BioData = Kelsey,
                School = UofA
            };
            SchoolsAttended JohnSchoolsAttended = new SchoolsAttended
            {
                YearStart = 2007,
                YearEnd = 2010,
                Graduated = true,
                BioData = John,
                School = ASU
            };
            SchoolsAttended SteveSchoolsAttended = new SchoolsAttended
            {
                YearStart = 2012,
                YearEnd = 2016,
                Graduated = false,
                BioData = Steve,
                School = Stanford
            };
            
            context.SchoolsAttended.AddOrUpdate(i => i.SchoolsAttendedID,
                KelseySchoolsAttended,
                JohnSchoolsAttended,
                SteveSchoolsAttended
            );
            
            // School Standings
            // 4 Years for Kelsey
            SchoolStandings KelseySchoolStandings = new SchoolStandings
            {
                YearOfRecord = 1,
                GPA = 3.2,
                AOMVal = 30,
                AOMTot = 300,
                OOMVal = 20,
                OOMTot = 300,
                InMajorVal = 20,
                InMajorTot = 200,
                SchoolsAttended = KelseySchoolsAttended
            };
            SchoolStandings KelseySchoolStandings2 = new SchoolStandings
            {
                YearOfRecord = 2,
                GPA = 3.2,
                AOMVal = 30,
                AOMTot = 300,
                OOMVal = 20,
                OOMTot = 300,
                InMajorVal = 20,
                InMajorTot = 200,
                SchoolsAttended = KelseySchoolsAttended
            };
            SchoolStandings KelseySchoolStandings3 = new SchoolStandings
            {
                YearOfRecord = 3,
                GPA = 3.2,
                AOMVal = 30,
                AOMTot = 300,
                OOMVal = 20,
                OOMTot = 300,
                InMajorVal = 20,
                InMajorTot = 200,
                SchoolsAttended = KelseySchoolsAttended
            };
            SchoolStandings KelseySchoolStandings4 = new SchoolStandings
            {
                YearOfRecord = 4,
                GPA = 3.2,
                AOMVal = 30,
                AOMTot = 300,
                OOMVal = 20,
                OOMTot = 300,
                InMajorVal = 20,
                InMajorTot = 200,
                SchoolsAttended = KelseySchoolsAttended
            };
            // 3 Years for John
            SchoolStandings JohnSchoolStandings = new SchoolStandings
            {
                YearOfRecord = 1,
                GPA = 2.3,
                AOMVal = 10,
                AOMTot = 600,
                OOMVal = 500,
                OOMTot = 500,
                InMajorVal = 87,
                InMajorTot = 500,
                SchoolsAttended = JohnSchoolsAttended
            };
            SchoolStandings JohnSchoolStandings2 = new SchoolStandings
            {
                YearOfRecord = 2,
                GPA = 2.3,
                AOMVal = 10,
                AOMTot = 600,
                OOMVal = 500,
                OOMTot = 500,
                InMajorVal = 87,
                InMajorTot = 500,
                SchoolsAttended = JohnSchoolsAttended
            };
            SchoolStandings JohnSchoolStandings3 = new SchoolStandings
            {
                YearOfRecord = 3,
                GPA = 2.3,
                AOMVal = 10,
                AOMTot = 600,
                OOMVal = 500,
                OOMTot = 500,
                InMajorVal = 87,
                InMajorTot = 500,
                SchoolsAttended = JohnSchoolsAttended
            };
            // 4 Years for Stever
            SchoolStandings SteveSchoolStandings = new SchoolStandings
            {
                YearOfRecord = 1,
                GPA = 3.9,
                AOMVal = 2,
                AOMTot = 80,
                OOMVal = 1,
                OOMTot = 50,
                InMajorVal = 2,
                InMajorTot = 90,
                SchoolsAttended = SteveSchoolsAttended
            };
            SchoolStandings SteveSchoolStandings2 = new SchoolStandings
            {
                YearOfRecord = 2,
                GPA = 3.9,
                AOMVal = 2,
                AOMTot = 80,
                OOMVal = 1,
                OOMTot = 50,
                InMajorVal = 2,
                InMajorTot = 90,
                SchoolsAttended = SteveSchoolsAttended
            };
            SchoolStandings SteveSchoolStandings3 = new SchoolStandings
            {
                YearOfRecord = 3,
                GPA = 3.9,
                AOMVal = 2,
                AOMTot = 80,
                OOMVal = 1,
                OOMTot = 50,
                InMajorVal = 2,
                InMajorTot = 90,
                SchoolsAttended = SteveSchoolsAttended
            };
            SchoolStandings SteveSchoolStandings4 = new SchoolStandings
            {
                YearOfRecord = 4,
                GPA = 3.9,
                AOMVal = 2,
                AOMTot = 80,
                OOMVal = 1,
                OOMTot = 50,
                InMajorVal = 2,
                InMajorTot = 90,
                SchoolsAttended = SteveSchoolsAttended
            };

            context.SchoolStandings.AddOrUpdate(i => i.SchoolStandingsID,
                KelseySchoolStandings,
                KelseySchoolStandings2,
                KelseySchoolStandings3,
                KelseySchoolStandings4,
                JohnSchoolStandings,
                JohnSchoolStandings2,
                JohnSchoolStandings3,
                SteveSchoolStandings,
                SteveSchoolStandings2,
                SteveSchoolStandings3,
                SteveSchoolStandings4
            );
            
            // Interviews
            Interview KelseyInterview = new Interview
            {
                Date = DateTime.Parse("2014/5/13 12:00:00"),
                Status = Status.Scheduled.ToString(),
                Location = "Room 203",
                StartTime = DateTime.Parse("2014/5/13 11:00:00"),
                EndTime = DateTime.Parse("2014/5/13 12:00:00"),
                InterviewerUser = Interviewer,
                BioData = Kelsey
            };
            Interview KelseyInterview2 = new Interview
            {
                Date = DateTime.Parse("2014/5/13 12:00:00"),
                Status = Status.Scheduled.ToString(),
                Location = "Room 203",
                StartTime = DateTime.Parse("2014/5/13 12:00:00"),
                EndTime = DateTime.Parse("2014/5/13 13:00:00"),
                InterviewerUser = Interviewer2,
                BioData = Kelsey
            };
            Interview KelseyInterview3 = new Interview
            {
                Date = DateTime.Parse("2014/5/13 12:00:00"),
                Status = Status.Scheduled.ToString(),
                Location = "Room 203",
                StartTime = DateTime.Parse("2014/5/13 13:00:00"),
                EndTime = DateTime.Parse("2014/5/13 14:00:00"),
                InterviewerUser = Interviewer3,
                BioData = Kelsey
            };
            Interview JohnInterview = new Interview
            {
                Date = DateTime.Parse("2014/1/20 12:00:00"),
                Status = Status.Entered.ToString(),
                Location = "Room 155",
                Comments = "Seemed like a good worker",
                StartTime = DateTime.Parse("2014/1/20 9:00:00"),
                EndTime = DateTime.Parse("2014/1/20 9:30:00"),
                Duration = 28,
                NR = true,
                ENLTECH = false,
                DOE = true,
                InterviewerUser = Interviewer,
                BioData = John
            };
            Interview SteveInterview = new Interview
            {
                Date = DateTime.Parse("2014/2/2 12:00:00"),
                Status = Status.Edited.ToString(),
                Location = "Room 555",
                Comments = "Gave off a bad impression",
                EditedComments = "Gave off a bad impression, did not respond well to questions",
                StartTime = DateTime.Parse("2014/2/2 11:00:00"),
                EndTime = DateTime.Parse("2014/2/2 12:30:00"),
                Duration = 80,
                EditTime = DateTime.Parse("2014/2/5 11:30:00"),
                NPS = true,
                PXO = false,
                CurrentlyEditingUser = Coord,
                InterviewerUser = Interviewer,
                BioData = Steve
            };

            Interview FinalInterview = new Interview
            {
                Date = DateTime.Parse("2014/5/18 12:00:00"),
                Status = Status.Final.ToString(),
                Location = "Room 757",
                Comments = "Gave off a bad impression",
                EditedComments = "Gave off a bad impression, did not respond well to questions",
                StartTime = DateTime.Parse("2014/5/18 11:00:00"),
                EndTime = DateTime.Parse("2014/5/18 11:30:00"),
                Duration = 30,
                EditTime = DateTime.Parse("2014/5/20 9:30:00"),
                NR = true,
                INST = true,
                CurrentlyEditingUser = Coord,
                InterviewerUser = Interviewer,
                BioData = Kelsey
            };

            context.Interview.AddOrUpdate(i => i.InterviewID,
                KelseyInterview,
                KelseyInterview2,
                KelseyInterview3,
                JohnInterview,
                SteveInterview,
                FinalInterview
            );

            // Admiral for FinalInterview
            Admiral FinalInterviewAdmiral = new Admiral
            {
                Decision = true,
                Accepted = true,
                Comments = "Great person",
                NP500 = false,
                NSTC = true,
                SelfStudy = true,
                PreSchool = false,
                Letter = false,
                LetterReceived = false,
                AdmiralNotes = "Welcome aboard",
                InviteBack = false,
                //SERVSEL = "what is this",
                BioData = Kelsey,
                Program = NR,
                Date = new DateTime(2014, 5, 16, 12, 0, 0)
            };

            context.Admiral.AddOrUpdate(i => i.AdmiralID,
                FinalInterviewAdmiral
            );

            // Many interviews for coord view
            for (int j = 0; j < 4; j++)
            {
                Interview Interview = new Interview
                {
                    Date = DateTime.Parse("2014/4/16 12:00:00"),
                    Status = Status.Scheduled.ToString(),
                    Location = "Room 700",
                    StartTime = DateTime.Parse("2014/4/16 11:00:00"),
                    EndTime = DateTime.Parse("2014/4/16 11:30:00"),
                    Duration = 30,
                    NR = true,
                    INST = true,
                    InterviewerUser = Interviewer,
                    BioData = Kelsey
                };
                context.Interview.AddOrUpdate(i => i.InterviewID,
                    Interview
                );
            }
            for (int j = 0; j < 4; j++)
            {
                Interview Interview = new Interview
                {
                    Date = DateTime.Parse("2014/4/16 12:00:00"),
                    Status = Status.Scheduled.ToString(),
                    Location = "Room 700",
                    StartTime = DateTime.Parse("2014/4/16 11:00:00"),
                    EndTime = DateTime.Parse("2014/4/16 11:30:00"),
                    Duration = 30,
                    NPS = true,
                    PXO = false,
                    InterviewerUser = Interviewer2,
                    BioData = Steve
                };
                context.Interview.AddOrUpdate(i => i.InterviewID,
                    Interview
                );
            }
            for (int j = 0; j < 4; j++)
            {
                Interview Interview = new Interview
                {
                    Date = DateTime.Parse("2014/4/16 12:00:00"),
                    Status = Status.Scheduled.ToString(),
                    Location = "Room 700",
                    StartTime = DateTime.Parse("2014/4/16 11:00:00"),
                    EndTime = DateTime.Parse("2014/4/16 11:30:00"),
                    Duration = 30,
                    NR = true,
                    ENLTECH = false,
                    DOE = true,
                    InterviewerUser = Interviewer3,
                    BioData = John
                };
                context.Interview.AddOrUpdate(i => i.InterviewID,
                    Interview
                );
            }
            
            // Classes Attended
            // 2 for Kelsey
            ClassesAttended KelseyClassesAttended = new ClassesAttended
            {
                YearTaken = 1,
                Grade = "B",
                SchoolsAttended = KelseySchoolsAttended,
                BioData = Kelsey,
                Classes = MATH109

            };
            ClassesAttended KelseyClassesAttended2 = new ClassesAttended
            {
                YearTaken = 1,
                Grade = "F",
                SchoolsAttended = KelseySchoolsAttended,
                BioData = Kelsey,
                Classes = ENGL266

            };
            ClassesAttended KelseyClassesAttended3 = new ClassesAttended
            {
                YearTaken = 2,
                Grade = "C-",
                SchoolsAttended = KelseySchoolsAttended,
                BioData = Kelsey,
                Classes = ENGL266

            };

            // 1 For John
            ClassesAttended JohnClassesAttended = new ClassesAttended
            {
                YearTaken = 2,
                Grade = "C+",
                SchoolsAttended = JohnSchoolsAttended,
                BioData = John,
                Classes = PHYS418
            };
            // 1 For Steve
            ClassesAttended SteveClassesAttended = new ClassesAttended
            {
                YearTaken = 3,
                Grade = "A-",
                SchoolsAttended = SteveSchoolsAttended,
                BioData = Steve,
                Classes = ENGL266
            };

            context.ClassesAttended.AddOrUpdate(i => i.ClassesAttendedID,
                KelseyClassesAttended,
                KelseyClassesAttended2,
                KelseyClassesAttended3,
                JohnClassesAttended,
                SteveClassesAttended
            );

            // Degrees
            Degree KelseyDegree = new Degree
            {
                DegreeDate = DateTime.Parse("2012-5-20"),
                SchoolsAttended = KelseySchoolsAttended,
                Major = MATH,
                DegreeType = BS
            };
            Degree JohnDegree = new Degree
            {
                DegreeDate = DateTime.Parse("2010-12-19"),
                SchoolsAttended = JohnSchoolsAttended,
                Major = PHYSICS,
                DegreeType = PhD
            };
            Degree SteveDegree = new Degree
            {
                DegreeDate = DateTime.Parse("2014-5-25"),
                SchoolsAttended = SteveSchoolsAttended,
                Major = CS,
                DegreeType = MS
            };

            context.Degree.AddOrUpdate(i => i.DegreeID,
                KelseyDegree,
                JohnDegree,
                SteveDegree
            );

            // RD
            RD JohnRD = new RD
            {
                Type = RDType.DEVOL.ToString(),
                Reason = "US Traitor",
                Date = DateTime.Parse("2009-1-1"),
                BioData = John
            };

            context.RD.AddOrUpdate(i => i.RDID,
                JohnRD
            );

            
            // Screens
            Screen JohnScreen = new Screen
            {
                Screener = "Dorothy Gale",
                Location = "U of A Career Fair",
                ScreenDate = DateTime.Parse("2009-1-1"),
                NRStatus = ScreenStatus.Maybe.ToString(),
                INSTStatus = ScreenStatus.No.ToString(),
                NPSStatus = ScreenStatus.Yes.ToString(),
                BioData = John
            };

            context.Screen.AddOrUpdate(i => i.ScreenID,
                JohnScreen
            );

            // Waiver
            Waiver SteveWaiver = new Waiver
            {
                Type = WaiverType.Drug.ToString(),
                Date = DateTime.Parse("2012-1-1"),
                Comments = "Addicted to the pipe",
                BioData = Steve
            };

            context.Waiver.AddOrUpdate(i => i.WaiverID,
                SteveWaiver
            );
            */

            /** Lots of data for chart generation **/
            // Month: (i % 4) + 1 (Jan-Apr)
            // Day: i + 1 (1-27)

            string letters = "abcdefghijklmnopqrstuvwxyz";
            List<Sources> SourcesList = new List<Sources> { USNA, NROTC, NUPOC, STA21N };
            List<Program> ProgramsList = new List<Program> { NR, INST, NPS };
            List<Ethnicity> EthnicitiesList = new List<Ethnicity> { CAUC, BLACK, HISP, API, OTHER };
            List<User> InterviewersList = new List<User> { Interviewer, Interviewer2, Interviewer3 };
            for (var i = 0; i < 26; i++)
            {
                // Create BioData
                Sex sex;
                if (i % 2 == 0)
                {
                    // If i is even, they are Male
                    sex = Sex.M;
                }
                else
                {
                    // If i is odd, they are female
                    sex = Sex.F;
                }
                Sources personSource = SourcesList.ElementAt((SourcesList.Count + i) % SourcesList.Count);
                Ethnicity personEthnicity = EthnicitiesList.ElementAt((EthnicitiesList.Count + i) % EthnicitiesList.Count);
                string ssn;
                if (i < 10)
                {
                    ssn = "123-45-678" + i;
                }
                else
                {
                    ssn = "123-45-67" + i;
                }
                BioData Person = new BioData
                {
                    SSN = ssn,
                    LName = "Doe" + letters[i],
                    MName = "Reginald" + letters[i],
                    FName = "Person" + letters[i],
                    DOB = DateTime.Parse("1992-4-" + (i + 1)),
                    Sex = sex.ToString(),
                    Programs = ProgramsList,
                    FYG = 2013,
                    Ethnicity = personEthnicity,
                    Sources = personSource
                };
                // Add BioData
                context.BioData.AddOrUpdate(x => x.ID, Person);

                // Create SchoolsAttended
                SchoolsAttended PersonSchoolsAttended = new SchoolsAttended
                {
                    YearStart = 2005,
                    YearEnd = 2008,
                    Graduated = true,
                    Comments = "Great student",
                    BioData = Person,
                    School = UofA
                };
                // Add SchoolsAttended
                context.SchoolsAttended.AddOrUpdate(x => x.SchoolsAttendedID, PersonSchoolsAttended);

                // Create Interview
                User personInterviewer = InterviewersList.ElementAt((InterviewersList.Count + i) % InterviewersList.Count);
                Interview PersonFinalInterview = new Interview
                {
                    Date = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 12:00:00"),
                    Status = Status.Final.ToString(),
                    Location = "Room 757",
                    Comments = "This is a test",
                    EditedComments = "This is an edited test",
                    StartTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 11:00:00"),
                    EndTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 11:30:00"),
                    Duration = 28,
                    EditTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 2) + " 9:30:00"),
                    InterviewerUser = personInterviewer,
                    BioData = Person
                };
                // Add Interview
                context.Interview.AddOrUpdate(x => x.InterviewID, PersonFinalInterview);

                Program chosenProgram = ProgramsList.ElementAt((ProgramsList.Count + i) % ProgramsList.Count);
                if (chosenProgram == NPS)
                {
                    // Choose SUB or SWO
                    if (i % 2 == 0)
                    {
                        chosenProgram = SUB;
                    }
                    else
                    {
                        chosenProgram = SWO;
                    }
                }
                // Create Admiral
                Admiral PersonAdmiral = new Admiral
                {
                    Decision = true,
                    Accepted = true,
                    Comments = "Great person",
                    NP500 = false,
                    NSTC = true,
                    SelfStudy = true,
                    PreSchool = false,
                    Letter = false,
                    LetterReceived = false,
                    AdmiralNotes = "Welcome aboard",
                    InviteBack = false,
                    BioData = Person,
                    Program = chosenProgram,
                    Date = new DateTime(2013, (i % 4) + 1, i + 1, 12, 0, 0)
                };
                // Add Admiral
                context.Admiral.AddOrUpdate(x => x.AdmiralID, PersonAdmiral);
            }

            // Even more people for chart generation
            for (var i = 0; i < 26; i++)
            {
                // Create BioData
                Sex sex;
                if (i % 2 == 0)
                {
                    // If i is even, they are Male
                    sex = Sex.M;
                }
                else
                {
                    // If i is odd, they are female
                    sex = Sex.F;
                }
                Sources personSource = SourcesList.ElementAt((SourcesList.Count + i) % SourcesList.Count);
                Ethnicity personEthnicity = EthnicitiesList.ElementAt((EthnicitiesList.Count + i) % EthnicitiesList.Count);
                string ssn;
                if (i < 10)
                {
                    ssn = "123-45-678" + i;
                }
                else
                {
                    ssn = "123-45-67" + i;
                }
                BioData Person = new BioData
                {
                    SSN = ssn,
                    LName = "Doe" + letters[i],
                    MName = "Reginald" + letters[i],
                    FName = "Anotherperson" + letters[i],
                    DOB = DateTime.Parse("1992-4-" + (i + 1)),
                    Sex = sex.ToString(),
                    Programs = ProgramsList,
                    FYG = 2013,
                    Ethnicity = personEthnicity,
                    Sources = personSource
                };
                // Add BioData
                context.BioData.AddOrUpdate(x => x.ID, Person);

                // Create SchoolsAttended
                SchoolsAttended PersonSchoolsAttended = new SchoolsAttended
                {
                    YearStart = 2005,
                    YearEnd = 2008,
                    Graduated = true,
                    Comments = "Great student",
                    BioData = Person,
                    School = UofA
                };
                // Add SchoolsAttended
                context.SchoolsAttended.AddOrUpdate(x => x.SchoolsAttendedID, PersonSchoolsAttended);

                // Create Interview
                User personInterviewer = InterviewersList.ElementAt((InterviewersList.Count + i) % InterviewersList.Count);
                Interview PersonFinalInterview = new Interview
                {
                    Date = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 12:00:00"),
                    Status = Status.Final.ToString(),
                    Location = "Room 757",
                    Comments = "This is a test",
                    EditedComments = "This is an edited test",
                    StartTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 11:00:00"),
                    EndTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 1) + " 11:30:00"),
                    Duration = 28,
                    EditTime = DateTime.Parse("2013/" + ((i % 4) + 1) + "/" + (i + 2) + " 9:30:00"),
                    InterviewerUser = personInterviewer,
                    BioData = Person
                };
                // Add Interview
                context.Interview.AddOrUpdate(x => x.InterviewID, PersonFinalInterview);

                Program chosenProgram = ProgramsList.ElementAt((ProgramsList.Count + i) % ProgramsList.Count);
                if (chosenProgram == NPS)
                {
                    // Choose SUB or SWO
                    if (i % 2 == 0)
                    {
                        chosenProgram = SUB;
                    }
                    else
                    {
                        chosenProgram = SWO;
                    }
                }
                // Create Admiral
                Admiral PersonAdmiral = new Admiral
                {
                    Decision = true,
                    Accepted = true,
                    Comments = "Great person",
                    NP500 = false,
                    NSTC = true,
                    SelfStudy = true,
                    PreSchool = false,
                    Letter = false,
                    LetterReceived = false,
                    AdmiralNotes = "Welcome aboard",
                    InviteBack = false,
                    BioData = Person,
                    Program = chosenProgram, //TODO: this probably isn't the best
                    Date = new DateTime(2013, (i % 4) + 1, i + 1, 12, 0, 0)
                };
                // Add Admiral
                context.Admiral.AddOrUpdate(x => x.AdmiralID, PersonAdmiral);
            }

            // Data for calendar/interviews
            for (var i = 0; i < 26; i++)
            {
                // Create BioData
                Sex sex;
                if (i % 2 == 0)
                {
                    // If i is even, they are Male
                    sex = Sex.M;
                }
                else
                {
                    // If i is odd, they are female
                    sex = Sex.F;
                }
                Sources personSource = SourcesList.ElementAt((SourcesList.Count + i) % SourcesList.Count);
                Ethnicity personEthnicity = EthnicitiesList.ElementAt((EthnicitiesList.Count + i) % EthnicitiesList.Count);
                string ssn;
                if (i < 10)
                {
                    ssn = "123-45-678" + i;
                }
                else
                {
                    ssn = "123-45-67" + i;
                }
                BioData Person = new BioData
                {
                    SSN = ssn,
                    LName = "Test" + letters[i],
                    MName = "Test" + letters[i],
                    FName = "Test" + letters[i],
                    DOB = DateTime.Parse("1992-4-" + (i + 1)),
                    Sex = sex.ToString(),
                    Programs = ProgramsList,
                    FYG = 2014,
                    Ethnicity = personEthnicity,
                    Sources = personSource
                };
                // Add BioData
                context.BioData.AddOrUpdate(x => x.ID, Person);

                DutyHistory PersonDutyHistory = new DutyHistory
                {
                    Branch = "Navy",
                    Rank = "E1",
                    Rating = "MM",
                    NUC = false,
                    BioData = Person
                };

                context.DutyHistory.AddOrUpdate(x => x.DutyHistoryID, PersonDutyHistory);

                DutyStation PersonDutyStation = new DutyStation
                {
                    Name = "A Duty Station",
                    ReportDate = DateTime.Parse("2011-1-1"),
                    DepartDate = DateTime.Parse("2013-1-1"),
                    Duties = "Corporal duties",
                    GPA = 3.94,
                    RankOverallVal = 2,
                    RankOverallTot = 100,
                    RankInRateVal = 3,
                    RankInRateTot = 80,
                    Notes = "One of the smartest",
                    DutyHistory = PersonDutyHistory
                };

                context.DutyStation.AddOrUpdate(x => x.DutyStationID, PersonDutyStation);

                SchoolsAttended PersonSchoolsAttended = new SchoolsAttended
                {
                    YearStart = 2009,
                    YearEnd = 2011,
                    Graduated = true,
                    Comments = "Great student",
                    BioData = Person,
                    School = UofA
                };

                context.SchoolsAttended.AddOrUpdate(x => x.SchoolsAttendedID, PersonSchoolsAttended);

                SchoolStandings PersonSchoolStandings1 = new SchoolStandings
                {
                    YearOfRecord = 1,
                    GPA = 3.2,
                    AOMVal = 30,
                    AOMTot = 300,
                    OOMVal = 20,
                    OOMTot = 300,
                    InMajorVal = 20,
                    InMajorTot = 200,
                    SchoolsAttended = PersonSchoolsAttended
                };
                SchoolStandings PersonSchoolStandings2 = new SchoolStandings
                {
                    YearOfRecord = 2,
                    GPA = 3.2,
                    AOMVal = 30,
                    AOMTot = 300,
                    OOMVal = 20,
                    OOMTot = 300,
                    InMajorVal = 20,
                    InMajorTot = 200,
                    SchoolsAttended = PersonSchoolsAttended
                };

                context.SchoolStandings.AddOrUpdate(x => x.SchoolStandingsID,
                    PersonSchoolStandings1,
                    PersonSchoolStandings2
                );

                Interview PersonInterview;
                User personInterviewer = InterviewersList.ElementAt((InterviewersList.Count + i) % InterviewersList.Count);
                if (i % 4 == 0)
                {
                    // Schedule
                    PersonInterview = new Interview
                    {
                        Date = DateTime.Parse("2014/5/6 12:00:00"),
                        Status = Status.Scheduled.ToString(),
                        Location = "Room 5" + i,
                        StartTime = DateTime.Parse("2014/5/6 15:00:00"),
                        EndTime = DateTime.Parse("2014/5/6 15:30:00"),
                        Duration = 28,
                        InterviewerUser = personInterviewer,
                        BioData = Person
                    };
                }
                else if (i % 4 == 1)
                {
                    // Entered
                    PersonInterview = new Interview
                    {
                        Date = DateTime.Parse("2014/5/6 12:00:00"),
                        Status = Status.Entered.ToString(),
                        Location = "Room 2" + i,
                        Comments = "Interview went normally",
                        StartTime = DateTime.Parse("2014/5/6 10:00:00"),
                        EndTime = DateTime.Parse("2014/5/6 10:30:00"),
                        Duration = 27,
                        EditTime = DateTime.Parse("2014/5/6 10:40:00"),
                        InterviewerUser = personInterviewer,
                        BioData = Person
                    };
                }
                else if (i % 4 == 2)
                {
                    // Edited
                    PersonInterview = new Interview
                    {
                        Date = DateTime.Parse("2014/5/6 12:00:00"),
                        Status = Status.Edited.ToString(),
                        Location = "Room 7" + i,
                        Comments = "Gave off a bad impression",
                        EditedComments = "Gave off a bad impression, did not respond well to questions",
                        StartTime = DateTime.Parse("2014/5/6 9:30:00"),
                        EndTime = DateTime.Parse("2014/5/6 10:00:00"),
                        Duration = 28,
                        EditTime = DateTime.Parse("2014/5/6 10:20:00"),
                        InterviewerUser = personInterviewer,
                        BioData = Person
                    };
                    if (i % 3 == 0)
                    {
                        // Add some currently being edited
                        PersonInterview.CurrentlyEditingUser = Coord;
                    }
                }
                else
                {
                    // Final
                    PersonInterview = new Interview
                    {
                        Date = DateTime.Parse("2014/5/6 12:00:00"),
                        Status = Status.Final.ToString(),
                        Location = "Room 6" + i,
                        Comments = "Gave off a bad impression",
                        EditedComments = "Gave off a bad impression, did not respond well to questions",
                        StartTime = DateTime.Parse("2014/5/6 9:00:00"),
                        EndTime = DateTime.Parse("2014/5/6 9:30:00"),
                        Duration = 28,
                        EditTime = DateTime.Parse("2014/5/6 9:35:00"),
                        InterviewerUser = personInterviewer,
                        BioData = Person
                    };
                }

                context.Interview.AddOrUpdate(x => x.InterviewID,
                    PersonInterview);

                if (i % 4 == 3)
                {
                    Program chosenProgram = ProgramsList.ElementAt((ProgramsList.Count + i) % ProgramsList.Count);
                    if (chosenProgram == NPS)
                    {
                        // Choose SUB or SWO
                        if (i % 2 == 0)
                        {
                            chosenProgram = SUB;
                        }
                        else
                        {
                            chosenProgram = SWO;
                        }
                    }
                    Admiral PersonAdmiral = new Admiral
                    {
                        Decision = true,
                        Accepted = true,
                        Comments = "Was very intelligent",
                        NP500 = false,
                        NSTC = true,
                        SelfStudy = true,
                        PreSchool = false,
                        Letter = false,
                        LetterReceived = false,
                        AdmiralNotes = "Welcome aboard",
                        InviteBack = false,
                        BioData = Person,
                        Program = chosenProgram,
                        Date = new DateTime(2014, 5, 6, 12, 0, 0)
                    };

                    context.Admiral.AddOrUpdate(x => x.AdmiralID,
                        PersonAdmiral
                    );
                }

                // ClassesAttended
                ClassesAttended PersonClassesAttended1 = new ClassesAttended
                {
                    YearTaken = 1,
                    Grade = "C+",
                    SchoolsAttended = PersonSchoolsAttended,
                    BioData = Person,
                    Classes = PHYS418
                };
                ClassesAttended PersonClassesAttended2 = new ClassesAttended
                {
                    YearTaken = 2,
                    Grade = "A-",
                    SchoolsAttended = PersonSchoolsAttended,
                    BioData = Person,
                    Classes = ENGL266
                };

                context.ClassesAttended.AddOrUpdate(x => x.ClassesAttendedID,
                PersonClassesAttended1,
                PersonClassesAttended2);

                // Degrees
                Degree PersonDegree = new Degree
                {
                    DegreeDate = DateTime.Parse("2011-5-20"),
                    SchoolsAttended = PersonSchoolsAttended,
                    Major = MATH,
                    DegreeType = BS
                };

                context.Degree.AddOrUpdate(x => x.DegreeID, PersonDegree);

                // RD
                RD PersonRD = new RD
                {
                    Type = RDType.DEVOL.ToString(),
                    Reason = "US Traitor",
                    Date = DateTime.Parse("2009-1-1"),
                    BioData = Person
                };

                context.RD.AddOrUpdate(x => x.RDID,
                    PersonRD
                );


                // Screens
                Screen PersonScreen = new Screen
                {
                    Screener = "Dorothy Gale",
                    Location = "U of A Career Fair",
                    ScreenDate = DateTime.Parse("2009-1-1"),
                    NRStatus = ScreenStatus.Maybe.ToString(),
                    INSTStatus = ScreenStatus.No.ToString(),
                    NPSStatus = ScreenStatus.Yes.ToString(),
                    ProgramsAppliedFor = ProgramsList,
                    BioData = Person
                };

                context.Screen.AddOrUpdate(x => x.ScreenID,
                    PersonScreen
                );

                // Waiver
                Waiver PersonWaiver = new Waiver
                {
                    Type = WaiverType.Drug.ToString(),
                    Date = DateTime.Parse("2012-1-1"),
                    Comments = "All the drugs",
                    BioData = Person
                };

                context.Waiver.AddOrUpdate(x => x.WaiverID,
                    PersonWaiver
                );

            } // End for loop

        }
    }
}
