using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class RulesController : Controller
    {        
        public ActionResult Rules()
        {
            ReportsRulesVM _vm = new ReportsRulesVM();
            _vm.Details= USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCMNreports();
            
            return View(_vm);
        }
        public ActionResult Documentation()
        {
            return View();
        }
        public ActionResult ReportRules(int reportId,string report)
        {
            ReportRulesDataVM _vm = new ReportRulesDataVM();
            _vm.ReportId = reportId;
            _vm.Report = report;
            var list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetReportRules(reportId);
            _vm.Details = list;
            if (Session["Payers"]==null)
            {
                var payers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayers();
                Session["Payers"] = payers;
            }
            _vm.Payers = new SelectList(Session["Payers"] as List<Payer>, "PayerId", "PayerName");
            if (Session["PayerTypes"] == null)
            {
                var payerTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayerTypes();
                Session["PayerTypes"] = payerTypes;
            }
            _vm.PayerTypes = new SelectList(Session["PayerTypes"] as List<PayerType>, "PayerTypeId", "PayerTypeTitle");
            if (Session["DocTypes"] == null)
            {
                var docTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetDocTypes();
                Session["DocTypes"] = docTypes;
            }
            _vm.DocTypes = new SelectList(Session["DocTypes"] as List<DocType>, "Id", "Description");
            if (Session["Providers"] == null)
            {
                var providers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetProviders();
                Session["Providers"] = providers;
            }
            _vm.Providers = new SelectList(Session["Providers"] as List<Provider>, "Id", "Name");
            if (Session["StatusCodes"] == null)
            {
                var statusCodes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetStatusCodes();
                Session["StatusCodes"] = statusCodes;
            }
            _vm.StatusCodes = new SelectList(Session["StatusCodes"] as List<StatusCode>, "Id", "StatusDescription");
            if (Session["Categories"] == null)
            {
                var categories = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCategories();
                Session["Categories"] = categories;
            }
            _vm.Categories = new SelectList(Session["Categories"] as List<Category>, "Id", "Description");
            return View(_vm);
        }
        public ActionResult DeleteReportRule(int cmrId, int reportId, string report)
        {
            var components = User.Identity.Name.Split('\\');
            var currentUser = components.Last();
            ReportRulesDataVM _vm = new ReportRulesDataVM();
            _vm.ReportId = reportId;
            _vm.Report = report;
            _vm.Message = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.DeleteReportRule(cmrId, currentUser);
            var list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetReportRules(reportId);
            _vm.Details = list;
            if (Session["Payers"] == null)
            {
                var payers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayers();
                Session["Payers"] = payers;
            }
            _vm.Payers = new SelectList(Session["Payers"] as List<Payer>, "PayerId", "PayerName");
            if (Session["PayerTypes"] == null)
            {
                var payerTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayerTypes();
                Session["PayerTypes"] = payerTypes;
            }
            _vm.PayerTypes = new SelectList(Session["PayerTypes"] as List<PayerType>, "PayerTypeId", "PayerTypeTitle");
            if (Session["DocTypes"] == null)
            {
                var docTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetDocTypes();
                Session["DocTypes"] = docTypes;
            }
            _vm.DocTypes = new SelectList(Session["DocTypes"] as List<DocType>, "Id", "Description");
            if (Session["Providers"] == null)
            {
                var providers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetProviders();
                Session["Providers"] = providers;
            }
            _vm.Providers = new SelectList(Session["Providers"] as List<Provider>, "Id", "Name");
            if (Session["StatusCodes"] == null)
            {
                var statusCodes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetStatusCodes();
                Session["StatusCodes"] = statusCodes;
            }
            _vm.StatusCodes = new SelectList(Session["StatusCodes"] as List<StatusCode>, "Id", "StatusDescription");
            if (Session["Categories"] == null)
            {
                var categories = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCategories();
                Session["Categories"] = categories;
            }
            _vm.Categories = new SelectList(Session["Categories"] as List<Category>, "Id", "Description");
            return View("ReportRules", _vm);
        }

        public ActionResult AddRules(ReportRulesDataVM reportRulesDataVM)
        {
            var components = User.Identity.Name.Split('\\');
            var currentUser = components.Last();
            //return RedirectToAction("ReportRules", new { reportId = reportRulesDataVM.ReportId, report = reportRulesDataVM.Report });
            ReportRulesDataVM _vm = new ReportRulesDataVM();
            _vm.ReportId = reportRulesDataVM.ReportId;
            _vm.Report = reportRulesDataVM.Report;
            _vm.Message = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.AddRules(reportRulesDataVM, currentUser);
            var list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetReportRules(reportRulesDataVM.ReportId);
            _vm.Details = list;
            if (Session["Payers"] == null)
            {
                var payers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayers();
                Session["Payers"] = payers;
            }
            _vm.Payers = new SelectList(Session["Payers"] as List<Payer>, "PayerId", "PayerName");
            if (Session["PayerTypes"] == null)
            {
                var payerTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetPayerTypes();
                Session["PayerTypes"] = payerTypes;
            }
            _vm.PayerTypes = new SelectList(Session["PayerTypes"] as List<PayerType>, "PayerTypeId", "PayerTypeTitle");
            if (Session["DocTypes"] == null)
            {
                var docTypes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetDocTypes();
                Session["DocTypes"] = docTypes;
            }
            _vm.DocTypes = new SelectList(Session["DocTypes"] as List<DocType>, "Id", "Description");
            if (Session["Providers"] == null)
            {
                var providers = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetProviders();
                Session["Providers"] = providers;
            }
            _vm.Providers = new SelectList(Session["Providers"] as List<Provider>, "Id", "Name");
            if (Session["StatusCodes"] == null)
            {
                var statusCodes = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetStatusCodes();
                Session["StatusCodes"] = statusCodes;
            }
            _vm.StatusCodes = new SelectList(Session["StatusCodes"] as List<StatusCode>, "Id", "StatusDescription");
            if (Session["Categories"] == null)
            {
                var categories = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ExpiringCMNs.GetCategories();
                Session["Categories"] = categories;
            }
            _vm.Categories = new SelectList(Session["Categories"] as List<Category>, "Id", "Description");                       
            return View("ReportRules", _vm);
        }
    }
}