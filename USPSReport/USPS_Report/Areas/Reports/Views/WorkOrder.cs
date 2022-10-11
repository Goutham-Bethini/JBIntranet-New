using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Text;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Configuration;
using USPS_Report.Areas.ColdFusionReports.Models;
using USPS_Report.Helper;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReportWorkOrder
    {
        public static void test(Int32? account, Int32 numbers)
        {
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                var productDetails = (from wol in _db.tbl_PS_WorkOrderLine
                                      join pro in _db.tbl_Product_Table
                                      on wol.ID_Product equals pro.ID
                                      join uom in _db.tbl_Inv_UOM_Table
                                      on pro.ID_UOM equals uom.ID
                                      from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                      from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode).DefaultIfEmpty()


                                      where wol.ID_PS_WorkOrder == 4903622
                                      select new ProductDetails
                                      {
                                          Product = pro.ProductCode,
                                          Description = pro.ProductDescription,
                                          Ordered = wol.QtyOrdered,
                                          Shipped = wol.QtyShipped,
                                          UOM = uom.UOMName + " of " + pro.PerUnitQty,
                                          UnitWeight = pro.UnitWeight,
                                          LineOrderQty = lin.linQtyAvailable

                                      }).ToList();
            }
        }
        public static IList<WorkOrder> GetWorkOrderByAccountByNumbers(Int32? account, Int32 numbers, string operatorName)
        {

            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    IList<HistoryList> _listhistory = new List<HistoryList>();
                    IList<tbl_PS_WorkOrder> _woListTemp = new List<tbl_PS_WorkOrder>();
                    if (account > 900000)
                        _woListTemp = _db.tbl_PS_WorkOrder.Where(t => t.ID == account).OrderByDescending(t => t.ID).Take(numbers).ToList();
                    else
                        _woListTemp = _db.tbl_PS_WorkOrder.Where(t => t.Account == account).OrderByDescending(t => t.ID).Take(numbers).ToList();

                    
                    var _woList = (from p in _woListTemp
                                   select new
                                   {
                                       p.Account,
                                       p.Request_Date,
                                       p.Cancel_By,
                                       p.ID,
                                       p.Cancel_Date,
                                       p.Completed_Date,
                                       p.LastPrintDate,
                                       p.HoldFromShipping,
                                       p.HoldFromShippingReason,
                                       p.ID_PrimaryAssignedUser,
                                       Cancel_User = p.Cancel_User != null ? (int)p.Cancel_User : 0,
                                       p.DateMovedToUser,
                                       p.ConfirmationNumber,
                                       p.Cancel_Note,
                                       op1_LegalName = _db.tbl_Operator_Table.Where(op => op.ID == p.Cancel_User).Select(op => op.LegalName).Take(1).SingleOrDefault(),
                                       ops_LegalName = _db.tbl_Operator_Table.Where(op => op.ID == p.ID_PrimaryAssignedUser).Select(op => op.LegalName).Take(1).SingleOrDefault(),
                                       fullName = _db.tbl_Account_Member.Where(mem => mem.Account == p.Account && mem.Member == 1).Select(t => t.First_Name + " " + t.Last_Name).SingleOrDefault(),
                                   }).ToList();

                    var _list = _woList.Select(t =>  new WorkOrder
                    {
                        fullname = t.fullName,
                        Account = t.Account,
                        Request_Date = t.Request_Date.Value,
                        CreatedBy = t.ID_PrimaryAssignedUser.Equals(221) ? "Auto" : t.ops_LegalName,
                        CancelledBy = t.op1_LegalName != null ? t.op1_LegalName : t.Cancel_By,
                        WorkOrderID = t.ID,
                        //Status = isFailedToInterface(t.Account, t.ID)? "Failed to Interface" : " not failed",
                        //isRMAdone(t.ID) ? "<strong>Return/RMA</strong>" :
                        Status =  t.Cancel_Date != null ? "<strong><u>Cancelled:</u></strong> " + Environment.NewLine + t.Cancel_Note :
                        t.Completed_Date != null ? (isFailedToInterface(t.Account, t.ID) ? "<strong style='color: red;'>Failed to Interface</strong>" : "<strong>Completed</strong> ") :
                        t.LastPrintDate != null ? (isFailedToInterface(t.Account, t.ID) ? "<strong style='color: red;'>Failed to Interface</strong>" : "<strong>Printed/Sent to oracle</strong> ") :
                          (t.HoldFromShipping == 1 && t.HoldFromShippingReason == null) ? "<strong>Created</strong>" :
                           (t.HoldFromShipping == 1 && t.HoldFromShippingReason != null) ? " <strong><u>Holding:</u></strong>" + "~" + t.HoldFromShippingReason :
                        (t.HoldFromShipping == 1 && t.HoldFromShippingReason.Contains("%Back Order%")) ? "<strong><u>Back Ordered and Holding:</u></strong> " + Environment.NewLine + t.HoldFromShippingReason :
                        (t.HoldFromShipping == 1 && t.HoldFromShippingReason.Contains("Back Order ~")) ? "<strong>Back Ordered</strong>" : " <strong>Waiting to Interface</strong>",

                        ReleasedBy = string.Join("\n", _db.WorkOrdersReleased.Where(u => u.worID_WorkOrder == t.ID).Select(u => u.worReleasedBy).ToArray()),

                        TrackingNumbers = string.Join(", \n", _db.tbl_UPS_WorkOrders.Where(u => u.ID_WorkOrder == t.ID && u.CancelDate == null).Select(u => u.ConfirmationNumber).ToArray()),

                        Length = _db.tbl_UPS_WorkOrders.Where(u => u.ID_WorkOrder == t.ID).Select(u => u.ConfirmationNumber.Length).Take(1).SingleOrDefault(),

                        productDetails = (from wol in _db.tbl_PS_WorkOrderLine
                                          join pro in _db.tbl_Product_Table
                                          on wol.ID_Product equals pro.ID
                                          join uom in _db.tbl_Inv_UOM_Table
                                          on pro.ID_UOM equals uom.ID
                                          from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                              //  from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode && w.linQty == wol.QtyOrdered).DefaultIfEmpty()

                                          where wol.ID_PS_WorkOrder == t.ID
                                          select new ProductDetails
                                          {
                                              Product = pro.ProductCode,
                                              Description = pro.ProductDescription,
                                              //ProductId= wol.ID_Product,
                                              Ordered = wol.QtyOrdered,
                                              Shipped = wol.QtyShipped,
                                              UOM = uom.UOMName + " of " + pro.PerUnitQty,
                                              UnitWeight = pro.UnitWeight,
                                              LineOrderQty = _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode && w.linQty == wol.QtyOrdered).Select(u => u.linQtyAvailable).Take(1).FirstOrDefault(),  //lin.linQtyAvailable,
                                              UnitPRice = wol.UnitPrice,
                                              lineId = wol.ID

                                          }).Distinct().ToList(),

                        ActiveInsurance = _db.Database.SqlQuery<string>(@"declare @AcOrWorkOrder int
                                                        set @AcOrWorkOrder=" + t.Account +
                                                        @" if (@AcOrWorkOrder>900000)
                                                        begin

                                                        Select top 1  ins.PayerName       
                                                             from HHSQLDB.dbo.tbl_PS_WOrkOrder t1      
      
                                                            inner join HHSQLDB.dbo.v_AR_MemberInsuranceCoverage ins           
                                                            on t1.Account = ins.Account            
                                                            where  	
                                                             ((ins.Expiration_Date IS NULL OR ins.Expiration_Date >= ISNULL(t1.Completed_Date,t1.LastPrintDate))       
                                                             AND (ins.Effective_Date IS NULL OR ins.Effective_Date <=  ISNULL(t1.Completed_Date,t1.LastPrintDate)))        
                                                             AND  id_payer not in(3622,3626,3623,3624,3625, 4738)       
                                                              and
                                                            (t1.ID = @AcOrWorkOrder)
	                                                        order by Sequence_Number  
	                                                        --print 'workorder'
                                                        end
                                                        else
                                                        begin
                                                        Select top 1  ins.PayerName       
                                                             from HHSQLDB.dbo.tbl_PS_WOrkOrder t1            
                                                            inner join HHSQLDB.dbo.v_AR_MemberInsuranceCoverage ins        
                                                            on t1.Account = ins.Account            
                                                            where       
                                                             ((ins.Expiration_Date IS NULL OR ins.Expiration_Date >= ISNULL(t1.Completed_Date,t1.LastPrintDate))         
                                                             AND (ins.Effective_Date IS NULL OR ins.Effective_Date <=  ISNULL(t1.Completed_Date,t1.LastPrintDate)))           
                                                             AND  id_payer not in(3622,3626,3623,3624,3625, 4738)       
                                                              and
                                                            (t1.Account = @AcOrWorkOrder)
	                                                        order by Sequence_Number  
	                                                        --print 'ac'
                                                        end").FirstOrDefault(),
                        historylist = _db.Database.SqlQuery<HistoryList>("SELECT " +
                                  "  DateMovedToUser AS Date, " +
                                 "   'Created' AS Process, " +
                                  "  1 as Sort " +
                            "    FROM tbl_ps_workorder wos " +
                              "  WHERE wos.id =  " + t.ID.ToString() +
                                "    AND DateMovedToUser IS NOT NULL " +
                           " UNION " +
                               " SELECT " +
                                   " ElrDateFinished, " +
                                   " 'Eligibility Checked', " +
                                  "  2 as Sort " +
                              "  FROM " +
                                  "  tbl_ps_workorder    wos join " +
                                          "  Intranet..Eligibility_orders    ord on wos.id = ord.ordWOId " +
                                  "  JOIN    Intranet..EligibilityRuns       elr ON elrID = ordElrID " +
                              "  WHERE " +
                                   " wos.Account = " + t.Account.ToString() +
                                      "  and wos.id = " + t.ID.ToString() +
                                   " AND elrDateFinished IS NOT NULL " +


                                  "  UNION " +
                               " SELECT " +
                                  "  worReleasedDate, " +
                                  "  'Released from Hold', " +
                                  "  3 AS Sort " +
                              "  FROM WorkOrdersReleased " +
                             "   WHERE worID_WorkOrder = " + t.ID.ToString() +
                                   " AND worReleasedDate IS NOT NULL  " +

                                  "  UNION " +
                              "  SELECT " +
                                   " LastPrintDate AS Date, " +
                                   " 'Printed in HDMS' AS Process, " +
                                   " 4 AS Sort" +
                              "  FROM tbl_ps_workorder wos " +
                              "  WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND LastPrintDate IS NOT NULL " +

                          "  UNION " +
                               " SELECT " +
                                   " LastPrintDate AS Date, " +
                                   " 'Printed in HDMS' AS Process, " +
                                   " 4 AS Sort " +
                               " FROM tbl_ps_workorder wos " +
                              "  WHERE wos.ID =  " + t.ID.ToString() +
                                  "  AND LastPrintDate IS NOT NULL " +
                          "  UNION " +
                               " SELECT " +
                                   " Cancel_date AS Date, " +
                                  "  'Canceled in HDMS' AS Process, " +
                                   " 5 AS Sort " +
                              "  FROM tbl_ps_workorder wos " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND Cancel_date IS NOT NULL " +

                                  "  UNION " +
                               " SELECT " +
                                   " Completed_date AS Date, " +
                                  "  'Completed in HDMS' AS Process, " +
                                   " 8 AS SOrt " +
                             "   FROM tbl_ps_workorder wos " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                  "  AND Completed_date IS NOT NULL " +
                           " UNION " +
                             "   SELECT woSent, " +
                               " 'Sent to Oracle', " +
                              "  7 AS Sort " +
                               " FROM ERP_OrdersSent erp join tbl_ps_workorder wos " +
                              "  on wos.id = erp.woWorkOrder " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND woSent IS NOT NULL " +

                                      "  ORDER BY Date, Sort").ToList<HistoryList>(),
                        AccountNotes = _db.Database.SqlQuery<AccountNote>(@"select an.ID 'NoteId',anh.ID 'NoteHistoryId',an.NoteHeading,anh.NoteDate,anh.NoteText,
                                        ope.LegalName 'NoteCreatedBy'
                                        from HHSQLDB.dbo.tbl_Account_Note an 
                                        join HHSQLDB.dbo.tbl_Account_Note_History anh 
                                        on an.ID=anh.ID_Note
                                        join HHSQLDB.dbo.tbl_Operator_Table ope on anh.ID_Operator=ope.ID
                                        where an.Account=" + t.Account + @"
                                        and anh.NoteDate>=(select dateadd(day, -30, getdate()))
                                        order by anh.NoteDate desc").ToList<AccountNote>()
                    }).ToList();

                    foreach (var item in _list)
                    {
                        
                        
                           
                        StringBuilder _str = new StringBuilder();
                        IList<HistoryList> _historyList = new List<HistoryList>();
                        _historyList = item.historylist;
                        foreach (var t in _historyList)
                        {
                            _str = _str.Append(t.Date.ToString());
                            _str = _str.Append(" - " + t.Process);
                            _str = _str.Append(Environment.NewLine);

                        }
                        item.History = _str.ToString();

                        IList<TrackingList> _trackingList = new List<TrackingList>();

                        if (item.TrackingNumbers == "")
                        {
                            //   _trackingList = _db.Database.SqlQuery<TrackingList>("select ConfirmationNum from Reports..FedEx_Tracking_Tbl where WorkOrder = " + item.WorkOrderID).ToList();

                            try
                            {
                                // web api - tracking info
                                HttpClient client = new HttpClient();


                                client.BaseAddress = new Uri("http://JBMAZWeb01/TrackingShippedBy/");
                                //  client.BaseAddress = new Uri("http://localhost:61027/");

                                var result2 = client.GetAsync("api/Values/" + item.WorkOrderID).Result;

                                //   var ser = JsonConvert.SerializeObject(typeof(CoInsDetail)); 
                                string _value;
                                using (var stm1 = result2.Content.ReadAsStreamAsync())
                                {
                                    using (StreamReader reader = new StreamReader(stm1.Result))
                                    {
                                        TrackingList trcking = new TrackingList();
                                        _value = reader.ReadToEnd();

                                        _trackingList = JsonConvert.DeserializeObject<IList<TrackingList>>(_value);

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
                        //------------------------------------
                        if (_trackingList.Count == 0 && item.TrackingNumbers == "")
                        {
                            string query = "select * from Reports.dbo.FedEx_Tracking_Tbl where WorkOrder = " + item.WorkOrderID + "";
                            _trackingList = _db.Database.SqlQuery<TrackingList>(query).ToList();
                        }
                        //----------------------------------------

                        if (_trackingList.Count != 0)
                        {
                            foreach (var t in _trackingList)
                            {
                                item.Length = t.ConfirmationNum.Length;
                                item.TrackingNumbers = item.TrackingNumbers + t.ConfirmationNum + ", \n";
                                item.ShippedBy = t.FullName;
                                if (!string.IsNullOrEmpty(item.ShippedBy))
                                {
                                    using (HHSQLDBEntities _hhdb = new HHSQLDBEntities())
                                    {
                                        string ShippedBy = _hhdb.Database.SqlQuery<string>("Select top 1 felName from FedExLogins where felID = " + item.ShippedBy + "").FirstOrDefault();
                                        if (!string.IsNullOrEmpty(ShippedBy))
                                        {
                                            item.ShippedBy = ShippedBy;
                                        }
                                    }
                                }

                            }

                            item.Cancel = 0; //no cancel button, order is already shipped
                        }
                        else if (item.TrackingNumbers != "")
                        {
                            item.ShippedBy = GetShippedByFromOracle(item.WorkOrderID);
                            item.Weight = GetWeightFromOracle(item.WorkOrderID);
                            item.Cancel = 0; //no cancel button, order is already shipped
                            //get the shipped by item
                            item.Release = 0;
                        }
                        else if (item.Status.Contains("Printed/Sent to oracle"))
                        {
                            item.Cancel = 2; //cancel in HDMS and oracle(send email)
                            item.Release = 0;
                        }
                        else if (item.Status.Contains("Cancelled") || item.Status.Contains("Completed"))
                        {
                            item.Cancel = 0;// disabled cancel button
                            item.Release = 0;
                        }
                        else

                        {
                            item.Cancel = 1; //enable cancel and delete in hdms
                            item.Release = 1;
                        }

                        item.CancelReason = "Reason";

                        if (item.TrackingNumbers != null && item.TrackingNumbers != "")
                        {
                            string trackingNumbers = item.TrackingNumbers.Replace(" \n", "");
                            if (trackingNumbers[trackingNumbers.Length - 1] == ',')
                            {
                                trackingNumbers = trackingNumbers.Remove(trackingNumbers.Length - 1, 1);
                            }
                            item.TrackingNumbersList = trackingNumbers.Split(',').ToList<string>();
                        }

                        RMAProduct obj;
                        List<RMAProduct> lstRMAProduct = new List<RMAProduct>();
                        try
                        {
                            string _conn = ConfigurationManager.ConnectionStrings["ColdFusionReportsEntitiesOracle"].ConnectionString;
                            OleDbConnection myConnection = new OleDbConnection(_conn);
                            String query = string.Empty;
                            myConnection.Open();
                            query = @"SELECT   rma_sh.order_number return_order_number,
msib.attribute2 ordered_item_HDMS,rma_sl.ordered_item,
           rma_sh.creation_date,
           SUM (rma_sl.ORDERED_QUANTITY * rma_sl.UNIT_SELLING_PRICE)
              ""Extended_Price"",
           r_sh.order_number sales_order_number,
           r_sh.creation_date,
           SUM(r_sl.ORDERED_QUANTITY * r_sl.UNIT_SELLING_PRICE)
              ""Extended_Price""
    FROM OE_ORDER_LINES_ALL rma_sl,
           OE_ORDER_HEADERS_ALL rma_sh,
           oe_order_lines_all r_sl,
           oe_order_headers_all r_sh,
           mtl_system_items_b msib
   WHERE rma_sl.header_id = rma_sh.header_id
           AND r_sh.order_number = " + item.WorkOrderID + @"
           AND rma_sh.order_category_code = 'RETURN'
           AND r_sl.header_id = r_sh.header_id
           AND rma_sl.reference_line_id = r_sl.line_id
           and msib.organization_id = 92
           and msib.segment1 = rma_sl.ordered_item
GROUP BY   rma_sh.order_number,msib.attribute2,rma_sl.ordered_item,
           r_sh.order_number,
           rma_sh.creation_date,
           r_sh.creation_date
ORDER BY r_sh.creation_date DESC
";
                            OleDbCommand myCommand = new OleDbCommand(query, myConnection);
                            OleDbDataReader reader = myCommand.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new RMAProduct();
                                obj.ReturnOrderNumber = GetSafeData.GetSafeInt(reader.GetValue(0));
                                obj.ProductCode = GetSafeData.GetSafeString(reader.GetValue(1));
                                obj.OrderNumber = GetSafeData.GetSafeInt(reader.GetValue(5));
                                lstRMAProduct.Add(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            lstRMAProduct = new List<RMAProduct>();
                        }

                        item.Status = lstRMAProduct.Count > 0 ? "<strong>Return/RMA</strong>" : item.Status;
                        foreach (ProductDetails product in item.productDetails)
                        {
                            product.isLineRMAdone = lstRMAProduct.Count > 0 ? lstRMAProduct.Any(t => t.ProductCode == product.Product) : false;
                        }
                    }

                    string query2 = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',19,GETDATE())";

                    int rowsinsert = _db.Database.ExecuteSqlCommand(query2);
                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<WorkOrder>();
            }


        }
        public static bool isRMAdone(int wo)
        {
            string _conn = ConfigurationManager.ConnectionStrings["ColdFusionReportsEntitiesOracle"].ConnectionString;
            OleDbConnection myConnection = new OleDbConnection(_conn);
            String query = string.Empty;
            myConnection.Open();
            query = @"SELECT   rma_sh.order_number return_order_number,
msib.attribute2 ordered_item_HDMS,rma_sl.ordered_item,
           rma_sh.creation_date,
           SUM (rma_sl.ORDERED_QUANTITY * rma_sl.UNIT_SELLING_PRICE)
              ""Extended_Price"",
           r_sh.order_number sales_order_number,
           r_sh.creation_date,
           SUM(r_sl.ORDERED_QUANTITY * r_sl.UNIT_SELLING_PRICE)
              ""Extended_Price""
    FROM OE_ORDER_LINES_ALL rma_sl,
           OE_ORDER_HEADERS_ALL rma_sh,
           oe_order_lines_all r_sl,
           oe_order_headers_all r_sh,
           mtl_system_items_b msib
   WHERE rma_sl.header_id = rma_sh.header_id
           AND r_sh.order_number = "+wo+@"
           AND rma_sh.order_category_code = 'RETURN'
           AND r_sl.header_id = r_sh.header_id
           AND rma_sl.reference_line_id = r_sl.line_id
           and msib.organization_id = 92
           and msib.segment1 = rma_sl.ordered_item
GROUP BY   rma_sh.order_number,msib.attribute2,rma_sl.ordered_item,
           r_sh.order_number,
           rma_sh.creation_date,
           r_sh.creation_date
ORDER BY r_sh.creation_date DESC
";
            OleDbCommand myCommand = new OleDbCommand(query, myConnection);
            OleDbDataReader reader = myCommand.ExecuteReader();
            RMAProduct obj;
            List<RMAProduct> lstRMAProduct = new List<RMAProduct>();
            while (reader.Read())
            {
                obj = new RMAProduct();
                obj.ReturnOrderNumber = GetSafeData.GetSafeInt(reader.GetValue(0));
                obj.ProductCode= GetSafeData.GetSafeString(reader.GetValue(1));
                obj.OrderNumber= GetSafeData.GetSafeInt(reader.GetValue(5));
                lstRMAProduct.Add(obj);
            }
            if (lstRMAProduct.Count > 0)
            {
                return true;
            }
            else
            return false;
        }

        public static bool isFailedToInterface(int? account, int wo)
        {
            try
            {
                string OraConnection = ConfigurationManager.ConnectionStrings["ColdFusionReportsEntitiesOracle"].ConnectionString;
                string Query = @"select status from jbm_hdms_order_interface where workorderid=" + wo + " and ROWNUM = 1 order by creation_date desc";
                bool isFailed = false;
                using (OleDbConnection conn = new OleDbConnection(OraConnection))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(Query, conn))
                    {
                        string res = cmd.ExecuteScalar().ToString();
                        isFailed = (res == "" || res == "L") ? false : true;
                        if (isFailed)
                        {
                            string Query2 = @"select error_code from JBM_ORDER_INTERFACE_ERROR_LOG where error_code not in ( 'FAILED PHONE DETAILS UPDATE', 'FAILED EMAIL ADDRESS UPDATE') and workorderid = " + wo + " and ROWNUM = 1 order by creation_date desc";
                            string res2 = string.Empty;

                            using (OleDbCommand cmd2 = new OleDbCommand(Query2, conn))
                            {
                                try
                                {
                                    res2 = cmd2.ExecuteScalar().ToString();
                                }
                                catch(Exception ex)
                                {

                                }                                

                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
                                mail.From = new MailAddress("noreply@jandbmedical.com");
                                mail.To.Add("ShippingTeam@jandbmedical.com");
                                //mail.To.Add("maheshkattamuribpl@jandbmedical.com");
                                //mail.To.Add("mott@jandbmedical.com");
                                //mail.CC.Add("maheshkattamuribpl@jandbmedical.com");
                                mail.Subject = "Failed to Interface";
                                string body = @"<html>
<body>
Hello All, <br/><br/>  Please find Work order details below-<br/><br/>
<table border=""1"">
<tr>
<th>
HDMS account number
</th>
<th>
WO number
</th>
<th>
Failed Reason
</th>
</tr>
<tr>
<td>
" + account + @"
</td>
<td>
" + wo + @"
</td>
<td>
" + res2 + @"
</td>
</tr>
</table>
<br/> Thanks,<br/> IT Team
</body>
</html> ";
                                mail.Body = body;
                                mail.IsBodyHtml = true;
                                SmtpServer.Send(mail);
                            }


                        }
                    }
                }
                return isFailed;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static decimal GetWeightFromOracle(int WorkOrderID)
        {
            try
            {
                string OraConnection = ConfigurationManager.ConnectionStrings["EntitiesOracle1"].ConnectionString;
                string Query = @"select INTWEIGHT from tbl_ups_workorders where id_workorder ='" + WorkOrderID + "'";
                decimal Weight = 0.0m;
                //decimal weigh_dec = 0.0m;
                using (OleDbConnection conn = new OleDbConnection(OraConnection))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(Query, conn))
                    {
                        Weight = cmd.ExecuteScalar() != null ? string.IsNullOrEmpty(cmd.ExecuteScalar().ToString()) ? 0.0m : (decimal)cmd.ExecuteScalar() : 0.0m;
                    }
                }
                return Weight;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string GetShippedByFromOracle(int WorkOrderID)
        {
            IList<TrackingShippedList> _trackingList = new List<TrackingShippedList>();
            string ShippedBy = "";
            try
            {
                // web api - tracking info
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://JBMAZWeb01/TrackingShippedBy/");
                //  client.BaseAddress = new Uri("http://localhost:61027/");

                var result2 = client.GetAsync("api/Values/" + WorkOrderID).Result;

                //   var ser = JsonConvert.SerializeObject(typeof(CoInsDetail)); 
                string _value;
                using (var stm1 = result2.Content.ReadAsStreamAsync())
                {
                    using (StreamReader reader = new StreamReader(stm1.Result))
                    {
                        TrackingShippedList trcking = new TrackingShippedList();
                        _value = reader.ReadToEnd();

                        _trackingList = JsonConvert.DeserializeObject<IList<TrackingShippedList>>(_value);

                    }
                }

                foreach (var item in _trackingList)
                {
                    if (!string.IsNullOrEmpty(item.FullName))
                    {
                        using (HHSQLDBEntities _hhdb = new HHSQLDBEntities())
                        {
                            string FullName = _hhdb.Database.SqlQuery<string>("Select top 1 felName from FedExLogins where felID = " + item.FullName + "").FirstOrDefault();
                            if (!string.IsNullOrEmpty(FullName))
                            {
                                item.FullName = FullName;
                            }
                        }
                    }
                }

                //StringBuilder sb = new StringBuilder();

                //_trackingList = JsonConvert.DeserializeObject<TrackingList>(_value);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }


            //------------------------------------
            if (_trackingList.Count == 0)
            {
                string query = "select * from Reports.dbo.FedEx_Tracking_Tbl where WorkOrder = " + WorkOrderID + "";
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _trackingList = _db.Database.SqlQuery<TrackingShippedList>(query).ToList();
                }
            }
            //----------------------------------------

            if (_trackingList.Count != 0)
            {
                foreach (var t in _trackingList)
                {
                    ShippedBy = t.FullName;
                }
            }
            return ShippedBy;
        }
        public static IList<WorkOrder> GetWorkOrderByAccountByNumbers_Wh(Int32? account, Int32 numbers)
        {

            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    IList<HistoryList> _listhistory = new List<HistoryList>();
                    IList<tbl_PS_WorkOrder> _woListTemp = new List<tbl_PS_WorkOrder>();
                    if (account > 900000)
                        _woListTemp = _db.tbl_PS_WorkOrder.Where(t => t.ID == account).OrderByDescending(t => t.ID).Take(numbers).ToList();
                    else
                        _woListTemp = _db.tbl_PS_WorkOrder.Where(t => t.Account == account).OrderByDescending(t => t.ID).Take(numbers).ToList();

                    var _woList = (from p in _woListTemp
                                   select new
                                   {
                                       p.Account,
                                       p.Request_Date,
                                       p.Cancel_By,
                                       p.ID,
                                       p.Cancel_Date,
                                       p.Completed_Date,
                                       p.LastPrintDate,
                                       p.HoldFromShipping,
                                       p.HoldFromShippingReason,
                                       p.ID_PrimaryAssignedUser,
                                       Cancel_User = p.Cancel_User != null ? (int)p.Cancel_User : 0,
                                       p.DateMovedToUser,
                                       p.ConfirmationNumber,
                                       p.Cancel_Note,
                                       op1_LegalName = _db.tbl_Operator_Table.Where(op => op.ID == p.Cancel_User).Select(op => op.LegalName).Take(1).SingleOrDefault(),
                                       ops_LegalName = _db.tbl_Operator_Table.Where(op => op.ID == p.ID_PrimaryAssignedUser).Select(op => op.LegalName).Take(1).SingleOrDefault(),
                                       fullName = _db.tbl_Account_Member.Where(mem => mem.Account == p.Account && mem.Member == 1).Select(t => t.First_Name + " " + t.Last_Name).SingleOrDefault(),
                                       // lastName = _db.tbl_Account_Member.Where(mem => mem.Account == p.Account).Select(t => t.Last_Name).SingleOrDefault(),



                                   }).ToList();
                    //var _woList = (from wos in _woListTemp2.AsEnumerable()
                    //               join ops in _db.tbl_Operator_Table
                    //               on wos.ID_PrimaryAssignedUser equals ops.ID

                    //               from op1 in _db.tbl_Operator_Table.Where(op=>op.ID == wos.Cancel_User).DefaultIfEmpty()
                    //             //  from wr in _db.WorkOrdersReleaseds.Where(w => w.worID_WorkOrder == wos.ID).DefaultIfEmpty()
                    //                   //  from ups in _db.tbl_UPS_WorkOrders.Where(u=>u.ID_WorkOrder == wos.ID).DefaultIfEmpty()
                    //           //   where wos.Account == account
                    //               select new
                    //               {
                    //                   wos.Account,
                    //                   wos.Request_Date,
                    //                   wos.Cancel_By,
                    //                   wos.ID,
                    //                   wos.Cancel_Date,
                    //                   wos.Completed_Date,
                    //                   wos.LastPrintDate,
                    //                   wos.HoldFromShipping,
                    //                   wos.HoldFromShippingReason,
                    //                   wos.ID_PrimaryAssignedUser,
                    //                   ops_LegalName = ops.LegalName ,
                    //                   op1.LegalName,
                    //                   wos.DateMovedToUser,
                    //                   wos.ConfirmationNumber,
                    //                   wos.Cancel_Note,
                    //                //  ups_confirmation =  ups.ConfirmationNumber
                    //                   //wr.worReleasedBy
                    //               }).ToList();



                    var _list = _woList.Select(t => new WorkOrder

                    {
                        fullname = t.fullName,
                        Account = t.Account,
                        Request_Date = t.Request_Date.Value,
                        CreatedBy = t.ID_PrimaryAssignedUser.Equals(221) ? "Auto" : t.ops_LegalName,
                        CancelledBy = t.op1_LegalName != null ? t.op1_LegalName : t.Cancel_By,
                        WorkOrderID = t.ID,

                        Status = t.Cancel_Date != null ? "<strong><u>Cancelled:</u></strong> " + Environment.NewLine + t.Cancel_Note : t.Completed_Date != null ? "<strong>Completed</strong> " : t.LastPrintDate != null ? "<strong>Printed/Sent to oracle</strong> " :
                          (t.HoldFromShipping == 1 && t.HoldFromShippingReason == null) ? "<strong>Created</strong>" :
                           (t.HoldFromShipping == 1 && t.HoldFromShippingReason != null) ? " <strong><u>Holding:</u></strong>" + "~" + t.HoldFromShippingReason :
                        (t.HoldFromShipping == 1 && t.HoldFromShippingReason.Contains("%Back Order%")) ? "<strong><u>Back Ordered and Holding:</u></strong> " + Environment.NewLine + t.HoldFromShippingReason :
                        (t.HoldFromShipping == 1 && t.HoldFromShippingReason.Contains("Back Order ~")) ? "<strong>Back Ordered</strong>" : "<strong>Waiting to Interface</strong>",

                        // (t.HoldFromShipping == 1 && t.HoldFromShippingReason == null) ? "<strong>Created</strong>" : "<strong>Waiting to Interface</strong>",

                        ReleasedBy = string.Join("\n", _db.WorkOrdersReleased.Where(u => u.worID_WorkOrder == t.ID).Select(u => u.worReleasedBy).ToArray()),

                        TrackingNumbers = string.Join(", \n", _db.tbl_UPS_WorkOrders.Where(u => u.ID_WorkOrder == t.ID).Select(u => u.ConfirmationNumber).ToArray()),

                        Length = _db.tbl_UPS_WorkOrders.Where(u => u.ID_WorkOrder == t.ID).Select(u => u.ConfirmationNumber.Length).Take(1).SingleOrDefault(),



                        productDetails = (from wol in _db.tbl_PS_WorkOrderLine
                                          join pro in _db.tbl_Product_Table
                                          on wol.ID_Product equals pro.ID
                                          join uom in _db.tbl_Inv_UOM_Table
                                          on pro.ID_UOM equals uom.ID
                                          from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                              // from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode && w.linQty == wol.QtyOrdered).DefaultIfEmpty()


                                          where wol.ID_PS_WorkOrder == t.ID
                                          select new ProductDetails
                                          {
                                              Product = pro.ProductCode,
                                              Description = pro.ProductDescription,
                                              //ProductId= wol.ID_Product,
                                              Ordered = wol.QtyOrdered,
                                              Shipped = wol.QtyShipped,
                                              UOM = uom.UOMName + " of " + pro.PerUnitQty,
                                              UnitWeight = pro.UnitWeight,
                                              LineOrderQty = _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode && w.linQty == wol.QtyOrdered).Select(u => u.linQtyAvailable).Take(1).FirstOrDefault(),
                                              //  lin.linQtyAvailable,
                                              UnitPRice = wol.UnitPrice,
                                              lineId = wol.ID

                                          }).Distinct().ToList(),

                        historylist = _db.Database.SqlQuery<HistoryList>("SELECT " +
                                  "  DateMovedToUser AS Date, " +
                                 "   'Created' AS Process, " +
                                  "  1 as Sort " +
                            "    FROM tbl_ps_workorder wos " +
                              "  WHERE wos.id =  " + t.ID.ToString() +
                                "    AND DateMovedToUser IS NOT NULL " +
                           " UNION " +
                               " SELECT " +
                                   " ElrDateFinished, " +
                                   " 'Eligibility Checked', " +
                                  "  2 as Sort " +
                              "  FROM " +
                                  "  tbl_ps_workorder    wos join " +
                                          "  Intranet..Eligibility_orders    ord on wos.id = ord.ordWOId " +
                                  "  JOIN    Intranet..EligibilityRuns       elr ON elrID = ordElrID " +
                              "  WHERE " +
                                   " wos.Account = " + t.Account.ToString() +
                                      "  and wos.id = " + t.ID.ToString() +
                                   " AND elrDateFinished IS NOT NULL " +


                                  "  UNION " +
                               " SELECT " +
                                  "  worReleasedDate, " +
                                  "  'Released from Hold', " +
                                  "  3 AS Sort " +
                              "  FROM WorkOrdersReleased " +
                             "   WHERE worID_WorkOrder = " + t.ID.ToString() +
                                   " AND worReleasedDate IS NOT NULL  " +

                                  "  UNION " +
                              "  SELECT " +
                                   " LastPrintDate AS Date, " +
                                   " 'Printed in HDMS' AS Process, " +
                                   " 4 AS Sort" +
                              "  FROM tbl_ps_workorder wos " +
                              "  WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND LastPrintDate IS NOT NULL " +

                          "  UNION " +
                               " SELECT " +
                                   " LastPrintDate AS Date, " +
                                   " 'Printed in HDMS' AS Process, " +
                                   " 4 AS Sort " +
                               " FROM tbl_ps_workorder wos " +
                              "  WHERE wos.ID =  " + t.ID.ToString() +
                                  "  AND LastPrintDate IS NOT NULL " +
                          "  UNION " +
                               " SELECT " +
                                   " Cancel_date AS Date, " +
                                  "  'Canceled in HDMS' AS Process, " +
                                   " 5 AS Sort " +
                              "  FROM tbl_ps_workorder wos " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND Cancel_date IS NOT NULL " +

                                  "  UNION " +
                               " SELECT " +
                                   " Completed_date AS Date, " +
                                  "  'Completed in HDMS' AS Process, " +
                                   " 8 AS SOrt " +
                             "   FROM tbl_ps_workorder wos " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                  "  AND Completed_date IS NOT NULL " +
                           " UNION " +
                             "   SELECT woSent, " +
                               " 'Sent to Oracle', " +
                              "  7 AS Sort " +
                               " FROM ERP_OrdersSent erp join tbl_ps_workorder wos " +
                              "  on wos.id = erp.woWorkOrder " +
                               " WHERE wos.ID =  " + t.ID.ToString() +
                                   " AND woSent IS NOT NULL " +

                                      "  ORDER BY Date, Sort").ToList<HistoryList>(),



                    }).ToList();

                    foreach (var item in _list)
                    {
                        StringBuilder _str = new StringBuilder();
                        IList<HistoryList> _historyList = new List<HistoryList>();
                        _historyList = item.historylist;
                        foreach (var t in _historyList)
                        {
                            _str = _str.Append(t.Date.ToString());
                            _str = _str.Append(" - " + t.Process);
                            _str = _str.Append(Environment.NewLine);

                        }
                        item.History = _str.ToString();

                        IList<TrackingShippedList> _trackingList = new List<TrackingShippedList>();

                        item.TrackingNumbers = "";
                        if (1 == 1)
                        {
                            //   _trackingList = _db.Database.SqlQuery<TrackingList>("select ConfirmationNum from Reports..FedEx_Tracking_Tbl where WorkOrder = " + item.WorkOrderID).ToList();

                            try
                            {
                                // web api - tracking info
                                HttpClient client = new HttpClient();


                                client.BaseAddress = new Uri("http://JBMAZWeb01/TrackingShippedBy/");
                                //  client.BaseAddress = new Uri("http://localhost:61027/");

                                var result2 = client.GetAsync("api/Values/" + item.WorkOrderID).Result;

                                //   var ser = JsonConvert.SerializeObject(typeof(CoInsDetail)); 
                                string _value;
                                using (var stm1 = result2.Content.ReadAsStreamAsync())
                                {
                                    using (StreamReader reader = new StreamReader(stm1.Result))
                                    {
                                        TrackingShippedList trcking = new TrackingShippedList();
                                        _value = reader.ReadToEnd();

                                        _trackingList = JsonConvert.DeserializeObject<IList<TrackingShippedList>>(_value);

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
                        //------------------------------------
                        if (_trackingList.Count == 0 && item.TrackingNumbers == "")
                        {
                            string query = "select * from Reports.dbo.FedEx_Tracking_Tbl where WorkOrder = " + item.WorkOrderID + "";
                            _trackingList = _db.Database.SqlQuery<TrackingShippedList>(query).ToList();
                        }
                        //----------------------------------------

                        if (_trackingList.Count != 0)
                        {
                            foreach (var t in _trackingList)
                            {
                                item.Length = t.ConfirmationNum.Length;
                                item.TrackingNumbers = item.TrackingNumbers + t.ConfirmationNum + ", \n";
                                item.ShippedBy = t.FullName;

                            }

                            item.Cancel = 0; //no cancel button, order is already shipped
                        }
                        else if (item.TrackingNumbers != "")
                        {
                            item.Cancel = 0; //no cancel button, order is already shipped
                        }
                        else if (item.Status.Contains("Printed/Sent to oracle"))
                        {
                            item.Cancel = 2; //cancel in HDMS and oracle(send email)

                        }
                        else if (item.Status.Contains("Cancelled") || item.Status.Contains("Completed"))
                        {
                            item.Cancel = 0;// disabled cancel button
                        }
                        else

                        {
                            item.Cancel = 1; //enable cancel and delete in hdms
                        }

                        item.CancelReason = "Reason";
                    }
                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<WorkOrder>();
            }


        }

        public static bool CancelOrderHTMS(int wo, string reason)
        {
            bool cancel = false;
            var components = HttpContext.Current.User.Identity.Name.Split('\\');

            var userName = components.Last();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                var _wo = _db.tbl_PS_WorkOrder.Where(t => t.ID == wo && t.Cancel_Date == null).Take(1).SingleOrDefault();
                if (_wo != null)
                {
                    cancel = true;
                    _wo.Cancel_Date = DateTime.Now;
                    _wo.Cancel_By = userName;
                    _wo.Cancel_Note = reason;
                    _db.Entry(_wo).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            return cancel;
        }


        public static bool ReleaselOrderHDMS(int wo, string reason)
        {
            bool cancel = false;
            var components = HttpContext.Current.User.Identity.Name.Split('\\');
            string holdFromShippingReason = "";
            var userName = components.Last();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                WorkOrdersReleased _wr = new WorkOrdersReleased();


                var _wo = _db.tbl_PS_WorkOrder.Where(t => t.ID == wo && t.Cancel_Date == null).Take(1).SingleOrDefault();
                if (_wo != null)
                {
                    holdFromShippingReason = _wo.HoldFromShippingReason;

                    _wo.HoldFromShipping = 0;

                    _db.Entry(_wo).State = EntityState.Modified;
                    _db.SaveChanges();

                    //---- add record in WorkOrdersReleased table-----
                    _wr.worID_WorkOrder = wo;
                    _wr.worReleasedDate = DateTime.Now;
                    _wr.worReleasedBy = userName;
                    _wr.worHoldReasonWhenReleased = holdFromShippingReason;
                    _wr.worReleaseType = 1;

                    _db.WorkOrdersReleased.Add(_wr);
                    _db.SaveChanges();

                    //--- add record in eligibility_orders table
                    using (IntranetEntities _INdb = new IntranetEntities())
                    {
                        Eligibility_Orders _eo = new Eligibility_Orders();
                        var maxID = _INdb.Eligibility_Orders.OrderByDescending(t => t.ordELRid).Select(t => t.ordELRid).Take(1).SingleOrDefault();

                        _eo.ordELRid = maxID;
                        _eo.ordWOId = wo;
                        _eo.ordAdded = DateTime.Now;

                        _INdb.Eligibility_Orders.Add(_eo);
                        _INdb.SaveChanges();


                    }

                }


            }


            return cancel;
        }

        public static IList<ProductDetails> GetProductDetailsById(Int32 workOrderId)
        {

            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {



                    var _proList = (from wol in _db.tbl_PS_WorkOrderLine
                                    join pro in _db.tbl_Product_Table
                                    on wol.ID_Product equals pro.ID
                                    join uom in _db.tbl_Inv_UOM_Table
                                    on pro.ID_UOM equals uom.ID
                                    from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                    from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode).DefaultIfEmpty()

                                    select new
                                    {
                                        pro.ProductCode,
                                        pro.ProductDescription,
                                        wol.QtyOrdered,
                                        wol.QtyBackOrdered,
                                        wol.QtyShipped,
                                        lin.linQtyAvailable,
                                        uom.UOMName,
                                        pro.PerUnitQty,
                                        wol.ID_PS_WorkOrder

                                    }).Where(p => p.ID_PS_WorkOrder == workOrderId).Select(p => new ProductDetails

                                    {
                                        Product = p.ProductCode,
                                        Description = p.ProductDescription,
                                        Ordered = p.QtyOrdered,
                                        Shipped = p.QtyShipped,
                                        UOM = p.UOMName + " of " + p.PerUnitQty,

                                    }).ToList();




                    return _proList;


                }
            }
            catch (Exception)
            {
                return new List<ProductDetails>();
            }


        }


        public static void sendEmailtoShipping(int wo, string reason, string otherreason)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");

            string cancel_reason = string.Format("{0} {1}", reason, otherreason);

            //  mail.To.Add("medsurgcsreps@jandbmedical.com");
            mail.To.Add("ShippingTeam@jandbmedical.com");
            mail.To.Add("ekarrumi@jandbmedical.com");
            mail.Bcc.Add("grani@jandbmedical.com");

            mail.Subject = "Cancel Order Tool";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += @"<td style= ""color: red; font-size :20px; width:30 %""> Please cancel order in oracle <td> <td> </td>";
            mail.Body += "</tr> <tr> <td>   <td> <td> </td> </tr> <tr> <td>   <td> <td> </td> </tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Work Order is:  </td> " + wo + "<td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Reason is:  </td> " + cancel_reason + "<td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Note:  </td> <td> It might be possible that, you won't see this order in oracle immediately. Because it will show once it get printed at the back in warehouse. But as it is already cancelled in HDMs. You have to go and cancel in Oracle.</td>";
            mail.Body += "</tr> <tr> <td>   <td> <td> </td> </tr> <tr> <td>   <td> <td> </td> </tr>";


            mail.Body += "<tr>";
            mail.Body += "<td> </td> <td> If you have any question , Please contact to your Team lead. </td>";
            mail.Body += "</tr>  <tr> <td>   <td> <td> </td> </tr> <tr> <td>   <td> <td> </td> </tr>";
            mail.Body += "<tr> ";
            mail.Body += "<td>Thank You!</td><td></td>";
            mail.Body += "</tr>";


            mail.Body += "</table>";
            mail.Body += "</body>";
            mail.Body += "</html>";
            mail.IsBodyHtml = true;
            // SmtpServer.Port = 25;
            //  SmtpServer.Credentials = new System.Net.NetworkCredential("geeta.arora2006@gmail.com", "GEETUgeet1");
            //  SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

        public static void AddNoteForCancelOrder(woVM _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var _acc = (from tbl in _db.tbl_PS_WorkOrder where tbl.ID == _vm.ID select tbl.Account).Take(1).SingleOrDefault();

                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {

                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "PRODUCT").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_acc);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "PRODUCT";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 10;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "PRODUCT").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                   // Environment.UserName


                    }
                    if (_note != null)
                    {


                        string noteString = "Order= " + _vm.ID + " Cancel for  Account = ";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;

                        _tHist.NoteText = noteString + _acc + Environment.NewLine + "Reason:" + _vm.Reason;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);


                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }
    }
    public class WorkOrder
    {
        public string ShippedBy { get; set; }
        public string CancelReason { get; set; }
        public int? Cancel { get; set; }
        public int? Release { get; set; }
        public int? Length { get; set; }
        public int WorkOrderID { get; set; }

        public string fullname { get; set; }

        public int Numbers { get; set; }
        public int? Account { get; set; }

        public string CancelledBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? Request_Date { get; set; }

        public string ReleasedBy { get; set; }


        public string Status { get; set; }


        public string TrackingNumbers { get; set; }

        public List<string> TrackingNumbersList { get; set; }


        public string History { get; set; }

        public IList<HistoryList> historylist { get; set; }

        public IList<AccountNote> AccountNotes { get; set; }


        public IList<ProductDetails> productDetails { get; set; }

        public decimal Weight { get; set; }
        public string ActiveInsurance { get; set; }
    }

    public class woVM
    {
        public bool ReleaseToolAccess { get; set; }
        public bool IsAccess { get; set; }
        public int Numbers { get; set; }
        public int? Account { get; set; }
        public int ID { get; set; }

        public string Reason { get; set; }


        public string OtherReason { get; set; }

        public int? CancelFlag { get; set; }
        public IList<WorkOrder> workOrder { get; set; }
    }


    public class TrackingList
    {

        public string ConfirmationNum { get; set; }

        public string FullName { get; set; }

    }

    public class TrackingShippedList
    {
        public string FullName { get; set; }
        public string ConfirmationNum { get; set; }



    }

    public class HistoryList
    {
        public DateTime? Date { get; set; }
        public string Process { get; set; }
        public int Sort { get; set; }
    }

    public class AccountNote
    {
        public int NoteId { get; set; }
        public int NoteHistoryId { get; set; }
        public string NoteHeading { get; set; }
        public DateTime? NoteDate { get; set; }
        public string NoteText { get; set; }
        public string NoteCreatedBy { get; set; }
    }

    public class RMAProduct
    {
        public int OrderNumber { get; set; }
        public int ReturnOrderNumber { get; set; }
        public string ProductCode { get; set; }
    }

    public class ProductDetails
    {
        public bool isLineRMAdone { get; set; }
        public int lineId { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        //public int? ProductId { get; set; }
        public decimal? UnitPRice { get; set; }
        public double? UnitWeight { get; set; }

        public double? TotalWeight { get; set; }

        public DateTime? CreateDate { get; set; }
        public string UOM { get; set; }

        public int? Ordered { get; set; }
        public int? Shipped { get; set; }
        public int? LineOrderQty { get; set; }
    }


    public class InactiveAccuntVM
    {
        public int? Account { get; set; }

    }

    public class CancelOrderVM
    {
        public int wo { get; set; }
        public int? cancelFlag { get; set; }
    }


    public class InactiveAccountReport
    {
        public static IList<InactiveAccuntVM> GetInactiveAccount()
        {
            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from rwo in _db.tbl_PS_RepeatingOrders
                                 join inf in _db.tbl_Account_Information
                                 on rwo.Account equals inf.Account
                                 select new
                                 {
                                     rwo.Account,
                                     inf.InActiveAccount
                                 }).Where(t => t.InActiveAccount == 1).Select(a => new InactiveAccuntVM
                                 {
                                     Account = a.Account
                                 }).Distinct().ToList();



                    return _list;
                }
            }
            catch (Exception)
            {
                return new List<InactiveAccuntVM>();
            }


        }



        public static IList<InactiveAccuntVM> GetRONotPurchased()
        {
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from ro in _db.tbl_PS_RepeatingOrders
                                 join op in _db.tbl_Operator_Table
                                 on ro.ID_CreateBy equals op.ID
                                 join op2 in _db.tbl_Operator_Table
                                 on ro.ID_Changed equals op2.ID
                                 where ro.WorkOrderType != 1
                                 select new InactiveAccuntVM
                                 {
                                     Account = ro.Account
                                 }).ToList();

                    return _list;
                }
            }
            catch (Exception)
            {
                return new List<InactiveAccuntVM>();
            }


        }
    }



}