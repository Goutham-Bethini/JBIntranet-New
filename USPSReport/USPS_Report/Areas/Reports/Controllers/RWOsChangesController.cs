using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.Reports.Models.RWOsChanges;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class RWOsChangesController : Controller
    {
        public ActionResult RWOsChanges()
        {
            RWOsChangesVM _vm = new RWOsChangesVM();
            //_vm.StartDt = DateTime.Today.AddDays(-5);
            //_vm.EndDt = DateTime.Today.AddDays(-1);
            return View(_vm);
        }
        public ActionResult RWOsChangesReport(RWOsChangesVM _vm)
        {
            RWOsChangesVM _locvm = new RWOsChangesVM();
            _locvm.Details = USPS_Report.Areas.Reports.Models.RWOsChanges.GetRWOsData(_vm.StartDt, _vm.EndDt);
            return View("RWOsChanges", _locvm);
        }
    }
}