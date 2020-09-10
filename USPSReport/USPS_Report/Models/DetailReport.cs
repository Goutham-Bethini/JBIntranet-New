using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class DetailReport
    {
      
        public string _id { get; set; }
        public bool? FileExists { get; set; }

        public Int32? Acccount { get; set; }
        public string Tracknum { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DDate { get; set; }
        public string Amount { get; set; }
      
        public bool? PdfExists { get; set; }

        public string Weight { get; set; }

        public string TDate { get; set; }

        public string PostMDate { get; set; }

        public string CustomerID { get; set; }

        public string PICNumber { get; set; }
        public string Name { get; set; }

        public string DStatus { get; set; }

        public string Confirmation { get; set; }

        public Int32? WorkOrderId { get; set; }
        public string Invoice_date { get; set; }
        public string Invoice_number { get; set; }

    }
}