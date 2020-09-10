using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class WareHouseController : Controller
    {
        // GET: Reports/WareHouse
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult WareHouseStatus()
        {
           IList<WareHouseStatusVM> vm = new List<WareHouseStatusVM>();
            vm = WareHouseBoard.GetWareHouseStatus();
            return View();
        }

        public ActionResult QtyOnHand()
        {
            QOH.getQOHOracle();
            return View();
        }

        public ActionResult UM_Tracking()
        {
            UM_TrackingLookupVM _vm = new UM_TrackingLookupVM();

            return View(_vm);
        }

        [HttpPost]
        public ActionResult UM_Tracking(UM_TrackingLookupVM _vm)
        {
        
            _vm.um_TrackingLookup = warehouse.Get_UM_TrackingInfo(_vm.SearchBy, _vm.SearchValue);
            return View(_vm);
        }

        public ActionResult EligSchedule()
        {
            IList<scheduleOrders> vm = new List<scheduleOrders>();
            vm = WareHouseBoard.GetEligScheduleStatus();
            return View();
        }

      

        
    }
}