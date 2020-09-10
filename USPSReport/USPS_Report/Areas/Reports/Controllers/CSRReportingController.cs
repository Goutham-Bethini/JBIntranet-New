using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class CSRReportingController : Controller
    {
        // GET: Reports/CSRReporting
        public ActionResult Index()
        {
            CSRReportModel _vm = new CSRReportModel();
            _vm.date = DateTime.Now;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult Index(CSRReportModel _vm)
        {
            _vm.callPerPeroson = CSRReport.GetTotalCallPerPerson(_vm.date);
            return View(_vm);
        }


        public ActionResult CSRAssessment()
        {
            CSRAssessmentVM _vm = new CSRAssessmentVM();
            _vm.csrAssessment = CSRReport.GetCSRAssessment();
            return View(_vm);
        }

        public ActionResult CSRCallLogReport()
        {
            callLogReportVM _vm = new callLogReportVM();
            _vm.startDt = DateTime.Today.AddDays(-5);
            _vm.endDt = DateTime.Today.AddDays(-1);
            return View(_vm);
        }

        [HttpPost]
        public ActionResult CSRCallLogReport(callLogReportVM _vm)
        {
            _vm.records = CSRReport.GetCalllogReport(_vm.startDt, _vm.endDt);
            return View(_vm);
        }

        public ActionResult HoldOrders()
        {
            SpecialHoldsVM _vm = new SpecialHoldsVM();
            _vm.holdlist = CSRReport.GetHoldReports();
            return View(_vm);
        }

        public ActionResult SuperiorHeldOrders()
        {
            SpecialHoldsVM _vm = new SpecialHoldsVM();
            _vm.holdlist = CSRReport.GetSuperiorHoldReports();
            return View(_vm);
        }

        public ActionResult AOBReport()
        {
            AOBReportModel _vm = new AOBReportModel();
            _vm.StartDate = DateTime.Now;
            _vm.EndDate = DateTime.Now;
            return View(_vm);
          
        }

        [HttpPost]
        public ActionResult AOBReport(AOBReportModel _vm)
        {
             _vm.AOBDetailList = CSRReport.GetAOBDetail(_vm.StartDate , _vm.EndDate);
            return View(_vm);
        }


    }
}