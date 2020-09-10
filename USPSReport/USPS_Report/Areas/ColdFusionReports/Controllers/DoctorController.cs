using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.Doctor;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class DoctorController : Controller
    {
        // GET: ColdFusionReports/Doctor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Doctor()
        {
            return View();
        }
        public ActionResult DoctorData(DoctorVM doctorVM)
        {
            DoctorVM _vm = new DoctorVM();
            _vm.PhysicianID = doctorVM.PhysicianID;
            IList<DoctorData> _list = new List<DoctorData>();
            _list = USPS_Report.Areas.ColdFusionReports.Models.DataModels.Doctor.GetDoctorData(Convert.ToInt32(doctorVM.PhysicianID));
            _vm.Details = _list;
            return View("Doctor", _vm);
        }
    }
}