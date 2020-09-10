using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.FedExPackStationLogins;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class FedExPackStationLoginsController : Controller
    {              
        public ActionResult FedExPackStationLogins()
        {
            FedExPackStationLoginsVM _vm = new FedExPackStationLoginsVM();            
            IList<FedExPackStationLoginsData> _list = new List<FedExPackStationLoginsData>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.FedExPackStationLogins.GetLogins();
            _vm.Details = _list;
            return View(_vm);            
        }
        public ActionResult FedExPackStationLoginAdd(FedExPackStationLoginsVM fedExPackStationLoginsVM)
        {
            var components = User.Identity.Name.Split('\\');
            var currentUser= components.Last();
            FedExPackStationLoginsVM _vm = new FedExPackStationLoginsVM();
            _vm=USPS_Report.Areas.ColdFusionReports.Models.DataModels.FedExPackStationLogins.AddOrUpdate(fedExPackStationLoginsVM,currentUser);
            return View("FedExPackStationLogins", _vm);
        }
    }
}