using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using ReportsDatabase;
using System.Diagnostics;
using System.Threading;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class CustomerServicesController : Controller
    {
         public ActionResult Index()
        {
            return View();
        }

   
        public ActionResult CSRCallLog(string str)
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();


            ID_VM id_op = new ID_VM();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                id_op = (from emp in _db.tbl_Operator_Table
                         where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                         select new ID_VM
                         {
                             // name = emp.empFullName,
                             ID = emp.ID
                         }).Take(1).SingleOrDefault();
            }


            CallLogVM _vm = new CallLogVM();
            if (id_op == null) { _vm.OpPermission = true; }
            _vm.errormsg = str;

            _vm.firstTime = false;
            return View(_vm);
        }
        [HttpPost]
        public ActionResult CSRCallLog(CallLogVM _vm)
        {
           // Guid fileid = Guid.NewGuid();
            _vm.firstTime = true;
            _vm.details = AddCSRLog.GetDetails(_vm.Account);
        
            _vm.TimerTxt = "0:0:0";

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',28,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }

            return View(_vm);


        }

        [HttpPost]
        public ActionResult AddCSRCallLog(CallLogVM _vm)
        {

           String _msg = String.Empty;
            int id;
       
            if (_vm != null)
            {

               
             id=   AddCSRLog.AddCallLog(_vm);
             _msg=   AddCSRLog.AddNote_CallLog(_vm , id);
                if (_vm.ComplainOutCome != "" && _vm.ComplainOutCome != null)
                {
                    if (_vm.ComplainOutCome.Contains("Not Resolved Transferred to Team Leaders"))
                    {
                        AddCSRLog.sendEmail(_msg, _vm.Account, id);
                    }
                }
            }
          
            return RedirectToAction("CSRCallLog");
        }



        public ActionResult CSRComlaintlog()
        {
            var components = User.Identity.Name.Split('\\');

            var userName = components.Last();

           

            ID_VM id_op = new ID_VM();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                id_op = (from emp in _db.tbl_Operator_Table
                         where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                         select new ID_VM
                         {
                             // name = emp.empFullName,
                             ID = emp.ID
                         }).Take(1).SingleOrDefault();
            }


            CSRComplaintVM _vm = new CSRComplaintVM();
            if (id_op == null) { _vm.OpPermission = true; }
          
           

          //  _vm.firstTime = false;
            return View(_vm);
        }
        [HttpPost]
        public ActionResult CSRComlaintlog(CSRComplaintVM _vm)
        {
            // Guid fileid = Guid.NewGuid();
            // _vm.firstTime = true;

           if (_vm.newAccount != true)
            {
                _vm = AddCSRLog.GetDetailsofAccountByRef(_vm.id);
           }

            if (_vm.newAccount == true)
            {
                AccountInfoVM acc = new AccountInfoVM();
                acc = AddCSRLog.GetDetails(_vm.id);

              //  acc.firstName = "New Memeber";
                _vm.details = acc;
                _vm.Account = _vm.id;
            }
              _vm.payerid = 0;
            _vm.payerType = new SelectList(AddCSRLog.HDMSPayerInfo(_vm.Account), "payerid", "payerType");
            _vm.payerTypeList = AddCSRLog.HDMSPayerInfo(_vm.Account).ToList();

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',27,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query);
            }

            return View(_vm);


        }



        [HttpPost]
        public ActionResult AddCSRComlaintlog(CSRComplaintVM _vm)
        {
            String _msg = String.Empty;
          
            int id=0; 
            if (_vm != null)
            {


                _vm.payerTypeList = AddCSRLog.HDMSPayerInfo(_vm.Account).ToList();
                _vm.id = AddCSRLog.AddComplaintLog(_vm);
                if (_vm.newAccount == true)
                {
                    CallLogVM _callVM = new CallLogVM();
                    _callVM.Account = _vm.Account;
                    _callVM.TrackingNumber = _vm.TrackingNumber;
                    _callVM.WorkOrder = _vm.WorkOrder;
                    _callVM.Product = _vm.Product;
                    _callVM.DeliveryCompany = _vm.DeliveryCompany;
                    _callVM.BCNProvider = _vm.BCNProvider;

                    _callVM.Damaged = _vm.Damaged;
                    _callVM.WrongProductShipped = _vm.WrongProductShipped;
                    _callVM.QualityOfProduct = _vm.QualityOfProduct;
                    _callVM.Defective = _vm.ProductDefective;
                    _callVM.MissingProduct = _vm.MissingProduct;
                    _callVM.Driver = _vm.Driver;
                    _callVM.DidntFollowDelIns = _vm.DidntFollowDelIns;
                    _callVM.WrongArea = _vm.WrongArea;
                    _callVM.Impolite_Offensive = _vm.ImpoliteORoffensive;
                    _callVM.VConfirmationCalls = _vm.VConfirmationCalls;
                    _callVM.VPaymentCalles = _vm.VPaymentCalles;
                    _callVM.SAJamesPhonePromts = _vm.SAJamesPhonePromts;
                    _callVM.SAJamesSelfService = _vm.SAJamesSelfService;
                    _callVM.VirtualCallBack = _vm.VirtualCallBack;
                    _callVM.Website = _vm.Website;
                    _callVM.HoldTimes = _vm.HoldTimes;
                    _callVM.NoFollowUp = _vm.NoFollowUp;
                    _callVM.NoFollowUpWithMem = _vm.NoFollowUpWithMem;
                    _callVM.ReturnedFromVM = _vm.ReturnedFromVM;
                    _callVM.NeverRecivedSupplies = _vm.NeverRecivedSupplies;
                    _callVM.PhysicianIssue = _vm.PhysicianIssue;
                    _callVM.InsLimitGuidelines = _vm.InsLimitGuidelines;
                    _callVM.BCNProviderIssue = _vm.BCNProviderIssue;
                    _callVM.Other = _vm.Other;
                    _callVM.Others = _vm.Others;


                    id = AddCSRLog.AddCallLog(_callVM);

                    _vm.id = id;
                }


                
                _msg = AddCSRLog.AddNote_ComplaintLog(_vm, id);

                if (_vm.ComplaintHasBeen != "" && _vm.ComplaintHasBeen != null)
                {
                   // if (_vm.ComplaintHasBeen.Contains("Not Resolved Transferred to Management"))
                    if (true)
                    {
                        AddCSRLog.sendComplainLogEmail(_msg, _vm.Account, _vm.id);
                    }
                }
            }
            //return Content("<script type='text/javascript'>window.close();</script>");
            return RedirectToAction("CSRComlaintlog");
        }

        public ActionResult SubmitForm()
        {
            return View();
        }

      

    }
}