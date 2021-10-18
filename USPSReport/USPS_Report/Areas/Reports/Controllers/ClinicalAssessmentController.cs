using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Globalization;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ClinicalAssessmentController : Controller
    {
        // GET: Reports/ClinicalAssessment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AssessmentLog(int? ID)
        {
            AssessmentLogList AsLog = new AssessmentLogList();
            AssessmentLog _vm = new AssessmentLog();
            AssessmentLog _vmRec = new AssessmentLog();
            AsLog.assessmentLog = _vm;
            if (ID != null && ID > 0)
            {
                _vmRec = AddAssessmentLogInfo.getAssessmentLog(ID);
            }
            AsLog.assessmentLogRec = _vmRec;
            //return View(ReportWorkOrder.GetWorkOrderByAccountByNumbers() );
            return View(AsLog);
        }

        [HttpPost]
        public ActionResult AssessmentLog(AssessmentLogList _vm)
        {
            AssessmentLog _newVM = new AssessmentLog();
            int? id = 0;
            if (_vm.assessmentLog != null)
            {
                id = AddAssessmentLogInfo.AddAssessmentLog(_vm.assessmentLog);
            }
            return RedirectToAction("AssessmentLog", new { ID = id });
        }


        public ActionResult AssessmentCount()
        {
            AssessmentCountVM _vm = new AssessmentCountVM();
            _vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
            _vm.EndDate = DateTime.Now;
            return View(_vm);
        }


        [HttpPost]
        public ActionResult AssessmentCount(AssessmentCountVM accnVM)
        {
            accnVM.GetAssessmentCountReport = ReportAssessmentCount.GetAssessmentCountByDate(accnVM.StartDate, accnVM.EndDate);
            return View(accnVM);


        }

     


        public ActionResult AssessmentLogReport()
        {

            AssessmentLogVM _vm = new AssessmentLogVM();
            _vm.UserIDList = new SelectList(GetAssessmentLogReport.GetUserIDs(), "User_ID", "User_ID");
          
            //  _vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
            //  _vm.EndDate = DateTime.Now;
            return View(_vm);
        }


        [HttpPost]
        public ActionResult AssessmentLogReport(AssessmentLogVM accnVM)
        {
            accnVM.UserIDList = new SelectList(GetAssessmentLogReport.GetUserIDs(), "User_ID", "User_ID");
            accnVM.GetAssessment = GetAssessmentLogReport.GetAssessmentLog(accnVM.StartDate, accnVM.EndDate, accnVM.Account, accnVM.User_ID).ToList();
            return View(accnVM);


        }

        public ActionResult AssessmentsDue()
        {
            AssessmentDueVM _vm = new AssessmentDueVM();
            _vm.assesment_counts = PrintableReports.GetAssessmentDueData();
            return View(_vm);
        }

        //public ActionResult AssessmentsDueByMonth(int? year, string month)
        //{

        //  //  var firstDayOfMonth = new DateTime(2014,2, 1);
        //  //  var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        //    AssessmentDueByMonth _vm = new AssessmentDueByMonth();
         

        //    int yr = year != null ? Convert.ToInt16(year) : 2016;
        //    int mon = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
        //    var firstdayOftheMonth = new DateTime(yr, mon, 1);
        //    var lastdayoftheMonth = firstdayOftheMonth.AddMonths(1).AddDays(-1);
        //  //  var firstdate = firstdayOftheMonth.Date;
        //  //  var lastdate = lastdayoftheMonth.Date;

        //    _vm.firstDate = firstdayOftheMonth;
        //    _vm.lastDate = lastdayoftheMonth;

        //    _vm.assessmentDueByMonth = PrintableReports.GetAssessmentDueByMonth(yr, firstdayOftheMonth, lastdayoftheMonth);
        //    return View(_vm);
        //}
        public ActionResult AssessmentsDueByMonth(int? year, string month, int count)
        {           
            AssessmentDueByMonth _vm = new AssessmentDueByMonth();
            int yr = year != null ? Convert.ToInt16(year) : 2016;
            int mon = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            var firstdayOftheMonth = new DateTime(yr, mon, 1);
            var lastdayoftheMonth = firstdayOftheMonth.AddMonths(1).AddDays(-1);
            _vm.firstDate = firstdayOftheMonth;
            _vm.lastDate = lastdayoftheMonth;
            _vm.AssessmentsCount = count;
            return View(_vm);
        }

        public ActionResult AssessmentsDueByMonthYr(int? year, string month, [DataSourceRequest]DataSourceRequest request)
        {
            AssessmentDueByMonth _vm = new AssessmentDueByMonth();
            int yr = year != null ? Convert.ToInt16(year) : 2016;
            int mon = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;
            var firstdayOftheMonth = new DateTime(yr, mon, 1);
            var lastdayoftheMonth = firstdayOftheMonth.AddMonths(1).AddDays(-1);
            _vm.firstDate = firstdayOftheMonth;
            _vm.lastDate = lastdayoftheMonth;
            _vm.assessmentDueByMonth = PrintableReports.GetAssessmentDueByMonth(yr, firstdayOftheMonth, lastdayoftheMonth);
            var jsonResult = Json(_vm.assessmentDueByMonth.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        //[HttpPost]
        public ActionResult AssessmentsDueAttempts(AssessmentDueByMonth _vm)
        {
            //int yr = _vm.firstDate.Year;
            //int mon = _vm.firstDate.Month;
            //var firstdayOftheMonth = new DateTime(yr, mon, 1);
            //var lastdayoftheMonth = firstdayOftheMonth.AddMonths(1).AddDays(-1);
            if(_vm.Account!=null)
            {
                PrintableReports.InsertAssessmentDueNote(_vm,_vm.Account.Value, User.Identity.Name.Split('\\').Last().ToLower(),_vm.Attempt1st, _vm.Attempt2nd, _vm.Attempt3rd);
            }
            
            return RedirectToAction("AssessmentsDueByMonth",new { year = _vm.firstDate.Year, month = _vm.firstDate.ToString("MMMM"), count = _vm.AssessmentsCount });
            //return RedirectToAction("AssessmentsDueByMonth", new {  year,  month,  count }  );
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        public ActionResult AssessmentsCompleted()
        {
            AssessmentVM _vm = new AssessmentVM();
            _vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
            _vm.EndDate = DateTime.Now;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult AssessmentsCompleted(AssessmentVM asmntVM)
        {
            asmntVM = AssessmentReports.GetAssessmentsData(asmntVM.StartDate, asmntVM.EndDate);
            asmntVM.TotalAssessmentBarChart = ChartClass.TotalAssessmentChart(asmntVM.totalAssessmentList);
            return View(asmntVM);
        }



    }
}