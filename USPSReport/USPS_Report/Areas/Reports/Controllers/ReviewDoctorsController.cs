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
    public class ReviewDoctorsController : Controller
    {
        // GET: Reports/ReviewDoctors
        public ActionResult ReviewDoctorReport()
        {
            return View();
        }

        public ActionResult ReadreviewDoctorDetails([DataSourceRequest]DataSourceRequest request)
        {
            var jsonResult = Json(GetData().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult HierarchyBinding_Patients(int dID, [DataSourceRequest]DataSourceRequest request)
        {
            var jsonResult = Json(GetHierarchyData(dID).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private IEnumerable<ReviewDoctorsViewModel> GetHierarchyData(int dID)
        {
            ReviewDoctorReport _report = new ReviewDoctorReport();
            IEnumerable<ReviewDoctorsViewModel> VM = new List<ReviewDoctorsViewModel>();
            VM = _report.GetHierarchyDetails(dID);
            return VM;
        }
        private IEnumerable<DoctorsInfoViewModel> GetData()
        {
            ReviewDoctorReport _report = new ReviewDoctorReport();
            IEnumerable<DoctorsInfoViewModel> VM = new List<DoctorsInfoViewModel>();
            VM = _report.GetDetails();
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