using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class WorkOrdersController : Controller
    {
        // GET: ColdFusionReports/WorkOrders
        public ActionResult Index()
        {



            return View();
        }

        [HttpPost]
        public ActionResult Index(DateTime startdate, DateTime enddate)
        {
            List<WorkOrderModel> workOrders = new List<WorkOrderModel>
                {
                    new WorkOrderModel
                    {
                        order_number = 12345,
                        customer_po_number = 54321,
                        creation_date = DateTime.Now,
                        order_type_code = "ABC",
                        confirmationnumber = 9876,
                        dateshipped = DateTime.Now.AddDays(1),
                        MyProperty = 42,
                        shipment_number = 100
                    },
                    new WorkOrderModel
                    {
                        order_number = 67890,
                        customer_po_number = 98765,
                        creation_date = DateTime.Now,
                        order_type_code = "XYZ",
                        confirmationnumber = 1234,
                        dateshipped = DateTime.Now.AddDays(2),
                        MyProperty = 99,
                        shipment_number = 200
                    },
                            };



            return View(workOrders);
        }
    }
}