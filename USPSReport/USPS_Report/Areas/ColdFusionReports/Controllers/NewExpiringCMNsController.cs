using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.NewExpiringCMNs;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class NewExpiringCMNsController : Controller
    {
        // GET: ColdFusionReports/NewExpiringCMNs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewExpiringCMNs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewExpiringCMNs(NewExpiringCMNsVM _vm)
        {
            return View(_vm);
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        public ActionResult NewExpiringCMNsData(int month, int year, [DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.ColdFusionReports.Models.DataModels.NewExpiringCMNs.GetNewExpiringCMNs(month, year).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;            
        }

    }
}