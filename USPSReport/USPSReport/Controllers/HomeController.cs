using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPSReport.Models;

namespace USPSReport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MainVM vm = new MainVM();
           vm.dataList = uspsDB.GetReport("");
            
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(string _trackNum)
        {
            MainVM vm = new MainVM();
            vm.dataList = uspsDB.GetReport("");
            vm.trackNum = _trackNum;
            return View(vm);
        
        }

        public ActionResult MainMethod(string _pId)
        {
            DetailReport _dRpt = uspsDB.GetData(_pId);
           
            return PartialView("_MainMethod", _dRpt);
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}