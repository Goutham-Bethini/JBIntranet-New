using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class WorkOrders
    {
        public int? WorkOrder { get; set; }
        public DateTime DateShipped { get; set; }
        public string PackageWeight { get; set; }
        public DateTime PackedBy { get; set; }
        public int? ConfirmationNumber { get; set; }
    }
}