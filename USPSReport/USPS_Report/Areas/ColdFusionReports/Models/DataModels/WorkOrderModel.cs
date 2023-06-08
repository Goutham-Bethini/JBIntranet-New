using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class WorkOrderModel
    {
        public int? order_number { get; set; }

        public int? customer_po_number { get; set; }

        public DateTime creation_date { get; set; }
        public string order_type_code { get; set; }
        public int? confirmationnumber { get; set; }
        public DateTime dateshipped { get; set; }
        public int? MyProperty { get; set; }
        public int? shipment_number { get; set; }
        
    }
}