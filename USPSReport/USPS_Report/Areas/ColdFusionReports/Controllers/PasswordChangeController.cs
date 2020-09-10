using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.PasswordChange;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class PasswordChangeController : Controller
    {      
        public ActionResult PasswordChange()
        {
            return View();
        }
        public ActionResult PasswordChangeRequest(PasswordChangeVM passwordChangeVM)
        {
            var components = User.Identity.Name.Split('\\');
            var currentUser = components.Last();
            PasswordChangeVM _vm = new PasswordChangeVM();
            _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.PasswordChange.AddOrUpdate(passwordChangeVM, currentUser);
            return View("PasswordChange", _vm);
        }
    }
}