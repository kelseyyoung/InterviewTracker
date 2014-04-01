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
        public ICollection<Admiral> Admiral { get; set; }
        public ICollection<BioData> BioData { get; set; }
        public ICollection<Classes> Classes { get; set; }
        public ICollection<ClassesAttended> ClassesAttended { get; set; }
        public ICollection<Degree> Degree { get; set; }
        public ICollection<DegreeType> DegreeType { get; set; }
        public ICollection<DutyHistory> DutyHistory { get; set; }
        public ICollection<DutyStation> DutyStation { get; set; }
        public ICollection<Ethnicity> Ethnicity { get; set; }
        public ICollection<FYGoals> FYGoals { get; set; }
        public ICollection<Interview> Interview { get; set; }
        public ICollection<Major> Major { get; set; }
        public ICollection<Program> Program { get; set; }
        public ICollection<RD> RD { get; set; }
        public ICollection<School> School { get; set; }
        public ICollection<SchoolsAttended> SchoolsAttended { get; set; }
        public ICollection<SchoolStandings> SchoolStandings { get; set; }
        public ICollection<Screen> Screen { get; set; }
        public ICollection<ServSel> ServSel { get; set; }
        public ICollection<Sources> Sources { get; set; }
        public ICollection<SubSources> SubSources { get; set; }
        public ICollection<User> User { get; set; }
        public ICollection<Waiver> Waiver { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevents tables from being Pluralized
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Prevents cascading delete on all objects
            // TODO: possibly change this to only prevent for some objects
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Base call
            base.OnModelCreating(modelBuilder);
        }

    }
}