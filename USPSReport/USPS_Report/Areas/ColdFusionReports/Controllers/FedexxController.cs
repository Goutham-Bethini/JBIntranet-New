using DocumentFormat.OpenXml.Office2010.Excel;
using Oracle.ManagedDataAccess.Client;
using ReportsDatabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;
using USPS_Report.Models;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class FedexxController : Controller
    {
        // GET: ColdFusionReports/Fedexx
        public ActionResult Index(int? workOrderId)
        {                       
            List<WorkOrdersModel> TrackingInfo = new List<WorkOrdersModel>();

            if (workOrderId != null)
            {
                try
                { 
                    string OraConnection = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                    string Query = @"select ID_WorkOrder, DateShipped, IntWeight, UserID, ConfirmationNumber from tbl_ups_workorders where id_workorder ='" + workOrderId + "' order by DateShipped desc";
                    
                    using (Oracle.ManagedDataAccess.Client.OracleConnection conn = new Oracle.ManagedDataAccess.Client.OracleConnection(OraConnection))
                    {
                        conn.Open();
                        using (Oracle.ManagedDataAccess.Client.OracleCommand cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(Query, conn))
                        {
                            var datalist = cmd.ExecuteReader();
                            while (datalist.Read())
                            {
                                TrackingInfo.Add(new WorkOrdersModel
                                {
                                    ID_WorkOrder = datalist[0] != DBNull.Value ? datalist.GetInt32(0): 0,
                                    DateShipped = datalist[1] != DBNull.Value ? datalist.GetDateTime(1): (DateTime?)null,
                                    IntWeight = datalist[2] != DBNull.Value ? datalist.GetDecimal(2): 0,
                                    UserID = datalist[3] != DBNull.Value ? datalist.GetString(3) : "",
                                    ConfirmationNumber = datalist[4] != DBNull.Value ? datalist.GetString(4) : "",
                                });
                            }
                        }
                    }
                    //getting the user name based on userid.
                    using(xCarrier_ProdEntities xcarrier = new xCarrier_ProdEntities())
                    {
                        foreach (var tracking in TrackingInfo)
                        {
                            int result = 0;
                            if(tracking.UserID != string.Empty)
                            {
                                if(int.TryParse(tracking.UserID, out result))
                                {
                                    tracking.UserID = xcarrier.XCARRIER_USER_REGISTRATION.Where(p => p.LOGIN_ID == result).Select(p => p.USERNAME).FirstOrDefault();
                                }                                
                            }
                        }
                    }                 
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View(TrackingInfo);
        }

      
    }
}