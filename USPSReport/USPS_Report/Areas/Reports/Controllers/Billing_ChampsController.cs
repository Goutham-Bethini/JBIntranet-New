using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.UI;


namespace USPS_Report.Areas.Reports.Controllers
{
    public class Billing_ChampsController : Controller
    {
        // GET: Reports/Billing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClaimsToSumit()  
        {
            ChampsVM _vm = new ChampsVM();
            _vm.batchVM = ChampsTools.GetSubmitBatch();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult ClaimsToSumit(ChampsVM _vm)
        {
            _vm.batchVM = ChampsTools.GetSubmitBatch();
            _vm.champs = ChampsTools.GetClaimstoTransmitToday(_vm.WillTransmit, _vm.Show);
              return View(_vm);
        }


        public ActionResult SubmittedClaims()
        {
            submittedClaimVM _vm = new submittedClaimVM();
            _vm.Date = DateTime.Now;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult SubmittedClaims(submittedClaimVM _vm)
        {
            _vm.submittedClaims = ChampsTools.GetSubmittedClaims(_vm.Date, _vm.ResponseType);

            return View(_vm);
        }

        public ActionResult ChampsErrorDecorder()
        {
            ErrorDecorderVM _vm = new ErrorDecorderVM();
           
            return View(_vm);
        }

        [HttpPost]
        public ActionResult ChampsErrorDecorder(ErrorDecorderVM _vm)
        {

            if (_vm.withoutIgnoreList != null)
            {
                foreach (var item in _vm.withoutIgnoreList)
                {
                    if (item.ignore == true)
                    {
                        ChampsTools.IgnoreError(item.errCode);
                    }
                }
            }
            if (_vm.IgnoreList != null)
            {
                foreach (var item in _vm.IgnoreList)
                {
                    if (item.ignore == true)
                    {
                        ChampsTools.IgnoreError(item.errCode);
                    }
                }
            }
            _vm = ChampsTools.GetErrorDecorderShowAll(_vm.RemarkCode, _vm.ReasonCode);
        
            return View(_vm);
        }

       
    }



}