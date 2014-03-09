namespace InterviewTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using InterviewTracker.Models;
    using System.Collections.Generic;

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
            ServSel servsel = new ServSel { ServSelValue = "What is this" };

            context.ServSel.AddOrUpdate(i => i.ServSelID,
                servsel
            );

            /**** Normal Table Data ****/

            // BioData
            List<Program> KelseyPrograms = new List<Program> { NR, INST };
            BioData Kelsey = new BioData
                {
                    SSN = 123456789,
                    LName = "Young",
                    MName = "Jeanne",
                    FName = "Kelsey",
                    DOB = DateTime.Parse("1992-4-16"),
                    Sex = Sex.F,
                    Programs = KelseyPrograms,
                    FYG = 2014,
                    Ethnicity = CAUC,
                    Sources = NUPOC
                };
            List<Program> JohnPrograms = new List<Program> { NR, ENLTECH, DOE };
            BioData John = new BioData
                {
                    SSN = 223456789,
                    LName = "Doe",
                    MName = "Edward",
                    FName = "John",
                    DOB = DateTime.Parse("1990-8-12"),
                    Sex = Sex.M,
                    Programs = JohnPrograms,
                    FYG = 2013,
                    Ethnicity = BLACK,
                    Sources = NROTC,
                    SubSources = ECP
                };
            List<Program> StevePrograms = new List<Program> { NPS, PXO };
            BioData Steve = new BioData
                {
                    SSN = 323456789,
                    LName = "Aardvark",
                    MName = "Hank",
                    FName = "Steve",
                    DOB = DateTime.Parse("1994-12-25"),
                    Sex = Sex.M,
                    Programs = StevePrograms,
                    FYG = 2012,
                    Ethnicity = API,
                    Sources = CWO
                };

            context.BioData.AddOrUpdate (i => i.ID,
                Kelsey,
                John,
                Steve
            );

            // Users
            User Admin = new User
            {
                LName = "admin",
                Initials = "aa",
                LoginID = "adminaa",
                Password = "admin",
                Code = "08A",
                UserGroup = UserGroup.ADMIN
            };
            User Coord = new User
            {
                LName = "coord",
                Initials = "bb",
                LoginID = "coordbb",
                Password = "coord",
                Code = "08B",
                UserGroup = UserGroup.COORD
            };
            User Interviewer = new User
            {
                LName = "interviewer",
                Initials = "cc",
                LoginID = "interviewercc",
                Password = "interviewer",
                Code = "08C",
                UserGroup = UserGroup.INTER
            };

            context.User.AddOrUpdate(i => i.UserID,
                Admin,
                Coord,
                Interviewer
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
                ReportDate = DateTime.Parse("2008-1-1"),
                DepartDate = DateTime.Parse("2011-1-1"),
                Duties = "Was an officer",
                GPA = 3.5M,
                RankOverallVal = 20,
                RankOverallTot = 200,
                RankInRateVal = 100,
                RankInRateTot = 200,
                Notes = "Was an excellent officer",
                DutyHistory = KelseyDutyHistory
            };
            DutyStation JohnDutyStation = new DutyStation
            {
                ReportDate = DateTime.Parse("2009-1-1"),
                DepartDate = DateTime.Parse("2009-12-31"),
                Duties = "Cooked meals",
                GPA = 2.4M,
                RankOverallVal = 100,
                RankOverallTot = 300,
                RankInRateVal = 200,
                RankInRateTot = 300,
                Notes = "Not a great cook",
                DutyHistory = JohnDutyHistory
            };
            DutyStation SteveDutyStation = new DutyStation
            {
                ReportDate = DateTime.Parse("2011-1-1"),
                DepartDate = DateTime.Parse("2013-1-1"),
                Duties = "Corporal duties",
                GPA = 3.94M,
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

            // FY Goals
            FYGoals FY2012 = new FYGoals
            {
                FY = 2012,
                Source = NROTC,
                SUB = 30,
                SWO = 25,
                NR = 10,
                INST = 50
            };
            FYGoals FY2013 = new FYGoals
            {
                FY = 2013,
                Source = NUPOC,
                SUB = 10,
                SWO = 30,
                NR = 13,
                INST = 77
            };
            FYGoals FY2014 = new FYGoals
            {
                FY = 2014,
                Source = USNA,
                SUB = 11,
                SWO = 84,
                NR = 44,
                INST = 29
            };

            context.FYGoals.AddOrUpdate(i => i.FY,
                FY2012,
                FY2013,
                FY2014
            );

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
            SchoolStandings KelseySchoolStandings = new SchoolStandings
            {
                YearOfRecord = 2010,
                GPA = 3.2M,
                AOMVal = 30,
                AOMTot = 300,
                OOMVal = 20,
                OOMTot = 300,
                InMajorVal = 20,
                InMajorTot = 200,
                SchoolsAttended = KelseySchoolsAttended
            };
            SchoolStandings JohnSchoolStandings = new SchoolStandings
            {
                YearOfRecord = 2009,
                GPA = 2.3M,
                AOMVal = 10,
                AOMTot = 600,
                OOMVal = 500,
                OOMTot = 500,
                InMajorVal = 87,
                InMajorTot = 500,
                SchoolsAttended = JohnSchoolsAttended
            };
            SchoolStandings SteveSchoolStandings = new SchoolStandings
            {
                YearOfRecord = 2012,
                GPA = 3.9M,
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
                JohnSchoolStandings,
                SteveSchoolStandings
            );

            // Interviews
            Interview KelseyInterview = new Interview
            {
                Date = DateTime.Parse("2014-5-13"),
                Status = Status.Scheduled,
                StartTime = DateTime.Parse("2014/5/13 15:00:00"),
                EndTime = DateTime.Parse("2014/5/13 16:00:00"),
                Duration = 60,
                NR = false,
                INST = false,
                NPS = false,
                PXO = true,
                EDO = true,
                ENLTECH = false,
                NR1 = true,
                SUPPLY = true,
                EOOW = false,
                DOE = false,
                InterviewerUser = Interviewer,
                BioData = Kelsey
            };
            Interview JohnInterview = new Interview
            {
                Date = DateTime.Parse("2014-1-20"),
                Status = Status.Entered,
                Comments = "Seemed like a good worker",
                StartTime = DateTime.Parse("2014/1/20 9:00:00"),
                EndTime = DateTime.Parse("2013/5/13 11:00:00"),
                Duration = 120,
                NR = true,
                INST = false,
                NPS = true,
                PXO = true,
                EDO = false,
                ENLTECH = false,
                NR1 = true,
                SUPPLY = false,
                EOOW = false,
                DOE = true,
                InterviewerUser = Interviewer,
                BioData = John
            };
            Interview SteveInterview = new Interview
            {
                Date = DateTime.Parse("2014-2-2"),
                Status = Status.Edited,
                Comments = "Gave off a bad impression",
                EditedComments = "Gave off a bad impression, did not respond well to questions",
                StartTime = DateTime.Parse("2014/2/2 11:00:00"),
                EndTime = DateTime.Parse("2014/2/2 14:00:00"),
                Duration = 180,
                EditTime = DateTime.Parse("2014/2/5 11:30:00"),
                NR = true,
                INST = true,
                NPS = true,
                PXO = false,
                EDO = false,
                ENLTECH = true,
                NR1 = false,
                SUPPLY = false,
                EOOW = false,
                DOE = false,
                CurrentlyEditingUser = Coord,
                InterviewerUser = Interviewer,
                BioData = John
            };
            
            context.Interview.AddOrUpdate(i => i.InterviewerID,
                KelseyInterview,
                JohnInterview,
                SteveInterview
            );
            
            // Classes Attended
            ClassesAttended KelseyClassesAttended = new ClassesAttended
            {
                YearTaken = 2012,
                Grade = "B",
                SchoolsAttended = KelseySchoolsAttended,
                BioData = Kelsey,
                Classes = MATH109

            };
            ClassesAttended JohnClassesAttended = new ClassesAttended
            {
                YearTaken = 2010,
                Grade = "C+",
                SchoolsAttended = JohnSchoolsAttended,
                BioData = John,
                Classes = PHYS418
            };
            ClassesAttended SteveClassesAttended = new ClassesAttended
            {
                YearTaken = 2013,
                Grade = "A-",
                SchoolsAttended = SteveSchoolsAttended,
                BioData = Steve,
                Classes = ENGL266
            };

            context.ClassesAttended.AddOrUpdate(i => i.ClassesAttendedID,
                KelseyClassesAttended,
                JohnClassesAttended,
                SteveClassesAttended
            );

            // Degree
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
                Type = RDType.DEVOL,
                Reason = "US Traitor",
                Date = DateTime.Parse("2009-1-1"),
                BioData = John
            };

            context.RD.AddOrUpdate(i => i.RDID,
                JohnRD
            );

            /*
            // Screen
            Screen KelseyScreen = new Screen
            {

            };
            Screen JohnScreen = new Screen
            {

            };
            Screen SteveScreen = new Screen
            {

            };

            context.Screen.AddOrUpdate(i => i.ScreenID,
                KelseyScreen,
                JohnScreen,
                SteveScreen
            );
            */

            // Admiral
            Admiral SteveAdmiral = new Admiral
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
                SERVSEL = "what is this",
                BioData = Steve,
                Interview = SteveInterview
            };

            context.Admiral.AddOrUpdate(i => i.AdmiralID,
                SteveAdmiral
            );

            // Waiver
            Waiver SteveWaiver = new Waiver
            {
                Type = WaiverType.Drug,
                Date = DateTime.Parse("2012-1-1"),
                Comments = "Addicted to the pipe",
                BioData = Steve
            };

            context.Waiver.AddOrUpdate(i => i.WaiverID,
                SteveWaiver
            );

        }
    }
}
