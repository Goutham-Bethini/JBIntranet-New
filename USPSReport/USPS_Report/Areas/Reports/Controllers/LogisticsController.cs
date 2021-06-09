using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class LogisticsController : Controller
    {
        // GET: Reports/Logistics
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult weightCalulator()
        {

            WeightCal_Vm _vm = new WeightCal_Vm();
           
            return View(_vm);
        }


        [HttpPost]
        public ActionResult weightCalulator(WeightCal_Vm _vm)
        {
            _vm.totalWt = 0.0;
            _vm.details = AddCSRLog.GetDetails(_vm.Account);

            _vm.workOrderDetail = WeightCalculator.GetWODetailByAccount(Convert.ToInt32(_vm.Account), User.Identity.Name.Split('\\').Last().ToLower());

            foreach (var rec in _vm.workOrderDetail)
            {
                double? totWt = 0.0;
                    foreach (var pro in rec.productDetails)
                    {
                        pro.TotalWeight = pro.Shipped * pro.UnitWeight;
                        totWt += pro.TotalWeight;
                    }
                rec.totalProductsWt = totWt;
            }

            return View(_vm);
        }


        [HttpPost]
        public ActionResult ReturnWeight(WeightCal_Vm _vm)
        {
            foreach (var rec in _vm.workOrderDetail)
            {
                if (rec.checkbox)
                {
                    foreach (var pro in rec.productDetails)
                    {
                        pro.TotalWeight = pro.Shipped * pro.UnitWeight;
                    }
                }
            }
          
            return View(_vm);
        }

        [HttpPost]
        public ActionResult AddTabWeight(WeightCal_Vm _vm)
        {
            string tab1 = string.Empty;
            _vm.tabs.Add(tab1);
            return View(_vm);
        }


        public ActionResult ProductWithoutUW()
        {
            ProductWith_No_UW _vm = new ProductWith_No_UW();
            _vm._start = DateTime.Today.AddDays(-2).Date;
            _vm._end = DateTime.Today.Date;
            return View(_vm);
        }

        [HttpPost]
        public ActionResult ProductWithoutUW(ProductWith_No_UW _vm)
        {
            _vm.productDetails = WeightCalculator.GetProduct_With_No_UW(_vm._start, _vm._end, User.Identity.Name.Split('\\').Last().ToLower()); 
            return View(_vm);
        }


        public ActionResult WaitingToInterface()
        {
            SpecialHoldsVM _vm = new SpecialHoldsVM();
            _vm.holdlist = CSRReport.GetHoldReports();
            return View(_vm);
        }

    }
}