using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.Reports.Models.RWOsChanges;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class RWOsChangesController : Controller
    {
        public ActionResult RWOsChanges(RWOsChangesVM _vm)
        {
            //RWOsChangesVM _vm = new RWOsChangesVM();
            //_vm.StartDt = DateTime.Today.AddDays(-5);
            //_vm.EndDt = DateTime.Today.AddDays(-1);
            return View(_vm);
        }
        //public ActionResult RWOsChangesReport(RWOsChangesVM _vm)
        //{
        //    RWOsChangesVM _locvm = new RWOsChangesVM();
        //    _locvm.Details = USPS_Report.Areas.Reports.Models.RWOsChanges.GetRWOsData(_vm.StartDt, _vm.EndDt);
        //    return View("RWOsChanges", _locvm);
        //}
        public ActionResult RWOsChangesReport([DataSourceRequest] DataSourceRequest request, RWOsChangesVM _vm)
        {
            //_vm.StartDt,_vm.EndDt
            //"1/1/2022", "1/4/2022"
            //var jsonResult = Json(USPS_Report.Areas.Reports.Models.RWOsChanges.GetRWOsData(Convert.ToDateTime("12/1/2021"), Convert.ToDateTime("12/1/2021")).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            var jsonResult = Json(USPS_Report.Areas.Reports.Models.RWOsChanges.GetRWOsData(_vm.StartDt, _vm.EndDt).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}