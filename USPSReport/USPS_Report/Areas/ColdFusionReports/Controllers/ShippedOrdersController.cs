using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using static USPS_Report.Areas.ColdFusionReports.Models.DataModels.ShippedOrders;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class ShippedOrdersController : Controller
    {
        // GET: ColdFusionReports/ShippedOrders
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShippedOrders()
        {
            var model = new ShippedOrdersVM();
            // In this case, it doesn't matter what this model is really since we're using AJAX binding
            return View(model);
        }
        [HttpPost]
        public ActionResult GetShippedOrders([DataSourceRequest] DataSourceRequest request, DateTime start, DateTime end)
        {            
            ShippedOrdersVM shippedOrdersVM = new ShippedOrdersVM();
            if (start!= Convert.ToDateTime(Session["Start"])  && end != Convert.ToDateTime(Session["End"]))
            {
                Session["Start"] = start;
                Session["End"] = end;
                DateTime lstart = start;
                if (lstart.Day != 1)
                {
                    lstart = lstart.AddMonths(1);
                }
                var firstDayOfMonth = new DateTime(lstart.Year, lstart.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var _data = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ShippedOrders.GetShippedOrders(firstDayOfMonth, lastDayOfMonth);
                Session["ShippedOrders"] = _data;
                return Json(_data.Details.ToDataSourceResult(request));
            }
            else
            {
                shippedOrdersVM = Session["ShippedOrders"] as ShippedOrdersVM;
                return Json(shippedOrdersVM.Details.ToDataSourceResult(request));
            }           
        }
        [HttpPost]
        public ActionResult ShippedOrdersSummery(DateTime start, DateTime end)
        {
            ShippedOrdersVM shippedOrdersVM = new ShippedOrdersVM();
            if (start != Convert.ToDateTime(Session["Start"]) && end != Convert.ToDateTime(Session["End"]))
            {
                Session["Start"] = start;
                Session["End"] = end;
                DateTime locstart = start;
                if (locstart.Day != 1)
                {
                    locstart = locstart.AddMonths(1);
                }
                var firstOfMonth = new DateTime(locstart.Year, locstart.Month, 1);
                var lastOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);
                var _locData = USPS_Report.Areas.ColdFusionReports.Models.DataModels.ShippedOrders.GetShippedOrders(firstOfMonth, lastOfMonth);
                Session["ShippedOrders"] = _locData;
                return Json(_locData);
            }
            else
            {
                shippedOrdersVM = Session["ShippedOrders"] as ShippedOrdersVM;
                return Json(shippedOrdersVM);
            }            
        }        
    }    
}