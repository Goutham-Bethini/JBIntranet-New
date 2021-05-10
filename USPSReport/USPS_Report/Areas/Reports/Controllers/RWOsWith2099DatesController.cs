using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.Reports.Models.RWOsWith2099Dates;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class RWOsWith2099DatesController : Controller
    {
        // GET: Reports/RWOsWith2099Dates
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RWOsWith2099Dates()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RWOsWith2099Dates(RWOsWith2099DatesVM _vm)
        {
            return View(_vm);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        [HttpGet]
        public ActionResult RWOsWith2099DatesData(string operatorName, string team, [DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.Reports.Models.RWOsWith2099Dates.GetRWOsWith2099Dates(operatorName,team).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult RWOsAuditData([DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.Reports.Models.RWOsWith2099Dates.GetRWOsWith2099DatesAudit().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}