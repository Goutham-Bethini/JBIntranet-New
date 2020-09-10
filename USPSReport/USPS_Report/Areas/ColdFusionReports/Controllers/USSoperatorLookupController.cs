using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.USSoperatorLookup;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class USSoperatorLookupController : Controller
    {
        // GET: ColdFusionReports/USSoperatorLookup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult USSoperatorLookup()
        {
            return View();
        }
        public ActionResult USSoperatorLookupData([DataSourceRequest] DataSourceRequest request)
        {
            var jsonResult = Json(USPS_Report.Areas.ColdFusionReports.Models.DataModels.USSoperatorLookup.GetUSSoperatorLookupData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}