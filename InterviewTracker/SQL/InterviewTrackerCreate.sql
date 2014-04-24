CREATE TABLE [dbo].[Admiral] (
    [AdmiralID] [int] NOT NULL IDENTITY,
    [Decision] [bit] NOT NULL,
    [Accepted] [bit] NOT NULL,
    [Comments] [nvarchar](max),
    [NP500] [bit] NOT NULL,
    [NSTC] [bit] NOT NULL,
    [SelfStudy] [bit] NOT NULL,
    [PreSchool] [bit] NOT NULL,
    [Letter] [bit] NOT NULL,
    [LetterReceived] [bit] NOT NULL,
    [AdmiralNotes] [nvarchar](max),
    [InviteBack] [bit] NOT NULL,
    [SERVSEL] [nvarchar](max) NOT NULL,
    [BioDataID] [int] NOT NULL,
    [InterviewID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Admiral] PRIMARY KEY ([AdmiralID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[Admiral]([BioDataID])
CREATE INDEX [IX_InterviewID] ON [dbo].[Admiral]([InterviewID])
CREATE TABLE [dbo].[BioData] (
    [ID] [int] NOT NULL IDENTITY,
    [SSN] [nvarchar](max) NOT NULL,
    [LName] [nvarchar](max) NOT NULL,
    [FName] [nvarchar](max) NOT NULL,
    [MName] [nvarchar](max) NOT NULL,
    [Suffix] [nvarchar](max),
    [DOB] [datetime] NOT NULL,
    [Sex] [nvarchar](max) NOT NULL,
    [UnitID] [int],
    [FYG] [int] NOT NULL,
    [ACTM] [int],
    [ACTV] [int],
    [SATM] [int],
    [SATV] [int],
    [EthnicityID] [int] NOT NULL,
    [SourcesID] [int] NOT NULL,
    [SubSourcesID] [int],
    CONSTRAINT [PK_dbo.BioData] PRIMARY KEY ([ID])
)
CREATE INDEX [IX_EthnicityID] ON [dbo].[BioData]([EthnicityID])
CREATE INDEX [IX_SourcesID] ON [dbo].[BioData]([SourcesID])
CREATE INDEX [IX_SubSourcesID] ON [dbo].[BioData]([SubSourcesID])
CREATE TABLE [dbo].[Program] (
    [ProgramID] [int] NOT NULL IDENTITY,
    [ProgramValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.Program] PRIMARY KEY ([ProgramID])
)
CREATE TABLE [dbo].[Screen] (
    [ScreenID] [int] NOT NULL IDENTITY,
    [Screener] [nvarchar](max) NOT NULL,
    [Location] [nvarchar](max) NOT NULL,
    [ScreenDate] [datetime] NOT NULL,
    [NRStatus] [nvarchar](max) NOT NULL,
    [INSTStatus] [nvarchar](max) NOT NULL,
    [NPSStatus] [nvarchar](max) NOT NULL,
    [Notes] [nvarchar](max) NOT NULL,
    [BioDataID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Screen] PRIMARY KEY ([ScreenID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[Screen]([BioDataID])
CREATE TABLE [dbo].[Ethnicity] (
    [EthnicityID] [int] NOT NULL IDENTITY,
    [EthnicityValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.Ethnicity] PRIMARY KEY ([EthnicityID])
)
CREATE TABLE [dbo].[Sources] (
    [SourcesID] [int] NOT NULL IDENTITY,
    [SourcesValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.Sources] PRIMARY KEY ([SourcesID])
)
CREATE TABLE [dbo].[SubSources] (
    [SubSourcesID] [int] NOT NULL IDENTITY,
    [SubSourcesValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.SubSources] PRIMARY KEY ([SubSourcesID])
)
CREATE TABLE [dbo].[SchoolsAttended] (
    [SchoolsAttendedID] [int] NOT NULL IDENTITY,
    [YearStart] [int] NOT NULL,
    [YearEnd] [int] NOT NULL,
    [Graduated] [bit] NOT NULL,
    [Comments] [nvarchar](max),
    [BioDataID] [int] NOT NULL,
    [SchoolID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.SchoolsAttended] PRIMARY KEY ([SchoolsAttendedID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[SchoolsAttended]([BioDataID])
CREATE INDEX [IX_SchoolID] ON [dbo].[SchoolsAttended]([SchoolID])
CREATE TABLE [dbo].[School] (
    [SchoolID] [int] NOT NULL IDENTITY,
    [SchoolValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.School] PRIMARY KEY ([SchoolID])
)
CREATE TABLE [dbo].[SchoolStandings] (
    [SchoolStandingsID] [int] NOT NULL IDENTITY,
    [YearOfRecord] [int] NOT NULL,
    [GPA] [float] NOT NULL,
    [AOMVal] [int] NOT NULL,
    [AOMTot] [int] NOT NULL,
    [OOMVal] [int] NOT NULL,
    [OOMTot] [int] NOT NULL,
    [InMajorVal] [int] NOT NULL,
    [InMajorTot] [int] NOT NULL,
    [SchoolsAttendedID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.SchoolStandings] PRIMARY KEY ([SchoolStandingsID])
)
CREATE INDEX [IX_SchoolsAttendedID] ON [dbo].[SchoolStandings]([SchoolsAttendedID])
CREATE TABLE [dbo].[ClassesAttended] (
    [ClassesAttendedID] [int] NOT NULL IDENTITY,
    [YearTaken] [int] NOT NULL,
    [Grade] [nvarchar](max) NOT NULL,
    [SchoolsAttendedID] [int] NOT NULL,
    [BioDataID] [int] NOT NULL,
    [ClassesID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.ClassesAttended] PRIMARY KEY ([ClassesAttendedID])
)
CREATE INDEX [IX_SchoolsAttendedID] ON [dbo].[ClassesAttended]([SchoolsAttendedID])
CREATE INDEX [IX_BioDataID] ON [dbo].[ClassesAttended]([BioDataID])
CREATE INDEX [IX_ClassesID] ON [dbo].[ClassesAttended]([ClassesID])
CREATE TABLE [dbo].[Classes] (
    [ClassesID] [int] NOT NULL IDENTITY,
    [Subject] [nvarchar](max) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](max) NOT NULL,
    [Technical] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Classes] PRIMARY KEY ([ClassesID])
)
CREATE TABLE [dbo].[Degree] (
    [DegreeID] [int] NOT NULL IDENTITY,
    [DegreeDate] [datetime] NOT NULL,
    [SchoolsAttendedID] [int] NOT NULL,
    [MajorID] [int] NOT NULL,
    [DegreeTypeID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Degree] PRIMARY KEY ([DegreeID])
)
CREATE INDEX [IX_SchoolsAttendedID] ON [dbo].[Degree]([SchoolsAttendedID])
CREATE INDEX [IX_MajorID] ON [dbo].[Degree]([MajorID])
CREATE INDEX [IX_DegreeTypeID] ON [dbo].[Degree]([DegreeTypeID])
CREATE TABLE [dbo].[Major] (
    [MajorID] [int] NOT NULL IDENTITY,
    [MajorValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.Major] PRIMARY KEY ([MajorID])
)
CREATE TABLE [dbo].[DegreeType] (
    [DegreeTypeID] [int] NOT NULL IDENTITY,
    [DegreeTypeValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.DegreeType] PRIMARY KEY ([DegreeTypeID])
)
CREATE TABLE [dbo].[DutyHistory] (
    [DutyHistoryID] [int] NOT NULL IDENTITY,
    [Branch] [nvarchar](max) NOT NULL,
    [Rank] [nvarchar](max) NOT NULL,
    [Rating] [nvarchar](max) NOT NULL,
    [NUC] [bit] NOT NULL,
    [BioDataID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.DutyHistory] PRIMARY KEY ([DutyHistoryID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[DutyHistory]([BioDataID])
CREATE TABLE [dbo].[DutyStation] (
    [DutyStationID] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [ReportDate] [datetime] NOT NULL,
    [DepartDate] [datetime] NOT NULL,
    [Duties] [nvarchar](max) NOT NULL,
    [GPA] [float] NOT NULL,
    [RankOverallVal] [int] NOT NULL,
    [RankOverallTot] [int] NOT NULL,
    [RankInRateVal] [int] NOT NULL,
    [RankInRateTot] [int] NOT NULL,
    [Notes] [nvarchar](max) NOT NULL,
    [DutyHistoryID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.DutyStation] PRIMARY KEY ([DutyStationID])
)
CREATE INDEX [IX_DutyHistoryID] ON [dbo].[DutyStation]([DutyHistoryID])
CREATE TABLE [dbo].[Interview] (
    [InterviewID] [int] NOT NULL IDENTITY,
    [Date] [datetime] NOT NULL,
    [Status] [nvarchar](max) NOT NULL,
    [Location] [nvarchar](max) NOT NULL,
    [Comments] [nvarchar](max),
    [EditedComments] [nvarchar](max),
    [StartTime] [datetime] NOT NULL,
    [EndTime] [datetime] NOT NULL,
    [Duration] [int] NOT NULL,
    [EditTime] [datetime],
    [NR] [bit],
    [INST] [bit],
    [NPS] [bit],
    [PXO] [bit],
    [EDO] [bit],
    [ENLTECH] [bit],
    [NR1] [bit],
    [SUPPLY] [bit],
    [EOOW] [bit],
    [DOE] [bit],
    [CurrentlyEditingID] [int],
    [InterviewerID] [int] NOT NULL,
    [BioDataID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Interview] PRIMARY KEY ([InterviewID])
)
CREATE INDEX [IX_CurrentlyEditingID] ON [dbo].[Interview]([CurrentlyEditingID])
CREATE INDEX [IX_InterviewerID] ON [dbo].[Interview]([InterviewerID])
CREATE INDEX [IX_BioDataID] ON [dbo].[Interview]([BioDataID])
CREATE TABLE [dbo].[User] (
    [UserID] [int] NOT NULL IDENTITY,
    [LName] [nvarchar](max) NOT NULL,
    [Initials] [nvarchar](max) NOT NULL,
    [LoginID] [nvarchar](max) NOT NULL,
    [Password] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](max) NOT NULL,
    [NR] [bit] NOT NULL,
    [INST] [bit] NOT NULL,
    [NPS] [bit] NOT NULL,
    [PXO] [bit] NOT NULL,
    [EDO] [bit] NOT NULL,
    [ENLTECH] [bit] NOT NULL,
    [NR1] [bit] NOT NULL,
    [SUPPLY] [bit] NOT NULL,
    [EOOW] [bit] NOT NULL,
    [DOE] [bit] NOT NULL,
    [UserGroup] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.User] PRIMARY KEY ([UserID])
)
CREATE TABLE [dbo].[RD] (
    [RDID] [int] NOT NULL IDENTITY,
    [Type] [nvarchar](max) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Reason] [nvarchar](max) NOT NULL,
    [BioDataID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.RD] PRIMARY KEY ([RDID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[RD]([BioDataID])
CREATE TABLE [dbo].[Waiver] (
    [WaiverID] [int] NOT NULL IDENTITY,
    [Type] [nvarchar](max) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Comments] [nvarchar](max) NOT NULL,
    [BioDataID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Waiver] PRIMARY KEY ([WaiverID])
)
CREATE INDEX [IX_BioDataID] ON [dbo].[Waiver]([BioDataID])
CREATE TABLE [dbo].[FYGoals] (
    [FY] [int] NOT NULL IDENTITY,
    [Source] [nvarchar](max) NOT NULL,
    [SUB] [int] NOT NULL,
    [SWO] [int] NOT NULL,
    [NR] [int] NOT NULL,
    [INST] [int] NOT NULL,
    CONSTRAINT [PK_dbo.FYGoals] PRIMARY KEY ([FY])
)
CREATE TABLE [dbo].[ServSel] (
    [ServSelID] [int] NOT NULL IDENTITY,
    [ServSelValue] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.ServSel] PRIMARY KEY ([ServSelID])
)
CREATE TABLE [dbo].[ProgramBioData] (
    [Program_ProgramID] [int] NOT NULL,
    [BioData_ID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.ProgramBioData] PRIMARY KEY ([Program_ProgramID], [BioData_ID])
)
CREATE INDEX [IX_Program_ProgramID] ON [dbo].[ProgramBioData]([Program_ProgramID])
CREATE INDEX [IX_BioData_ID] ON [dbo].[ProgramBioData]([BioData_ID])
CREATE TABLE [dbo].[ScreenProgram] (
    [Screen_ScreenID] [int] NOT NULL,
    [Program_ProgramID] [int] NOT NULL,
    CONSTRAINT [PK_dbo.ScreenProgram] PRIMARY KEY ([Screen_ScreenID], [Program_ProgramID])
)
CREATE INDEX [IX_Screen_ScreenID] ON [dbo].[ScreenProgram]([Screen_ScreenID])
CREATE INDEX [IX_Program_ProgramID] ON [dbo].[ScreenProgram]([Program_ProgramID])
ALTER TABLE [dbo].[Admiral] ADD CONSTRAINT [FK_dbo.Admiral_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[Admiral] ADD CONSTRAINT [FK_dbo.Admiral_dbo.Interview_InterviewID] FOREIGN KEY ([InterviewID]) REFERENCES [dbo].[Interview] ([InterviewID])
ALTER TABLE [dbo].[BioData] ADD CONSTRAINT [FK_dbo.BioData_dbo.Ethnicity_EthnicityID] FOREIGN KEY ([EthnicityID]) REFERENCES [dbo].[Ethnicity] ([EthnicityID])
ALTER TABLE [dbo].[BioData] ADD CONSTRAINT [FK_dbo.BioData_dbo.Sources_SourcesID] FOREIGN KEY ([SourcesID]) REFERENCES [dbo].[Sources] ([SourcesID])
ALTER TABLE [dbo].[BioData] ADD CONSTRAINT [FK_dbo.BioData_dbo.SubSources_SubSourcesID] FOREIGN KEY ([SubSourcesID]) REFERENCES [dbo].[SubSources] ([SubSourcesID])
ALTER TABLE [dbo].[Screen] ADD CONSTRAINT [FK_dbo.Screen_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[SchoolsAttended] ADD CONSTRAINT [FK_dbo.SchoolsAttended_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[SchoolsAttended] ADD CONSTRAINT [FK_dbo.SchoolsAttended_dbo.School_SchoolID] FOREIGN KEY ([SchoolID]) REFERENCES [dbo].[School] ([SchoolID])
ALTER TABLE [dbo].[SchoolStandings] ADD CONSTRAINT [FK_dbo.SchoolStandings_dbo.SchoolsAttended_SchoolsAttendedID] FOREIGN KEY ([SchoolsAttendedID]) REFERENCES [dbo].[SchoolsAttended] ([SchoolsAttendedID])
ALTER TABLE [dbo].[ClassesAttended] ADD CONSTRAINT [FK_dbo.ClassesAttended_dbo.SchoolsAttended_SchoolsAttendedID] FOREIGN KEY ([SchoolsAttendedID]) REFERENCES [dbo].[SchoolsAttended] ([SchoolsAttendedID])
ALTER TABLE [dbo].[ClassesAttended] ADD CONSTRAINT [FK_dbo.ClassesAttended_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[ClassesAttended] ADD CONSTRAINT [FK_dbo.ClassesAttended_dbo.Classes_ClassesID] FOREIGN KEY ([ClassesID]) REFERENCES [dbo].[Classes] ([ClassesID])
ALTER TABLE [dbo].[Degree] ADD CONSTRAINT [FK_dbo.Degree_dbo.SchoolsAttended_SchoolsAttendedID] FOREIGN KEY ([SchoolsAttendedID]) REFERENCES [dbo].[SchoolsAttended] ([SchoolsAttendedID])
ALTER TABLE [dbo].[Degree] ADD CONSTRAINT [FK_dbo.Degree_dbo.Major_MajorID] FOREIGN KEY ([MajorID]) REFERENCES [dbo].[Major] ([MajorID])
ALTER TABLE [dbo].[Degree] ADD CONSTRAINT [FK_dbo.Degree_dbo.DegreeType_DegreeTypeID] FOREIGN KEY ([DegreeTypeID]) REFERENCES [dbo].[DegreeType] ([DegreeTypeID])
ALTER TABLE [dbo].[DutyHistory] ADD CONSTRAINT [FK_dbo.DutyHistory_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[DutyStation] ADD CONSTRAINT [FK_dbo.DutyStation_dbo.DutyHistory_DutyHistoryID] FOREIGN KEY ([DutyHistoryID]) REFERENCES [dbo].[DutyHistory] ([DutyHistoryID])
ALTER TABLE [dbo].[Interview] ADD CONSTRAINT [FK_dbo.Interview_dbo.User_CurrentlyEditingID] FOREIGN KEY ([CurrentlyEditingID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Interview] ADD CONSTRAINT [FK_dbo.Interview_dbo.User_InterviewerID] FOREIGN KEY ([InterviewerID]) REFERENCES [dbo].[User] ([UserID])
ALTER TABLE [dbo].[Interview] ADD CONSTRAINT [FK_dbo.Interview_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[RD] ADD CONSTRAINT [FK_dbo.RD_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[Waiver] ADD CONSTRAINT [FK_dbo.Waiver_dbo.BioData_BioDataID] FOREIGN KEY ([BioDataID]) REFERENCES [dbo].[BioData] ([ID])
ALTER TABLE [dbo].[ProgramBioData] ADD CONSTRAINT [FK_dbo.ProgramBioData_dbo.Program_Program_ProgramID] FOREIGN KEY ([Program_ProgramID]) REFERENCES [dbo].[Program] ([ProgramID]) ON DELETE CASCADE
ALTER TABLE [dbo].[ProgramBioData] ADD CONSTRAINT [FK_dbo.ProgramBioData_dbo.BioData_BioData_ID] FOREIGN KEY ([BioData_ID]) REFERENCES [dbo].[BioData] ([ID]) ON DELETE CASCADE
ALTER TABLE [dbo].[ScreenProgram] ADD CONSTRAINT [FK_dbo.ScreenProgram_dbo.Screen_Screen_ScreenID] FOREIGN KEY ([Screen_ScreenID]) REFERENCES [dbo].[Screen] ([ScreenID]) ON DELETE CASCADE
ALTER TABLE [dbo].[ScreenProgram] ADD CONSTRAINT [FK_dbo.ScreenProgram_dbo.Program_Program_ProgramID] FOREIGN KEY ([Program_ProgramID]) REFERENCES [dbo].[Program] ([ProgramID]) ON DELETE CASCADE
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](255) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId])
)