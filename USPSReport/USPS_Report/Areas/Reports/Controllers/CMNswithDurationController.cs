using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class CMNswithDurationController : Controller
    {
        // GET: Reports/CMNswithDuration
        public ActionResult CMNswithDuration99Data()
        {
            return View();
        }

        public ActionResult ReadCMNswithDuration99Data([DataSourceRequest]DataSourceRequest request, CMNReportModel _vm)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<CMNWithDurationReportViewModel> GetData()
        {
            CMNWithDurationReport _report = new CMNWithDurationReport();
            IEnumerable<CMNWithDurationReportViewModel> VM = new List<CMNWithDurationReportViewModel>();
            VM = _report.GetDetails();
            return VM;
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}