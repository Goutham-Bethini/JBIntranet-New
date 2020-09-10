using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.BackOrders;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class BackOrdersController : Controller
    {
        // GET: ColdFusionReports/BackOrders
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BackOrders()
        {
            BackOrdersVM _vm = new BackOrdersVM();         
            _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.BackOrders.GetBackOrdersData();            
            return View(_vm);
        }
    }
}