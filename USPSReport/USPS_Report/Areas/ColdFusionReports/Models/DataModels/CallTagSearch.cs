using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class CallTagSearch
    {
        public class CallTagSearchVM
        {
            [Required]
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The CallTagSearchValue field is only intergers.")]
            public string CallTagSearchValue { get; set; }
            [Required]
            public string CallTagSearchType { get; set; }

            public IList<CallTagSearchData> Details { get; set; }

        }
        public class CallTagSearchData
        {
            public int? Account { get; set; }
            public int? WorkOrder { get; set; }
            public DateTime? Shipped { get; set; }
            public string Reshipped { get; set; }
            public DateTime? DateReturned { get; set; }
            public string TagType { get; set; }
            public int? OracleRMA { get; set; }
            public string ReturnNote { get; set; }
            public string Reason { get; set; }
            public string OtherReason { get; set; }
            public string TrackingNumber { get; set; }
            public int? BoxesReturned { get; set; }
            public string ProductAndQty { get; set; }
        }

        public static IList<CallTagSearchData> GetCallTagSearchData(string searchType, int SearchValue)
        {
            List<CallTagSearchData> lstCallTagSearchData = new List<CallTagSearchData>();
            try
            {
                using (USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities _db = new USPS_Report.Areas.ColdFusionReports.Models.ColdFusionReportsEntities())
                {
                    lstCallTagSearchData = (from item in _db.sp_CallTagSearch(searchType,SearchValue)
                                   select new CallTagSearchData
                                   {
                                       Account = item.Account,
                                       WorkOrder = item.WorkOrder_ID,
                                       Shipped = item.completed_date,
                                       Reshipped = item.Reshipped,
                                       DateReturned = item.Date_Returned,
                                       TagType = item.Tag_Type,
                                       OracleRMA = item.OracleRMA,
                                       ReturnNote = item.Return_Note,
                                       Reason = item.List_Option_Text,
                                       OtherReason = item.Return_Other_Reason,
                                       TrackingNumber = item.Tracking_Number,
                                       BoxesReturned = item.Boxes_Returned,
                                       ProductAndQty = item.ProductsQty,
                                   }
                               ).ToList();
                }
                return lstCallTagSearchData;
            }
            catch (Exception ex)
            {
                return new List<CallTagSearchData>();
            }
        }

    }
}