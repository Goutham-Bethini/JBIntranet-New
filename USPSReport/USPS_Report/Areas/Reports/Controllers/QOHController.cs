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
    public class QOHController : Controller
    {
        // GET: Reports/QOH
        public ActionResult QOHReport(QOHSearchDataDetails _vm)
        {
            _vm.QOHDetails = new List<QOHDetails>();
            return View(_vm);
        }

        public ActionResult ReadQOHDetails([DataSourceRequest]DataSourceRequest request, QOHSearchDataDetails _vm)
        {
            var jsonResult = Json(GetData(_vm.ProductCode).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<QOHDetails> GetData(string ProductCode)
        {
            QOHReport _report = new QOHReport();
            IEnumerable<QOHDetails> VM = new List<QOHDetails>();
            VM = _report.GetDetails(ProductCode);
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