using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class WorkOrdersModel
    {
        public int ID_WorkOrder { get; set; }
        public DateTime? DateShipped { get; set; }
        public decimal? IntWeight { get; set; }
        public string UserID { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}