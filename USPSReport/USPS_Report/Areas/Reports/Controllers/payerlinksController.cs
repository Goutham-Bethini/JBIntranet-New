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
    public class payerlinksController : Controller
    {
        // GET: Reports/payerlinks
        public ActionResult PayerlinksData(PayerLinkSearchModel vm)
        {
            List<SelectListItem> RecordsForPayer = new List<SelectListItem>();
            RecordsForPayer.Add(new SelectListItem { Text = "All", Value = "All", Selected = true });
            RecordsForPayer.Add(new SelectListItem { Text = "3179 - IN Medicaid", Value = "3179" });
            RecordsForPayer.Add(new SelectListItem { Text = "7 - MI Medicaid", Value = "7" });
            ViewBag.RecordsForPayerList = new SelectList(RecordsForPayer, "Value", "Text");
            List<SelectListItem> WhichRecord = new List<SelectListItem>();
            WhichRecord.Add(new SelectListItem { Text = "All", Value = "All", Selected = true });
            WhichRecord.Add(new SelectListItem { Text = "Linked", Value = "Linked" });
            WhichRecord.Add(new SelectListItem { Text = "UnLinked", Value = "UnLinked" });
            ViewBag.WhichRecordsList = new SelectList(WhichRecord, "Value", "Text");
            return View(vm);
        }

        public ActionResult ReadpayerlinksData([DataSourceRequest]DataSourceRequest request, PayerLinkSearchModel vm)
        {
            var jsonResult = Json(GetData(vm.RecordsForPayer, vm.WhichRecords).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<PayerLinkViewModel> GetData(string RecordsForPayer, string WhichRecords)
        {
            PayerLinkReport _report = new PayerLinkReport();
            IEnumerable<PayerLinkViewModel> VM = new List<PayerLinkViewModel>();
            VM = _report.GetDetails(RecordsForPayer, WhichRecords);
            return VM;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditpayerlinksData_Update([DataSourceRequest] DataSourceRequest request, PayerLinkViewModel vm)
        {
            PayerLinkReport _report = new PayerLinkReport();
            if (vm != null && ModelState.IsValid)
            {
                _report.UpdateDatabase(vm);
            }

            return Json(new[] { vm }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}