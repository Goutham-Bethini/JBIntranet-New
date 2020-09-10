using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class ExpiringCMNsController : Controller
    {       
        public ActionResult ExpiringCMNs()
        {
            ReportsVM _vm = new ReportsVM();
            _vm.Details = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCMNreports();
            return View(_vm);
        }
        public ActionResult TeamReport(string team,int year)
        {
            ExpiringCMNsVM _vm = new ExpiringCMNsVM();
            _vm.ReportName = team;
            _vm.SelectedYear = year;
            _vm.PreviousYear = year-1;
            _vm.NextYear = year+1;
            IList<ReportMonthWise> _list = new List<ReportMonthWise>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetData(year, team);
            IList<CMNreport> _listReports = new List<CMNreport>();
            _listReports = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCMNreports();
            _vm.Details = _list;
            _vm.Reports = _listReports;
            return View(_vm);
        }
        public ActionResult ExpiringCMNs_Report(string team, string month, string recert)
        {
            Session["ReportData"] = null;
            var _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetReportData(team, month, recert);
            Session["ReportData"] = _vm.Details;           
            return View(_vm);
        }
        public ActionResult ExpiringCMNs_Report2([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["ReportData"] != null)
            {
                var data = Session["ReportData"] as List<ReportData>;                
                var jsonResult = Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;                
            }
            else
                return null;
        }
        
    }
}