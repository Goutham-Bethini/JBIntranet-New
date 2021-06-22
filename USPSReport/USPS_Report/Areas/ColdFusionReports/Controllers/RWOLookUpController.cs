using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class RWOLookUpController : Controller
    {
        // GET: Reports/RWOLookUp
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RWOLookUpData(RwoLookUpSearchViewModel _vm)
        {
          _vm.ProductCodeList =   RwoLookUpReport.GetProducts();
          _vm.LocationList = RwoLookUpReport.GetLocations();
          _vm.MethodList = RwoLookUpReport.GetMethods();
            _vm.PayerList = RwoLookUpReport.GetPayers();
           RwoLookUpReport.setOtherSearchDetails(_vm);
            return View(_vm);
        }

        public ActionResult SearchForResults(RwoLookUpSearchViewModel _vm)
        {
            RwoLookUpReport _report = new RwoLookUpReport();
            _report.GetDetailsCount(_vm);
            return RedirectToAction("RWOLookUpData", "RWOLookUp", _vm);
        }

        public ActionResult ReadRWOLookUpData([DataSourceRequest]DataSourceRequest request, RwoLookUpSearchViewModel _vm)
        {
            var jsonResult = Json(GetData(_vm).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<RwoLookUpResultVM> GetData(RwoLookUpSearchViewModel _vm)
        {
            RwoLookUpReport _report = new RwoLookUpReport();
            IEnumerable<RwoLookUpResultVM> VM = new List<RwoLookUpResultVM>();
            VM = _report.GetDetails(_vm);
            return VM;
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}