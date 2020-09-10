using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class WoReportsController : Controller
    {
        // GET: Reports/WoReports
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult HeldOrders()
        {
            return View();
        }
        public ActionResult GetHeldOredrs([DataSourceRequest]DataSourceRequest request)
        {
            // Int32? account=0;
            //   Int32 num=0;
            return Json(CSRReport.GetDiabOsUroHeldOrders().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public ActionResult RWO_OlderDatesReport()
        {

            RWOwithOlderDatesModel _vm = new RWOwithOlderDatesModel();
            return View(_vm);
        }

        [HttpPost]
        public ActionResult RWO_OlderDatesReport(RWOwithOlderDatesModel _vm)
        {

            _vm.rwoWithOlderDates = RWOwithOlderDates.GetRWOwithOlderDates();
            return View(_vm);
        }

        //public ActionResult RWOwithOlderDatesReport([DataSourceRequest] DataSourceRequest request)
        //{
        //    return Json(RWOwithOlderDates.GetRWOwithOlderDates().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}


        public ActionResult ShippedOrders()
        {
            ShippingOrderVM _vm = new ShippingOrderVM();
            _vm.ShipData = ShippingOrderReport.GetShippingOrders();
            _vm.ShippingChart = ChartClass.ShippingOrderChart(_vm.ShipData);
            return View(_vm);
        }


        public ActionResult ExpiredProduct()
        {
            return View(ExpiredProductReport.GetExpiredProduct());
        }

      

        public ActionResult ProductForcast()
        {

            ProductVM _vm = new ProductVM();
            _vm.vendorid = 0;
            _vm.VendorList = new SelectList(ProductsReport.GetVendorName(), "vendorid", "VendorName");
            

            return View(_vm);
        }


        [HttpPost]
        public ActionResult ProductForcast(ProductVM prodVM)
        {
            prodVM.VendorList = new SelectList(ProductsReport.GetVendorName(), "vendorid", "VendorName");
            prodVM.productData = ProductsReport.GetProducts(prodVM.StartDate, prodVM.EndDate, prodVM.vendorid, prodVM.productCode ).ToList();
            return View(prodVM);

        }

        public ActionResult HoldCountByReason()
        {
            HoldReasonVM _vm = new HoldReasonVM();
          _vm.GetHoldReason=   HoldCountByReasonReport.GetHoldCountByReason();
            _vm.HoldReasonPieChart = ChartClass.HoldReasonChart(_vm.GetHoldReason);
            return View(_vm);
        }

        public ActionResult RWOMultipleLocations()
        {
            RWOMultipleLocationsVM _vm = new RWOMultipleLocationsVM();
            _vm.rwoMultipleLocations = RWOMultipleLocationsReport.GetRWOMultipleLocations();
            return View(_vm);
        }

        public ActionResult RWOMultipleMethods()
        {
            RWOMultipleMethodsVM _vm = new RWOMultipleMethodsVM();
            _vm.rwoMultipleMethods = RWOMultipleMethodsReport.GetRWOMultipleMethods();
            return View(_vm);
        }

        public ActionResult RWOMultipleTimes()
        {
            RWOMultipleTimesVM _vm = new RWOMultipleTimesVM();
            _vm.rwoMultipleTimes = RWOMultipleTimesReport.GetRWOMultipleTimes();
            return View(_vm);
        }
      

        public ActionResult Binlocation()
        {
            BinLocationVM _vm = new BinLocationVM();
            _vm.binLocationData = BinLocationReport.GetBinLocation();
            return View(_vm);

        }

        public ActionResult RwoLookUp()
        {

            RWOLookUPVM _vm = new RWOLookUPVM();
            _vm.TotalRecord = 0;
            // _vm.ProductList = RWOLookUPReport.GetProducts(); 
            _vm.MethodList = RWOLookUPReport.GetMethods();
            _vm.DeliveryTimeList = RWOLookUPReport.GetDeliveryTimes();
            _vm.locationList = RWOLookUPReport.GetLocations();
            _vm.FrequencyList = RWOLookUPReport.GetFrequencyTitle();
            _vm.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
            _vm.EndDate = DateTime.Now;
            _vm.HoldCode = "2";
            _vm.ServiceType = "1";
            return View(_vm);
        }

     

        [HttpPost]
        public ActionResult RwoLookUp(RWOLookUPVM _rwo)
        {
            _rwo.TotalRecord = 0;
            _rwo.locationList = RWOLookUPReport.GetLocations();
            _rwo.MethodList = RWOLookUPReport.GetMethods();
            _rwo.DeliveryTimeList = RWOLookUPReport.GetDeliveryTimes();
            _rwo.FrequencyList = RWOLookUPReport.GetFrequencyTitle();
            // _rwo.ProductList = RWOLookUPReport.GetProducts(); 
            //  _rwo.rwoLookUp = RWOLookUPReport.RWOLookUPDetail(_rwo.StartDate, _rwo.EndDate, _rwo.HoldCode, _rwo.ProductCode);
          _rwo.rwoLookUp = RWOLookUPReport.RWOLookUPDetail(_rwo.StartDate,_rwo.EndDate,_rwo.HoldCode,_rwo.ProductCode,_rwo.PayerId,_rwo.locationId, _rwo.methodId, _rwo.InactiveORActive, _rwo.delTimeId, _rwo.FreqID, _rwo.IsAssigned, _rwo.makeRwoIncomplete, _rwo.ServiceType, _rwo.HCPC , _rwo.all);
            return View(_rwo);


        }


        public ActionResult rwo(DateTime? StartDate, DateTime? EndDate, string HoldCode, string ProductCode, string PayerId, int? locationId, int? MethodId, string InactiveCode, int? DelTimeId, string FreqId, string IsAssigned, string MakeRWOIncomplete, string ServiceTypeId, string HCPC, bool all)
        {
            
            RWOLookUPVM _vm = new RWOLookUPVM();
            _vm.StartDate = StartDate;
            _vm.EndDate = EndDate;
            _vm.HoldCode = HoldCode;
            _vm.ProductCode = ProductCode;
            _vm.PayerId = PayerId;
            _vm.locationId = locationId;
            _vm.methodId = MethodId;
            _vm.InactiveORActive = InactiveCode;
            _vm.delTimeId = DelTimeId;
            _vm.FreqID = FreqId;
            _vm.IsAssigned = IsAssigned;
            _vm.makeRwoIncomplete = MakeRWOIncomplete;
            _vm.ServiceType = ServiceTypeId;
            _vm.HCPC = HCPC;
            _vm.all = all;
            
            return View(_vm);
        }

        public ActionResult Rwo_LookUp(DateTime? StartDate, DateTime? EndDate, string HoldCode,string ProductCode,string PayerId, int? locationId,int? MethodId, string InactiveCode, int? DelTimeId, string FreqId, string IsAssigned, string MakeRWOIncomplete ,string ServiceTypeId, string HCPC, bool all, [DataSourceRequest]DataSourceRequest request)
        {
            // Int32? account=0;
            //   Int32 num=0;
            return Json(RWOLookUPReport.RWOLookUPDetail(StartDate, EndDate, HoldCode, ProductCode, PayerId, locationId,MethodId, InactiveCode, DelTimeId, FreqId, IsAssigned, MakeRWOIncomplete, ServiceTypeId, HCPC, all).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult USS_OperatorLookUp()
        {
            USSOperatorLookUpVM _vm = new USSOperatorLookUpVM();
            _vm.uSSOperatorLookUp = USSOperatorLookUPReport.GetUSSOperatorLoopUp();
            return View(_vm);
        }


        public ActionResult RWO_RushOrders()
        {
            RwoRushOrders _vm = new RwoRushOrders();
            _vm.rwo_RushOrdersVM = RWOAsRushOrders.GetRWOSetAsRushOrders();
            return View(_vm);
        }

        public ActionResult FutureOreder_Confirmation()
        {

            FuruteOrderConfirmationVM _vm = new FuruteOrderConfirmationVM();
          
            _vm.InsTypeList = new SelectList(FutureOrderConfirmationReport.GetInsType(), "InsType", "InsType");
           

            return View(_vm);
        }


        [HttpPost]
        public ActionResult FutureOreder_Confirmation(FuruteOrderConfirmationVM _OrderVM)
        {
            _OrderVM.InsTypeList = new SelectList(FutureOrderConfirmationReport.GetInsType(), "InsType", "InsType");
            _OrderVM.futureOredreConfirmationData = FutureOrderConfirmationReport.GetFutureOredreConfirmation(_OrderVM.StartDate, _OrderVM.EndDate, _OrderVM.InsType).ToList();
            return View(_OrderVM);


        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


    }
}