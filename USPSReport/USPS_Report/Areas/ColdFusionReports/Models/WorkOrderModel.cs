using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models
{
    public class WorkOrderModel
    {

        public string WorkOrder { get; set; }
        public string DateShipped { get; set; }
        public string PackageWeight { get; set; }
        public string PackedBy { get; set; }
        public string ConfirmationNumber { get; set; }

    }
}