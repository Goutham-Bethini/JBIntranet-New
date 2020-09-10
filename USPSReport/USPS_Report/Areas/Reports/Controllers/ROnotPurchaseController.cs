using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ROnotPurchaseController : Controller
    {
        // GET: Reports/ROnotPurchase
        public ActionResult ROnotPurchaseDetails()
        {
            ROnotPurchaseReport _report = new ROnotPurchaseReport();
            IEnumerable<ROnotPurchaseViewModel> VM = new List<ROnotPurchaseViewModel>();
            VM = _report.GetDetails();
            return View(VM);
        }
        public ActionResult ReadROnotPurchaseData([DataSourceRequest]DataSourceRequest request, CMNReportModel _vm)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<ROnotPurchaseViewModel> GetData()
        {
            ROnotPurchaseReport _report = new ROnotPurchaseReport();
            IEnumerable<ROnotPurchaseViewModel> VM = new List<ROnotPurchaseViewModel>();
            VM = _report.GetDetails();
            return VM;
        }
    }
}