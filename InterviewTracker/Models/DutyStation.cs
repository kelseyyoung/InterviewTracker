using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DutyStation
    {
        public int DutyStationID { get; set; } // PK for Duty Station table
        [Required]
        public DateTime ReportDate { get; set; } // Date reported
        [Required]
        public DateTime DepartDate { get; set; } // Date departed
        [Required]
        public string Duties { get; set; } // Description of duties
        [Required]
        [RegularExpression(@"^[0-4]\.[0-9]([0-9])?$", ErrorMessage="GPA must be a valid decimal Grade Point Average")]
        public double? GPA { get; set; } // GPA or FITREP
        [Required]
        public int? RankOverallVal { get; set; } // Overall Rank value
        [Required]
        public int? RankOverallTot { get; set; } // Overall Rank total number 
        [Required]
        public int? RankInRateVal { get; set; } // Rank in NPS value
        [Required]
        public int? RankInRateTot { get; set; } // Rank in NPS total number
        [Required]
        public string Notes { get; set; } // Comments about performance

        [ForeignKey("DutyHistory")]
        public virtual int DutyHistoryID { get; set; } // FK to Duty History table
        public virtual DutyHistory DutyHistory { get; set; }
    }
}