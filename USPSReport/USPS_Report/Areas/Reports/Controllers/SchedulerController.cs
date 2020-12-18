using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using ReportsDatabase;
using USPS_Report.Models;
//using ReportsEntities = USPS_Report.Models.ReportsEntities;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class SchedulerController : Controller
    {
        // GET: Scheduler
        public ActionResult Index()
        {
            var model = new TaskViewModel();

            // In this case, it doesn't matter what this model is really since we're using AJAX binding
            return View(model);
        }

        // I usually have my binding methods for Kendo use HttpPost
        [HttpPost]
        public ActionResult GetData([DataSourceRequest] DataSourceRequest request)
        {
            var data = new List<TaskViewModel>
            {
                new TaskViewModel
                    {
                        Start = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        End = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        Title = "1000"
                    }
            };

            var _data = getData();
            return Json(_data.ToDataSourceResult(request));
        }


        private IList<TaskViewModel> getData()
        {
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
               
                DateTime? _dt = DateTime.Now.AddMonths(2);
                DateTime? todayDate = DateTime.Now;
                var _list2 = (from p in _db.tbl_PS_RepeatingOrders
                              where p.NextRepeatDate < _dt && p.NextRepeatDate != null && p.NextRepeatDate > todayDate
                              select new {p.Account, p.NextRepeatDate }).Distinct().ToList();

                var _list = (from p in _list2                            
                             group p by new {p.NextRepeatDate} into g
                            select new TaskViewModel
                            {
                                Start = (DateTime)g.Key.NextRepeatDate,
                                End = (DateTime)g.Key.NextRepeatDate,
                                Title = g.Count().ToString(),
                              
                            }).ToList<TaskViewModel>();



                return _list;

            }

        }



        public ActionResult ShippedOrder()
        {
            var model = new ShippedOrderVM();

            // In this case, it doesn't matter what this model is really since we're using AJAX binding
            return View(model);
        }


        [HttpPost]
        public ActionResult GetOrder([DataSourceRequest] DataSourceRequest request)
        {
           
            var data = new List<ShippedOrderVM>
            {
                new ShippedOrderVM
                    {
                        Start = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        End = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        Title = "1000"
                    }
            };

            var _data = getOrder();
            return Json(_data.ToDataSourceResult(request));
        }


        private IList<ShippedOrderVM> getOrder()
        {


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
               
                DateTime? _dt = DateTime.Now.AddMonths(-4);
                DateTime? todayDate = DateTime.Now;
                var _list2 = (from p in _db.tbl_PS_WorkOrder
                              join del in _db.tbl_DeliveryLocation_Table
                              on p.ID_DeliveryLocation equals del.ID
                              where p.Cancel_Date == null && p.Completed_Date != null && p.Completed_Date > _dt
                              select new { del.DeliveryLocationName, p.Completed_Date }).ToList();
                              
                var _list = (from p in _list2
                             group p by new { p.DeliveryLocationName, p.Completed_Date } into g
                             select new ShippedOrderVM
                             {
                                 Start = (DateTime)g.Key.Completed_Date,
                                 End = (DateTime)g.Key.Completed_Date,                                 
                                 
                                Location   =  g.Key.DeliveryLocationName == "J & B SHIPPING DEPT DIS" ? "DIS" :
                                            g.Key.DeliveryLocationName == "J & B SHIPPING DEPT DME" ? "DME" :
                                            g.Key.DeliveryLocationName == "Wright & Filippis" ?"WF" :
                                            g.Key.DeliveryLocationName == "DIS - VA 3000 Monroe GR" ? "VA GR" :
                                            g.Key.DeliveryLocationName == "VA 425 FISHER MQ" ? "VA MQ" :
                                            g.Key.DeliveryLocationName == "903 SHERIDAN - SHERIDAN, IN" ? "VA SH" :
                                            g.Key.DeliveryLocationName == "W ELM AVE" ? "VA ELM" : g.Key.DeliveryLocationName,

                                 Title = g.Count().ToString()  

                             }).ToList<ShippedOrderVM>();

           


                return _list.Select(t=> new ShippedOrderVM {

                      Start = t.Start, End = t.End, Title = t.Title + " "+t.Location
                }).ToList();

            }

        }



        public ActionResult FutureOrder()
        {
            var model = new ShippedOrderVM();

            // In this case, it doesn't matter what this model is really since we're using AJAX binding
            return View(model);
        }


        [HttpPost]
        public ActionResult GetFutureOrder([DataSourceRequest] DataSourceRequest request)
        {

            var data = new List<ShippedOrderVM>
            {
                new ShippedOrderVM
                    {
                        Start = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        End = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day),
                        Title = "1000"
                    }
            };

            var _data = getFutureOrder();
            return Json(_data.ToDataSourceResult(request));
        }


        private IList<ShippedOrderVM> getFutureOrder()
        {


            using (ReportsEntities _db = new ReportsEntities())
            {

                DateTime? _dt = DateTime.Now.AddMonths(-4);
                DateTime? todayDate = DateTime.Now;
                var _list2 = (from p in _db.v_FutureOrderTypes2
                             select p).OrderBy(t=>t.NextRepeatDate).ToList();

                var _list = (from p in _list2
                             group p by new { p.NextRepeatDate , p.OrderType} into g
                             select new ShippedOrderVM
                             {
                                 Start = (DateTime)g.Key.NextRepeatDate,
                                 End = (DateTime)g.Key.NextRepeatDate,


                                 Location = g.Key.OrderType ,

                                 Title = g.Count().ToString()

                             }).ToList<ShippedOrderVM>();


                return _list.Select(t => new ShippedOrderVM
                {


                    Start = t.Start,
                    End = t.End,
                    Title = t.Title + " " + t.Location
                }).ToList();

            }

        }

    }
}