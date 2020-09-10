using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.AssessmentReport;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class AssessmentReportController : Controller
    {
        // GET: ColdFusionReports/AssessmentReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AssessmentReport()
        {
            AssessmentReportVM _vm = new AssessmentReportVM();            
            _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.AssessmentReport.GetAssessmentReport(null, null);
            return View(_vm);
            
        }
        public ActionResult AssessmentReportData(AssessmentReportVM assessmentReportVM)
        {
            AssessmentReportVM _vm = new AssessmentReportVM();
            _vm = USPS_Report.Areas.ColdFusionReports.Models.DataModels.AssessmentReport.GetAssessmentReport(assessmentReportVM.Start,assessmentReportVM.End);
            return View("AssessmentReport", _vm);

        }
    }
}