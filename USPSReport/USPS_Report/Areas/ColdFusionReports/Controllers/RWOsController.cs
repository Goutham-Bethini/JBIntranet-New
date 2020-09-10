using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.RWOs;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class RWOsController : Controller
    {
        // GET: ColdFusionReports/RWOs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RWOs()
        {
            return View();
        }

        public ActionResult RWOsData(RWOsVM rWOsVM)
        {            
            RWOs.RWOsVM _vm = new RWOs.RWOsVM();
            _vm.AccountNumber = rWOsVM.AccountNumber;
            IList<RWOsData> _list = new List<RWOsData>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.RWOs.GetRWOsData(Convert.ToInt32(rWOsVM.AccountNumber));
            _vm.Details = _list;            
            return View("RWOs", _vm);
        }
    }
}