using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class ClassesAttended
    {
        public int ClassesAttendedID { get; set; }
        public int ClassID { get; set; }
        public int YearTaken { get; set; }
        public string Grade { get; set; }

        [ForeignKey("SchoolsAttended")]
        public int AttendedID { get; set; }
        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
        [ForeignKey("Classes")]
    }
}