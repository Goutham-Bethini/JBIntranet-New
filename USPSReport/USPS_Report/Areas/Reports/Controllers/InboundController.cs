using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Models;
using ReportsDatabase;
using System.Data.Entity;
using USPS_Report.Areas.Reports.Models;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class InboundController : Controller
    {
        static string constring = ConfigurationManager.ConnectionStrings["CallAgentDbconnectionstring"].ConnectionString;
        static SqlConnection conns = new SqlConnection(constring);

        // GET: Reports/Inbound
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddChange()
        {
            return View();
        }

        public ActionResult UpdateDoctorInfo()
        {
            return View();
        }

        public ActionResult UpdateInsInfo()
        {
            return View();
        }

        public ActionResult CustomerAccountAddChngeList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCustomerAcctChngAccList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        public ActionResult CustomerInsuranceChangeList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCustomerInsChangeList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerDoctorChangeList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCustomerDrChangeList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomerAddresslist_Update([DataSourceRequest] DataSourceRequest request , AddressChangeVM _rec)
        {
            bool Billing = false;
            bool Shipping = false;
            bool Same = false;
            NewAdd newAddress = new NewAdd();
            newAddress.Account = _rec.Account;

            if (_rec != null && ModelState.IsValid)
            {
               // Inbound.AddNoteAddressChange(_rec);
                using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
                    {

                    //var _data = _callDB.InboundChangeAddresses.Where(t => (t.UpdateTime == _rec.UpdateTime && t.Account == _rec.Account)).OrderByDescending(t=>t.UpdateTime).Take(1);
                    var _data = (from t in _callDB.InboundChangeAddresses
                                 where  t.Account == _rec.Account && t.UpdateStatus == false
                                 select t ).OrderByDescending(t=>t.UpdateTime).ToList();

                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = _rec.UpdateStatus;
                        
                        _callDB.Entry(_item).State = EntityState.Modified;

                        if (_item.B_Address_1 != null && _item.B_Address_1 != ""
                            && _item.B_State != null && _item.B_State != ""
                             && _item.B_City != null && _item.B_City != "")
                        {
                            Billing = true;
                            newAddress.B_Address1 = _item.B_Address_1;
                            newAddress.B_Address2 = _item.B_Address_2;
                            newAddress.B_City = _item.B_City;
                            newAddress.B_State = _item.B_State;
                            newAddress.B_Zip = _item.B_Zip;
                        }

                        if (_item.S_Address_1 != null && _item.S_Address_1 != ""
                         && _item.S_State != null && _item.S_State != ""
                          && _item.S_City != null && _item.S_City != "")
                        {
                            Shipping = true;
                            newAddress.S_Address1 = _item.S_Address_1;
                            newAddress.S_Address2 = _item.S_Address_2;
                            newAddress.S_City = _item.S_City;
                            newAddress.S_State = _item.S_State;
                            newAddress.S_Zip = _item.S_Zip;
                        }

                        if (Billing == true && Shipping == true)
                        {
                            if (_item.B_Address_1 == _item.S_Address_1 && _item.B_State == _item.S_State && _item.B_Zip == _item.S_Zip)
                            {
                                Same = true;
                            }
                        }

                  
                         


                    }

                    
                   

                    try
                    {
                        _callDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }

                using (HHSQLDBEntities _hdb = new HHSQLDBEntities())
                {
                    var _acc = _hdb.tbl_Account_Member.Where(t => (t.Account == _rec.Account && t.Member == 1)).Take(1);
                   
                        foreach (var _account in _acc)
                    {
                        if (Billing == true)
                        {
                            _account.Address_1 = newAddress.B_Address1;
                            _account.Address_2 = newAddress.B_Address2;
                            _account.City = newAddress.B_City;
                            _account.State = newAddress.B_State;
                            _account.Zip = newAddress.B_Zip;
                        }

                        if ((Shipping == true && Same != true) || (Shipping == true && Billing != true))
                        {
                            _account.ShipToAddress_1 = newAddress.S_Address1;
                            _account.ShipToAddress_2 = newAddress.S_Address2;
                            _account.ShipToCity = newAddress.S_City;
                            _account.ShipToState = newAddress.S_State;
                            _account.ShipToZip = newAddress.S_Zip;
                        }
                        _hdb.Entry(_account).State = EntityState.Modified;
                    }

                    try
                    {
                        _hdb.SaveChanges();
                        // add note in HDMS
                        Inbound.AddNoteAddressChange(newAddress);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }

                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomerInsurance_Update([DataSourceRequest] DataSourceRequest request, InsChangeVM _rec)
        { 

            if (_rec != null && ModelState.IsValid)
            { 
                  
                        Inbound.AddNoteInsuranceChange(_rec);
                 
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomerDoctor_Update([DataSourceRequest] DataSourceRequest request, DoctorChangeVM _rec)
        {

            if (_rec != null && ModelState.IsValid)
            {

                Inbound.AddNoteDoctorChange(_rec);

            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

        private IEnumerable<AddressChangeVM> GetCustomerAcctChngAccList()

        {
            IList<AddressChangeVM> _AccList = new List<AddressChangeVM>();

            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                _AccList = _callDB.Database.SqlQuery<AddressChangeVM>( " select  max(inb.id) as id, mem.Account, mem.First_Name, mem.Last_Name, mem.Middle, "+
                    "mem.BirthDate, mem.EmailAddress, max(inb.UpdateTime) as UpdateTime, inb.UpdateStatus from InboundChangeAddress INb "+
" Join[HHSQLDB].[dbo].[tbl_Account_Member] mem "+
 " on INb.Account = mem.Account and mem.Member = 1 "+
   " where inb.Uploaded is NUll "+

  "  group by mem.Account, mem.First_Name, mem.Last_Name, mem.Middle, mem.BirthDate, mem.EmailAddress, inb.UpdateStatus "+
  "  order by UpdateTime asc").ToList<AddressChangeVM>();

                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',46,GETDATE())";

                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        private IEnumerable<InsChangeVM> GetCustomerInsChangeList()

        {
            IList<InsChangeVM> _AccList = new List<InsChangeVM>();

            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                _AccList = (from t in _callDB.InboundUpdateInsurances
                            where t.UpdateStatus != true
                            select new InsChangeVM
                            {
                                id = t.Id,
                                Account = t.Account,
                                InsuranceProvider = t.InsuranceProvider,
                                InsuranceProviderID = t.InsuranceProviderID,
                                InsuranceProviderPhone = t.InsuranceProviderPhone,
                                UpdateStatus = t.UpdateStatus,
                                UpdateTime = t.UpdateTime
                         }).ToList();
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',50,GETDATE())";
                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        private IEnumerable<DoctorChangeVM> GetCustomerDrChangeList()

        {
            IList<DoctorChangeVM> _AccList = new List<DoctorChangeVM>();

            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                _AccList = (from t in _callDB.InboundUpdateDoctors
                            where t.UpdateStatus != true
                            select new DoctorChangeVM
                            {
                                id = t.Id,
                                Account = t.Account,
                                DoctorName = t.DoctorName,
                                DoctorPhone = t.DoctorPhone,
                                DoctorLocation = t.DoctorLocation,
                                UpdateStatus = t.UpdateStatus,
                                UpdateTime = t.UpdateTime
                            }).ToList();
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',49,GETDATE())";
                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        public ActionResult AccountAddListByAccount(int AccountNum, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetAccAddListAcct(AccountNum).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<ChangeAddReq> GetAccAddListAcct(int _acct)
        {
            IList<ChangeAddReq> _addReq = new List<ChangeAddReq>();
            IList<ChangeAddReq> _addReqH = new List<ChangeAddReq>();
            IList<ChangeAddReq> _addReqC = new List<ChangeAddReq>();

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _addReqH = (from t in _db.tbl_Account_Member
                            where t.Account == _acct && t.Member == 1
                            select new ChangeAddReq
                            {
                                Account = _acct,
                                DB = "HDMS",
                                Billing = (t.Address_1 != null || t.City != null || t.State != null) ? t.Address_1 + " " + t.Address_2 + ", " + t.City + ", " + t.State + ", " + t.Zip : "",
                                // HDMSBAdd2 =   t.City +","+ t.State + ","+ t.Zip,

                                Shipping = (t.ShipToAddress_1 !=null || t.ShipToCity != null || t.ShipToState !=null) ? t.ShipToAddress_1 + " " + t.ShipToAddress_2 + ", " + t.ShipToCity + ", " + t.ShipToState + ", " + t.ShipToZip : "",
                                // HDMSSAdd2 = t.ShipToCity + "," + t.ShipToState + "," + t.ShipToZip

                            }
                           ).Take(1).ToList();



            }

            using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
            {
                _addReqC = (from t in _CallDB.InboundChangeAddresses
                            where t.Account == _acct && t.UpdateStatus == false
                            select new ChangeAddReq
                            {
                                Account = _acct,
                                DB = "Requested",
                                Billing = (t.B_Address_1 !=null || t.B_State != null || t.B_City != null) ? t.B_Address_1 + "  " + t.B_Address_2  +" , " + t.B_City + " ," + t.B_State + " ," + t.B_Zip : null,
                               Shipping =  (t.S_Address_1 != null || t.S_State != null || t.S_City != null) ? t.S_Address_1 + "  " + t.S_Address_2 + " ," + t.S_City + " ," + t.S_State + " ," + t.S_Zip : "",
                                 UpdateTime = t.UpdateTime

                            }
                           ).OrderByDescending(t=>t.UpdateTime).Take(1).ToList();

            }
            _addReq = _addReqH.Concat(_addReqC).ToList();

            return _addReq;
        }

        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CustomerAccountList_Destroy([DataSourceRequest] DataSourceRequest request, AddressChangeVM _rec)
        {
            using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
            {
                var _data = (from t in _CallDB.InboundChangeAddresses
                             where t.Account == _rec.Account
                             select t
                             ).ToList();


                foreach (var _item in _data)
                {
                    _item.Uploaded = true;

                    _CallDB.Entry(_item).State = EntityState.Modified;
                }

                try
                {
                    _CallDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
                return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CustomerInsurance_Destroy([DataSourceRequest] DataSourceRequest request, InsChangeVM _rec)
        {
            if (_rec != null && ModelState.IsValid)
            {
                if (_rec.UpdateStatus != true)
                {
                    Inbound.AddNoteInsuranceChange(_rec);
                }



                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    var _data = (from t in _CallDB.InboundUpdateInsurances
                                 where t.Id == _rec.id
                                 select t
                                 ).ToList();


                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = true;

                        _CallDB.Entry(_item).State = EntityState.Modified;
                    }

                    try
                    {
                        _CallDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CustomerDoctor_Destroy([DataSourceRequest] DataSourceRequest request, DoctorChangeVM _rec)
        {
            if (_rec != null && ModelState.IsValid)
            {
                if (_rec.UpdateStatus != true)
                {
                    Inbound.AddNoteDoctorChange(_rec);
                }



                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    var _data = (from t in _CallDB.InboundUpdateDoctors
                                 where t.Id == _rec.id
                                 select t
                                 ).ToList();


                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = true;

                        _CallDB.Entry(_item).State = EntityState.Modified;
                    }

                    try
                    {
                        _CallDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult ReorderSupplies()
        {
            return View();

        }

        public ActionResult PharmacyReorderSupplies()
        {
            return View();

        }


        public ActionResult CustomerAccountReorderList([DataSourceRequest] DataSourceRequest request)
        {

            return Json(GetCustomerReorderAccList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PharmacyAccountReorderList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetPharmacyReorderAccList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<ReorderSuppliesVM> GetCustomerReorderAccList()

        {
            IList<ReorderSuppliesVM> _AccList = new List<ReorderSuppliesVM>();
            // Run the Mark done process for the orders with all products needed
            if (conns.State == ConnectionState.Closed)
                conns.Open();
            SqlCommand cmd = new SqlCommand("dbo.sp_CustomerReordersupplies_MarkDone", conns);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.ExecuteNonQuery();
            conns.Close();
            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
               

             //   _callDB.sp_CustomerReordersupplies_MarkDone();
                _AccList = _callDB.Database.SqlQuery<ReorderSuppliesVM>(" select  max(calls.id) as id, mem.Account, mem.First_Name, mem.Last_Name, mem.Middle,  "+
                  "  mem.BirthDate, mem.EmailAddress, calls.OrderDate, calls.UpdateTime as UpdateTime , calls.ISConfirmed, calls.IsDeleted from InboundCall calls " +
" join [HHSQLDB].[dbo].[tbl_Account_Member] mem " +
" on calls.Account = mem.Account and mem.Member = 1 "+
 " where calls.UpdateStatus = 0  and calls.IsDeleted = 0"+
 " group by mem.Account, mem.First_Name, mem.Last_Name, mem.Middle, mem.BirthDate, mem.EmailAddress, calls.UpdateTime ,calls.ISConfirmed, calls.IsDeleted, calls.OrderDate " +
  "  order by UpdateTime asc").ToList<ReorderSuppliesVM>();
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',44,GETDATE())";

                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        private IEnumerable<ReorderSuppliesVM> GetPharmacyReorderAccList()

        {
            IList<ReorderSuppliesVM> _AccList = new List<ReorderSuppliesVM>();
            IList<ReorderSuppliesVM> _AccListPhar = new List<ReorderSuppliesVM>();
            IList<ReorderSuppliesVM> _AccListMeridian = new List<ReorderSuppliesVM>();

            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                _AccListPhar = _callDB.Database.SqlQuery<ReorderSuppliesVM>(" select  max(calls.id) as id, mem.Account, mem.First_Name, mem.Last_Name, mem.Middle,  " +
                  "  mem.BirthDate, mem.EmailAddress, calls.RefillDate, calls.UpdateTime as UpdateTime , calls.ISConfirmed, calls.IsDeleted, 'INBOUND' as service  from PharmacyCall calls " +
" join [HHSQLDB].[dbo].[tbl_Account_Member] mem " +
" on calls.Account = mem.Account and mem.Member = 1 " +
 " where calls.UpdateStatus = 0  and calls.IsDeleted = 0" +
 " group by mem.Account, mem.First_Name, mem.Last_Name, mem.Middle, mem.BirthDate, mem.EmailAddress, calls.UpdateTime ,calls.ISConfirmed, calls.IsDeleted, calls.RefillDate " +
  "  order by UpdateTime asc").ToList<ReorderSuppliesVM>();

                _AccListMeridian = _callDB.Database.SqlQuery<ReorderSuppliesVM>(" select  calls.id, mem.Account, mem.First_Name, mem.Last_Name, mem.Middle, " +
" mem.BirthDate, mem.EmailAddress, calls.OrderDate as RefillDate, calls.CallTime "+
"as UpdateTime, calls.IsOrderConfirmed, 'OUTBOUND' as service  from Call calls "+
"left join UpdatedMeridianCallsOB m on m.callid = calls.Id "+
"join [HHSQLDB].[dbo].[tbl_Account_Member] mem on calls.AccountNumber = mem.Account "+
 " and mem.Member = 1 where  calls.callTypeId = 3 and m.updated is null "+
" order by CallTime asc").ToList<ReorderSuppliesVM>();

                _AccList = _AccListPhar.Concat(_AccListMeridian).ToList();

                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',45,GETDATE())";

                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomerReorderList_Update([DataSourceRequest] DataSourceRequest request, ReorderSuppliesVM _rec)
        {
            DateTime _dt = _rec.UpdateTime;
            IList<int> _prodNoReq = new List<int>(); 
            if (_rec != null && ModelState.IsValid)
            {
                using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
                {
                    
                    //var _data = _callDB.InboundChangeAddresses.Where(t => (t.UpdateTime == _rec.UpdateTime && t.Account == _rec.Account)).OrderByDescending(t=>t.UpdateTime).Take(1);
                    var _data = (from t in _callDB.InboundCalls
                                 where t.Account == _rec.Account && t.UpdateStatus == false  && t.OrderDate == _rec.OrderDate 
                                 && t.IsDeleted == false
                                 select t).OrderByDescending(t => t.UpdateTime).ToList();

                      _prodNoReq = (from p in _callDB.InboundProductConfirmations
                                 where p.OrderID == _rec.id 
                                 && p.NeedsProduct == false
                                 select p.ProductCategoryId).ToList();


                    foreach (var _item in _data)
                    {
                         
                            _item.ISConfirmed =   _rec.ISConfirmed;
                           _item.IsDeleted = _rec.IsDeleted;

                        if (_rec.IsDeleted == true)
                        {
                            _item.ISConfirmed = false;
                            _rec.ISConfirmed = false;
                        }

                          _callDB.Entry(_item).State = EntityState.Modified;
                        // add note in hdms
                       Inbound.AddNoteInboundSupplies(_rec);
                        // confirm or not confirm the order in confirmation tool
                        // if it is not confirm move any other confirmation from the order confirm table
                        Inbound.ConfirmInboundSupplies(_rec);

                    }
                     
                    try
                    {
                        _callDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }

                    if (_rec.ISConfirmed == true && _prodNoReq.Count()>0)
                    {
                        using (HHSQLDBEntities _db = new HHSQLDBEntities())
                        {
                            var _list1 = (from t in _db.tbl_PS_RepeatingOrders
                                          where t.NextRepeatDate == _rec.OrderDate
                                         && t.Account == _rec.Account
                                          select t).ToList();

                            foreach (var item in _list1)
                            {
                                int? prodId = (from pro in _db.tbl_Product_Table
                                              where pro.ID == item.ID_Product
                                              select pro.ID_ProductCategory).Take(1).FirstOrDefault();

                                
                                if (_prodNoReq.Contains(Convert.ToInt32(prodId)))
                                {
                                    DateTime _nextRepeatDt = Convert.ToDateTime(item.NextRepeatDate);
                                    switch (item.Frequency)
                                    {
                                        case "Y":
                                            _nextRepeatDt = _nextRepeatDt.AddYears(1);
                                            break;
                                        case "M":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(1);
                                            break;
                                        case "D":
                                            _nextRepeatDt = _nextRepeatDt.AddDays(1);
                                            break;
                                        case "2":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(2);
                                            break;
                                        case "3":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(3);
                                            break;
                                        case "4":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(4);
                                            break;
                                        case "5":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(5);
                                            break;
                                        case "6":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(6);
                                            break;
                                        case "7":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(7);
                                            break;
                                        case "8":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(8);
                                            break;
                                        case "9":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(9);
                                            break;
                                        case "10":
                                            _nextRepeatDt = _nextRepeatDt.AddMonths(10);
                                            break;



                                    }

                                   


                                    DateTime? _oldRWO = item.NextRepeatDate;
                                    item.NextRepeatDate = _nextRepeatDt;
                                    try
                                    {

                                        //uncomment when deploy this to production : grani
                                          _db.Entry(item).State = EntityState.Modified;
                                         _db.SaveChanges(); // save the new repeating date in rwo 
                                      
                                       

                                    }
                                    catch (Exception ex)
                                    {
                                        
                                        // send an email and test alert if the program failed with the account number on which program has failed. 
                                        ProgramFailedMsg("Program is failed for Inbound Service the order date: " +Convert.ToDateTime(_rec.OrderDate).ToShortDateString(), "Program is failed to updating the RWO order line date in Inbound Service  for account = " + _rec.Account.ToString(), "FailedChangeRWOwithInboundSS");
                                    }
                                }
                            }


                        }


                    }
                }

                
 
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PharmacyReorderList_Update([DataSourceRequest] DataSourceRequest request, ReorderSuppliesVM _rec)
        {
            // Archive the Pharmcy onfirmations if the number recahes to 940000
            if (_rec.id < 940000)
            {
                DateTime _dt = _rec.UpdateTime;
                IList<int> _prodNoReq = new List<int>();
                if (_rec != null && ModelState.IsValid)
                {
                    using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
                    {

                        //var _data = _callDB.InboundChangeAddresses.Where(t => (t.UpdateTime == _rec.UpdateTime && t.Account == _rec.Account)).OrderByDescending(t=>t.UpdateTime).Take(1);
                        var _data = (from t in _callDB.PharmacyCalls
                                     where t.Account == _rec.Account && t.UpdateStatus == false && t.RefillDate == _rec.RefillDate
                                     && t.IsDeleted == false
                                     select t).OrderByDescending(t => t.UpdateTime).ToList();

                        _prodNoReq = (from p in _callDB.PharmacyConfirmations
                                      where p.OrderID == _rec.id
                                      && p.NeedsProduct == false
                                      select p.ProductCategoryId).ToList();





                        foreach (var _item in _data)
                        {


                            _item.ISConfirmed = _rec.ISConfirmed;
                            _item.IsDeleted = _rec.IsDeleted;

                            if (_rec.IsDeleted == true)
                            {
                                _item.ISConfirmed = false;
                                _rec.ISConfirmed = false;
                            }

                            _callDB.Entry(_item).State = EntityState.Modified;
                            // add note in hdms
                            Inbound.AddNotePharmaInboundSupplies(_rec);
                            // confirm or not confirm the order in confirmation tool
                            // if it is not confirm move any other confirmation from the order confirm table
                            Inbound.ConfirmPharmacySupplies(_rec);

                        }

                        try
                        {
                            _callDB.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;
                        }

                        if (_rec.ISConfirmed == true && _prodNoReq.Count() > 0)
                        {
                            using (HHSQLDBEntities _db = new HHSQLDBEntities())
                            {
                                var _list1 = (from t in _db.tbl_PS_RepeatingOrders
                                              where t.NextRepeatDate == _rec.OrderDate
                                             && t.Account == _rec.Account
                                              select t).ToList();

                                foreach (var item in _list1)
                                {
                                    int? prodId = (from pro in _db.tbl_Product_Table
                                                   where pro.ID == item.ID_Product
                                                   select pro.ID_ProductCategory).Take(1).FirstOrDefault();


                                    if (_prodNoReq.Contains(Convert.ToInt32(prodId)))
                                    {
                                        DateTime _nextRepeatDt = Convert.ToDateTime(item.NextRepeatDate);
                                        switch (item.Frequency)
                                        {
                                            case "Y":
                                                _nextRepeatDt = _nextRepeatDt.AddYears(1);
                                                break;
                                            case "M":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(1);
                                                break;
                                            case "D":
                                                _nextRepeatDt = _nextRepeatDt.AddDays(1);
                                                break;
                                            case "2":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(2);
                                                break;
                                            case "3":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(3);
                                                break;
                                            case "4":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(4);
                                                break;
                                            case "5":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(5);
                                                break;
                                            case "6":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(6);
                                                break;
                                            case "7":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(7);
                                                break;
                                            case "8":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(8);
                                                break;
                                            case "9":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(9);
                                                break;
                                            case "10":
                                                _nextRepeatDt = _nextRepeatDt.AddMonths(10);
                                                break;



                                        }




                                        DateTime? _oldRWO = item.NextRepeatDate;
                                        item.NextRepeatDate = _nextRepeatDt;
                                        try
                                        {

                                            // NO RWO update for Pharmacy Confirmaiton
                                            //uncomment when deploy this to production : grani
                                            //    _db.Entry(item).State = EntityState.Modified;
                                            //   _db.SaveChanges(); // save the new repeating date in rwo 



                                        }
                                        catch (Exception ex)
                                        {

                                            // send an email and test alert if the program failed with the account number on which program has failed. 
                                            ProgramFailedMsg("Program is failed for Inbound Service the order date: " + Convert.ToDateTime(_rec.OrderDate).ToShortDateString(), "Program is failed to updating the RWO order line date in Inbound Service  for account = " + _rec.Account.ToString(), "FailedChangeRWOwithInboundSS");
                                        }
                                    }
                                }


                            }


                        }
                    }



                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
         
        }


        public static void ProgramFailedMsg(string _sub, string _body, string fileName)
        {
            //to send excel as an attachment in email
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("AlertAutoRWOFailed@jandbmedical.com");
            mail.To.Add("grani@jandbmedical.com");
            mail.To.Add("2158767046@vzwpix.com");
            mail.Subject = _sub;
            mail.Body = _body;
            mail.IsBodyHtml = true;


            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CustomerReorderAccountList_Destroy([DataSourceRequest] DataSourceRequest request, ReorderSuppliesVM _rec)
        {

            if (_rec.ISConfirmed != true && _rec.IsDeleted != true)
            {
                Inbound.AddNoteInboundSupplies(_rec);
                Inbound.ConfirmInboundSupplies(_rec);
            }
           

            using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
            {
                var _data = (from t in _CallDB.InboundCalls
                             where t.Account == _rec.Account && _rec.OrderDate == t.OrderDate && t.UpdateStatus == false
                             select t
                             ).ToList();

                if (_data.Count() > 0)
                {
                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = true;

                        _CallDB.Entry(_item).State = EntityState.Modified;
                    }
                }

                try
                {
                    _CallDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult PharmacyReorderAccountList_Destroy([DataSourceRequest] DataSourceRequest request, ReorderSuppliesVM _rec)
        {
            // Archive the Pharmcy onfirmations if the number recahes to 940000
            if (_rec.id < 940000)
            {

                if (_rec.ISConfirmed != true && _rec.IsDeleted != true)
                {
                    Inbound.AddNoteInboundSupplies(_rec);
                    Inbound.ConfirmInboundSupplies(_rec);
                }


                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    var _data = (from t in _CallDB.PharmacyCalls
                                 where t.Account == _rec.Account && t.RefillDate == t.RefillDate && t.UpdateStatus == false
                                 select t
                                 ).ToList();

                    if (_data.Count() > 0)
                    {
                        foreach (var _item in _data)
                        {
                            _item.UpdateStatus = true;

                            _CallDB.Entry(_item).State = EntityState.Modified;
                        }
                    }

                    try
                    {
                        _CallDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }

            }
            else {
                UpdatedMeridianCallsOB _order = new UpdatedMeridianCallsOB();
                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    var components = User.Identity.Name.Split('\\');

                    var userName = components.Last();
                    var _data = (from t in _CallDB.Calls
                                 where t.Id == _rec.id
                                 select t
                               ).SingleOrDefault();
                    if (_data != null)
                    {
                        _order.CallId = _data.Id;
                        _order.Updated = true;
                        _order.ISConfirmed = _rec.ISConfirmed;
                        _order.UpdateBy = userName;
                        _order.Upload = DateTime.Now;

                    }
                    _CallDB.UpdatedMeridianCallsOBs.Add(_order);
                    try
                    {
                        _CallDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
               
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult ProductListByAccount(int IDNum, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetProdAddListAcct(IDNum).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PharmacyProdListByAccount(int IDNum, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetPharProdAddListAcct(IDNum).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<ProductReq> GetProdAddListAcct(int _id)
        {
            IList<ProductReq> _prodList = new List<ProductReq>();
            

            using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
            {
                _prodList = (from t in _CallDB.InboundProductConfirmations
                            where t.OrderID == _id  
                            select new  ProductReq
                            {
                                Id = t.Id,
                                ProdCategoryId = t.ProductCategoryId,
                                ProductDescription = t.ProductDescription,
                                ProdNeeded = t.NeedsProduct == true ? true : false,
                                Qty = t.Qty_Number

                            }
                           ).OrderByDescending(t => t.ProductDescription).ToList();

            }
            
            return _prodList;
        }


        private IEnumerable<ProductReq> GetPharProdAddListAcct(int _id)
        {
           

                IList<ProductReq> _prodList = new List<ProductReq>();


                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                if (_id < 940000)
                {
                    _prodList = (from t in _CallDB.PharmacyConfirmations
                                 where t.OrderID == _id
                                 select new ProductReq
                                 {
                                     Id = t.Id,
                                     ProdCategoryId = t.ProductCategoryId,
                                     ProductDescription = t.ProductDescription,
                                     ProdNeeded = t.NeedsProduct == true ? true : false,
                                     Qty = t.Qty_Number

                                 }
                               ).OrderByDescending(t => t.ProductDescription).ToList();

                }

                else
                {
                    _prodList = _CallDB.Database.SqlQuery<ProductReq>("select pro.id as Id, pro.ProductCategoryId as ProdCategoryId , "+
 " cat.Description as ProductDescription, "+
 " pro.NeedsProduct as ProdNeeded, "+
 " pro.Qty_Number as Qty from ProductConfirmation pro "+
 " join HHSQLDB.dbo.tbl_Category_Table cat "+
 " on cat.ID = pro.ProductCategoryId where callid =   " + _id).ToList<ProductReq>();

                }
            }

            return _prodList;
        }

        public ActionResult UpdatePhone()
        {
            phoneModel _vm = new phoneModel();
            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                 


                _vm.updatePhone = _callDB.Database.SqlQuery<UpdatePhoneVm>(" with table1 as ( select account, max(ID) as id from InboundChangePhone where UpdateStatus != 1 "+
                             " group by account ) select phn.*from table1 t join InboundChangePhone phn on t.id = phn.id "+
            " order by phn.id ").ToList<UpdatePhoneVm>();
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',47,GETDATE())";

                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }

                return View(_vm);
        }


        
        public ActionResult UpdatePhoneinHDMS(int ID )
        {
            Inbound.AddPhoneinHDMS(ID);
            phoneModel _vm = new phoneModel();
            

                return RedirectToAction("UpdatePhone");
        }

        public ActionResult DeleteUpdatePhoneReq(int ID)
        {
            Inbound.DeletePhoneInboundReq(ID);
            phoneModel _vm = new phoneModel();


            return RedirectToAction("UpdatePhone");
        }

        public ActionResult NewAccount(Int16? id)
        {
            NewAcctDetails _details = new NewAcctDetails();
            IList<NewAccount> _list = new List<NewAccount>();
          
            _list = SMOutbound.getNewAccountDetails(id);
            _details.details = _list;
            using (CallAgentDBEntitiesnew _db = new CallAgentDBEntitiesnew())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',48,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }
            return View(_details);
        }

        //[HttpPost]
        //public ActionResult NewAccount(Int16 id)
        //{
        //    NewAcctDetails _details = new NewAcctDetails();
        //    IList<NewAccount> _list = new List<NewAccount>();
     
        //    _list = SMOutbound.getNewAccountDetails(id);
        //    _details.details = _list;
        //    return View(_details);
        //}

        public ActionResult InboundReassessment()
        {
            return View();

        }

        public ActionResult CustomerAccountReassessList([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetCustomerReassessmentList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<InboundReassessmentVM> GetCustomerReassessmentList()

        {
            IList<InboundReassessmentVM> _AccList = new List<InboundReassessmentVM>();

            using (
                CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                _AccList = _callDB.Database.SqlQuery<InboundReassessmentVM>(" select  c.id as id, c.Account, c.Relationship , c.HasPhoneChanged,c.HasAddressChanged,   "+
              "     c.HasInsuranceChanged, c.NewInsuranceProvider, c.NewInsuranceProviderId, c.NewInsuranceProviderPhone, "+
              "     c.DoctorName, c.IsMeterWorking, c.TestingTimes, c.IsInjectInsulin, c.DayContType, c.NiteContType, "+
               "    c.IsDayBowelIncontinent, c.IsNightBowelIncontinent, c.IsDayUrineIncontinent, c.IsNightUrineIncontinent, c.IsUsageEvening, c.IsMentallyImpaired, "+
                "   c.IsDiapering, c.HasSeizureDisorder, c.CanWalk, c.UseWalkAssist, c.HasAllergy, c.AllergyMaterials, c.HasSoreOrRash, c.Weight, c.EatByMouth, c.IsTubeFed, c.IsVerbal, c.HasProductOverstock, "+
                 "  c.AdditionalSupplies,c.IncontinentType, c.IsReassessmentComplete, c.UploadTime, c.Uploaded from InboundReassessment c " +
                " where c.Uploaded = 0 order by c.UploadTime asc ").ToList<InboundReassessmentVM>();
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',51,GETDATE())";

                int rowsinsert = _callDB.Database.ExecuteSqlCommand(query);
            }
            return _AccList;
        }

        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CustomerReassessmentList_Destroy([DataSourceRequest] DataSourceRequest request, InboundReassessmentVM _rec)
        {

            
               // Inbound.AddNoteInboundSupplies(_rec);
                

            using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
            {
                var _data = (from t in _CallDB.InboundReassessments
                             where t.Account == _rec.Account && _rec.id == t.id  
                             select t
                             ).ToList();

                if (_data.Count() > 0)
                {
                    foreach (var _item in _data)
                    {
                        _item.Uploaded = true;

                        _CallDB.Entry(_item).State = EntityState.Modified;
                    }
                }

                try
                {
                    _CallDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CustomeReassessment_Update([DataSourceRequest] DataSourceRequest request, InboundReassessmentVM _rec)
        {
            
         
            if (_rec != null && ModelState.IsValid)
            {
                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {


                    var _data = (from t in _CallDB.InboundReassessments
                                 where t.Account == _rec.Account && _rec.id == t.id
                                 select t
                                 ).ToList();

                    if (_data.Count() > 0)
                    {
                        foreach (var _item in _data)
                        {
                           
                            _item.Uploaded = true;

                            _CallDB.Entry(_item).State = EntityState.Modified;

                            // add in hdms note 
                            if (_rec.Uploaded == true)
                            {
                                Inbound.AddNoteInboundReassessment(_item.id);
                                
                            }

                        }
                    }

                    try
                    {
                        _CallDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }


                }
                }
             
             
            return Json(new[] { _rec }.ToDataSourceResult(request, ModelState));
        }
    }
}