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
    public class RWOsOlderDatesController : Controller
    {
        // GET: Reports/RWOsOlderDates
        public ActionResult RWOsOlderDatesReportData(RwosViewModel _vm)
        {
            return View(_vm);
        }

        public ActionResult ReadRWOsOlderDatesDetails([DataSourceRequest]DataSourceRequest request, RwosViewModel _vm)
        {
            var jsonResult = Json(GetData(_vm).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<RwosViewModel> GetData(RwosViewModel _vm)
        {
            RwosReport _report = new RwosReport();
            IEnumerable<RwosViewModel> VM = new List<RwosViewModel>();
            VM = _report.GetDetails(_vm.AccountToSearch);
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