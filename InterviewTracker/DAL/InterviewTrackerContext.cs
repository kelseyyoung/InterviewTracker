using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using InterviewTracker.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InterviewTracker.DAL
{
    public class InterviewTrackerContext : DbContext
    {
        public DbSet<Admiral> Admiral { get; set; }
        public DbSet<BioData> BioData { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassesAttended> ClassesAttended { get; set; }
        public DbSet<Degree> Degree { get; set; }
        public DbSet<DegreeType> DegreeType { get; set; }
        public DbSet<DutyHistory> DutyHistory { get; set; }
        public DbSet<DutyStation> DutyStation { get; set; }
        public DbSet<Ethnicity> Ethnicity { get; set; }
        public DbSet<FYGoals> FYGoals { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<Major> Major { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<RD> RD { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<SchoolsAttended> SchoolsAttended { get; set; }
        public DbSet<SchoolStandings> SchoolStandings { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<ServSel> ServSel { get; set; }
        public DbSet<Sources> Sources { get; set; }
        public DbSet<SubSources> SubSources { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Waiver> Waiver { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevents tables from being Pluralized
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Prevents cascading delete on all objects
            // TODO: possibly change this to only prevent for some objects
            // There is a cycle with BioData, ClassesAttended, and SchoolsAttended
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Base call
            base.OnModelCreating(modelBuilder);
        }

    }
}