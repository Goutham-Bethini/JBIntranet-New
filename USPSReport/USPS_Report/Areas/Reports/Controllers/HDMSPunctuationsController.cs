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
    public class HDMSPunctuationsController : Controller
    {
        // GET: Reports/HDMSPunctuations
        public ActionResult Index()
        {
            HDMSPunctuationsVM VM = new HDMSPunctuationsVM();
          //  List<HDMSPunctuationsVM> VM = _service.GetDetails_Puncuation();
            return View();
        }

        public ActionResult ReadPunctuationsDetails([DataSourceRequest]DataSourceRequest request)
        {
            HDMSPunctuationsService _service = new HDMSPunctuationsService();
            var jsonResult = Json(_service.GetDetails_Puncuation().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}