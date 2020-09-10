using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class AlphaSplitController : Controller
    {        
        public ActionResult AlphaSplit()
        {
            var _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetDeptsEmpsDetails();
            return View(_vm);
        }
        public ActionResult AlphaSplitDeptEmployees(int deptId,string dept)
        {
            var _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetDeptEmployees(deptId,dept);
            return View(_vm);
        }

        public ActionResult AlphaSplitUpdateDeptEmployees(AlphaSplitUpdateVM alphaSplitUpdateVM)
        {
            var components = User.Identity.Name.Split('\\');
            var currentUser = components.Last();
            var _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.UpdateDeptEmployees(alphaSplitUpdateVM,currentUser);
            return View("AlphaSplitDeptEmployees", _vm);
        }

    }
}