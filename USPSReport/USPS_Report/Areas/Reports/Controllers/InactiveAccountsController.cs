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
    public class InactiveAccountsController : Controller
    {
        // GET: Reports/InactiveAccounts
        public ActionResult InactiveAccountsData()
        {
            InactiveAccountsReport _report = new InactiveAccountsReport();
            IEnumerable<InactiveAccountsReportViewModel> VM = new List<InactiveAccountsReportViewModel>();
            VM = _report.GetDetails();
            return View(VM);
        }
        public ActionResult ReadInactiveAccountsData([DataSourceRequest]DataSourceRequest request, CMNReportModel _vm)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<InactiveAccountsReportViewModel> GetData()
        {
            InactiveAccountsReport _report = new InactiveAccountsReport();
            IEnumerable<InactiveAccountsReportViewModel> VM = new List<InactiveAccountsReportViewModel>();
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