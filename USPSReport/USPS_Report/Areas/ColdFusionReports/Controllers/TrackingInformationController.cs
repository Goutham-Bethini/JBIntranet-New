using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class TrackingInformationController : Controller
    {
        // GET: ColdFusionReports/TrackingInformation
        public ActionResult AmazonWalmartTracking()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AmazonWalmartTracking(DateTime? FromDate, DateTime? ToDate)
        {
            List<TrackingInfo> trackingInfo = new List<TrackingInfo>();

            if(FromDate != null && ToDate != null)
            {
                try
                {
                    string OraConnection = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    string FromDateStr = FromDate.Value.ToShortDateString();
                    string ToDateStr = ToDate.Value.AddDays(1).ToShortDateString();
                    string Query = @"SELECT
                                    a.order_number SALES_ORDER_NUMBER,
                                    c.customer_po_number PO_NUMBER,
                                    a.creation_date ORDER_CREATIONDATE,
                                    a.order_type_code ORDER_TYPE,
                                    b.confirmationnumber TRACKING_NUMBER,
                                    b.dateshipped TRACKING_CREATIONDATE,
                                    b.shipment_number DELIVERY_NUMBER
                                    FROM
                                    xxjbm_doo_headers_all a JOIN tbl_ups_workorders b on a.order_number = b.id_workorder
                                    LEFT JOIN XXJBM_ORDER_INTERFACE c on a.order_number = c.cloud_order_number
                                    WHERE
                                    order_type_code = 'AMAZON Standard Sales Order'
                                    and a.creation_date >= to_date('" + FromDateStr + "','MM/DD/YYYY') and a.creation_date < to_date('" + ToDateStr + "','MM/DD/YYYY')";
                    using (Oracle.ManagedDataAccess.Client.OracleConnection conn = new Oracle.ManagedDataAccess.Client.OracleConnection(OraConnection))
                    {
                        conn.Open();
                        using (Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(Query, conn))
                        {
                            var datalist = cmd.ExecuteReader();
                            while (datalist.Read())
                            {
                                trackingInfo.Add(new TrackingInfo
                                {
                                   OrderNumber = datalist[0] != DBNull.Value ? datalist.GetString(0): "",
                                   CustomerPONumber = datalist[1] != DBNull.Value ? datalist.GetString(1) : "",
                                   OrderCreationDate = datalist[2] != DBNull.Value ? datalist.GetDateTime(2): DateTime.MinValue,
                                   OrderType = datalist[3] != DBNull.Value ? datalist.GetString(3) : "",
                                   TrackingNumber = datalist[4] != DBNull.Value ? datalist.GetString(4) : "",
                                   TrackingCreationDate = datalist[5] != DBNull.Value ? datalist.GetDateTime(5) : (DateTime?) null,
                                   ShipmentNumber = datalist[6] != DBNull.Value ? datalist.GetInt32(6) : 0
                                });
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return View(trackingInfo);
        }
    }
}