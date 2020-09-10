using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class CSRController : Controller
    {
        // GET: Reports/Test
        public ActionResult Index()
        {
            ExpiredLMNs_VM _vm = new ExpiredLMNs_VM(); 
           

          _vm.expiredLMNs =  CSR.GetExpiredLMNs();
           _vm.LMNTypeList = new SelectList(CSR.GetLMNTypes(_vm.expiredLMNs), "LMNTypeid", "LMNType");
            _vm.MonthList = new SelectList(CSR.GetMonthList(), "month", "month");
            _vm.YearList = new SelectList(CSR.GetYearList(_vm.expiredLMNs), "year", "year");
            return View(_vm);
        }


        [HttpPost]
        public ActionResult Index(ExpiredLMNs_VM _vm)
        {


            _vm.expiredLMNs = CSR.GetExpiredLMNs();
            _vm.LMNTypeList = new SelectList(CSR.GetLMNTypes(_vm.expiredLMNs), "LMNTypeid", "LMNType");
            _vm.MonthList = new SelectList(CSR.GetMonthList(), "month", "month");
            _vm.YearList = new SelectList(CSR.GetYearList(_vm.expiredLMNs), "year", "year");
            _vm.expiredLMNs = CSR.GetExpiredLMNsFiltered(_vm.LMNTypeid, _vm.Month ,_vm.Year);
          
            return View(_vm);
        }

        public ActionResult AccountChanges()
        {
            RWOAccountChangesVM _vm = new RWOAccountChangesVM();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult AccountChanges(RWOAccountChangesVM _vm)
        {
             _vm = CSRReport.GetAccountChanges(_vm.Account);
             return View(_vm);
        }

        public ActionResult AccountChangesInfo()
        {
            RWOAccountChangesModel _vm = new RWOAccountChangesModel();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult AccountChangesInfo(RWOAccountChangesModel _vm)
        {
            _vm = CSRReport.GetAccountChangesInfo(_vm.startDt, _vm.endDt);
            return View(_vm);
        }

        public ActionResult RWOChanges()
        {
            RWOChangesVM _vm = new RWOChangesVM();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult RWOChanges(RWOChangesVM _vm)
        {
            _vm = CSRReport.GetRWOChanges(_vm.Account);
            return View(_vm);
        }


        public ActionResult DownloadEx()
        {
            return View();
        }

        public FilePathResult DownloadExampleFiles(string fileName)
        {
            return new FilePathResult(string.Format(@"~\Files\{0}", fileName + ".txt"), "text/plain");
        }

        public ActionResult BCNClaimReport()
        {
            BCNClaims _vm = new BCNClaims();
            _vm._startDt = DateTime.Today.AddDays(-6);
          //  _vm.ServName = CSRReport.GetServName();
            _vm._endDt = DateTime.Today;
            return View(_vm);
        }


        [HttpPost]
        public ActionResult BCNClaimReport(BCNClaims _vm)
        {
             
            return View(_vm);
        }



        public ActionResult GetClaimReport(DateTime? _startDt, DateTime? _endDt, string Col, bool others, string prod, double? memId, string  serv, [DataSourceRequest]DataSourceRequest request)
        {
            BCNClaims _vm = new BCNClaims();
            _vm._startDt = _startDt;
            _vm._endDt = _endDt;
            _vm.chk = Col;
            _vm.others = others;
            _vm.prod = prod;
            _vm.contract = memId;
            _vm.ServName = serv;
           
            return Json(CSRReport.GetClaimList(_vm._startDt, _vm._endDt, _vm.chk , _vm.others, _vm.prod, _vm.contract, serv  ).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


    }
}