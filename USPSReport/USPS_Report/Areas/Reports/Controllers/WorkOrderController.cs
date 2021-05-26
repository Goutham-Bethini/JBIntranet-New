using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using USPS_Report.Areas.Reports.Models;
using System.Net.Http;
using System.IO;
using System.Security.Principal;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class WorkOrderController : Controller
    {
        // GET: Reports/WorkOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WoNotPrinted()
        {

            return View(ReportWoNotPrinted.GetWO_NotPrinted());

        }


        public ActionResult woReports(int? ID)
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();

            woVM _vm = new woVM();

            _vm.IsAccess = IsInGroup(userName, "CancelOrder");
            _vm.ReleaseToolAccess = IsInGroup(userName, "ReleaseOrders");
            _vm.Numbers = 1;
            if (ID != null)
            {
                
                _vm.workOrder = ReportWorkOrder.GetWorkOrderByAccountByNumbers(ID, _vm.Numbers, User.Identity.Name.Split('\\').Last().ToLower());
              
                _vm.CancelFlag = 0;
            }
           // return View(_vm);

            return View(_vm);
        }


        public ActionResult WarehouseWOReport(int? ID)
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();

            woVM _vm = new woVM();

            _vm.IsAccess = IsInGroup(userName, "CancelOrder");
            _vm.ReleaseToolAccess = IsInGroup(userName, "ReleaseOrders");
            _vm.Numbers = 1;
            if (ID != null)
            {

                _vm.workOrder = ReportWorkOrder.GetWorkOrderByAccountByNumbers_Wh(ID, _vm.Numbers);

                _vm.CancelFlag = 0;
            }
            // return View(_vm);

            return View(_vm);
        }

        bool IsInGroup(string user, string group)
        {
            using (var identity = new WindowsIdentity(user))
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(group);
            }
        }

        public ActionResult wo_Search(int? account, int num,[DataSourceRequest]DataSourceRequest request)
        {
           // Int32? account=0;
            //   Int32 num=0;
            return Json(ReportWorkOrder.GetWorkOrderByAccountByNumbers(account, Convert.ToInt32(num), User.Identity.Name.Split('\\').Last().ToLower()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult test()
        { return View(); }

        [HttpPost]
        public ActionResult woReports(woVM _vm)
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();
            _vm.IsAccess = IsInGroup(userName, "CancelOrder");

            _vm.ReleaseToolAccess = IsInGroup(userName, "ReleaseOrders");

            _vm.workOrder = ReportWorkOrder.GetWorkOrderByAccountByNumbers(_vm.Account, _vm.Numbers, User.Identity.Name.Split('\\').Last().ToLower());
            return View(_vm);


        }

        [HttpPost]
        public ActionResult WarehouseWOReport(woVM _vm)
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();
            _vm.IsAccess = IsInGroup(userName, "CancelOrder");
            _vm.ReleaseToolAccess = IsInGroup(userName, "ReleaseOrders");
            _vm.workOrder = ReportWorkOrder.GetWorkOrderByAccountByNumbers_Wh(_vm.Account, _vm.Numbers);
            return View(_vm);


        }


        public ActionResult CancelWoOrder(int ID, int? Flag)
        {

            woVM _vm = new woVM();
            _vm.CancelFlag = Flag;
            _vm.ID = ID;
           
            return PartialView("_cancelwo", _vm);
        }

        public ActionResult ReleaseWoOrder(int ID, int? Flag)
        {

            woVM _vm = new woVM();
           // _vm.CancelFlag = Flag;
            _vm.ID = ID;

            return PartialView("_releasewo", _vm);
        }

        [HttpPost]
         public ActionResult ReleaseOrder(woVM _vm)
        {

            bool release = false;

             release = ReportWorkOrder.ReleaselOrderHDMS(_vm.ID, _vm.Reason);

            return RedirectToAction("woReports", new { ID = _vm.ID, reason = _vm.Reason });

        }


        [HttpPost]
         public ActionResult CancelOrder(woVM _vm)
        {
            

                bool cancel = false;
                string Reason = _vm.Reason;

                //Cancel the order in HDMS and send an email to Shipping Team if CancelFlag is 2 
            if (_vm.Reason.Contains("OTHER"))
                {
                Reason = _vm.OtherReason;
                //_vm.Reason = _vm.OtherReason;
                }
                cancel = ReportWorkOrder.CancelOrderHTMS(_vm.ID, Reason);


                Int32 count = 0;
                //chcek the inetrface table if _vm.CancelFlag != 2
                if (_vm.CancelFlag != 2) //
                {
                    try
                    {
                        // web api - tracking info
                        HttpClient client = new HttpClient();


                        client.BaseAddress = new Uri("http://10.10.1.49/TrackingOracle/");


                        var result2 = client.GetAsync("api/Interface/" + _vm.ID).Result;

                        //   var ser = JsonConvert.SerializeObject(typeof(CoInsDetail)); 
                        string _value;
                        using (var stm1 = result2.Content.ReadAsStreamAsync())
                        {
                            using (StreamReader reader = new StreamReader(stm1.Result))
                            {

                                _value = reader.ReadToEnd();


                                count = Convert.ToInt32(_value);
                                if (count > 0)
                                {
                                    _vm.CancelFlag = 2;
                                }
                                // _trackingList = JsonConvert.DeserializeObject<IList<TrackingList>>(_value);

                            }
                        }



                        //StringBuilder sb = new StringBuilder();

                        //_trackingList = JsonConvert.DeserializeObject<TrackingList>(_value);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }



                if (_vm.CancelFlag == 2) //_vm.CancelFlag == 2
                {
                    ReportWorkOrder.sendEmailtoShipping(_vm.ID, _vm.Reason, _vm.OtherReason);
                }


                ReportWorkOrder.AddNoteForCancelOrder(_vm);

                //  int? ID = _vm.ID;
                _vm.Account = _vm.ID;
            
            return RedirectToAction("woReports", new { ID = _vm.ID, reason = Reason });

        
        }
        //public ActionResult CancelOrder1( int? CancelFlag,int ID, string Reason)
        //{

        //    CancelOrderVM _vm = new CancelOrderVM();
        //    //Cancel the order in HDMS and send an email to Shipping Team if CancelFlag is 2 
        //    ReportWorkOrder.CancelOrderHTMS(ID);
        //    if (CancelFlag == 2)
        //    {
        //        ReportWorkOrder.sendEmailtoShipping(ID);
                 
        //    }

        //    _vm.wo = ID;
        //    _vm.cancelFlag = CancelFlag;
            

        //  return View(_vm);
        //}


        public ActionResult pd_Report(Int32 woID, [DataSourceRequest]DataSourceRequest request)
        {

            return Json(ReportWorkOrder.GetProductDetailsById(woID).ToDataSourceResult(request));
        }

        public ActionResult WoLost()
        {

            return View(ReportWoLost.GetWO_Lost());

        }


        public ActionResult Wo_HoldTypesQty()
        {
            HoldPayerVM _vm = new HoldPayerVM();
            _vm.GetHoldPayer= ReportWOHolds.GetAllWoHoldTypes_Qty();
           // _vm.HoldPayerPieChart = ChartClass.HoldPayerChart(_vm.GetHoldPayer);
            return View(_vm);
            // return View(ReportWOHolds.GetAllWoHoldTypes_Qty());
        }

        public ActionResult woHold(string id)
        {
            return View(ReportWOHolds.GetAllWoHolds(id));
        }


        public ActionResult InactiveAccount()
        {
            return View(InactiveAccountReport.GetInactiveAccount());
        }

        public ActionResult Calender()
        {
            return View();
        }

    public ActionResult RONotPurchaseReport()
        {
            return View(InactiveAccountReport.GetRONotPurchased());
        }

       // Shows accounts with discontinued products on the RO
        public ActionResult DiscontProdReport()
        {
            return View(DiscontinuedProductReport.GetDisconProd());
        }

        public ActionResult MultipleShipDates(multidatesVM _vm )
        {
            if (_vm.NotFirstTime != true)
            {
                DateTime date = DateTime.Today;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                _vm.StartDate = firstDayOfMonth;
                _vm.EndDate = lastDayOfMonth;
                _vm.AutoCount = MultipleshipsReport.GetAutoCorrcetCount(_vm.StartDate, _vm.EndDate);
                _vm.details = MultipleshipsReport.GetMultipleshipDates(_vm.StartDate, _vm.EndDate);
            }
            else
            {
                _vm.AutoCount = MultipleshipsReport.GetAutoCorrcetCount(_vm.StartDate, _vm.EndDate);
                _vm.details = MultipleshipsReport.GetMultipleshipDates(_vm.StartDate, _vm.EndDate);
            }
                //new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            return View(_vm);
        }

        public ActionResult LastMonthShipDates(DateTime? _stDt, DateTime? _endDt)
        {
            DateTime date = Convert.ToDateTime( _stDt).AddMonths(-1);
            var firstDayOfLastMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfLastMonth = firstDayOfLastMonth.AddMonths(1).AddDays(-1);
            multidatesVM _vm = new multidatesVM();
            _vm.StartDate = firstDayOfLastMonth;
            _vm.EndDate = lastDayOfLastMonth;
            _vm.details = MultipleshipsReport.GetMultipleshipDates(firstDayOfLastMonth, lastDayOfLastMonth);
            _vm.NotFirstTime = true;
           
            return RedirectToAction("MultipleShipDates",_vm );
            // return View(_vm);
        }

        public ActionResult NextMonthShipDates(DateTime? _stDt, DateTime? _endDt)
        {
            DateTime date = Convert.ToDateTime(_stDt).AddMonths(1);
            var firstDayOfNextMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfNextMonth = firstDayOfNextMonth.AddMonths(1).AddDays(-1);
            multidatesVM _vm = new multidatesVM();
            _vm.StartDate = firstDayOfNextMonth;
            _vm.EndDate = lastDayOfNextMonth;
            _vm.details = MultipleshipsReport.GetMultipleshipDates(firstDayOfNextMonth, lastDayOfNextMonth);
            _vm.NotFirstTime = true;
            return RedirectToAction("MultipleShipDates",  _vm );
        }


       // [HttpPost]
        //public ActionResult MultipleShipDates(multidatesVM _vm)
        //{
        //    _vm.details = MultipleshipsReport.GetMultipleshipDates(_vm.StartDate, _vm.EndDate);

        //    return View(_vm);
        //}

        //public ActionResult GetMultipleOrderDates(DateTime? _startDt,DateTime? _endDt, [DataSourceRequest]DataSourceRequest request)
        //{
        //    // Int32? account=0;
        //    //   Int32 num=0;
        //    return Json(MultipleshipsReport.GetMultipleshipDates(_startDt,_endDt).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}



        public ActionResult WorkOrderReport()
        {
            return View();
        }
    }
}