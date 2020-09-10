using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    public class Reports
    {
    }

    public class POTA_VM
    {
        public int ID { get; set; }
        public int Account { get; set; }

        public int Claim { get; set; }
        public DateTime? From_date { get; set; }
        public string Procedure_Code { get; set; }
    }
}