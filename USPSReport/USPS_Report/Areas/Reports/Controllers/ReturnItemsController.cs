using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using USPS_Report.Areas.Reports.Models;
using static USPS_Report.Areas.Reports.Models.ReturnItems;
using WorkOrder = USPS_Report.Areas.Reports.Models.ReturnItems.WorkOrder;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class ReturnItemsController : Controller
    {
        // GET: Reports/ReturnItems
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReturnItemsData()


        {
            ReturnItemsVM _vm = new ReturnItemsVM();
            try
            {                
                IList<ReturnItemsData> _list = new List<ReturnItemsData>();
                _list = ReturnItems.GetReturnItemsData();
                _vm.Details = _list;              
                return View(_vm);
            }
            catch(Exception ex)
            {                
                throw;
            }
            
        }

        public ActionResult HistoryReturns()
        {
            HistoryItemsVM _vm = new HistoryItemsVM();
            _vm.Details = ReturnItems.GetReturnHistoryItemsData();
            return View(_vm);
        }
        public ActionResult ReturnInfo(string returnId,string selectedWO)
        {            
            WorkOrder SelWO = new WorkOrder();
            ReturnItemsInfoVM _vm = new ReturnItemsInfoVM();
            try
            {
                IList<ReturnReason> _list = new List<ReturnReason>();
                _list = ReturnItems.GetReturnReasonList();
                _vm.ReturnReasonList = new SelectList(_list, "ReturnReasonId", "ReturnReasonText");
                if (returnId != null)
                {
                    ReturnItem returnItem = ReturnItems.GetReturnItem(returnId);
                    if (returnItem != null)
                    {
                        //_vm.Date_Returned = DateTime.Today.Date;
                        if (returnItem.ScheduledFor == null)
                        {
                            _vm.ScheduledFor = DateTime.Today.Date;
                        }
                        else
                        {
                            _vm.ScheduledFor = returnItem.ScheduledFor;
                        }
                        _vm.Account = returnItem.Account;
                        _vm.Account_Member = returnItem.Account_Member;
                        _vm.Boxes_Returned = returnItem.Boxes_Returned;
                        _vm.Date_Returned = returnItem.Date_Returned;
                        _vm.Dont_Display = returnItem.Dont_Display == 1 ? true : false;
                        _vm.FirstName = returnItem.FirstName;
                        _vm.LastName = returnItem.LastName;
                        _vm.OracleRMA = returnItem.OracleRMA;
                        _vm.Reason__List_Option_ID = returnItem.Reason__List_Option_ID;
                        _vm.RequestDate = returnItem.RequestDate;
                        _vm.Reshipped = returnItem.Reshipped;
                        _vm.Return_ID = returnItem.Return_ID;
                        _vm.Return_Note = returnItem.Return_Note;
                        _vm.Return_Other_Reason = returnItem.Return_Other_Reason != null ? returnItem.Return_Other_Reason.Trim() : returnItem.Return_Other_Reason;
                        _vm.Send_To_Billing = returnItem.Send_To_Billing == 1 ? true : false;
                        _vm.Tag_Type = returnItem.Tag_Type;
                        _vm.Tracking_Number = returnItem.Tracking_Number;
                        _vm.WorkOrder_ID = returnItem.WorkOrder_ID;
                        IList<ReturnLineItem> returnLineItems = ReturnItems.GetReturnLineItemsList(returnId);
                        SelWO = ReturnItems.GetWorkOrderInfo(returnItem.Account.Value, returnItem.WorkOrder_ID.Value);
                        IList<WorkOrderItem> _WorkOrderItemsList = new List<WorkOrderItem>();
                        foreach (WorkOrderProduct item in SelWO.WorkOrderProducts)
                        {
                            WorkOrderItem workOrderItem = new WorkOrderItem();
                            workOrderItem.ProductCode = item.ProductCode;
                            workOrderItem.ProductDescription = item.ProductDescription;
                            workOrderItem.QtyShipped = item.QtyShipped;
                            workOrderItem.WorkOrder_ID = item.WorkOrder_ID;
                            workOrderItem.Account = item.Account;
                            workOrderItem.RequestDate = item.RequestDate;
                            workOrderItem.ID_DeliveryLocation = item.ID_DeliveryLocation;
                            workOrderItem.WorkOrder_Line = item.WorkOrder_Line;
                            var returnLineItem = returnLineItems.Where(i => i.WorkOrderLineID == item.WorkOrder_Line).FirstOrDefault();
                            if (returnLineItem != null)
                            {
                                workOrderItem.Need = true;
                                workOrderItem.QtyReturned = returnLineItem.QtyReturn;
                            }
                            _WorkOrderItemsList.Add(workOrderItem);
                        }
                        _vm.WorkOrderItems = _WorkOrderItemsList;
                    }
                }
                else if (selectedWO != null)
                {
                    var WoNo = selectedWO.Split(',')[0];
                    var WoAc = selectedWO.Split(',')[1];
                    _vm.Date_Returned = DateTime.Today.Date;
                    if (_vm.ScheduledFor == null)
                    {
                        _vm.ScheduledFor = DateTime.Today.Date;
                    }
                    _vm.WorkOrder_ID = Convert.ToInt32(WoNo);
                    _vm.Account = Convert.ToInt32(WoAc);
                    //var list=(IList<WorkOrder>)TempData["WorkOrders"];
                    //SelWO=list.Where(i => i.WorkOrder_Number == WoNo && i.Account == WoAc).FirstOrDefault();
                    SelWO = ReturnItems.GetWorkOrderInfo(Convert.ToInt32(WoAc), Convert.ToInt32(WoNo));
                    if (SelWO != null)
                    {
                        IList<WorkOrderItem> _WorkOrderItemsList = new List<WorkOrderItem>();
                        foreach (WorkOrderProduct item in SelWO.WorkOrderProducts)
                        {
                            WorkOrderItem workOrderItem = new WorkOrderItem();
                            workOrderItem.ProductCode = item.ProductCode;
                            workOrderItem.ProductDescription = item.ProductDescription;
                            workOrderItem.QtyShipped = item.QtyShipped;
                            workOrderItem.WorkOrder_ID = item.WorkOrder_ID;
                            workOrderItem.Account = item.Account;
                            workOrderItem.RequestDate = item.RequestDate;
                            workOrderItem.ID_DeliveryLocation = item.ID_DeliveryLocation;
                            workOrderItem.WorkOrder_Line = item.WorkOrder_Line;
                            _WorkOrderItemsList.Add(workOrderItem);
                        }
                        _vm.WorkOrderItems = _WorkOrderItemsList;
                    }
                }                
                return View(_vm);
            }
           
            catch (Exception ex)
            {                
                throw;
            }               
        }
        public ActionResult ChooseAccount(ReturnItemsVM returnItemsVM)
        {
            ChooseAccountVM _vm = new ChooseAccountVM();
            try
            {
                if (!string.IsNullOrEmpty(returnItemsVM.Account))
                {
                    IList<AccountData> _list = new List<AccountData>();
                    _list = ReturnItems.GetSearchAccounts(returnItemsVM);
                    _vm.AccountDetails = _list;
                    return View(_vm);
                }
                else
                {
                    ChooseWorkOrderVM _vmwo = new ChooseWorkOrderVM();
                    IList<WorkOrder> _listwo = new List<WorkOrder>();
                    _listwo = ReturnItems.GetWorkOrders(returnItemsVM.WorkOrder, "WO");
                    //TempData["WorkOrders"] = _list;
                    _vmwo.WorkOrders = _listwo.OrderByDescending(x => x.Request_Date).ToList();
                    return RedirectToAction("ReturnInfo", "ReturnItems", new { @returnId = "", @selectedWO = _vmwo.WorkOrders.FirstOrDefault().WorkOrder_Number + "," + _vmwo.WorkOrders.FirstOrDefault().Account });
                }
                //return View(_vm);
                  
            }
            catch (Exception ex)
            {                
                throw;
            }           
        }
        public ActionResult ChooseWorkOrder(string selectedItem)
        {            
            ChooseWorkOrderVM _vm = new ChooseWorkOrderVM();
            try
            {
                IList<WorkOrder> _list = new List<WorkOrder>();
                _list = ReturnItems.GetWorkOrders(selectedItem,"ACCOUNT");
                //TempData["WorkOrders"] = _list;
                _vm.WorkOrders = _list.OrderByDescending(x=>x.Request_Date).ToList();
                //if (!string.IsNullOrEmpty(returnItemsVM.Account))
                //{
                //    return View(_vm);
                //}
                //else
                //{
                //    return RedirectToAction("ReturnInfo", "ReturnItems", new { @returnId = "", @selectedWO = _vm.WorkOrders.FirstOrDefault().WorkOrder_Number+","+ _vm.WorkOrders.FirstOrDefault().Account });
                //}
                
                return View(_vm);
            }
            catch (Exception ex)
            {                
                throw;
            }           
        }
        public ActionResult CreateReturn(ReturnItemsInfoVM returnItemsInfoVM)
        {
            CreateReturnVM _vm = new CreateReturnVM();
            try
            {
                var components = User.Identity.Name.Split('\\');
                var userName = components.Last();
                _vm.WorkOrder_ID = returnItemsInfoVM.WorkOrder_ID.ToString();
                if (returnItemsInfoVM.Return_ID != 0)
                {
                    ReturnItems.UpdateReturn(returnItemsInfoVM.Return_ID, returnItemsInfoVM.Reshipped, returnItemsInfoVM.Tag_Type, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Date_Returned, returnItemsInfoVM.Tracking_Number, Convert.ToInt16(returnItemsInfoVM.Send_To_Billing), Convert.ToInt16(returnItemsInfoVM.Dont_Display), returnItemsInfoVM.Boxes_Returned, returnItemsInfoVM.ScheduledFor);
                    foreach (WorkOrderItem item in returnItemsInfoVM.WorkOrderItems)
                    {
                        if (item.Need)
                        {
                            var return_Line_ID = ReturnItems.DoesReturnLineExist(returnItemsInfoVM.Return_ID, item.WorkOrder_Line);
                            if (return_Line_ID != null)
                            {
                                ReturnItems.UpdateReturnLine(return_Line_ID.Value, item.QtyReturned ?? 0);
                            }
                            else
                            {
                                ReturnItems.InsertReturnLine(returnItemsInfoVM.Return_ID, item.WorkOrder_Line, item.QtyReturned ?? 0);
                            }
                        }
                    }
                    if (returnItemsInfoVM.Date_Returned == null)
                    {
                        ReturnItems.InsertAccountNoteCallTag(returnItemsInfoVM.Account.Value, userName, returnItemsInfoVM.WorkOrder_ID.Value, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID.Value, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Tracking_Number, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.ScheduledFor, returnItemsInfoVM.ProductCode, returnItemsInfoVM.QtyReturned);
                    }
                    if (returnItemsInfoVM.Date_Returned != null)
                    {
                        ReturnItems.InsertAccountNote(returnItemsInfoVM.Account.Value, userName, returnItemsInfoVM.WorkOrder_ID.Value, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID.Value, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Tracking_Number, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.Date_Returned, returnItemsInfoVM.ProductCode, returnItemsInfoVM.QtyReturned);
                    }
                }
                else
                {
                    if (!ReturnItems.DoesReturnExist(returnItemsInfoVM.WorkOrder_ID.Value))
                    {
                        ReturnItems.InsertReturn(returnItemsInfoVM.Account, returnItemsInfoVM.WorkOrder_ID, returnItemsInfoVM.Reshipped, returnItemsInfoVM.Tag_Type, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Date_Returned, returnItemsInfoVM.Tracking_Number, Convert.ToInt16(returnItemsInfoVM.Send_To_Billing), Convert.ToInt16(returnItemsInfoVM.Dont_Display), returnItemsInfoVM.Boxes_Returned, returnItemsInfoVM.ScheduledFor);
                        var latestReturn_ID = ReturnItems.GetLatestReturnID(returnItemsInfoVM.WorkOrder_ID.Value);
                        if(latestReturn_ID!=null)
                        {
                            foreach (WorkOrderItem item in returnItemsInfoVM.WorkOrderItems)
                            {
                                if (item.Need)
                                {
                                    ReturnItems.InsertReturnLine(latestReturn_ID.Value, item.WorkOrder_Line, item.QtyReturned ?? 0);
                                }
                            }
                        }
                        if (returnItemsInfoVM.Date_Returned == null)
                        {
                            ReturnItems.InsertAccountNoteCallTag(returnItemsInfoVM.Account.Value, userName, returnItemsInfoVM.WorkOrder_ID.Value, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID.Value, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Tracking_Number, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.ScheduledFor, returnItemsInfoVM.ProductCode, returnItemsInfoVM.QtyReturned);
                        }
                        if (returnItemsInfoVM.Date_Returned != null)
                        {
                            ReturnItems.InsertAccountNote(returnItemsInfoVM.Account.Value, userName, returnItemsInfoVM.WorkOrder_ID.Value, returnItemsInfoVM.Return_Note, returnItemsInfoVM.Reason__List_Option_ID.Value, returnItemsInfoVM.Return_Other_Reason, returnItemsInfoVM.Tracking_Number, returnItemsInfoVM.OracleRMA, returnItemsInfoVM.Date_Returned, returnItemsInfoVM.ProductCode, returnItemsInfoVM.QtyReturned);
                        }
                    }
                    else
                    {
                        _vm.DoesReturnExist = true;
                    }
                }                              
                
                //ViewBag.WorkOrderId=returnItemsInfoVM.WorkOrder_ID.ToString();                
                return View(_vm);
            }
            catch (Exception ex)
            {                
                throw;
            }           
        }        
    }
}