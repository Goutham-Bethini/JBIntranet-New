using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReportAudit
    {
        public class ReportsAuditVM
        {
            [Required]
            public string ReportId { get; set; }
            public IList<ReportAuditData> Details { get; set; }
        }
        public class ReportAuditData
        {
            public string Report { get; set; }
            public string UserName { get; set; }
            public DateTime? AccessDate { get; set; }
        }

        public static IList<ReportAuditData> GetReportAuditInfo(int reportId)
        {
            List<ReportAuditData> lstReportAuditData = new List<ReportAuditData>();
            try
            {
                using (ReportsEntities _db = new ReportsEntities())
                {
                    lstReportAuditData = (from item in _db.sp_GetReportAuditInfo(reportId)
                                                select new ReportAuditData
                                                {
                                                    Report = item.Report,
                                                    UserName = item.OperatorName,
                                                    AccessDate = item.ReportAccessDate
                                                }
                               ).ToList();
                }
                return lstReportAuditData;
            }
            catch (Exception ex)
            {
                return new List<ReportAuditData>();
            }
        }
    }
    
}