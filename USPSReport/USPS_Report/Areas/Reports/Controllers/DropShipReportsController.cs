using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class DropShipReportsController : Controller
    {
        // GET: Reports/DropShipReports
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DropShip_Reports()
        {
            DropshipOrderVM _vm = new DropshipOrderVM();
            _vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(-7).Month, 1);
            _vm.EndDate = DateTime.Now;
            return View(_vm);
        }


        [HttpPost]
        public ActionResult DropShip_Reports(DropshipOrderVM _ds)
        {
            _ds.dropShipOrderReceived = DropShipOrderReports.GetDropShipOrderReceived(_ds.StartDate, _ds.EndDate);
            return View(_ds);


        }
    }
}