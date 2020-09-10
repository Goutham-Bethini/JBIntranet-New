using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.ProductivityReport;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class ProductivityReportController : Controller
    {
        // GET: ColdFusionReports/ProductivityReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductivityReport()
        {
            var res=USPS_Report.Areas.ColdFusionReports.Models.DataModels.ProductivityReport.GetDetails(DateTime.Now);
            return View(res);
        }
        public ActionResult ProductivityReportByDate(DateTime selectedDate)
        {
            var res = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ProductivityReport.GetDetails(selectedDate);
            return View("ProductivityReport", res);
        }
        public ActionResult StationUpdate(string meter_Number,string stationName)
        {           
            StationUpdateVM _vm = new StationUpdateVM();
            _vm.MeterNumber = meter_Number;
            _vm.Name = stationName;
            return View(_vm);
        }
        [HttpPost]
        public ActionResult UpdateStationDetails(StationUpdateVM stationUpdateVM)
        {
            var components = User.Identity.Name.Split('\\');
            var userName = components.Last();
            var _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ProductivityReport.UpdateStationDetails(stationUpdateVM, userName);
            return View("StationUpdate", _vm);
        }
    }
}