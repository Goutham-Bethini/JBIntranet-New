using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
namespace USPS_Report.Areas.Reports.Controllers
{
    public class CMNController : Controller
    {
        // GET: Reports/CMN
        public ActionResult Index()
        {
            CMNReportsVM _vm = new CMNReportsVM();
            _vm.cmnWithDuration = CMNReports.GetCMNWithDuration99();
            return View(_vm);
        }
    }
}