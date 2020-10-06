using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using USPS_Report.Models;
using USPS_Report.Areas.Reports.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReturnItems
    {
        public class ReturnItemsVM
        {
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Account field is only intergers.")]
            public string Account { get; set; }
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Workorder field is only intergers.")]
            public string WorkOrder { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }

            public string State { get; set; }

            public string ZipCode { get; set; }

            public IList<ReturnItemsData> Details { get; set; }

        }

        public class HistoryItemsVM
        {
            [RegularExpression("^([0-9]+)$", ErrorMessage = "The Account field is only intergers.")]
            public string SearchString { get; set; }
            public string SearchType { get; set; }

            public IList<HistoryItemsData> Details { get; set; }

        }

        public class ChooseAccountVM
        {
            public string SelectedAccount { get; set; }
            public IList<AccountData> AccountDetails { get; set; }
        }

        public class AccountData
        {
            public string Account { get; set; }
            public string FirstName { get; set; }
            public string Middle { get; set; }
            public string LastName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Reference { get; set; }
            public string Phone { get; set; }
            public int Member { get; set; }
        }
        public class ReturnItemsData
        {
            public int ReturnId { get; set; }
            public int? RMAnum { get; set; }
            public int? woID { get; set; }
            public int? AccountNum { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string PhNum { get; set; }
            public string TagType { get; set; }
            public DateTime? RequestDate { get; set; }
            public DateTime? ScheduledFor { get; set; }

        }
        public class HistoryItemsData
        {
            public int ReturnId { get; set; }
            public int? RMAnum { get; set; }
            public int? AccountNum { get; set; }
            public int? WorkOrder_ID { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string FullNAme
            {
                get
                {
                    if (this.First_Name != null && this.Last_Name != null)
                    {
                        return this.First_Name + " " + this.Last_Name;
                    }
                    else if (this.First_Name != null && this.Last_Name == null)
                    {
                        return this.First_Name;
                    }
                    else if (this.First_Name == null && this.Last_Name != null)
                    {
                        return this.Last_Name;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            public string PhNum { get; set; }
            public string TagType { get; set; }
            public DateTime? RequestDate { get; set; }
            public string Return_Note { get; set; }
            public string Tracking_Number { get; set; }
            public DateTime? Date_Returned { get; set; }
        }
        public class WorkOrderProduct
        {
            public int WorkOrder_ID { get; set; }
            public int? Account { get; set; }
            public DateTime? RequestDate { get; set; }
            public string ProductCode { get; set; }
            public string ProductDescription { get; set; }
            public int? QtyShipped { get; set; }
            public int? ID_DeliveryLocation { get; set; }
            public int WorkOrder_Line { get; set; }
        }
        public class WorkOrder
        {
            public string WorkOrder_Number { get; set; }
            public string Account { get; set; }
            public DateTime? Request_Date { get; set; }
            public IList<WorkOrderProduct> WorkOrderProducts { get; set; }
        }
        public class ChooseWorkOrderVM
        {
            public IList<WorkOrder> WorkOrders { get; set; }
        }
        public class WorkOrderDB
        {
            public int WorkOrder_ID { get; set; }
            public int? Account { get; set; }
            public DateTime? RequestDate { get; set; }
            public string ProductCode { get; set; }
            public string ProductDescription { get; set; }
            public int? QtyShipped { get; set; }
            public int? ID_DeliveryLocation { get; set; }
            public int WorkOrder_Line { get; set; }

        }

        public class WorkOrderItem
        {
            public bool Need { get; set; }
            public int? WorkOrder_ID { get; set; }
            public int? Account { get; set; }
            public DateTime? RequestDate { get; set; }
            public string ProductCode { get; set; }
            public int? ID_DeliveryLocation { get; set; }
            public string ProductDescription { get; set; }
            public int? QtyShipped { get; set; }
            public int? QtyReturned { get; set; }
            public int WorkOrder_Line { get; set; }

        }
        public class ReturnReason
        {
            public int ReturnReasonId { get; set; }
            public string ReturnReasonText { get; set; }
        }
        public class ReturnItem
        {
            public int Return_ID { get; set; }
            public int? Account { get; set; }
            public int? Account_Member { get; set; }
            public string Reshipped { get; set; }
            public string Tag_Type { get; set; }
            public int? OracleRMA { get; set; }
            public string Return_Note { get; set; }            
            public int? Reason__List_Option_ID { get; set; }
            public string Return_Other_Reason { get; set; }
            public int? WorkOrder_ID { get; set; }
            public DateTime? Date_Returned { get; set; }
            public string Tracking_Number { get; set; }
            public short? Send_To_Billing { get; set; }
            public short? Dont_Display { get; set; }
            public int? Boxes_Returned { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? RequestDate { get; set; }
            public DateTime? ScheduledFor { get; set; }
        }
        public class ReturnItemsInfoVM
        {
            public int Return_ID { get; set; }
            public int? Account { get; set; }
            public int? Account_Member { get; set; }
            public string Reshipped { get; set; }
            public string Tag_Type { get; set; }
            public int? OracleRMA { get; set; }
            public string Return_Note { get; set; }
            public SelectList ReturnReasonList { get; set; }
            [Required(ErrorMessage = "Please choose a reason for this return from the drop down list.")]
            public int? Reason__List_Option_ID { get; set; }
            public string Return_Other_Reason { get; set; }
            public int? WorkOrder_ID { get; set; }
            public DateTime? Date_Returned { get; set; }
            public string Tracking_Number { get; set; }
            public bool Send_To_Billing { get; set; }
            public bool Dont_Display { get; set; }
            public int? Boxes_Returned { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? RequestDate { get; set; }
            public DateTime? ScheduledFor { get; set; }
            public string ProductCode
            {
                get
                {
                    string result = "";
                    if (this.WorkOrderItems.Count()>0)
                    {
                        foreach (var item in this.WorkOrderItems)
                        {
                            result += item.ProductCode + "   ";
                        }
                    }
                    return result;
                }
            }
            public IList<WorkOrderItem> WorkOrderItems { get; set; }
        }

        public class ReturnLineItem
        {
            public int ReturnLineID { get; set; }
            public int? ReturnID { get; set; }
            public int? WorkOrderLineID { get; set; }
            public int? QtyReturn { get; set; }
        }
        public class CreateReturnVM
        {
            public bool DoesReturnExist { get; set; }
            public string WorkOrder_ID { get; set; }
        }
        public static IList<HistoryItemsData> GetReturnHistoryItemsData()
        {
            List<HistoryItemsData> lstReturnItesData = new List<HistoryItemsData>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstReturnItesData = (from item in _db.Database.SqlQuery<sp_GetHistoryReturnItemsData_Result>("exec sp_GetHistoryReturnItemsData").ToList<sp_GetHistoryReturnItemsData_Result>()
                                         select new HistoryItemsData
                                         {
                                             ReturnId = item.Return_ID,
                                             RMAnum = item.OracleRMA,
                                             AccountNum = item.Account,
                                             First_Name = item.first_name,
                                             Last_Name = item.last_name,
                                             PhNum = item.phone,
                                             TagType = item.Tag_Type,
                                             Return_Note = item.Return_Note,
                                             Date_Returned = item.Date_Returned,
                                             Tracking_Number = item.Tracking_Number,
                                             WorkOrder_ID = item.WorkOrder_ID,
                                             RequestDate = item.Request_Date
                                         }
                               ).ToList();
                }
                DateTime dt = DateTime.Today.AddMonths(-12);
                lstReturnItesData = lstReturnItesData.Where(x => x.Date_Returned > dt).ToList();
                return lstReturnItesData;
            }
            catch (Exception ex)
            {
                throw;
                //return new List<ReturnItemsData>();
            }
        }
        public static IList<ReturnItemsData> GetReturnItemsData()
        {
            List<ReturnItemsData> lstReturnItesData = new List<ReturnItemsData>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstReturnItesData = (from item in _db.Database.SqlQuery<sp_GetReturnItemsData_Result>("exec sp_GetReturnItemsData").ToList<sp_GetReturnItemsData_Result>()
                                         select new ReturnItemsData
                                         {
                                             ReturnId = item.Return_ID,
                                             RMAnum = item.OracleRMA,
                                             AccountNum = item.Account,
                                             First_Name = item.first_name,
                                             Last_Name = item.last_name,
                                             PhNum = item.phone,
                                             TagType = item.Tag_Type,
                                             RequestDate = item.Request_Date,
                                             ScheduledFor = item.PickUpDate,
                                             woID = item.ID
                                         }
                               ).ToList();
                }
                return lstReturnItesData;
            }
            catch (Exception ex)
            {
                throw;
                //return new List<ReturnItemsData>();
            }
        }

        public static IList<AccountData> GetSearchAccounts(ReturnItemsVM returnItemsVM)
        {
            List<AccountData> lstAccounts = new List<AccountData>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                if (!string.IsNullOrEmpty(returnItemsVM.Account))
                {
                    lstAccounts = (from item in _db.Database.SqlQuery<sp_GetSearchAccounts_Result>("exec sp_GetSearchAccounts @account,@firstName,@lastName,@address1,@address2,@city,@state,@zipCode", new SqlParameter("account", Convert.ToInt32(returnItemsVM.Account)), new SqlParameter("firstName", returnItemsVM.FirstName.GetDBNullOrValue()), new SqlParameter("lastName", returnItemsVM.LastName.GetDBNullOrValue()), new SqlParameter("address1", returnItemsVM.Address1.GetDBNullOrValue()), new SqlParameter("address2", returnItemsVM.Address2.GetDBNullOrValue()), new SqlParameter("city", returnItemsVM.City.GetDBNullOrValue()), new SqlParameter("state", returnItemsVM.State.GetDBNullOrValue()), new SqlParameter("zipCode", returnItemsVM.ZipCode.GetDBNullOrValue())).ToList<sp_GetSearchAccounts_Result>()
                                       //lstAccounts = (from item in _db.sp_GetSearchAccounts(Convert.ToInt32(returnItemsVM.Account), returnItemsVM.FirstName, returnItemsVM.LastName, returnItemsVM.Address1, returnItemsVM.Address2, returnItemsVM.City, returnItemsVM.State, returnItemsVM.ZipCode)
                                   select new AccountData
                                   {
                                       Account = item.account.ToString(),
                                       LastName = item.last_name,
                                       FirstName = item.first_name,
                                       Middle = item.middle,
                                       Address1 = item.address_1,
                                       Address2 = item.address_2,
                                       City = item.city,
                                       State = item.state,
                                       ZipCode = item.zip,
                                       Phone = item.phone
                                   }
                           ).ToList();
                }
                else if(!string.IsNullOrEmpty(returnItemsVM.WorkOrder))
                {

                }
           }
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                
            }
            return lstAccounts;
        }
        public static IList<WorkOrder> GetWorkOrders(ReturnItemsVM returnItemsVM)
        {
            List<WorkOrderDB> lstWorkOrdersDB = new List<WorkOrderDB>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                if (!string.IsNullOrEmpty(returnItemsVM.Account))
                {
                    lstWorkOrdersDB = (from item in _db.Database.SqlQuery<sp_GetWorkOrders_Result>("exec sp_GetWorkOrders @account", new SqlParameter("account", Convert.ToInt32(returnItemsVM.Account))).ToList<sp_GetWorkOrders_Result>()
                                           //lstWorkOrdersDB = (from item in _db.sp_GetWorkOrders(Convert.ToInt32(selectedAccount))
                                       select new WorkOrderDB
                                       {
                                           WorkOrder_ID = item.WorkOrder_Number.Value,
                                           Account = item.account,
                                           RequestDate = item.Request_Date,
                                           ProductCode = item.ProductCode,
                                           ProductDescription = item.ProductDescription,
                                           QtyShipped = item.qtyshipped,
                                           ID_DeliveryLocation = item.ID_DeliveryLocation,
                                           WorkOrder_Line = item.WorkOrder_Line
                                       }
                                   ).ToList();
                    DateTime dt = DateTime.Today.AddMonths(-6);
                    lstWorkOrdersDB = lstWorkOrdersDB.Where(x => x.RequestDate >= dt).ToList();
                }
                if (!string.IsNullOrEmpty(returnItemsVM.WorkOrder))
                {
                    lstWorkOrdersDB = (from item in _db.Database.SqlQuery<sp_GetWorkOrders_Result>("exec sp_GetWorkOrdersBByWo @WO", new SqlParameter("WO", Convert.ToInt32(returnItemsVM.WorkOrder))).ToList<sp_GetWorkOrders_Result>()
                                           //lstWorkOrdersDB = (from item in _db.sp_GetWorkOrders(Convert.ToInt32(selectedAccount))
                                       select new WorkOrderDB
                                       {
                                           WorkOrder_ID = item.WorkOrder_Number.Value,
                                           Account = item.account,
                                           RequestDate = item.Request_Date,
                                           ProductCode = item.ProductCode,
                                           ProductDescription = item.ProductDescription,
                                           QtyShipped = item.qtyshipped,
                                           ID_DeliveryLocation = item.ID_DeliveryLocation,
                                           WorkOrder_Line = item.WorkOrder_Line
                                       }
                                   ).ToList();
                }
                
            }
            List<WorkOrder> lstWorkOrders = new List<WorkOrder>();            
            var distinctWOs = lstWorkOrdersDB.Select(m => new { m.WorkOrder_ID, m.RequestDate,m.Account }).Distinct().ToList();
            foreach (var item in distinctWOs)
            {
                WorkOrder wo = new WorkOrder();
                wo.WorkOrder_Number = item.WorkOrder_ID.ToString();
                wo.Account = item.Account.ToString();
                wo.Request_Date = item.RequestDate;
                wo.WorkOrderProducts = lstWorkOrdersDB.Where(i => i.WorkOrder_ID == item.WorkOrder_ID && i.RequestDate == item.RequestDate)
                    .Select(t => 
                    new WorkOrderProduct {
                        ProductCode = t.ProductCode,
                        ProductDescription = t.ProductDescription,
                        QtyShipped = t.QtyShipped,
                        WorkOrder_ID= t.WorkOrder_ID,
                        Account= t.Account,
                        RequestDate=t.RequestDate,
                        ID_DeliveryLocation=t.ID_DeliveryLocation,
                        WorkOrder_Line=t.WorkOrder_Line
                    }).ToList();
                lstWorkOrders.Add(wo);
            }            
            return lstWorkOrders;
        }

        public static IList<ReturnReason> GetReturnReasonList()
        {
            List<ReturnReason> lstReturnReason = new List<ReturnReason>();
            try
            {
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstReturnReason = (from item in _db.Database.SqlQuery<sp_GetReturnReasonList_Result>("exec sp_GetReturnReasonList").ToList<sp_GetReturnReasonList_Result>()
                                         select new ReturnReason
                                         {
                                             ReturnReasonId = item.List_Option_ID,
                                             ReturnReasonText = item.List_Option_text                                             
                                         }
                               ).ToList();
                }
                return lstReturnReason;
            }
            catch (Exception ex)
            {
                throw;
                //return new List<ReturnReason>();
            }
        }

        public static IList<ReturnLineItem> GetReturnLineItemsList(string returnId)
        {
            try
            {
                List<ReturnLineItem> lstReturnLineItems = new List<ReturnLineItem>();
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    lstReturnLineItems = (from item in _db.Database.SqlQuery < sp_GetReturnLineItems_Result >("exec sp_GetReturnLineItems @returnId", new SqlParameter("returnId", Convert.ToInt32(returnId))).ToList<sp_GetReturnLineItems_Result>()
                                              //lstReturnLineItems = (from item in _db.sp_GetReturnLineItems(Convert.ToInt32(returnId))
                                          select new ReturnLineItem
                                          {
                                              ReturnLineID = item.Return_Line_ID,
                                              ReturnID = item.Return_ID,
                                              WorkOrderLineID = item.WorkOrder_Line_ID,
                                              QtyReturn = item.Qty_Return
                                          }
                                       ).ToList();
                }
                return lstReturnLineItems;
            }
            catch (Exception ex)
            {
                throw;
                //return new List<ReturnLineItem>();
            }

        }
        public static ReturnItem GetReturnItem(string returnId)
        {
            try
            {
                ReturnItem returnItem = new ReturnItem();
                using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
                {
                    returnItem = (from item in _db.Database.SqlQuery<sp_GetReturnItem_Result>("exec sp_GetReturnItem @returnId", new SqlParameter("returnId", Convert.ToInt32(returnId))).ToList<sp_GetReturnItem_Result>()
                    //returnItem = (from item in _db.sp_GetReturnItem(Convert.ToInt32(returnId))
                                          select new ReturnItem
                                          {
                                              Return_ID = item.Return_ID,
                                              Account = item.Account,
                                              Account_Member = item.Account_Member,
                                              WorkOrder_ID = item.WorkOrder_ID,
                                              Reshipped = item.Reshipped,
                                              Date_Returned = item.Date_Returned,
                                              Tag_Type = item.Tag_Type,
                                              Return_Note = item.Return_Note,
                                              Reason__List_Option_ID = item.Reason__List_Option_ID,
                                              Return_Other_Reason = item.Return_Other_Reason,
                                              Tracking_Number = item.Tracking_Number,
                                              Send_To_Billing = item.Send_To_Billing,
                                              Dont_Display = item.Dont_Display,
                                              Boxes_Returned = item.Boxes_Returned,
                                              OracleRMA = item.OracleRMA,
                                              FirstName = item.first_name,
                                              LastName = item.last_name,
                                              RequestDate = item.Request_Date,
                                              ScheduledFor = item.PickUpDate
                                          }
                                       ).FirstOrDefault();
                }
                return returnItem;
            }
            catch (Exception ex)
            {
                throw;
                //return new ReturnItem();
            }

        }

        public static WorkOrder GetWorkOrderInfo(int account,int workOrder_ID)
        {
            List<WorkOrderDB> lstWorkOrdersDB = new List<WorkOrderDB>();
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                lstWorkOrdersDB = (from item in _db.Database.SqlQuery < sp_GetWorkOrderInfo_Result >("exec sp_GetWorkOrderInfo @account,@workOrder_ID", new SqlParameter("account", account.GetDBNullOrValue()), new SqlParameter("workOrder_ID", workOrder_ID.GetDBNullOrValue())).ToList< sp_GetWorkOrderInfo_Result >()
                                       //lstWorkOrdersDB = (from item in _db.sp_GetWorkOrderInfo(account,workOrder_ID)
                                   select new WorkOrderDB
                                   {
                                       WorkOrder_ID = item.WorkOrder_Number.Value,
                                       Account = item.account,
                                       RequestDate = item.Request_Date,
                                       ProductCode = item.ProductCode,
                                       ProductDescription = item.ProductDescription,
                                       QtyShipped = item.qtyshipped,
                                       ID_DeliveryLocation = item.ID_DeliveryLocation,
                                       WorkOrder_Line = item.WorkOrder_Line
                                   }
                                   ).ToList();
            }
            List<WorkOrder> lstWorkOrders = new List<WorkOrder>();
            var distinctWOs = lstWorkOrdersDB.Select(m => new { m.WorkOrder_ID, m.RequestDate, m.Account }).Distinct().ToList();
            foreach (var item in distinctWOs)
            {
                WorkOrder wo = new WorkOrder();
                wo.WorkOrder_Number = item.WorkOrder_ID.ToString();
                wo.Account = item.Account.ToString();
                wo.Request_Date = item.RequestDate;
                wo.WorkOrderProducts = lstWorkOrdersDB.Where(i => i.WorkOrder_ID == item.WorkOrder_ID && i.RequestDate == item.RequestDate)
                    .Select(t =>
                    new WorkOrderProduct
                    {
                        ProductCode = t.ProductCode,
                        ProductDescription = t.ProductDescription,
                        QtyShipped = t.QtyShipped,
                        WorkOrder_ID = t.WorkOrder_ID,
                        Account = t.Account,
                        RequestDate = t.RequestDate,
                        ID_DeliveryLocation = t.ID_DeliveryLocation,
                        WorkOrder_Line = t.WorkOrder_Line
                    }).ToList();
                lstWorkOrders.Add(wo);
            }
            return lstWorkOrders.FirstOrDefault();
        }
        public static void UpdateReturn(int return_ID,string reshipped,string tag_Type,int? oracleRMA,string return_Note,int? reason__List_Option_ID,string return_Other_Reason,DateTime? date_Returned,string tracking_Number,short? send_To_Billing,short? dont_Display,int? boxes_Returned, DateTime? ScheduledFor)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_UpdateReturn @return_ID,@reshipped,@tracking_Number,@return_Note,@tag_Type,@reason__List_option_ID,@return_Other_Reason,@boxesReturned,@dateRtrn,@send_To_Billing,@dont_Display,@oracleRMA,@PickUpDate",
                    new SqlParameter("return_ID", return_ID), new SqlParameter("reshipped", reshipped.GetDBNullOrValue()) , new SqlParameter("tracking_Number", tracking_Number.GetDBNullOrValue()) , new SqlParameter("return_Note", return_Note.GetDBNullOrValue()) ,
                    new SqlParameter("tag_Type", tag_Type.GetDBNullOrValue()) , new SqlParameter("reason__List_option_ID", reason__List_Option_ID.GetDBNullOrValue()) , new SqlParameter("return_Other_Reason", return_Other_Reason.GetDBNullOrValue()) ,
                    new SqlParameter("boxesReturned", boxes_Returned.GetDBNullOrValue()) , new SqlParameter("dateRtrn", date_Returned.GetDBNullOrValue()) , new SqlParameter("send_To_Billing", send_To_Billing.GetDBNullOrValue()) ,
                    new SqlParameter("dont_Display", dont_Display.GetDBNullOrValue()) , new SqlParameter("oracleRMA", oracleRMA.GetDBNullOrValue()), new SqlParameter("PickUpDate", ScheduledFor.GetDBNullOrValue()));
                //_db.sp_UpdateReturn(return_ID, reshipped, tracking_Number, return_Note, tag_Type, reason__List_Option_ID, return_Other_Reason, boxes_Returned, date_Returned, send_To_Billing, dont_Display, oracleRMA);
            }
               
        }
        public static void InsertReturn(int? account, int? workOrder_ID, string reshipped, string tag_Type, int? oracleRMA, string return_Note, int? reason__List_Option_ID, string return_Other_Reason, DateTime? date_Returned, string tracking_Number, short? send_To_Billing, short? dont_Display, int? boxes_Returned, DateTime? ScheduledFor)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_InsertReturn @account,@workOrder_ID,@reshipped,@tracking_Number,@return_Note,@tag_Type,@reason__List_option_ID,@return_Other_Reason,@boxesReturned,@dateRtrn,@send_To_Billing,@dont_Display,@oracleRMA,@PickUpDate", 
                    new SqlParameter("account", account.GetDBNullOrValue()) , new SqlParameter("workOrder_ID", workOrder_ID.GetDBNullOrValue()) , new SqlParameter("reshipped", reshipped.GetDBNullOrValue()) , new SqlParameter("tracking_Number", tracking_Number.GetDBNullOrValue()) , 
                    new SqlParameter("return_Note", return_Note.GetDBNullOrValue()) , new SqlParameter("tag_Type", tag_Type.GetDBNullOrValue()) , new SqlParameter("reason__List_option_ID", reason__List_Option_ID.GetDBNullOrValue()) , 
                    new SqlParameter("return_Other_Reason", return_Other_Reason.GetDBNullOrValue()) , new SqlParameter("boxesReturned", boxes_Returned.GetDBNullOrValue()) , new SqlParameter("dateRtrn", date_Returned.GetDBNullOrValue()) , new SqlParameter("send_To_Billing", send_To_Billing.GetDBNullOrValue()) , new SqlParameter("dont_Display", dont_Display.GetDBNullOrValue()) , new SqlParameter("oracleRMA", oracleRMA.GetDBNullOrValue()), new SqlParameter("PickUpDate", ScheduledFor.GetDBNullOrValue()));
                //_db.sp_InsertReturn(account,workOrder_ID,reshipped, tracking_Number, return_Note, tag_Type, reason__List_Option_ID, return_Other_Reason, boxes_Returned, date_Returned, send_To_Billing, dont_Display, oracleRMA);
            }

        }
        public static bool DoesReturnExist(int workOrder_ID)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                var res = _db.Database.SqlQuery<int>("exec sp_DoesReturnExist @workOrder_ID", new SqlParameter("workOrder_ID", workOrder_ID)).ToList<int>();               
                //var res=_db.sp_DoesReturnExist(workOrder_ID).Select(t=>t).ToList();
                return res.Count() > 0 ? true : false;
            }
        }
        public static int? GetLatestReturnID(int workOrder_ID)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                var res = _db.Database.SqlQuery<int>("exec sp_DoesReturnExist @workOrder_ID", new SqlParameter("workOrder_ID", workOrder_ID)).ToList<int>();
                //var res = _db.sp_DoesReturnExist(workOrder_ID).Select(t => t).ToList();
                return res.FirstOrDefault();
            }
        }
        public static int? DoesReturnLineExist(int return_ID,int workOrder_Line_ID)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                return _db.Database.SqlQuery<int>("exec sp_DoesReturnLineExist @return_ID,@workOrder_Line_ID", new SqlParameter("return_ID", return_ID), new SqlParameter("workOrder_Line_ID", workOrder_Line_ID)).FirstOrDefault();
                //return _db.sp_DoesReturnLineExist(Return_ID, WorkOrder_Line_ID).Select(t => t).FirstOrDefault();                
            }
        }
        public static void UpdateReturnLine(int return_Line_ID,int qty_Return)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_UpdateReturn_Line @return_Line_ID,@qty_Return", new SqlParameter("return_Line_ID", return_Line_ID), new SqlParameter("qty_Return", qty_Return) );
                //_db.sp_UpdateReturn_Line(return_Line_ID,qty_Return);
            }

        }
        public static void InsertReturnLine(int return_ID, int workOrder_Line_ID, int qty_Return)
        {
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_InsertReturn_Line @return_ID,@workOrder_Line_ID,@qty_Return", new SqlParameter("return_ID", return_ID) , new SqlParameter("workOrder_Line_ID", workOrder_Line_ID) , new SqlParameter("qty_Return", qty_Return) );
                //_db.sp_InsertReturn_Line(return_ID, workOrder_Line_ID,qty_Return);
            }

        }
        public static void InsertAccountNote(int account, string userName, int workOrder_ID,string note,int reason__List_Option_ID,string return_Other_Reason,string tracking_Number,int? OracleRMA, DateTime? DateReturned,string ProductCode)
        {
            string DateReturnedString = Convert.ToDateTime(DateReturned).ToShortDateString();
            string OracleRMAString = OracleRMA.ToString();
            string workOrder_IDstring = workOrder_ID.ToString();
            string NoteText = "Return for workorder " + workOrder_IDstring + " modified by web user. ";
            if (!string.IsNullOrEmpty(ProductCode))
            {
                NoteText += Environment.NewLine + "Returned Products: " + ProductCode.Trim() + ", ";
            }
            if (!string.IsNullOrEmpty(OracleRMAString))
            {
                NoteText += Environment.NewLine+ "Oracle RMA#: "+OracleRMAString.Trim()+", ";
            }
            if (!string.IsNullOrEmpty(note))
            {
                NoteText += Environment.NewLine + "Notes about Return: " + note + ", ";
            }
            if (!string.IsNullOrEmpty(return_Other_Reason))
            {
                NoteText += Environment.NewLine + "Other Reason: " + return_Other_Reason + ", ";
            }
            if (!string.IsNullOrEmpty(tracking_Number))
            {
                NoteText += Environment.NewLine + "Call Tag #s: " + tracking_Number + ", ";
            }
            if (!string.IsNullOrEmpty(DateReturnedString))
            {
                NoteText += Environment.NewLine + "Return Date: " + DateReturnedString + ""+ Environment.NewLine ;
            }
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_InsertAccountNote @account,@userName,@workOrder_ID,@note,@reason__List_Option_ID,@return_Other_Reason,@tracking_Number,@OracleRMA,@NoteText", new SqlParameter("account", account.GetDBNullOrValue()), new SqlParameter("userName", userName.GetDBNullOrValue()), new SqlParameter("workOrder_ID", workOrder_ID.GetDBNullOrValue()), new SqlParameter("note", note.GetDBNullOrValue()), new SqlParameter("reason__List_Option_ID", reason__List_Option_ID.GetDBNullOrValue()), new SqlParameter("return_Other_Reason", return_Other_Reason.GetDBNullOrValue()), new SqlParameter("tracking_Number", tracking_Number.GetDBNullOrValue()), new SqlParameter("OracleRMA", OracleRMAString.GetDBNullOrValue()), new SqlParameter("NoteText", NoteText.GetDBNullOrValue()));

                //                _db.Database.ExecuteSqlCommand(
                //@"if(@note is null)
                //begin
                //set @note = ' '
                //end
                //if (@return_Other_Reason is null)
                //                    begin
                //                    set @return_Other_Reason = ' '
                //end
                //if (@tracking_Number is null)
                //                    begin
                //                    set @tracking_Number = ' '
                //end
                //declare @returnReasonText varchar(100)= null
                //SELECT top 1 @returnReasonText = List_Option_text FROM HHSQLDB_Offshore.dbo.List_Option WHERE List_Option_ID = @reason__List_Option_ID
                //if (@returnReasonText is null)
                //                    begin
                //                    set @returnReasonText = ' '
                //end
                //declare @OperatorID int
                //select top 1 @OperatorID = id from HHSQLDB_Offshore.dbo.tbl_Operator_Table where OperatorName = @userName and DeletedDate is null and InactiveDate is null
                //declare @count int
                //SELECT @count = count(ID) FROM HHSQLDB_Offshore.dbo.tbl_Account_Note WHERE account = @account AND NoteHeading = 'PRODUCT RETURNS'
                //if (@count = 0)
                //                    begin
                //                    INSERT HHSQLDB_Offshore.dbo.tbl_Account_Note(Account, Member, NoteHeading, NoteCreateDate, NoteCreatedBy)
                //VALUES(@account, 1, 'PRODUCT RETURNS', getdate(), @OperatorID)
                //end
                //declare @ID int
                //SELECT top 1 @ID = ID  FROM HHSQLDB_Offshore.dbo.tbl_Account_Note WHERE account = @account AND NoteHeading = 'PRODUCT RETURNS'
                //if (@ID is not null)
                //begin
                //INSERT HHSQLDB_Offshore.dbo.tbl_Account_Note_History(ID_Note, NoteDate, NoteText, ID_Operator)
                //VALUES(@ID, getdate(), 'Return for workorder ' + cast(@workOrder_ID as varchar(10)) + ' modified by web user. Reason: ' + @returnReasonText + ' Other Reason: ' + RTRIM(LTRIM(@return_Other_Reason)) + ' Notes: ' + @note + ' Call Tag #s: ' + @tracking_Number, @OperatorID)
                //end", new SqlParameter("account", account.GetDBNullOrValue()), new SqlParameter("userName", userName.GetDBNullOrValue()), new SqlParameter("workOrder_ID", workOrder_ID.GetDBNullOrValue()), new SqlParameter("note", note.GetDBNullOrValue()), new SqlParameter("reason__List_Option_ID", reason__List_Option_ID.GetDBNullOrValue()), new SqlParameter("return_Other_Reason", return_Other_Reason.GetDBNullOrValue()), new SqlParameter("tracking_Number", tracking_Number.GetDBNullOrValue()));


                //_db.sp_InsertAccountNote(account, userName, workOrder_ID,note,reason__List_Option_ID,return_Other_Reason,tracking_Number);
            }

        }

        public static void InsertAccountNoteCallTag(int account, string userName, int workOrder_ID, string note, int reason__List_Option_ID, string return_Other_Reason, string tracking_Number, int? OracleRMA,DateTime? ScheduledFor,string ProductCode)
        {
           
            string ScheduledForString = "";
            if (ScheduledFor != null)
            {
                ScheduledForString = Convert.ToDateTime(ScheduledFor).ToShortDateString();
            }
            string OracleRMAString = OracleRMA.ToString();
            string workOrder_IDstring = workOrder_ID.ToString();
            string NoteText = "Return for workorder " + workOrder_IDstring + " modified by web user. ";
            if (!string.IsNullOrEmpty(ProductCode))
            {
                NoteText += Environment.NewLine + "Returned Products: " + ProductCode.Trim() + ", ";
            }
            if (!string.IsNullOrEmpty(OracleRMAString))
            {
                NoteText += Environment.NewLine + "Oracle RMA#: " + OracleRMAString.Trim() + ", ";
            }
            if (!string.IsNullOrEmpty(note))
            {
                NoteText += Environment.NewLine + "Notes about Return: " + note + ", ";
            }
            if (!string.IsNullOrEmpty(return_Other_Reason))
            {
                NoteText += Environment.NewLine + "Other Reason: " + return_Other_Reason + ", ";
            }
            if (!string.IsNullOrEmpty(tracking_Number))
            {
                NoteText += Environment.NewLine + "Call Tag #s: " + tracking_Number + ", ";
            }
            if (!string.IsNullOrEmpty(ScheduledForString))
            {
                NoteText += Environment.NewLine + "Scheduled For: " + ScheduledForString + "" + Environment.NewLine;
            }
            using (USPS_Report.Models.ReportsEntities _db = new USPS_Report.Models.ReportsEntities())
            {
                _db.Database.ExecuteSqlCommand("sp_InsertAccountNoteCallTag @account,@userName,@workOrder_ID,@note,@reason__List_Option_ID,@return_Other_Reason,@tracking_Number,@OracleRMA,@ScheduledFor,@NoteText", new SqlParameter("account", account.GetDBNullOrValue()), new SqlParameter("userName", userName.GetDBNullOrValue()), new SqlParameter("workOrder_ID", workOrder_ID.GetDBNullOrValue()), new SqlParameter("note", note.GetDBNullOrValue()), new SqlParameter("reason__List_Option_ID", reason__List_Option_ID.GetDBNullOrValue()), new SqlParameter("return_Other_Reason", return_Other_Reason.GetDBNullOrValue()), new SqlParameter("tracking_Number", tracking_Number.GetDBNullOrValue()), new SqlParameter("OracleRMA", OracleRMAString.GetDBNullOrValue()), new SqlParameter("ScheduledFor", ScheduledForString.GetDBNullOrValue()), new SqlParameter("NoteText", NoteText.GetDBNullOrValue()));
                
            }

        }

    }
}