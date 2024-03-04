using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models
{
    public class TrackingInfo
    {
        public string OrderNumber { get; set; }
        public string CustomerPONumber { get; set; }
        public DateTime OrderCreationDate { get; set; }
        public string OrderType { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? TrackingCreationDate { get; set; }
        public int? ShipmentNumber { get; set; }
    }
}