using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DutyStation
    {
        public int DutyStationID { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime DepartDate { get; set; }
        public string Duties { get; set; }
        public float GPA { get; set; }
        public int RankOverallVal { get; set; }
        public int RankOverallTot { get; set; }
        public int RankInRateVal { get; set; }
        public int RankInRateTot { get; set; }
        public string Notes { get; set; }

        [ForeignKey("DutyHistory")]
        public int DutyHistoryID { get; set; }
    }
}