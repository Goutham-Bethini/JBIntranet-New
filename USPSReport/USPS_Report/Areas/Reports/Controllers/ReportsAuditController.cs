using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.Reports.Models.ReportAudit;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ReportsAuditController : Controller
    {
        // GET: Reports/ReportsAudit
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportsAudit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReportsAudit(ReportsAuditVM _vm)
        {
            return View(_vm);
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        public ActionResult ReportAuditData(string reportId, [DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.Reports.Models.ReportAudit.GetReportAuditInfo(Convert.ToInt32(reportId)).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}