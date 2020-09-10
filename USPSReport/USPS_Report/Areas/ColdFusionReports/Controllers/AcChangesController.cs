using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.AcChanges;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class AcChangesController : Controller
    {
        // GET: ColdFusionReports/AcChanges
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AcChanges()
        {
            return View();
        }

        public ActionResult AcChangesData(AcChangesVM acChangesVM)
        {
            AcChangesVM _vm = new AcChangesVM();
            _vm = GetAcChangesData(Convert.ToInt32(acChangesVM.AccountNumber));         
            return View("AcChanges", _vm);
        }

    }
}