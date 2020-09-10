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
    public class USSOperatorLookupController : Controller
    {
        // GET: Reports/USSOperatorLookup
        public ActionResult USSOperatorLookUpData()
        {
            return View();
        }

        public ActionResult ReadUSSOperatorLookupData([DataSourceRequest]DataSourceRequest request, CMNReportModel _vm)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<USSOperatorLookUp> GetData()
        {
            IEnumerable<USSOperatorLookUp> VM = new List<USSOperatorLookUp>();
            VM = USSOperatorLookUPReport.GetUSSOperatorLoopUp();
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