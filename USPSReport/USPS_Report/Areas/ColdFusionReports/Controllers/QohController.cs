using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.Qoh;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class QohController : Controller
    {
        // GET: ColdFusionReports/Qoh
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Qoh()
        {
            return View();
        }
        public ActionResult QohData(QohVM qohVM)
        {
            QohVM _vm = new QohVM();
            _vm.ProductCode = qohVM.ProductCode;
            IList<QohData> _list = new List<QohData>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.Qoh.GetQohData(qohVM.ProductCode);
            _vm.Details = _list;
            return View("Qoh", _vm);
        }
    }
}