using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class POTAReportsController : Controller
    {
        // GET: Reports/POTAReports
        public ActionResult Index()
        {
            POTA_Report_Model _vm = new POTA_Report_Model();
           
            return View(_vm);
        }


        public ActionResult POTAReport()
        {
            POTA_Report_Model _vm = new POTA_Report_Model();
             return View(_vm);
        }

        [HttpPost]
        public ActionResult POTAReport(POTA_Report_Model _vm)
        {
            _vm.pota_ReportVM = POTA_Report.GetPOTAByPayerReport(_vm.PayerId);
            return View(_vm);
        }
    }
}