using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InterviewTracker.Models
{
    public class DutyHistory
    {
        public int DutyHistoryID { get; set; }
        public string Branch { get; set; }
        public string Rank { get; set; }
        public string Rating { get; set; }
        public bool NUC { get; set; }

        [ForeignKey("BioData")]
        public int BioDataID { get; set; }
    }
}