using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DutyStation
    {
        public int DutyStationID { get; set; } // PK for Duty Station table
        public DateTime ReportDate { get; set; } // Date reported
        public DateTime DepartDate { get; set; } // Date departed
        public string Duties { get; set; } // Description of duties
        public decimal GPA { get; set; } // GPA or FITREP
        public int RankOverallVal { get; set; } // Overall Rank value
        public int RankOverallTot { get; set; } // Overall Rank total number 
        public int RankInRateVal { get; set; } // Rank in NPS value
        public int RankInRateTot { get; set; } // Rank in NPS total number
        public string Notes { get; set; } // Comments about performance

        [ForeignKey("DutyHistory")]
        public int DutyHistoryID { get; set; } // FK to Duty History table
    }
}