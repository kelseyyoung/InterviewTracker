using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class Ethnicity
    {
        public int EthnicityID { get; set; } // PK for Ethnicity table
        [Required]
        public String EthnicityValue { get; set; } // Ethnicity
    }
}