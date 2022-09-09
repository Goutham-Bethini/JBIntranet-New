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
    public class BillingController : Controller
    {
        // GET: Reports/Billing
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EligibilitySearch_MI()
        {
            EligSearchMIVM _vm = new EligSearchMIVM();

            return View(_vm);
        }

        [HttpPost]
        public ActionResult EligibilitySearch_MI(EligSearchMIVM _vm)
        {
            if (_vm.Account == null && _vm.WorkOrder == null && _vm.DateOfService == null && _vm.Claim == null)
            {
                _vm.noCriteria = true;
            }
            else
            {
                _vm.eligSerachMIVM = EligibilitySearchMIString.GetEligibilityString_MI(_vm.Claim, _vm.WorkOrder, _vm.Account, _vm.DateOfService);
            }
            return View(_vm);
        }

        public ActionResult EligibilitySearch_WI()
        {
            EligSearchWIVM _vm = new EligSearchWIVM();

            return View(_vm);
        }

        [HttpPost]
        public ActionResult EligibilitySearch_WI(EligSearchWIVM _vm)
        {
           
            _vm.eligSerach_WI = EligibilitySearchWIString.GetEligibilityString_WI(_vm.SearchString, _vm.Parameter);
          
            
            return View(_vm);
        }

        public ActionResult ClaimByPayer()
        {
            ClaimModel _vm = new ClaimModel();
            _vm.StartDt = DateTime.Today.AddDays(-10);
            _vm.EndDt = DateTime.Today;
            _vm.allNPcode = true;
            // _vm.PANotNeeded = PARule.GetPANotNeeded();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult ClaimByPayer(ClaimModel _vm)
        {
           
            return View(_vm);
        }
        public ActionResult ClaimNeeded(int PayerID, DateTime StartDt, DateTime EndDt, string NPcode, bool allNPcode, [DataSourceRequest] DataSourceRequest request)
        {
            var result = Json(ChampsTools.GetClaimsByPayer(PayerID, StartDt, EndDt, NPcode, allNPcode, User.Identity.Name.Split('\\').Last().ToLower()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
           // return Json(ChampsTools.GetClaimsByPayer(PayerID, StartDt, EndDt, NPcode, allNPcode, User.Identity.Name.Split('\\').Last().ToLower()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }



        public ActionResult CollectionByOperator()
        {
            CollectionModel _vm = new CollectionModel();
            _vm.ActivityStartDt = DateTime.Today.AddDays(-1);
            _vm.ActivityEndDt = DateTime.Today;
           
            // _vm.PANotNeeded = PARule.GetPANotNeeded();
            return View(_vm);
        }

        public ActionResult CollectionNeeded(int OpID, DateTime StartDt, DateTime EndDt,   [DataSourceRequest] DataSourceRequest request)
        {
            return Json(ChampsTools.GetCollectionByOp(OpID, StartDt, EndDt, User.Identity.Name.Split('\\').Last().ToLower()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CollectionByOperator(CollectionModel _vm)
        {
         
            return View(_vm);
        }
    }
}