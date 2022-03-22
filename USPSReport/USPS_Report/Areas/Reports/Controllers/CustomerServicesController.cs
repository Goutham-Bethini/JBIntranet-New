using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using ReportsDatabase;
using System.Diagnostics;
using System.Threading;
using System.IO;

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
            _vm.payerTypeList = AddCSRLog.HDMSPayerInfo(_vm.Account).ToList();
            _vm.isPayer7or3739= _vm.payerTypeList.Any(i => new List<int> { 7, 3739 }.Contains(i.payerid.Value));
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


                id = AddCSRLog.AddCallLog(_vm);
                _msg = AddCSRLog.AddNote_CallLog(_vm, id);
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

        [HttpGet]
        public ActionResult Download(string FileName)
        {
            try
            {
                var fullPath = Path.Combine(@"\\jbmwix-azfs01\IT\IntranetDocuments\StateAudit$\Files\Complaint log files", FileName);
                string mimeType = System.Web.MimeMapping.GetMimeMapping(FileName);
                // return File(fullPath, "application/vnd.ms-excel", FileName);
                return File(fullPath, mimeType, FileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddCSRComlaintlog(CSRComplaintVM _vm, HttpPostedFileBase file, FormCollection form)
        {
            String _msg = String.Empty;
            //int id = 0;
            int complaintId = 0;
            if (_vm != null)
            {
                _vm.payerTypeList = AddCSRLog.HDMSPayerInfo(_vm.Account).ToList();
                string dir = @"\\jbmwix-azfs01\IT\IntranetDocuments\StateAudit$\Files\Complaint log files";
                if (_vm.newAccount == true)
                {
                    CallLogVM _callVM = new CallLogVM();
                    _callVM.Account = _vm.Account;
                    _callVM.TrackingNumber = _vm.TrackingNumber;
                    _callVM.WorkOrder = _vm.WorkOrder;
                    _callVM.Product = _vm.Product;
                    _callVM.DeliveryCompany = _vm.DeliveryCompany;
                    //_callVM.BCNProvider = _vm.BCNProvider;

                    _callVM.BloodPressureMonitors = _vm.BloodPressureMonitors;
                    _callVM.BreastPumps = _vm.BreastPumps;
                    _callVM.ContGlucoseMonitoring = _vm.ContGlucoseMonitoring;
                    _callVM.DiabeticTestSup = _vm.DiabeticTestSup;
                    _callVM.EnteralNutrition = _vm.EnteralNutrition;
                    _callVM.ExternalDefibrillator = _vm.ExternalDefibrillator;
                    _callVM.IncontinenceSupplies = _vm.IncontinenceSupplies;
                    _callVM.InsulinPumpsSupplies = _vm.InsulinPumpsSupplies;
                    _callVM.InsSyrPenNeed = _vm.InsSyrPenNeed;
                    _callVM.OstomySupplies = _vm.OstomySupplies;
                    _callVM.PleurXDrainSys = _vm.PleurXDrainSys;
                    _callVM.PTINRTesting = _vm.PTINRTesting;
                    _callVM.TENSUnitSup = _vm.TENSUnitSup;
                    _callVM.UrologicalSupplies = _vm.UrologicalSupplies;
                    _callVM.WoundCareSupplies = _vm.WoundCareSupplies;
                    _callVM.OtherUnsureSupplies = _vm.OtherUnsureSupplies;

                    _callVM.BDI = _vm.BDI;
                    _callVM.BPnBPM = _vm.BPnBPM;
                    _callVM.CallCenter = _vm.CallCenter;
                    _callVM.CSRAssessment = _vm.CSRAssessment;
                    _callVM.DynamicSynergy = _vm.DynamicSynergy;
                    _callVM.Enteral = _vm.Enteral;
                    _callVM.HGS = _vm.HGS;
                    _callVM.InsulinPumpCGM = _vm.InsulinPumpCGM;
                    _callVM.MedicalDocuments = _vm.MedicalDocuments;
                    _callVM.NewAccountTeam = _vm.NewAccountTeam;
                    _callVM.Nurses = _vm.Nurses;
                    _callVM.QualityAssurance = _vm.QualityAssurance;
                    _callVM.Shipping = _vm.Shipping;
                    _callVM.THC = _vm.THC;
                    _callVM.Troy = _vm.Troy;
                    _callVM.Verification = _vm.Verification;
                    _callVM.WebSupport = _vm.WebSupport;
                    _callVM.WoundCareOstomyTENS = _vm.WoundCareOstomyTENS;
                    _callVM.OtherUnsureTeam = _vm.OtherUnsureTeam;
                    _callVM.ITHelpDesk = _vm.ITHelpDesk;

                    //_callVM.Damaged = _vm.Damaged;
                    _callVM.Compliance = _vm.Compliance;
                    _callVM.CustomerService = _vm.CustomerService;
                    _callVM.Discrimination = _vm.Discrimination;
                    _callVM.HealthPlan = _vm.HealthPlan;
                    _callVM.ProductDefectiveQuality = _vm.ProductDefectiveQuality;
                    _callVM.ShippingUSPS = _vm.ShippingUSPS;
                    _callVM.ShippingWarehouse = _vm.ShippingWarehouse;
                    _callVM.SmartAction = _vm.SmartAction;
                    _callVM.TextMessaging = _vm.TextMessaging;
                    _callVM.WebsitePortal = _vm.WebsitePortal;
                    _callVM.ProcessDelay = _vm.ProcessDelay;
                    _callVM.PhoneFaxIssues = _vm.PhoneFaxIssues;

                    //_callVM.WrongProductShipped = _vm.WrongProductShipped;
                    //_callVM.QualityOfProduct = _vm.QualityOfProduct;
                    //_callVM.Defective = _vm.ProductDefective;
                    _callVM.MissingProduct = _vm.MissingProduct;
                    //_callVM.Driver = _vm.Driver;
                    //_callVM.DidntFollowDelIns = _vm.DidntFollowDelIns;
                    //_callVM.WrongArea = _vm.WrongArea;
                    //_callVM.Impolite_Offensive = _vm.ImpoliteORoffensive;
                    //_callVM.VConfirmationCalls = _vm.VConfirmationCalls;
                    //_callVM.VPaymentCalles = _vm.VPaymentCalles;
                    //_callVM.SAJamesPhonePromts = _vm.SAJamesPhonePromts;
                    //_callVM.SAJamesSelfService = _vm.SAJamesSelfService;
                    //_callVM.VirtualCallBack = _vm.VirtualCallBack;
                    //_callVM.Website = _vm.Website;
                    //_callVM.HoldTimes = _vm.HoldTimes;
                    //_callVM.NoFollowUp = _vm.NoFollowUp;
                    //_callVM.NoFollowUpWithMem = _vm.NoFollowUpWithMem;
                    //_callVM.ReturnedFromVM = _vm.ReturnedFromVM;
                    //_callVM.NeverRecivedSupplies = _vm.NeverRecivedSupplies;
                    //_callVM.PhysicianIssue = _vm.PhysicianIssue;
                    //_callVM.InsLimitGuidelines = _vm.InsLimitGuidelines;
                    //_callVM.BCNProviderIssue = _vm.BCNProviderIssue;
                    //_callVM.Other = _vm.Other;
                    _callVM.Others = _vm.Others;


                    _vm.id = AddCSRLog.AddCallLog(_callVM);

                    //_vm.id = id;
                }
                complaintId = AddCSRLog.AddComplaintLog(_vm);
                string path = string.Empty;
                if (file != null && file.ContentLength > 0)
                {
                    path = Path.Combine(dir, Path.GetFileName(file.FileName));
                    if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                    {
                        // if(System.IO.File.di)
                        // If file found, delete it    
                        System.IO.File.Delete(path);
                    }
                    //Saving the file  
                    file.SaveAs(path);
                    var components = System.Web.HttpContext.Current.User.Identity.Name.Split('\\');
                    var userName = components.Last();
                    using (IntranetEntities _db = new IntranetEntities())
                    {
                        tbl_CSRComplaintLog_Attachments att = new tbl_CSRComplaintLog_Attachments();
                        att.Account = _vm.Account;
                        att.ComplaintId = complaintId;
                        att.FileName = file.FileName;
                        att.UploadedBy = userName;
                        att.UploadedDate = DateTime.Now;
                        _db.tbl_CSRComplaintLog_Attachments.Add(att);
                        _db.SaveChanges();
                    }
                }

                _msg = AddCSRLog.AddNote_ComplaintLog(_vm, _vm.id);

                if (_vm.ComplaintHasBeen != "" && _vm.ComplaintHasBeen != null && !_vm.ComplaintHasBeen.Contains("Pending Resolution–Supervisor") && !_vm.ComplaintHasBeen.Contains("Pending Resolution–Management"))
                {
                    // if (_vm.ComplaintHasBeen.Contains("Not Resolved Transferred to Management"))
                    if (true)
                    {
                        AddCSRLog.sendComplainLogEmail(_msg, _vm.Account, _vm.id);
                    }
                }
                if (_vm.ComplaintHasBeen != "" && _vm.ComplaintHasBeen != null && _vm.ComplaintHasBeen.Contains("Pending Resolution–Supervisor"))
                {
                    AddCSRLog.sendComplainLogEmailToSupervisors(_msg, _vm.Account, _vm.id);
                }
                if (_vm.ComplaintHasBeen != "" && _vm.ComplaintHasBeen != null && _vm.ComplaintHasBeen.Contains("Pending Resolution–Management"))
                {
                    AddCSRLog.sendComplainLogEmailToManagers(_msg, _vm.Account, _vm.id, _vm);
                }
            }
            //return Content("<script type='text/javascript'>window.close();</script>");
            return RedirectToAction("CSRComlaintlog");
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file, FormCollection form)
        //{

        //}

        public ActionResult SubmitForm()
        {
            return View();
        }



    }
}