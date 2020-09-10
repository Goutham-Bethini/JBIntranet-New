using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;


namespace USPS_Report.Areas.Reports.Controllers
{
    public class SmartActionController : Controller
    {
        // GET: Reports/SmartAction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SA_Notes_HoldingOrders()
        {
            SmartActionNoteHoldingVm _vm = new SmartActionNoteHoldingVm();
            _vm.smartActionNotesHoldingOrders = SmartActionReport.GetSmartActionNotesHoldingOrders();
            return View(_vm);                
        }

        public ActionResult SA_Payers()
        {
            SmartActionPayerVM _vm = new SmartActionPayerVM();
            _vm.smartActionPayer = SmartActionReport.GetSmartActionPayer();
            return View(_vm);
        }

        public ActionResult SA_ConfirmedHoldingOrders()
        {
            ConfirmedHoldingOrdersVM _vm = new ConfirmedHoldingOrdersVM();
            _vm.confirmedHoldingOrders = SmartActionReport.GetConfirmedHoldingOrders();
            return View(_vm);
        }

        public ActionResult SA_UnConfirmedHoldingOrders()
        {
            UnConfirmedHoldingOrdersVM _vm = new UnConfirmedHoldingOrdersVM();
            _vm.unConfirmedHoldingOrders = SmartActionReport.GetUnConfirmedHoldingOrders();
            return View(_vm);
        }
    }
}