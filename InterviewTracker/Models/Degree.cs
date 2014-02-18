using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Degree
    {
        public int DegreeID { get; set; }
        public DateTime DegreeDate { get; set; }

        [ForeignKey("SchoolsAttended")]
        public int AttendedID { get; set; }
        [ForeignKey("Major")]
        public int MajorID { get; set; }
        [ForeignKey("DegreeType")]
        public int DegreeTypeID { get; set; }

    }
}