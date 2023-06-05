using ReportsDatabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.ColdFusionReports.Models;
using USPS_Report.Areas.ColdFusionReports.Models.DataModels;

namespace USPS_Report.Areas.ColdFusionReports.Controllers
{
    public class FedexxController : Controller
    {
        // GET: ColdFusionReports/Fedexx
        public ActionResult Index(int? workOrderId)
        {
           
            
            List<WorkOrdersModel> list = new List<WorkOrdersModel>();

            if (workOrderId != null)
            {
                try
                { 
                    string OraConnection = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;
                    string Query = @"select * from tbl_ups_workorders where id_workorder ='" + workOrderId + "'";
                    
                    using (OleDbConnection conn = new OleDbConnection(OraConnection))
                    {
                        conn.Open();
                        using (OleDbCommand cmd = new OleDbCommand(Query, conn))
                        {
                            list = (List<WorkOrdersModel>)cmd.ExecuteScalar();
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View();
        }

      
    }
}