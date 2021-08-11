using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class IllegalpunctuationController : Controller
    {
        // GET: Reports/Illegalpunctuation
        public ActionResult IllegalpunctuationData()
        {
            return View();
        }

        public ActionResult ReadIllegalpunctuationData([DataSourceRequest]DataSourceRequest request)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<IllegalPuncuationVM> GetData()
        {
            IEnumerable<IllegalPuncuationVM> VM = new List<IllegalPuncuationVM>();
            VM = IllegalPuncuationReport.GetDetails();
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