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
    public class CMNReportingController : Controller
    {
        // GET: Reports/CMNReporting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpiringCMNReport(CMNReportModel _vm)
        {
            _vm.ExpiringCMNDetails = new List<ExpiringCMNDetails>();
            return View(_vm);
        }


        public ActionResult ReadExpiringCMNDetails([DataSourceRequest]DataSourceRequest request, CMNReportModel _vm)
        {
            var jsonResult = Json(GetData(_vm.pickedMonth, _vm.pickedYear).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<ExpiringCMNDetails> GetData(int pickedMonth, int pickedYear)
        {
            CMNReport _report = new CMNReport();
            IEnumerable<ExpiringCMNDetails> VM = new List<ExpiringCMNDetails>();
            VM = _report.GetDetails(pickedMonth, pickedYear);
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