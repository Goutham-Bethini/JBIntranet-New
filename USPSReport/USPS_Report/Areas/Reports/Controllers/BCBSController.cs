using System;
using System.Collections.Generic;
using System.Linq;
 using System.Web;
using System.Web.Mvc;
using USPS_Report.Areas.Reports.Models;
using ReportsDatabase;
using System.Diagnostics;
using System.Threading;
using System.Net.Mail;
 using System.Text;
 using System.ComponentModel.DataAnnotations;
 

namespace USPS_Report.Areas.Reports.Controllers
{
    public class BCBSController : Controller
    {
        // GET: Reports/BCBS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BCNCallLog(string str)
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


            BCNCallLogVM _vm = new BCNCallLogVM();
          
            if (id_op == null) { _vm.OpPermission = true; }
            _vm.errormsg = str;

            _vm.firstTime = false;
            return View(_vm);
        }
        [HttpPost]
        public ActionResult BCNCallLog(BCNCallLogVM _vm)
        {
            CallLogInsDetail insDetail = new CallLogInsDetail();
            // Guid fileid = Guid.NewGuid();
            _vm.firstTime = true;
           
                _vm.details = GetBCNDetails(_vm.Account);

            //-------------------
            _vm.ProviderList = new SelectList(GetProviderList(_vm.Account), "ProviderId", "ProviderName");
            //---------------------------
            // Infusion sets lists
            // _vm.Assmnt_InfusionsettxtCGMDW = new SelectList(GetInfusionSetList(), "Assmnt_InfusionsettxtCGMID", "Assmnt_InfusionsettxtCGM");
            _vm.Assmnt_InfusionsettxtCGMDW = GetInfusionSetList();
            //----------------------------------
            // Cartridges lists
            // _vm.Assmnt_catridgestxtCGMDW = new SelectList(GetCartridgesSetList(), "Assmnt_catridgestxtCGMID", "Assmnt_catridgestxtCGM");
            //----------------------------------
            _vm.Assmnt_catridgestxtCGMDW = GetCartridgesSetList();

            _vm.TimerTxt = "0:0:0";
            _vm.InsDetail = insDetail;
            return View(_vm);


        }


        [HttpPost]
        public ActionResult AddBCNCSRCallLog(BCNCallLogVM _vm)
        {

            int? idInsu = _vm.Assmnt_catridgestxtCGMID;
            string infusionSet = _vm.Assmnt_catridgestxtCGM; 
            IntranetEntities _Ndb = new IntranetEntities();
            HHSQLDBEntities _Hdb = new HHSQLDBEntities();
            String _msg = String.Empty;
            BCNAccInfoVM accdetails = new BCNAccInfoVM();
            accdetails = _vm.details;
            if(_vm.Assmnt_InfusionsettxtCGMID != null)
            {
                _vm.Assmnt_InfusionsettxtCGM = _Hdb.tbl_Product_Table.Where(t => t.ID == _vm.Assmnt_InfusionsettxtCGMID).Select(t => t.ProductCode).SingleOrDefault();
            }

            if (_vm.Assmnt_catridgestxtCGMID != null)
            {
                _vm.Assmnt_catridgestxtCGM = _Hdb.tbl_Product_Table.Where(t => t.ID == _vm.Assmnt_catridgestxtCGMID).Select(t => t.ProductCode).SingleOrDefault();
            }
            if (_vm.Providerid == null)
            {
                _vm.Providerid = 9;
            }

            int id;
            _vm.ProviderName = _Ndb.BCBS_ProviderList.Where(t => t.ID == _vm.Providerid).Select(t => t.Name).SingleOrDefault();
            if (_vm != null)
            {

                if (_vm.Newacc != true && _vm.Account != null)
                {
                    _vm.details = GetBCNDetails(_vm.Account);
                    _vm.Mem_FN = _vm.details.firstName;
                    _vm.Mem_LN = _vm.details.lastName;
                    _vm.Mem_Add1 = _vm.details.address1;
                    _vm.Mem_Add2 = _vm.details.address2;
                    _vm.Mem_City = _vm.details.city;
                    _vm.Mem_State = _vm.details.state;
                    _vm.Mem_ZIp = _vm.details.zipcode;
                    _vm.Mem_County = _vm.details.County;
                    _vm.Mem_Phone = _vm.details.phone;
                    _vm.Mem_AltPhone = _vm.details.ALtphone;
                    _vm.Mem_DOB = Convert.ToDateTime(_vm.details.DOB);
                }

                id = AddBCNCallLog(_vm);
                _msg =  AddBCNNote_CallLog(_vm, id);
                if (_vm.ComplainOutCome != "" && _vm.ComplainOutCome != null)
                {
                    if (_vm.ComplainOutCome.Contains("Not Resolved Transferred to Team Leaders"))
                    {
                        sendEmail(_msg, _vm.Account, id);
                    }
                }
            }

            return RedirectToAction("BCNCallLog");
        }

        public static int AddBCNCallLog(BCNCallLogVM _vm)
        {
            String _msg = String.Empty;
            int id = 0;
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {
                    // Guid fileid = Guid.NewGuid();
                    var components = System.Web.HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();
                    //added deelte after
                    DateTime dtsx = DateTime.Now;
                    //added end

                    tbl_BCNCallLog _rec = new tbl_BCNCallLog();
                    _rec.Mem_FirstName = _vm.Mem_FN;
                    _rec.Mem_LastName = _vm.Mem_LN;
                    _rec.Mem_Address1 = _vm.Mem_Add1;
                    _rec.Mem_Address2 = _vm.Mem_Add2;
                    _rec.Mem_City = _vm.Mem_City;
                    _rec.Mem_State = _vm.Mem_State;
                    _rec.Mem_ZipCode = _vm.Mem_ZIp;
                    _rec.Mem_DOB = _vm.Mem_DOB;
                    _rec.Mem_County = _vm.Mem_County;
                 //   _rec.Primary_Physician = _vm.PriPhysician; commneted by pradeep
                    //modified to providername pradeep
                    //    _rec.Provider = _vm.Provider;
                    _rec.Provider = _vm.ProviderName;
                    _rec.Phone = _vm.Phone;
                    //  _rec.RefNum = _vm.RefNum; 
                    //added pradeep 11/30                 
                    _rec.Mem_Gender = _vm.details.Gender;
                    _rec.Primary_Physician = _vm.details.PrimaryPhysician;
                    _rec.WebAccountCreated = _vm.WebAccountCreated;
                    _rec.RxForm = _vm.RxForm;
                    _rec.AOBReceived = _vm.AOBReceived;
                    _rec.RXReceived = _vm.RXReceived;
                    _rec.PASent = _vm.PASent;
                    _rec.PAReceived = _vm.PAReceived;
                    _rec.ClinicalDocsReq = _vm.ClinicalDocsReq;
                    _rec.ClinicalDocsReceived = _vm.ClinicalDocsReceived;
                    _rec.Manufacturer = _vm.Manufacturer;
                    _rec.OrderStatus = _vm.OrderStatus;
                    _rec.VerifiedPHI = _vm.VerifiedPHI;
                    _rec.VoiceForHealth = _vm.VoicesForHealth;
                    //added pradeep 11/30 end
                    //LogType
                    _rec.Call = _vm.Call;
                    _rec.Task = _vm.Task;
                    _rec.Web = _vm.Web;
                    _rec.Email = _vm.Email;
                    _rec.Fax = _vm.Fax;
                    _rec.LogTypeOther = _vm.LogTypeOther;
                    //Account
                    _rec.Account = Convert.ToInt32(_vm.Account);
                    _rec.CreatedOn = DateTime.Now;
                    _rec.CreatedBy = userName;
                    _rec.CallDuration = _vm.TimerTxt;
                    _rec.Name = _vm.Name;
                    _rec.Relation = _vm.Relation;
                    _rec.OtherRelName = _vm.otherRelname;               
                    //Billing
                    _rec.BillingTxt = _vm.BillingTxt;
                    _rec.Copay = _vm.Copay;
                    _rec.MadePayment = _vm.MadePayment;
                    _rec.Billing = _vm.Billing;
                  

                    //Demographic
                    _rec.DemographicChangesTxt = _vm.DemographicChanges;
                    _rec.Address = _vm.Address;
                    _rec.Physician = _vm.Physician;
                    _rec.Phone = _vm.Phone;
                    //Documentation
                    _rec.DocumentationTxt = _vm.DocumentationTxt;                                      
                    _rec.AOB = _vm.AOB;
                    _rec.Prescription = _vm.Prescription;
                    _rec.PriorAuthorization = _vm.PriorAuthorization;
                    _rec.SupportingDoc = _vm.SupportingDoc;
                    //Insurance Changes
                    _rec.InsuarnceChanges_ = _vm.InsuarnceChanges;
                    //added pradeep 11/30
                    _rec.EligMedicare = _vm.EligMedicare;
                    _rec.EligMedicaid = _vm.EligMedicaid;                  
                    _rec.EligOther = _vm.EligOther;
                
                    //if medicare is selected
                    if (_vm.EligMedicare == true)
                    {
                        tbl_CSRInsDetail ins = new tbl_CSRInsDetail();
                        //generate a new id, insert into csinsdetailcopy tbl
                        string uids = Guid. NewGuid().ToString();
                       ins.id = uids;
                        ins.Name = _vm.InsDetail.NameMedicare;
                        ins.State = _vm.InsDetail.StateMedicare;
                        ins.VerifiedThrough = _vm.InsDetail.VerifiedThroughMedicare;
                        ins.NameMatched = _vm.InsDetail.NameMatchedMedicare;
                        ins.NameNotMatched = _vm.InsDetail.NameNotMatchedMedicare;
                        ins.EffectiveDate= _vm.InsDetail.EffectiveDateMedicare;
                        ins.TermDate = _vm.InsDetail.TermDateMedicare;
                        ins.BenefitPlan = _vm.InsDetail.BenefitPlanMedicare;
                        ins.LevelOfCare = _vm.InsDetail.LevelOfCareMedicare;
                        ins.COBnOtherIns = _vm.InsDetail.COBnOtherInsMedicare;
                        ins.InNetwork = _vm.InsDetail.InNetworkMedicare;
                        ins.OutofNetwork= _vm.InsDetail.OutofNetworkMedicare;
                        ins.Deductible= (decimal)_vm.InsDetail.DeductibleMedicare;
                        ins.OutofPocketMax = (decimal)_vm.InsDetail.OutofPocketMaxMedicare;
                        ins.CopayCoins = _vm.InsDetail.CopayCoinsMedicare;
                        ins.CopayCoinsWaived = _vm.InsDetail.CopayCoinsWaivedMedicare;
                        ins.CopayCoinsNotWaived = _vm.InsDetail.CopayCoinsNotWaivedMedicare;
                        ins.DME = _vm.InsDetail.DMEMedicare;
                        ins.Medical = _vm.InsDetail.MedicalMedicare;
                        ins.Pharmacy = _vm.InsDetail.PharmacyMedicare;
                        ins.Medicare = _vm.InsDetail.MedicareMedicare;
                        ins.Medicaid = _vm.InsDetail.MedicaidMedicare;
                        ins.Other = _vm.InsDetail.OtherMedicare;
                        ins.HCPCnQtyLimitations = _vm.InsDetail.HCPCnQtyLimitationsMedicare;
                        ins.HHE = _vm.InsDetail.HHEMedicare;
                        ins.Hospice = _vm.InsDetail.HospitalMedicare;
                        ins.Hospital = _vm.InsDetail.HospitalMedicare;
                        ins.NursingHome = _vm.InsDetail.Nursing_HomeMedicare;
                        ins.MemEnrollNone = _vm.InsDetail.NoneMedicare;
                        //insert into db
                        _db.tbl_CSRInsDetail.Add(ins);
                        _db.SaveChanges();
                        //set the bcncalllog insmedicareid to uid
                        _rec.InsIdMedicare = uids;
                    } //medicare
                      //if medicaid is selected
                    if (_vm.EligMedicaid == true)
                    {
                        tbl_CSRInsDetail ins = new tbl_CSRInsDetail();
                        //generate a new id, insert into csinsdetailcopy tbl
                        string uids =  Guid.NewGuid().ToString();
                        ins.id = uids;
                        ins.Name = _vm.InsDetail.NameMedicaid;
                        ins.State = _vm.InsDetail.StateMedicaid;
                        ins.VerifiedThrough= _vm.InsDetail.VerifiedThroughMedicaid;
                        ins.NameMatched= _vm.InsDetail.NameMatchedMedicaid;
                        ins.NameNotMatched = _vm.InsDetail.NameNotMatchedMedicaid;
                        ins.EffectiveDate = _vm.InsDetail.EffectiveDateMedicaid;
                        ins.TermDate = _vm.InsDetail.TermDateMedicaid;
                        ins.BenefitPlan = _vm.InsDetail.BenefitPlanMedicaid;
                        ins.LevelOfCare = _vm.InsDetail.LevelOfCareMedicaid;
                        ins.COBnOtherIns = _vm.InsDetail.COBnOtherInsMedicaid;
                        ins.InNetwork = _vm.InsDetail.InNetworkMedicaid;
                        ins.OutofNetwork= _vm.InsDetail.OutofNetworkMedicaid;
                        ins.Deductible = (decimal)_vm.InsDetail.DeductibleMedicaid;
                        ins.OutofPocketMax = (decimal)_vm.InsDetail.OutofPocketMaxMedicaid;
                        ins.CopayCoins = _vm.InsDetail.CopayCoinsMedicaid;
                        ins.CopayCoinsWaived = _vm.InsDetail.CopayCoinsWaivedMedicaid;
                        ins.CopayCoinsNotWaived = _vm.InsDetail.CopayCoinsNotWaivedMedicaid;
                        ins.DME = _vm.InsDetail.DMEMedicaid;
                        ins.Medical = _vm.InsDetail.MedicalMedicaid;
                        ins.Pharmacy = _vm.InsDetail.PharmacyMedicaid;
                        ins.Medicare = _vm.InsDetail.MedicareMedicaid;
                        ins.Medicaid = _vm.InsDetail.MedicaidMedicaid;
                        ins.Other = _vm.InsDetail.OtherMedicaid;
                        ins.HCPCnQtyLimitations = _vm.InsDetail.HCPCnQtyLimitationsMedicaid;
                        ins.HHE = _vm.InsDetail.HHEMedicaid;
                        ins.Hospice = _vm.InsDetail.HospiceMedicaid;
                        ins.Hospital = _vm.InsDetail.HospitalMedicaid;
                        ins.NursingHome = _vm.InsDetail.Nursing_HomeMedicaid;
                        ins.MemEnrollNone = _vm.InsDetail.NoneMedicaid;
                        //insert into db
                        _db.tbl_CSRInsDetail.Add(ins);
                        _db.SaveChanges();
                        //set the bcncalllog insmedicareid to uid
                        _rec.InsIdMedicaid = uids;
                    } //medicaid
                      //if other is selected
                    if (_vm.EligOther == true)
                    {
                        //generate a new id, insert into csinsdetailcopy tbl
                        tbl_CSRInsDetail ins = new tbl_CSRInsDetail();
                        string uids = Guid.NewGuid().ToString();
                       ins.id = uids;
                        ins.Name = _vm.InsDetail.Name;
                        ins.State = _vm.InsDetail.State;
                        ins.VerifiedThrough = _vm.InsDetail.VerifiedThrough;
                        ins.NameMatched = _vm.InsDetail.NameMatched;
                        ins.NameNotMatched = _vm.InsDetail.NameNotMatched;
                        ins.EffectiveDate = _vm.InsDetail.EffectiveDate;
                        ins.TermDate = _vm.InsDetail.TermDate;
                        ins.BenefitPlan = _vm.InsDetail.BenefitPlan;
                        ins.LevelOfCare = _vm.InsDetail.LevelOfCare;
                        ins.COBnOtherIns = _vm.InsDetail.COBnOtherIns;
                        ins.InNetwork = _vm.InsDetail.InNetwork;
                        ins.OutofNetwork = _vm.InsDetail.OutofNetwork;
                        ins.Deductible = (decimal)_vm.InsDetail.Deductible;
                        ins.OutofPocketMax = (decimal)_vm.InsDetail.OutofPocketMax;
                        ins.CopayCoins = _vm.InsDetail.CopayCoins;
                        ins.CopayCoinsWaived = _vm.InsDetail.CopayCoinsWaived;
                        ins.CopayCoinsNotWaived = _vm.InsDetail.CopayCoinsNotWaived;
                        ins.DME = _vm.InsDetail.DME;
                        ins.Medical = _vm.InsDetail.Medical;
                        ins.Pharmacy = _vm.InsDetail.Pharmacy;
                        ins.Medicare = _vm.InsDetail.Medicare;
                        ins.Medicaid = _vm.InsDetail.Medicaid;
                        ins.Other = _vm.InsDetail.Other;
                        ins.HCPCnQtyLimitations = _vm.InsDetail.HCPCnQtyLimitations;
                        ins.HHE = _vm.InsDetail.HHE;
                        ins.Hospice = _vm.InsDetail.Hospice;
                        ins.Hospital = _vm.InsDetail.Hospital;
                        ins.NursingHome = _vm.InsDetail.Nursing_Home;
                        ins.MemEnrollNone = _vm.InsDetail.None;
                        //insert into db
                        _db.tbl_CSRInsDetail.Add(ins);
                        _db.SaveChanges();
                        //set the bcncalllog insmedicareid to uid
                        _rec.InsIdOther = uids;
                    }
                    //added pradeep 11/30 end
                    //
                    _rec.InsuranceChangeTxt = _vm.InsuranceChangeTxt;
                    //New Account/Restart
                    _rec.NewAcconunt = _vm.NewAccount;
                    _rec.Restart = _vm.Restart;
                    _rec.TypeSupplies_1 = _vm.TypeSupplies1;
                    _rec.TypeSupplies_2 = _vm.TypeSupplies2;
                    _rec.TypeSuppliesOther = _vm.TypeSuppliesOther;
                    _rec.NewAccountTxt = _vm.NewAccountTxtArea;
                    //Order Confirmation
                    _rec.OrderConfirmation = _vm.OrderConfirmation;
                    _rec.OrderConfirmationTxt = _vm.OrderConfirmationTxt;                           
                    //Order Status
                    _rec.OrderStatusTxt = _vm.OrderStatusTxt;
                    _rec.FedExOrUSPSTracking = _vm.FedExOrUSPSTracking;
                    //added pradeep
                    _rec.FedExOrUSPSTrackingNumber = _vm.FedExOrUSPSTrackingNumber;
                    //added end
                    _rec.OrderShipped = _vm.OrderShipped;
                    _rec.OrderDropShipped = _vm.OrderDropShipped;
                    _rec.OrderETA = _vm.OrderETA;
                    _rec.OrderHolding = _vm.OrderHolding;
                    _rec.RWOCreated = _vm.RWOCreated;
                    //RWO Changes
                    _rec.PC_IncreaseOrDecrease = _vm.PC_IncreaseOrDecrease;
                    _rec.PC_Hold = _vm.PC_Hold;
                    _rec.PC_RemoveOrAdd = _vm.PC_RemoveOrAdd;
                    _rec.ProductChange = _vm.ProductChange;
                    _rec.RWOChangeTxt = _vm.ProductChangeTxt;
                    //Sample
                    _rec.SampleTxt = _vm.SampleTxt;
                    _rec.SampleChoice = _vm.SampleChoice;
                    _rec.SampleOrderSent = _vm.SampleOrderSent;
                    _rec.SampleTask = _vm.SampleTask;
                    //Shipping Issue
                    _rec.DefectiveProductOrNotUsable = _vm.DefectiveProductOrNotUsable;
                    _rec.WrongOrExtraProductShipped = _vm.WrongOrExtraProductShipped;
                    _rec.MissingProduct = _vm.MissingProduct;
                    _rec.ShippingOther = _vm.Sh_Other;
                    //added pradeep 11/30
                    _rec.ShippingOthersName = _vm.ShippingOtherName;
                    //added end
                    _rec.ShippingIssueTxt = _vm.ShippingIssueTxt;                
                    //Transferred Call
                    _rec.TransferredCall = _vm.TransferredCall;
                    _rec.TransferredCallTxt = _vm.TransferredCallTxtArea;
                    //Complaints
                    _rec.TrackingNumber = _vm.TrackingNumber;
                    _rec.WorkOrder = _vm.WorkOrder;
                   // _rec.Product = _vm.Product;
                  //  _rec.DeliveryCompany = _vm.DeliveryCompany;
                 //   _rec.BCNProvider = _vm.BCNProvider;

                    _rec.Damaged = _vm.Damaged;
                    _rec.WrongProductShipped = _vm.WrongProductShipped;
                    _rec.QualityOfProdut = _vm.QualityOfProduct;
                    _rec.ProductDefective = _vm.Defective;
                    _rec.Complain_MissingProduct = _vm.Complain_MissingProduct;
                    _rec.Driver = _vm.Driver;
                 //   _rec.DidntFollowDelIns = _vm.DidntFollowDelIns;
                    _rec.WrongArea = _vm.WrongArea;
                    _rec.Incorrect = _vm.Incorrect;
                    _rec.MisPick = _vm.Mispick;
                    _rec.ImpoliteORoffensive = _vm.Impolite_Offensive;
                 //   _rec.VConfirmationCalls = _vm.VConfirmationCalls;
                  //  _rec.VPaymentCalles = _vm.VPaymentCalles;
                    _rec.Julie_VictorCalls = _vm.Julie_VictorCalls;
                    _rec.PhonePrompts_SelfService = _vm.PhonePrompts_SelfService;
                   // _rec.VirtualCallBack = _vm.VirtualCallBack;
                   // _rec.Website = _vm.Website;
                    _rec.HoldTimes = _vm.HoldTimes;
                    _rec.NoFollowUp = _vm.NoFollowUp;
                  //  _rec.ReturnedFromVM = _vm.ReturnedFromVM;
                    _rec.NoPresORCMN = _vm.NoPresORCMN;
                  //  _rec.NeverRecivedSupplies = _vm.NeverRecivedSupplies;
                  //  _rec.PhysicianIssue = _vm.PhysicianIssue;
                  //  _rec.InsLimitGuidelines = _vm.InsLimitGuidelines;
                 //   _rec.BCNProviderIssue = _vm.BCNProviderIssue;
                    _rec.Other = _vm.Other;
                    _rec.ComplaintsOthers = _vm.Others;
                    _rec.OtherHandlingTxt = _vm.OtherHandlingTxt;
                    //   Complaint Outcome
                    _rec.ComplaintOutcome = _vm.ComplainOutCome;
                    //Other call handling
                    _rec.ReturnedCall_LeftVoicemail = _vm.ReturnedCall_LeftVoicemail;
                    _rec.WrongNumber = _vm.WrongNumber;
                    _rec.AccountDeactivate = _vm.AccountDeactivated;
                    _rec.SentReqPurchasing = _vm.SentReqPurchasing;
                    _rec.Other_CallHandling = _vm.Other_CallHandling;
                    //added pradeep 11/30
                    _rec.OtherHandlingTxt = _vm.OtherHandlingTxt;
                    //added end
                    //      
                    // _rec.CMN = _vm.CMN; commented not needed

                    // //commented pradeep not needed      
                    //  _rec.TeacherLetter = _vm.TeacherLetter;
                    //  _rec.Logs = _vm.Logs;
                    //  _rec.ABN = _vm.ABN;

                    // _rec.Eligibility = _vm.Eligibility;                                 
                    //   _rec.FedExTxt = _vm.FedExTextArea;
                    //     _rec.ProductIncrease = _vm.Incorrect; // change to product incorrect
                    //    _rec.ProductMispick = _vm.Mispick;               
                    //   _rec.ProductTxt = _vm.ProductTextArea;              
                    //   _rec.customerServiceTxt = _vm.CustomerServiceTextArea;
                    //   _rec.OtherTxt = _vm.Others;                 
                    //            
                    //    _rec.LMN = _vm.LMN;

                    //---------------------------new update---------------------------

                    //added for assessmnet 01/31/2018

                    _rec.Assmnt_completedtxt = _vm.Assmnt_completedtxt;
                    _rec.Assmnt_reltomember = _vm.Assmnt_reltomember;
                    _rec.Assmnt_edudoneby = _vm.Assmnt_edudoneby;
                    _rec.Assmnt_memtrained = _vm.Assmnt_memtrained;
                    _rec.assmnt_Diab = _vm.assmnt_Diab;
                    _rec.assmnt_Insulin = _vm.assmnt_Insulin;
                    //added end

                    //added pradeep 02/14/2018

                    //Assessment
                    _rec.Assmnt_completedtxt = _vm.Assmnt_completedtxt;
                    _rec.Assmnt_reltomember = _vm.Assmnt_reltomember;
                    _rec.Assmnt_edudoneby = _vm.Assmnt_edudoneby;
                    _rec.Assmnt_memtrained = _vm.Assmnt_memtrained;
                    _rec.Assmnt_physicianforsupp = _vm.Assmnt_physicianforsupp;
                    _rec.Assmnt_currsuppfrom = _vm.Assmnt_currsuppfrom;
                    _rec.Assmnt_lastorderrecieveddt = _vm.Assmnt_lastorderrecieveddt;
                    _rec.Assmnt_supporderfrom = _vm.Assmnt_supporderfrom;
                    _rec.Assmnt_Ord30or90 = _vm.Assmnt_Ord30or90;
                    _rec.Assmnt_remsupplies = _vm.Assmnt_remsupplies;

                    //Diabetic
                    _rec.Assmnt_neworexistdiab = _vm.Assmnt_neworexistdiab;
                    _rec.Assmnt_Testingtimesdiab = _vm.Assmnt_Testingtimesdiab;
                    _rec.Assmnt_InsulinTreateddiab = _vm.Assmnt_InsulinTreateddiab;
                    _rec.Assmnt_nameofinsulindiab = _vm.Assmnt_nameofinsulindiab;
                    _rec.Assmnt_nameofinsulindiab_Other = _vm.Assmnt_nameofinsulindiab_Other;
                    _rec.Assmnt_pregduedatediab = _vm.Assmnt_pregduedatediab;
                    _rec.Assmnt_pregexistingdiab = _vm.Assmnt_pregexistingdiab == true? "Yes" : "No";
                    _rec.Assmnt_currmeterdiab = _vm.Assmnt_currmeterdiab;
                    _rec.Assmnt_meterservicediab = _vm.Assmnt_meterservicediab;
                    _rec.Assmnt_diffmeterdiab = _vm.Assmnt_diffmeterdiab;
                    _rec.Assmnt_talkingmeterdiab = _vm.Assmnt_talkingmeterdiab;
                    _rec.Assmnt_lancetsservicediab = _vm.Assmnt_lancetsservicediab;
                    _rec.Assmnt_injsuppliesdiab = _vm.Assmnt_injsuppliesdiab;
                    _rec.Assmnt_injfromothersuppdiab = _vm.Assmnt_injfromothersuppdiab;
                    _rec.Assmnt_injothersupptxtdiab = _vm.Assmnt_injothersupptxtdiab;
                    _rec.Assmnt_syrwtneedlediab = _vm.Assmnt_syrwtneedlediab;
                    _rec.Assmnt_syrwtneedle_gaugediab = _vm.Assmnt_syrwtneedle_gaugediab;
                    _rec.Assmnt_syrwtneedle_lendiab = _vm.Assmnt_syrwtneedle_lendiab;
                    _rec.Assmnt_syrwtneedle_unitsdiab = _vm.Assmnt_syrwtneedle_unitsdiab;
                    _rec.Assmnt_syrwtneedle_qtydiab = _vm.Assmnt_syrwtneedle_qtydiab;
                    _rec.Assmnt_needleonly_diab = _vm.Assmnt_needleonly_diab;
                    _rec.Assmnt_needleonly_gauge = _vm.Assmnt_needleonly_gauge;
                    _rec.Assmnt_needleonly_len = _vm.Assmnt_needleonly_len;
                    _rec.Assmnt_needleonly_qty = _vm.Assmnt_needleonly_qty;
                    _rec.Assmnt_alcoholwipes = _vm.Assmnt_alcoholwipes;
                    _rec.assmnt_ketonediab = _vm.assmnt_ketonediab;
                    _rec.Assmnt_alcoholwp_othersuppdiab = _vm.Assmnt_alcoholwp_othersuppdiab;
                    _rec.Assmnt_UrineKetonediab = _vm.Assmnt_UrineKetonediab;
                    _rec.Assmnt_UrineKetone_Freqtestingdiab = _vm.Assmnt_UrineKetone_Freqtestingdiab;
                    _rec.Assmnt_BloodKetonediab = _vm.Assmnt_BloodKetonediab;
                    _rec.Assmnt_BloodKetone_Freqtestingdiab = _vm.Assmnt_BloodKetone_Freqtestingdiab;

                    //Insulin Pump
                    _rec.Assmnt_diagnosedCGM = _vm.Assmnt_diagnosedCGM;
                    _rec.Assmnt_InsTreatedCGM = _vm.Assmnt_InsTreatedCGM;
                    _rec.Assmnt_InsTreatedTypeCGM = _vm.Assmnt_InsTreatedTypeCGM;
                    _rec.Assmnt_reqpumpCGM = _vm.Assmnt_reqpumpCGM;
                    _rec.Assmnt_reqpumpsuppCGM = _vm.Assmnt_reqpumpsuppCGM;
                    _rec.Assmnt_pumpothersuppCGM = _vm.Assmnt_pumpothersuppCGM;               
                   _rec.Assmnt_pumpothersupptxtCGM = _vm.Assmnt_pumpothersupptxtCGM;
                    _rec.Assmnt_neworreplacementCGM = _vm.Assmnt_neworreplacementCGM;
                    _rec.Assmnt_manufacturerCGM = _vm.Assmnt_manufacturerCGM;
                    _rec.Assmnt_currpumpnmCGM = _vm.Assmnt_currpumpnmCGM;
                    _rec.Assmnt_serialnumCGM = _vm.Assmnt_serialnumCGM;
                    _rec.Assmnt_outofwarrantydtCGM = _vm.Assmnt_outofwarrantydtCGM;
                    _rec.Assmnt_InsurancepaidforCGM = _vm.Assmnt_InsurancepaidforCGM;
                    _rec.Assmnt_NewPumpNameCGM = _vm.Assmnt_NewPumpNameCGM;
                    _rec.Assmnt_NewPumpColorCGM = _vm.Assmnt_NewPumpColorCGM;
                    _rec.Assmnt_PumpReplacereasonCGM = _vm.Assmnt_PumpReplacereasonCGM;
                    _rec.Assmnt_meterusedCGM = _vm.Assmnt_meterusedCGM;
                    _rec.Assmnt_InfusionsetCGM = _vm.Assmnt_InfusionsetCGM;
                    _rec.Assmnt_InfusionsettxtCGM = _vm.Assmnt_InfusionsettxtCGM;                             
                     _rec.Assmnt_catridgesCGM = _vm.Assmnt_catridgesCGM;                    
                    _rec.Assmnt_catridgestxtCGM = _vm.Assmnt_catridgestxtCGM;
                    _rec.Assmnt_catridgesoftnchngtxtCGM = _vm.Assmnt_catridgesoftnchngtxtCGM;                              
                    _rec.Assmnt_BarrierWipesCGM = _vm.Assmnt_BarrierWipesCGM;
                    _rec.Assmnt_RemoverWipesCGM = _vm.Assmnt_RemoverWipesCGM;
                    _rec.Assmnt_AlcoholWipesCGM = _vm.Assmnt_AlcoholWipesCGM;
                    _rec.Assmnt_TransparentdressingCGM = _vm.Assmnt_TransparentdressingCGM;
                    _rec.Assmnt_BatteriesCGM = _vm.Assmnt_BatteriesCGM;
                    _rec.Assmnt_memownedorusedCGM = _vm.Assmnt_memownedorusedCGM;
                    _rec.Assmnt_memcurronCGM = _vm.Assmnt_memcurronCGM;
                    _rec.Assmnt_transmitter_dtreceivedCGM = _vm.Assmnt_transmitter_dtreceivedCGM;
                    _rec.Assmnt_transmitter_outofwarrantydtCGM = _vm.Assmnt_transmitter_outofwarrantydtCGM;
                    _rec.Assmnt_transmitter_serialnoCGM = _vm.Assmnt_transmitter_serialnoCGM;
                    _rec.Assmnt_receiver_ProductCode = _vm.Assmnt_receiver_ProductCode;
                    _rec.Assmnt_transmitter_ProductCode = _vm.Assmnt_transmitter_ProductCode;
                    _rec.Assmnt_receiver_dtreceivedCGM = _vm.Assmnt_receiver_dtreceivedCGM;
                    _rec.Assmnt_receiver_outofwarrantydtCGM = _vm.Assmnt_receiver_outofwarrantydtCGM;
                    _rec.Assmnt_receiver_serialnoCGM = _vm.Assmnt_receiver_serialnoCGM;
                    _rec.Assmnt_memneedtransmitterCGM = _vm.Assmnt_memneedtransmitterCGM;
                    _rec.Assmnt_memneedreceiverCGM = _vm.Assmnt_memneedreceiverCGM;
                    _rec.Assmnt_memneedsensorsCGM = _vm.Assmnt_memneedsensorsCGM;
                    _rec.Assmnt_othersupplierCGM = _vm.Assmnt_othersupplierCGM;                  
                        _rec.Assmnt_othersuppliertxtCGM = _vm.Assmnt_othersuppliertxtCGM;                                 
                    _rec.Assmnt_transmittertypeCGM = _vm.Assmnt_transmittertypeCGM;
                    _rec.Assmnt_receivertypeCGM = _vm.Assmnt_receivertypeCGM;
                    _rec.Assmnt_transmitorreceiver_replacementreasonCGM = _vm.Assmnt_transmitorreceiver_replacementreasonCGM;
                    _rec.Assmnt_sensorsforprodcodeCGM = _vm.Assmnt_sensorsforprodcodeCGM;
                    _rec.Assmnt_memawareofsignCGM = _vm.Assmnt_memawareofsignCGM;


                    //added end

                    //-----------------------------------------------------------------

                    _db.tbl_BCNCallLog.Add(_rec);


                    try { }
                    catch (Exception ex) { var msg = ex.Message; }

                    _db.SaveChanges();
                    id = _rec.id;
                }
            }
            catch (Exception ex)
            {
                _msg = ex.Message;
            }
            return id;
        }

        public static string AddBCNNote_CallLog(BCNCallLogVM _vm, int reference)
        {


            StringBuilder otherStr = new StringBuilder();

            otherStr.Append("Call Regarding = ");

            //  string noteString = "";
            StringBuilder noteString = new StringBuilder();
            try
            {
                ID_VM id_op = new ID_VM();

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = System.Web.HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();


                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);

                    if (_vm.Name != null && _vm.Name != "")
                        noteString.Append("Spoke To = " + _vm.Name + Environment.NewLine);
                    // noteString = noteString + "Spoke To = " + _vm.Name + "_";

                    if (_vm.Relation != null && _vm.Relation != "")
                        noteString.Append("Relation = " + _vm.Relation + Environment.NewLine);
                    // noteString = noteString + "Relation = " + _vm.Relation + "_";

                    if (_vm.otherRelname != null && _vm.otherRelname != "")
                        noteString.Append("Relation = " + _vm.otherRelname + Environment.NewLine);
                    // noteString = noteString + "Relation = " + _vm.otherRelname + "_";

                    if (_vm.VerifiedPHI == true)
                        noteString.Append("VerifiedPHI" + Environment.NewLine);

                    if (_vm.VoicesForHealth == true)
                        noteString.Append("Voices For Health" + Environment.NewLine);

                    if (_vm.WebAccountCreated != null)
                        noteString.Append("Web Account Created = " + _vm.WebAccountCreated + Environment.NewLine);

                    //noteString = noteString + "VerifiedPHI_";
                    if (_vm.ProviderName !=null)
                        noteString.Append("Provider Name = " +_vm.ProviderName + Environment.NewLine);

                    if (_vm.Manufacturer != null)
                        noteString.Append("Manufacturer = " + _vm.Manufacturer + Environment.NewLine);

                    if (_vm.Call == true || _vm.Task == true || _vm.Fax == true || _vm.Web == true || _vm.Email == true || _vm.RxForm == true || _vm.LogTypeOther == true)
                    {
                        if (_vm.Call == true)
                            noteString.Append("LogType - Call " + Environment.NewLine);
                        if (_vm.Task == true)
                            noteString.Append("LogType - Task " + Environment.NewLine);
                        if (_vm.Fax == true)
                            noteString.Append("LogType - Fax " + Environment.NewLine);
                        if (_vm.Web == true)
                            noteString.Append("LogType - Web " + Environment.NewLine);
                        if (_vm.Email == true)
                            noteString.Append("LogType - Email " + Environment.NewLine);
                        if (_vm.RxForm == true)
                            noteString.Append("LogType - Rx/Referral Form " + Environment.NewLine);
                        if (_vm.LogTypeOther == true)
                            noteString.Append("LogType - Other " + Environment.NewLine);

                    }
                    else
                    {
                        noteString.Append("LogType - None " + Environment.NewLine);
                    }



                    //IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();

                    //_notelist = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && (t.NoteHeading == "SAMPLE CHOICE" || t.NoteHeading == "CO-PAY COLLECTIONS" || t.NoteHeading == "BILLING"
                    //|| t.NoteHeading == "COMMMUNICATION" || t.NoteHeading == "SHIPPING ISSUE" || t.NoteHeading == "PRODUCT" || t.NoteHeading == "ORDER HELD"
                    //|| t.NoteHeading == "ORDER CONFIRMATION" || t.NoteHeading == "NEW ACCOUNT" || t.NoteHeading == "INSURANCE" || t.NoteHeading == "OVER QUANTITY DOCUMENTATION"
                    //|| t.NoteHeading == "PRIOR AUTH" || t.NoteHeading == "CMN" || t.NoteHeading == "PRESCRIPTI0N" || t.NoteHeading == "AOB" || t.NoteHeading == "DEMOGRAPHICS "
                    //|| t.NoteHeading == "ACTIVE/INACTIVE ")).ToList();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    // CO-PAY COLLECTIONS 
                    if ( (_vm.Copay == true || _vm.MadePayment == true) && (_vm.BillingTxt != "" && _vm.BillingTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "CO-PAY COLLECTIONS";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 14;
                            _db.tbl_Account_Note.Add(_tbl);
                            try
                            {
                                _db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                string str = ex.Message;
                            }


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                            // Environment.UserName1


                        }

                        // web account created


                        if (_note != null)
                        {

                            StringBuilder _msgStr1 = new StringBuilder();
                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr1.Append(noteString);
                            _msgStr1.Append("Call Regarding = ");
                            if(_vm.Copay == true)
                                _msgStr1.Append("Copay");
                            if (_vm.MadePayment == true)
                                _msgStr1.Append("- Made Payment");
                            _msgStr1.Append( Environment.NewLine + "Note = " + _vm.BillingTxt);

                            _tHist.NoteText = _msgStr1.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Copay_" + "Note = " + _vm.BillingTxt;



                            _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    // CO-PAY COLLECTIONS  : Made payments
                   /* if (_vm.MadePayment == true && (_vm.BillingTxt != "" && _vm.BillingTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "CO-PAY COLLECTIONS";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 14;
                            _db.tbl_Account_Note.Add(_tbl);
                            try
                            {
                                _db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                string str = ex.Message;
                            }


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                            // Environment.UserName1


                        }

                        // web account created


                        if (_note != null)
                        {

                            StringBuilder _msgStr1 = new StringBuilder();
                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr1.Append(noteString);
                            _msgStr1.Append("Call Regarding = Payment" + Environment.NewLine + "Note = " + _vm.BillingTxt);

                            _tHist.NoteText = _msgStr1.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Copay_" + "Note = " + _vm.BillingTxt;



                            _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    } */
                    // BILLING
                    if (_vm.Billing == true && (_vm.BillingTxt != "" && _vm.BillingTxt != null))
                    {


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "BILLING").FirstOrDefault(); // && t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "BILLING";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 5;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "BILLING").FirstOrDefault(); // && t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            StringBuilder _msgStr2 = new StringBuilder();

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;


                            _msgStr2.Append(noteString);
                            _msgStr2.Append("Call Regarding = Billing" + Environment.NewLine + "Note = " + _vm.BillingTxt);

                            _tHist.NoteText = _msgStr2.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = Billing_" + "Note = " + _vm.BillingTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);
                            //try
                            //{
                            //    _db.SaveChanges();
                            //}
                            //catch (Exception Ex)
                            //{
                            //    string msg = Ex.Message;

                            //}
                        }
                    }

                    // COMMMUNICATION
                    if (_vm.Eligibility == true && (_vm.InsuranceChangeTxt != "" && _vm.InsuranceChangeTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "INSURANCE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 9;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            StringBuilder _msgStr3 = new StringBuilder();

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr3.Append(noteString);
                            _msgStr3.Append("Call Regarding = Eligibility" + Environment.NewLine + "Note = " + _vm.InsuranceChangeTxt);

                            _tHist.NoteText = _msgStr3.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding =  Eligibility_" + "Note = " + _vm.BillingTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //Assessment Insulin
                    if ((_vm.Assmnt_completedtxt != null || _vm.Assmnt_reltomember != null || _vm.Assmnt_edudoneby != null
                        || _vm.Assmnt_memtrained != null || _vm.Assmnt_physicianforsupp != null || _vm.assmnt_Insulin == true
                        || _vm.assmnt_Diab == true || _vm.Assmnt_lastorderrecieveddt != null || _vm.Assmnt_supporderfrom != null ||
                        _vm.Assmnt_Ord30or90 != null || _vm.Assmnt_remsupplies != null) && _vm.IsInsulin == true)
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ASSESSMENT - DIABETIC INSULIN PUMP AND CGM").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ASSESSMENT - DIABETIC INSULIN PUMP AND CGM";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 43;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ASSESSMENT - DIABETIC INSULIN PUMP AND CGM").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            StringBuilder _msgStr3 = new StringBuilder();

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr3.Append(noteString);
                            _msgStr3.Append("Call Regarding = Assessment" + Environment.NewLine);
                            _msgStr3.Append("Assessment Completed With : " + _vm.Assmnt_completedtxt + Environment.NewLine);
                            _msgStr3.Append("Relationship to Member : " + _vm.Assmnt_reltomember + Environment.NewLine);
                            _msgStr3.Append("Education done By : " + _vm.Assmnt_edudoneby + Environment.NewLine);
                            _msgStr3.Append("Member Adequately trained: " + _vm.Assmnt_memtrained + Environment.NewLine);
                            _msgStr3.Append("Physician for Supplies : " + _vm.Assmnt_physicianforsupp + Environment.NewLine);
                            _msgStr3.Append("Currently getting supplies from : " + _vm.Assmnt_currsuppfrom + Environment.NewLine);
                            _msgStr3.Append("Date last order was received  : " + _vm.Assmnt_lastorderrecieveddt + Environment.NewLine);
                            _msgStr3.Append("What supplier was the order from : " + _vm.Assmnt_supporderfrom + Environment.NewLine);
                            _msgStr3.Append("30 or 90 day Order  : " + _vm.Assmnt_Ord30or90 + Environment.NewLine);
                            _msgStr3.Append("All Remaining supplies : " + _vm.Assmnt_remsupplies + Environment.NewLine);

                          /*  if (_vm.assmnt_Diab == true)
                            {
                                _msgStr3.Append("...Diabetic Information ...");
                                _msgStr3.Append(Environment.NewLine);

                                _msgStr3.Append("New or Existing diabetic : " + _vm.Assmnt_neworexistdiab + Environment.NewLine);
                                _msgStr3.Append("Testing Times : " + _vm.Assmnt_Testingtimesdiab + Environment.NewLine);
                                _msgStr3.Append("Insulin Treated : " + _vm.Assmnt_InsulinTreateddiab + Environment.NewLine);
                                _msgStr3.Append("Name of Insulin : " + _vm.Assmnt_nameofinsulindiab + Environment.NewLine);
                                if (_vm.Assmnt_pregduedatediab != null)
                                {
                                    _msgStr3.Append("Pregnant, Due date " + Convert.ToDateTime(_vm.Assmnt_pregduedatediab).ToShortDateString() + Environment.NewLine);
                                    _msgStr3.Append("Existing diabetic  " + _vm.Assmnt_pregexistingdiab + Environment.NewLine);
                                }
                                _msgStr3.Append("Current Meter : " + _vm.Assmnt_currmeterdiab + Environment.NewLine);
                                _msgStr3.Append("Meter we will service : " + _vm.Assmnt_meterservicediab + Environment.NewLine);
                                if (_vm.Assmnt_diffmeterdiab != null)
                                    _msgStr3.Append("Meter is different : " + _vm.Assmnt_diffmeterdiab + Environment.NewLine);
                                if (_vm.Assmnt_talkingmeterdiab != null)
                                    _msgStr3.Append("Talking Meter : " + _vm.Assmnt_talkingmeterdiab + Environment.NewLine);
                                if (_vm.Assmnt_lancetsservicediab != null)
                                    _msgStr3.Append("Lancets we will service : " + _vm.Assmnt_lancetsservicediab + Environment.NewLine);
                                _msgStr3.Append("Does the member require injection supplies : " + _vm.Assmnt_injsuppliesdiab + Environment.NewLine);
                                _msgStr3.Append(" Obtaining from another supplier : " + _vm.Assmnt_injfromothersuppdiab + Environment.NewLine);
                                if (_vm.Assmnt_injothersupptxtdiab != null)
                                    _msgStr3.Append("Supplier Name :" + _vm.Assmnt_injothersupptxtdiab + Environment.NewLine);

                                if (_vm.Assmnt_syrwtneedlediab == true)
                                {
                                    _msgStr3.Append("....Syring w/needle...." + Environment.NewLine);
                                    _msgStr3.Append("Gauge :" + _vm.Assmnt_syrwtneedle_gaugediab + Environment.NewLine);
                                    _msgStr3.Append("Length :" + _vm.Assmnt_syrwtneedle_lendiab + Environment.NewLine);
                                    _msgStr3.Append("CC/Units :" + _vm.Assmnt_syrwtneedle_unitsdiab + Environment.NewLine);
                                    _msgStr3.Append("Qty/Injections per day :" + _vm.Assmnt_syrwtneedle_qtydiab + Environment.NewLine);
                                }

                                if (_vm.Assmnt_needleonly_diab == true)
                                {
                                    _msgStr3.Append("....Needle Only...." + Environment.NewLine);
                                    _msgStr3.Append("Gauge :" + _vm.Assmnt_needleonly_gauge + Environment.NewLine);
                                    _msgStr3.Append("Length :" + _vm.Assmnt_needleonly_len + Environment.NewLine);
                                    _msgStr3.Append("Qty/Injections per day :" + _vm.Assmnt_needleonly_qty + Environment.NewLine);
                                }

                                if (_vm.Assmnt_alcoholwipes == true)
                                {
                                    _msgStr3.Append("....Alcohol Wipes (Only for Injection Supplies)...." + Environment.NewLine);
                                    _msgStr3.Append("Does the member require ketone supplies? :" + _vm.assmnt_ketonediab + Environment.NewLine);
                                    _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_alcoholwp_othersuppdiab + Environment.NewLine);

                                }

                                if (_vm.Assmnt_UrineKetonediab == true)
                                {
                                    _msgStr3.Append("....Urine Ketone Strips...." + Environment.NewLine);
                                    _msgStr3.Append(" Frequencey of testing :" + _vm.Assmnt_UrineKetone_Freqtestingdiab + Environment.NewLine);

                                }

                                if (_vm.Assmnt_BloodKetonediab == true)
                                {
                                    _msgStr3.Append("....Blood Ketone strips...." + Environment.NewLine);
                                    _msgStr3.Append(" Frequencey of testing :" + _vm.Assmnt_BloodKetone_Freqtestingdiab + Environment.NewLine);

                                }
                            } */

                            
                              if (_vm.assmnt_Insulin == true)
                             {
                                 _msgStr3.Append("...Insulin Information ...");
                                 _msgStr3.Append(Environment.NewLine + "Diagnosed  :" + _vm.Assmnt_diagnosedCGM + Environment.NewLine);
                                 _msgStr3.Append("Insulin Treated :" + _vm.Assmnt_InsTreatedCGM + Environment.NewLine);
                                 _msgStr3.Append("Insulin Type :" + _vm.Assmnt_InsTreatedTypeCGM + Environment.NewLine);
                                 _msgStr3.Append("Does the member require a pump :" + _vm.Assmnt_reqpumpCGM + Environment.NewLine);
                                 _msgStr3.Append("Does the member require pump supplies :" + _vm.Assmnt_reqpumpsuppCGM + Environment.NewLine);
                                 _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_othersupplierCGM + Environment.NewLine);
                                 if(_vm.Assmnt_othersuppliertxtCGM != null)
                                 _msgStr3.Append("Supplier Name  :" + _vm.Assmnt_othersuppliertxtCGM + Environment.NewLine);
                                 _msgStr3.Append("New or replacement :" + _vm.Assmnt_neworreplacementCGM + Environment.NewLine);
                                 _msgStr3.Append("Manufacturer :" + _vm.Assmnt_manufacturerCGM + Environment.NewLine);
                                 _msgStr3.Append("Current Pump Name :" + _vm.Assmnt_currpumpnmCGM + Environment.NewLine);
                                 _msgStr3.Append("Serial Number :" + _vm.Assmnt_serialnumCGM + Environment.NewLine);
                                 if(_vm.Assmnt_outofwarrantydtCGM != null)
                                 _msgStr3.Append("Out of Warranty Date :" + Convert.ToDateTime(_vm.Assmnt_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                 _msgStr3.Append("Insurance that paid for pump :" + _vm.Assmnt_InsurancepaidforCGM + Environment.NewLine);
                                 _msgStr3.Append("New/Replacement pump name/product code :" + _vm.Assmnt_NewPumpNameCGM + Environment.NewLine);
                                 _msgStr3.Append("New/Replacement pump color/size :" + _vm.Assmnt_NewPumpColorCGM + Environment.NewLine);
                                 _msgStr3.Append("Replacement, reason for replacement :" + _vm.Assmnt_PumpReplacereasonCGM + Environment.NewLine);
                                 _msgStr3.Append("Type of meter/strips used with pump :" + _vm.Assmnt_meterusedCGM + Environment.NewLine);
                                 if(_vm.Assmnt_InfusionsetCGM == true)
                                 _msgStr3.Append("Infusion set/pod :" + _vm.Assmnt_InfusionsettxtCGM + Environment.NewLine);
                                 if (_vm.Assmnt_catridgesCGM == true)
                                 {
                                     _msgStr3.Append("Cartridges/reservoirs :" + _vm.Assmnt_catridgestxtCGM + Environment.NewLine);
                                     _msgStr3.Append("    How often are they changed :" + _vm.Assmnt_catridgesoftnchngtxtCGM + Environment.NewLine);
                                 }
                                 if (_vm.Assmnt_BarrierWipesCGM == true)
                                     _msgStr3.Append("Barrier Wipes"  + Environment.NewLine);
                                 if (_vm.Assmnt_RemoverWipesCGM == true)
                                     _msgStr3.Append("Remover Wipes" + Environment.NewLine);
                                 if (_vm.Assmnt_AlcoholWipesCGM == true)
                                     _msgStr3.Append("Alcohol Wipes" + Environment.NewLine);
                                 if (_vm.Assmnt_TransparentdressingCGM == true)
                                     _msgStr3.Append("Transparent Dressing" + Environment.NewLine);
                                 if (_vm.Assmnt_BatteriesCGM == true)
                                     _msgStr3.Append("Batteries" + Environment.NewLine);

                                 _msgStr3.Append("Has the member ever owned or used a CGM :" + _vm.Assmnt_memownedorusedCGM + Environment.NewLine);
                                 _msgStr3.Append("Is the member currently on a CGM :" + _vm.Assmnt_memcurronCGM + Environment.NewLine);
                                 if (_vm.Assmnt_transmitter_dtreceivedCGM != null || _vm.Assmnt_transmitter_outofwarrantydtCGM != null || _vm.Assmnt_transmitter_serialnoCGM != null)
                                 {
                                     _msgStr3.Append("Transmitter :" + Environment.NewLine);
                                    if (_vm.Assmnt_transmitter_ProductCode != null)
                                        _msgStr3.Append("Product Code # :" + _vm.Assmnt_transmitter_ProductCode + Environment.NewLine);
                                    if ( _vm.Assmnt_transmitter_dtreceivedCGM != null)
                                     _msgStr3.Append("    Date Received :" +Convert.ToDateTime(_vm.Assmnt_transmitter_dtreceivedCGM).ToShortDateString() + Environment.NewLine);
                                     if (_vm.Assmnt_transmitter_outofwarrantydtCGM != null)
                                         _msgStr3.Append("    Out of warranty date :" + Convert.ToDateTime(_vm.Assmnt_transmitter_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                     _msgStr3.Append("Serial # :" + _vm.Assmnt_transmitter_serialnoCGM + Environment.NewLine);

                                 }

                                 if (_vm.Assmnt_receiver_dtreceivedCGM != null || _vm.Assmnt_receiver_outofwarrantydtCGM != null || _vm.Assmnt_receiver_serialnoCGM != null)
                                 {
                                     _msgStr3.Append("Receiver :" + Environment.NewLine);
                                    if (_vm.Assmnt_receiver_ProductCode != null)
                                        _msgStr3.Append("Product Code # :" + _vm.Assmnt_receiver_ProductCode + Environment.NewLine);
                                    if (_vm.Assmnt_receiver_dtreceivedCGM != null)
                                         _msgStr3.Append("    Date Received :" + Convert.ToDateTime(_vm.Assmnt_receiver_dtreceivedCGM).ToShortDateString() + Environment.NewLine);
                                     if (_vm.Assmnt_receiver_outofwarrantydtCGM != null)
                                         _msgStr3.Append("    Out of warranty date :" + Convert.ToDateTime(_vm.Assmnt_receiver_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                     _msgStr3.Append("Serial # :" + _vm.Assmnt_receiver_serialnoCGM + Environment.NewLine);

                                 }
                                 _msgStr3.Append("Does the member require a Transmitter :" + _vm.Assmnt_memneedtransmitterCGM + Environment.NewLine);
                                 _msgStr3.Append("Does the member require a Receiver :" + _vm.Assmnt_memneedreceiverCGM + Environment.NewLine);
                                 _msgStr3.Append("Does the member require Sensors :" + _vm.Assmnt_memneedsensorsCGM + Environment.NewLine);
                                 _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_othersupplierCGM + Environment.NewLine);
                                 if(_vm.Assmnt_othersuppliertxtCGM != null)
                                 _msgStr3.Append("Supplier Name  :" + _vm.Assmnt_othersuppliertxtCGM + Environment.NewLine);
                                 _msgStr3.Append("Transmitter :" + _vm.Assmnt_transmittertypeCGM + Environment.NewLine);
                                 _msgStr3.Append("Receiver :" + _vm.Assmnt_receivertypeCGM + Environment.NewLine);
                                 _msgStr3.Append("Reason for Replacement :" + _vm.Assmnt_transmitorreceiver_replacementreasonCGM + Environment.NewLine);
                                 _msgStr3.Append("Sensors product code  :" + _vm.Assmnt_sensorsforprodcodeCGM + Environment.NewLine);
                                 _msgStr3.Append("Is member aware that signature is required for full CGM systems and all insulin pumps  :" + _vm.Assmnt_memawareofsignCGM + Environment.NewLine);
                             } 

                            _tHist.NoteText = _msgStr3.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding =  Eligibility_" + "Note = " + _vm.BillingTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //Assessment Diabetic 
                    if ((_vm.Assmnt_completedtxt != null || _vm.Assmnt_reltomember != null || _vm.Assmnt_edudoneby != null
                        || _vm.Assmnt_memtrained != null || _vm.Assmnt_physicianforsupp != null || _vm.assmnt_Insulin == true
                        || _vm.assmnt_Diab == true || _vm.Assmnt_lastorderrecieveddt != null || _vm.Assmnt_supporderfrom != null|| 
                        _vm.Assmnt_Ord30or90 != null || _vm.Assmnt_remsupplies != null) && _vm.IsDiab == true)
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ASSESSMENT - DIABETIC TESTING AND SYRINGE").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ASSESSMENT - DIABETIC TESTING AND SYRINGE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 25;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ASSESSMENT - DIABETIC TESTING AND SYRINGE").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            StringBuilder _msgStr3 = new StringBuilder();

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr3.Append(noteString);
                            _msgStr3.Append("Call Regarding = Assessment" + Environment.NewLine);
                            _msgStr3.Append("Assessment Completed With : " + _vm.Assmnt_completedtxt + Environment.NewLine);
                            _msgStr3.Append("Relationship to Member : " + _vm.Assmnt_reltomember + Environment.NewLine);
                            _msgStr3.Append("Education done By : " + _vm.Assmnt_edudoneby + Environment.NewLine);
                            _msgStr3.Append("Member Adequately trained: " + _vm.Assmnt_memtrained + Environment.NewLine);
                            _msgStr3.Append("Physician for Supplies : " + _vm.Assmnt_physicianforsupp + Environment.NewLine);
                            _msgStr3.Append("Currently getting supplies from : " + _vm.Assmnt_currsuppfrom + Environment.NewLine);
                            _msgStr3.Append("Date last order was received  : " + _vm.Assmnt_lastorderrecieveddt + Environment.NewLine);
                            _msgStr3.Append("What supplier was the order from : " + _vm.Assmnt_supporderfrom + Environment.NewLine);
                            _msgStr3.Append("30 or 90 day Order  : " + _vm.Assmnt_Ord30or90 + Environment.NewLine);
                            _msgStr3.Append("All Remaining supplies : " + _vm.Assmnt_remsupplies + Environment.NewLine);

                            if (_vm.assmnt_Diab == true)
                            {
                                _msgStr3.Append("...Diabetic Information ...");
                                _msgStr3.Append(Environment.NewLine);

                                _msgStr3.Append("New or Existing diabetic : " + _vm.Assmnt_neworexistdiab + Environment.NewLine);
                                _msgStr3.Append("Testing Times : " + _vm.Assmnt_Testingtimesdiab + Environment.NewLine);
                                _msgStr3.Append("Insulin Treated : " + _vm.Assmnt_InsulinTreateddiab + Environment.NewLine);
                                _msgStr3.Append("Name of Insulin : " + _vm.Assmnt_nameofinsulindiab );
                                if (_vm.Assmnt_nameofinsulindiab_Other != null)
                                {
                                    _msgStr3.Append(", " + _vm.Assmnt_nameofinsulindiab_Other);
                                }
                                _msgStr3.Append(Environment.NewLine);
                                if (_vm.Assmnt_pregduedatediab != null)
                                {
                                    _msgStr3.Append("Pregnant, Due date " + Convert.ToDateTime(_vm.Assmnt_pregduedatediab).ToShortDateString() + Environment.NewLine);
                                    _msgStr3.Append("Existing diabetic  " +  _vm.Assmnt_pregexistingdiab  + Environment.NewLine);
                                }
                                _msgStr3.Append("Current Meter : " + _vm.Assmnt_currmeterdiab + Environment.NewLine);
                                _msgStr3.Append("Meter we will service : " + _vm.Assmnt_meterservicediab + Environment.NewLine);
                                if(_vm.Assmnt_diffmeterdiab != null)
                                _msgStr3.Append("Meter is different : " + _vm.Assmnt_diffmeterdiab + Environment.NewLine);
                                if (_vm.Assmnt_talkingmeterdiab != null)
                                    _msgStr3.Append("Talking Meter : " + _vm.Assmnt_talkingmeterdiab + Environment.NewLine);
                                if (_vm.Assmnt_lancetsservicediab != null)
                                    _msgStr3.Append("Lancets we will service : " + _vm.Assmnt_lancetsservicediab + Environment.NewLine);
                                _msgStr3.Append("Does the member require injection supplies : " + _vm.Assmnt_injsuppliesdiab + Environment.NewLine);
                                _msgStr3.Append(" Obtaining from another supplier : " + _vm.Assmnt_injfromothersuppdiab + Environment.NewLine);
                                if(_vm.Assmnt_injothersupptxtdiab != null)
                                    _msgStr3.Append("Supplier Name :" + _vm.Assmnt_injothersupptxtdiab + Environment.NewLine);

                                if (_vm.Assmnt_syrwtneedlediab == true)
                                {
                                    _msgStr3.Append("....Syring w/needle...." + Environment.NewLine);
                                    _msgStr3.Append("Gauge :" + _vm.Assmnt_syrwtneedle_gaugediab + Environment.NewLine);
                                    _msgStr3.Append("Length :" + _vm.Assmnt_syrwtneedle_lendiab + Environment.NewLine);
                                    _msgStr3.Append("CC/Units :" + _vm.Assmnt_syrwtneedle_unitsdiab + Environment.NewLine);
                                    _msgStr3.Append("Qty/Injections per day :" + _vm.Assmnt_syrwtneedle_qtydiab + Environment.NewLine);
                                }

                                if (_vm.Assmnt_needleonly_diab == true)
                                {
                                    _msgStr3.Append("....Needle Only...." + Environment.NewLine);
                                    _msgStr3.Append("Gauge :" + _vm.Assmnt_needleonly_gauge + Environment.NewLine);
                                    _msgStr3.Append("Length :" + _vm.Assmnt_needleonly_len + Environment.NewLine);
                                    _msgStr3.Append("Qty/Injections per day :" + _vm.Assmnt_needleonly_qty + Environment.NewLine);
                                }

                                if (_vm.Assmnt_alcoholwipes == true)
                                {
                                    _msgStr3.Append("....Alcohol Wipes (Only for Injection Supplies)...." + Environment.NewLine);
                                    _msgStr3.Append("Does the member require ketone supplies? :" + _vm.assmnt_ketonediab + Environment.NewLine);
                                    _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_alcoholwp_othersuppdiab + Environment.NewLine);
                                    
                                }

                                if (_vm.Assmnt_UrineKetonediab == true)
                                {
                                    _msgStr3.Append("....Urine Ketone Strips...." + Environment.NewLine);
                                    _msgStr3.Append(" Frequencey of testing :" + _vm.Assmnt_UrineKetone_Freqtestingdiab + Environment.NewLine);
                                    
                                }

                                if (_vm.Assmnt_BloodKetonediab == true)
                                {
                                    _msgStr3.Append("....Blood Ketone strips...." + Environment.NewLine);
                                    _msgStr3.Append(" Frequencey of testing :" + _vm.Assmnt_BloodKetone_Freqtestingdiab + Environment.NewLine);

                                }
                            }

                           /* 
                             if (_vm.assmnt_Insulin == true)
                            {
                                _msgStr3.Append("...Insulin Information ...");
                                _msgStr3.Append(Environment.NewLine + "Diagnosed  :" + _vm.Assmnt_diagnosedCGM + Environment.NewLine);
                                _msgStr3.Append("Insulin Treated :" + _vm.Assmnt_InsTreatedCGM + Environment.NewLine);
                                _msgStr3.Append("Insulin Type :" + _vm.Assmnt_InsTreatedTypeCGM + Environment.NewLine);
                                _msgStr3.Append("Does the member require a pump :" + _vm.Assmnt_reqpumpCGM + Environment.NewLine);
                                _msgStr3.Append("Does the member require pump supplies :" + _vm.Assmnt_reqpumpsuppCGM + Environment.NewLine);
                                _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_othersupplierCGM + Environment.NewLine);
                                if(_vm.Assmnt_othersuppliertxtCGM != null)
                                _msgStr3.Append("Supplier Name  :" + _vm.Assmnt_othersuppliertxtCGM + Environment.NewLine);
                                _msgStr3.Append("New or replacement :" + _vm.Assmnt_neworreplacementCGM + Environment.NewLine);
                                _msgStr3.Append("Manufacturer :" + _vm.Assmnt_manufacturerCGM + Environment.NewLine);
                                _msgStr3.Append("Current Pump Name :" + _vm.Assmnt_currpumpnmCGM + Environment.NewLine);
                                _msgStr3.Append("Serial Number :" + _vm.Assmnt_serialnumCGM + Environment.NewLine);
                                if(_vm.Assmnt_outofwarrantydtCGM != null)
                                _msgStr3.Append("Out of Warranty Date :" + Convert.ToDateTime(_vm.Assmnt_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                _msgStr3.Append("Insurance that paid for pump :" + _vm.Assmnt_InsurancepaidforCGM + Environment.NewLine);
                                _msgStr3.Append("New/Replacement pump name/product code :" + _vm.Assmnt_NewPumpNameCGM + Environment.NewLine);
                                _msgStr3.Append("New/Replacement pump color/size :" + _vm.Assmnt_NewPumpColorCGM + Environment.NewLine);
                                _msgStr3.Append("Replacement, reason for replacement :" + _vm.Assmnt_PumpReplacereasonCGM + Environment.NewLine);
                                _msgStr3.Append("Type of meter/strips used with pump :" + _vm.Assmnt_meterusedCGM + Environment.NewLine);
                                if(_vm.Assmnt_InfusionsetCGM == true)
                                _msgStr3.Append("Infusion set/pod :" + _vm.Assmnt_InfusionsettxtCGM + Environment.NewLine);
                                if (_vm.Assmnt_catridgesCGM == true)
                                {
                                    _msgStr3.Append("Cartridges/reservoirs :" + _vm.Assmnt_catridgestxtCGM + Environment.NewLine);
                                    _msgStr3.Append("    How often are they changed :" + _vm.Assmnt_catridgesoftnchngtxtCGM + Environment.NewLine);
                                }
                                if (_vm.Assmnt_BarrierWipesCGM == true)
                                    _msgStr3.Append("Barrier Wipes"  + Environment.NewLine);
                                if (_vm.Assmnt_RemoverWipesCGM == true)
                                    _msgStr3.Append("Remover Wipes" + Environment.NewLine);
                                if (_vm.Assmnt_AlcoholWipesCGM == true)
                                    _msgStr3.Append("Alcohol Wipes" + Environment.NewLine);
                                if (_vm.Assmnt_TransparentdressingCGM == true)
                                    _msgStr3.Append("Transparent Dressing" + Environment.NewLine);
                                if (_vm.Assmnt_BatteriesCGM == true)
                                    _msgStr3.Append("Batteries" + Environment.NewLine);

                                _msgStr3.Append("Has the member ever owned or used a CGM :" + _vm.Assmnt_memownedorusedCGM + Environment.NewLine);
                                _msgStr3.Append("Is the member currently on a CGM :" + _vm.Assmnt_memcurronCGM + Environment.NewLine);
                                if (_vm.Assmnt_transmitter_dtreceivedCGM != null || _vm.Assmnt_transmitter_outofwarrantydtCGM != null || _vm.Assmnt_transmitter_serialnoCGM != null)
                                {
                                    _msgStr3.Append("Transmitter :" + Environment.NewLine);
                                   if( _vm.Assmnt_transmitter_dtreceivedCGM != null)
                                    _msgStr3.Append("    Date Received :" +Convert.ToDateTime(_vm.Assmnt_transmitter_dtreceivedCGM).ToShortDateString() + Environment.NewLine);
                                    if (_vm.Assmnt_transmitter_outofwarrantydtCGM != null)
                                        _msgStr3.Append("    Out of warranty date :" + Convert.ToDateTime(_vm.Assmnt_transmitter_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                    _msgStr3.Append("Serial # :" + _vm.Assmnt_transmitter_serialnoCGM + Environment.NewLine);
                                   
                                }

                                if (_vm.Assmnt_receiver_dtreceivedCGM != null || _vm.Assmnt_receiver_outofwarrantydtCGM != null || _vm.Assmnt_receiver_serialnoCGM != null)
                                {
                                    _msgStr3.Append("Receiver :" + Environment.NewLine);
                                    if (_vm.Assmnt_receiver_dtreceivedCGM != null)
                                        _msgStr3.Append("    Date Received :" + Convert.ToDateTime(_vm.Assmnt_receiver_dtreceivedCGM).ToShortDateString() + Environment.NewLine);
                                    if (_vm.Assmnt_receiver_outofwarrantydtCGM != null)
                                        _msgStr3.Append("    Out of warranty date :" + Convert.ToDateTime(_vm.Assmnt_receiver_outofwarrantydtCGM).ToShortDateString() + Environment.NewLine);
                                    _msgStr3.Append("Serial # :" + _vm.Assmnt_receiver_serialnoCGM + Environment.NewLine);

                                }
                                _msgStr3.Append("Does the member require a Transmitter :" + _vm.Assmnt_memneedtransmitterCGM + Environment.NewLine);
                                _msgStr3.Append("Does the member require a Receiver :" + _vm.Assmnt_memneedreceiverCGM + Environment.NewLine);
                                _msgStr3.Append("Does the member require Sensors :" + _vm.Assmnt_memneedsensorsCGM + Environment.NewLine);
                                _msgStr3.Append("Obtaining from another supplier :" + _vm.Assmnt_othersupplierCGM + Environment.NewLine);
                                if(_vm.Assmnt_othersuppliertxtCGM != null)
                                _msgStr3.Append("Supplier Name  :" + _vm.Assmnt_othersuppliertxtCGM + Environment.NewLine);
                                _msgStr3.Append("Transmitter :" + _vm.Assmnt_transmittertypeCGM + Environment.NewLine);
                                _msgStr3.Append("Receiver :" + _vm.Assmnt_receivertypeCGM + Environment.NewLine);
                                _msgStr3.Append("Reason for Replacement :" + _vm.Assmnt_transmitorreceiver_replacementreasonCGM + Environment.NewLine);
                                _msgStr3.Append("Sensors product code  :" + _vm.Assmnt_sensorsforprodcodeCGM + Environment.NewLine);
                                _msgStr3.Append("Is member aware that signature is required for full CGM systems and all insulin pumps  :" + _vm.Assmnt_memawareofsignCGM + Environment.NewLine);
                            } */

                                _tHist.NoteText = _msgStr3.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding =  Eligibility_" + "Note = " + _vm.BillingTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //Other Issues- Communication

                    // COMMMUNICATION
                    if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                        || _vm.Damaged == true || _vm.Driver == true || _vm.WrongProductShipped == true || _vm.QualityOfProduct == true
                        || _vm.WrongArea == true || _vm.Complain_MissingProduct == true || (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                        || _vm.Incorrect == true || _vm.Mispick == true || _vm.Defective || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                        || _vm.Impolite_Offensive == true || _vm.HoldTimes == true || (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null)
                        || (_vm.Others != "" && _vm.Others != null) //|| _vm.DidntFollowDelIns == true 
                      //  || _vm.VConfirmationCalls == true 
                       // || _vm.VPaymentCalles == true
                        ||  _vm.Julie_VictorCalls == true
                            || _vm.PhonePrompts_SelfService == true 
                            //|| _vm.VirtualCallBack == true 
                            //|| _vm.Website == true 
                            || _vm.NoFollowUp == true //|| _vm.ReturnedFromVM == true
                            || _vm.NoPresORCMN == true 
                            //|| _vm.NeverRecivedSupplies == true 
                            || _vm.PhysicianIssue == true //|| _vm.InsLimitGuidelines == true 
                         //   || _vm.BCNProviderIssue == true 
                            || _vm.Other == true //||
                        //(_vm.Product != "" && _vm.Product != null) ||
                      //  (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null) || 
                       // (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                        )
                    {

                        if (_vm.ComplainOutCome != null && _vm.ComplainOutCome.Contains("Not Resolved Transferred to Team Leaders"))
                        {

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMPLAINTS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                            if (_note == null)
                            {
                                tbl_Account_Note _tbl = new tbl_Account_Note();
                                _tbl.Account = Convert.ToInt32(_vm.Account);
                                _tbl.Member = 1;
                                _tbl.NoteHeading = "COMPLAINTS";
                                _tbl.NoteCreateDate = DateTime.Now;
                                _tbl.NoteCreatedBy = id;
                                _tbl.SystemRecordType = 100;
                                _tbl.ID_NoteLibrary = 31;
                                _db.tbl_Account_Note.Add(_tbl);
                                _db.SaveChanges();


                                _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMPLAINTS").FirstOrDefault(); //&& t.NoteCreatedBy == id

                            }

                        }
                        else
                        {

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                            if (_note == null)
                            {

                                tbl_Account_Note _tbl = new tbl_Account_Note();
                                _tbl.Account = Convert.ToInt32(_vm.Account);
                                _tbl.Member = 1;
                                _tbl.NoteHeading = "COMMMUNICATION";
                                _tbl.NoteCreateDate = DateTime.Now;
                                _tbl.NoteCreatedBy = id;
                                _tbl.SystemRecordType = 100;
                                _tbl.ID_NoteLibrary = 9;
                                _db.tbl_Account_Note.Add(_tbl);
                                _db.SaveChanges();


                                _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id

                            }
                        }

                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;




                            if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                || _vm.Damaged == true || _vm.Driver == true || _vm.WrongProductShipped == true || _vm.QualityOfProduct == true
                                || _vm.WrongArea == true || _vm.Complain_MissingProduct == true //|| _vm.DidntFollowDelIns == true 
                               // || _vm.VConfirmationCalls == true
                              //  || _vm.VPaymentCalles == true 
                                || _vm.Julie_VictorCalls == true
                                || _vm.PhonePrompts_SelfService == true //|| _vm.VirtualCallBack == true
                              //  || _vm.Website == true 
                                || _vm.NoFollowUp == true // || _vm.ReturnedFromVM == true
                                || _vm.NoPresORCMN == true // ||  _vm.NeverRecivedSupplies == true 
                                || _vm.PhysicianIssue == true //|| _vm.InsLimitGuidelines == true 
                                //|| _vm.BCNProviderIssue == true 
                                || _vm.Other == true ||
                                (_vm.FedExTextArea != "" && _vm.FedExTextArea != null) 
                                //|| (_vm.Product != "" && _vm.Product != null) 
                             //   || (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null) 
                                //|| (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                                )
                            {
                                if (_vm.TrackingNumber != "" && _vm.TrackingNumber != null)
                                    otherStr.Append("Tracking Number = " + _vm.TrackingNumber + Environment.NewLine);
                                //  otherStr = otherStr + " Tracking Number = " + _vm.TrackingNumber + "_";

                                if (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                    otherStr.Append("WorkOrder = " + _vm.WorkOrder + Environment.NewLine);
                                // otherStr = otherStr + " WorkOrder = " + _vm.WorkOrder + "_";


                                //if (_vm.Product != "" && _vm.Product != null)
                                //    otherStr.Append("Product = " + _vm.Product + Environment.NewLine);

                               // if (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null)
                                  //  otherStr.Append("Delivery Company = " + _vm.DeliveryCompany + Environment.NewLine);

                             //   if (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                                 //   otherStr.Append("BCN Provider = " + _vm.BCNProvider + Environment.NewLine);

                                if (_vm.Damaged == true)
                                    otherStr.Append("Damaged" + Environment.NewLine);
                                //otherStr = otherStr + "Damaged_";

                                if (_vm.Driver == true)
                                    otherStr.Append("Driver" + Environment.NewLine);
                                // otherStr = otherStr + "Driver_";



                                //if (_vm.DidntFollowDelIns == true)
                                 //   otherStr.Append("Did not follow delivery instructions" + Environment.NewLine);

                             //   if (_vm.VConfirmationCalls == true)
                                  //  otherStr.Append("Victor Confirmation Calls " + Environment.NewLine);

                              //  if (_vm.VPaymentCalles == true)
                                //    otherStr.Append("Victor Payment Calls " + Environment.NewLine);

                                if (_vm.Julie_VictorCalls == true)
                                    otherStr.Append("Julie/Victor Calls to members " + Environment.NewLine);

                                if (_vm.PhonePrompts_SelfService == true)
                                    otherStr.Append("PhonePrompts or SelfService " + Environment.NewLine);

                               // if (_vm.VirtualCallBack == true)
                                  //  otherStr.Append("Virtual Callback feature " + Environment.NewLine);

                                if (_vm.NoFollowUp == true)
                                    otherStr.Append("No follow up on account/on documentation" + Environment.NewLine);

                               // if (_vm.ReturnedFromVM == true)
                                  //  otherStr.Append("Call not returned from VM" + Environment.NewLine);

                                if (_vm.WrongProductShipped == true)
                                    otherStr.Append("Wrong Product Shipped" + Environment.NewLine);
                                //otherStr = otherStr + "WrongProductShipped_";

                                if (_vm.NoPresORCMN == true)
                                    otherStr.Append("No Prescription Or CMN " + Environment.NewLine);

                                //if (_vm.NeverRecivedSupplies == true)
                                  //  otherStr.Append("Never received supplies" + Environment.NewLine);

                                if (_vm.PhysicianIssue == true)
                                    otherStr.Append("Physician Issue" + Environment.NewLine);

                               // if (_vm.InsLimitGuidelines == true)
                                   // otherStr.Append("Insurance Limitations/Guidelines" + Environment.NewLine);

                              //  if (_vm.BCNProviderIssue == true)
                                  //  otherStr.Append("BCN Provider Issue " + Environment.NewLine);

                                if (_vm.QualityOfProduct == true)
                                    otherStr.Append("Quality Of Product" + Environment.NewLine);

                                if (_vm.Other == true)
                                    otherStr.Append("Other" + Environment.NewLine);


                               // if (_vm.Website == true)
                                 //   otherStr.Append("Website" + Environment.NewLine);

                                if (_vm.WrongArea == true)
                                    otherStr.Append("Wrong Area" + Environment.NewLine);
                                //otherStr = otherStr + "WrongArea_";

                                if (_vm.Complain_MissingProduct == true)
                                    otherStr.Append("Missing Product" + Environment.NewLine);
                                // otherStr = otherStr + "MissingProduct_";

                                if (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                                    otherStr.Append(" Note =" + _vm.FedExTextArea + Environment.NewLine);
                                // otherStr = otherStr + " FedExNote =" + _vm.FedExTextArea + "_";

                            }

                            if (_vm.Incorrect == true || _vm.Mispick == true || _vm.Defective || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null))
                            {

                                if (_vm.Incorrect == true)
                                    otherStr.Append("Incorrect Product" + Environment.NewLine);
                                //otherStr = otherStr + "Incorrect_";

                                if (_vm.Mispick == true)
                                    otherStr.Append("Mispick Product" + Environment.NewLine);
                                //otherStr = otherStr + "Mispick_";

                                if (_vm.Defective == true)
                                    otherStr.Append("Defective Product" + Environment.NewLine);
                                //otherStr = otherStr + "Defective_";



                                if (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                                    otherStr.Append("Product Note =" + _vm.ProductTextArea + Environment.NewLine);
                                //otherStr = otherStr + " Product Note =" + _vm.ProductTextArea + "_";

                            }

                            if (_vm.Impolite_Offensive == true || _vm.HoldTimes == true || (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null))
                            {

                                if (_vm.Impolite_Offensive == true)
                                    otherStr.Append("Impolite or Offensive" + Environment.NewLine);
                                //otherStr = otherStr + "Impolite/Offensive_";

                                if (_vm.HoldTimes == true)
                                    otherStr.Append("Hold Times" + Environment.NewLine);
                                // otherStr = otherStr + "HoldTimes_";


                                if (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null)
                                    otherStr.Append("Customer Service Note =" + _vm.CustomerServiceTextArea + Environment.NewLine);
                                // otherStr = otherStr + " Customer Service Note =" + _vm.CustomerServiceTextArea + "_";

                            }

                            if (_vm.Others != "" && _vm.Others != null)
                            {
                                otherStr.Append(" Others Note =" + _vm.Others + Environment.NewLine);
                                // otherStr = otherStr + " Others Note =" + _vm.Others + "_";

                            }

                            StringBuilder _msgStr4 = new StringBuilder();
                            _msgStr4.Append(noteString);
                            _msgStr4.Append(otherStr);
                            _msgStr4.Append("OutCome = " + _vm.ComplainOutCome + Environment.NewLine);
                           // _msgStr4.Append("Reference Number = " + reference + Environment.NewLine);

                            _tHist.NoteText = _msgStr4.ToString();

                            //  _tHist.NoteText = noteString + otherStr + "OutCome = " +_vm.ComplainOutCome + "_Reference Number = " +reference; 
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);
                            //try
                            //{
                            //    _db.SaveChanges();
                            //}
                            //catch (Exception Ex)
                            //{
                            //    string msg = Ex.Message;

                            //}
                        }
                    }

                    //DEMOGRAPHICS  
                    if ((_vm.Address == true || _vm.Physician == true || _vm.Phone == true) && (_vm.DemographicChanges != "" && _vm.DemographicChanges != null))
                    {


                        StringBuilder _DemStr = new StringBuilder();
                        // string str = "";
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {

                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "DEMOGRAPHICS";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 4;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;
                            if (_vm.Address)
                            {
                                _DemStr.Append("Address" + Environment.NewLine);

                                //str = str + "Address_";
                            }
                            if (_vm.Physician)
                            {
                                _DemStr.Append("Physician" + Environment.NewLine);
                                // str = str + "Physician_";
                            }
                            if (_vm.Phone)
                            {
                                _DemStr.Append("Phone Number" + Environment.NewLine);
                                //str = str + "Phone Number_";
                            }

                            StringBuilder _msgStr5 = new StringBuilder();
                            _msgStr5.Append(noteString);
                            _msgStr5.Append("Call Regarding = " + _DemStr + Environment.NewLine);
                            _msgStr5.Append("Note = " + _vm.DemographicChanges + Environment.NewLine);

                            _tHist.NoteText = _msgStr5.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = " + str + "Note = " + _vm.DemographicChanges;

                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }








                    //AOB 
                    if ((_vm.AOB == true || _vm.AOBReceived == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "AOB").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {

                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "AOB";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 16;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "AOB").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr6 = new StringBuilder();
                            _msgStr6.Append(noteString);
                            _msgStr6.Append("Call Regarding = ");
                            if (_vm.AOB == true)
                                _msgStr6.Append("AOB Sent");

                            if (_vm.AOBReceived == true)
                                _msgStr6.Append("- AOB Received");
                            _msgStr6.Append( Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr6.ToString();

                            //  _tHist.NoteText = noteString + "Call Regarding = AOB_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }


                    //PRESCRIPTI0N 
                    if ((_vm.Prescription == true || _vm.RXReceived == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRESCRIPTI0N").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {

                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "PRESCRIPTI0N";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 2;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRESCRIPTI0N").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr7 = new StringBuilder();
                            _msgStr7.Append(noteString);
                            _msgStr7.Append("Call Regarding = ");
                            if (_vm.Prescription == true)
                                _msgStr7.Append("Prescription");

                            if (_vm.RXReceived == true)
                                _msgStr7.Append(" - RXReceived");
                            _msgStr7.Append( Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr7.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = Prescription_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }

                    }

                    //CMN 
                    //if (_vm.CMN == true && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    //{

                    //    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CMN").FirstOrDefault();  //&& t.NoteCreatedBy == id

                    //    if (_note == null)
                    //    {
                    //        tbl_Account_Note _tbl = new tbl_Account_Note();
                    //        _tbl.Account = Convert.ToInt32(_vm.Account);
                    //        _tbl.Member = 1;
                    //        _tbl.NoteHeading = "CMN";
                    //        _tbl.NoteCreateDate = DateTime.Now;
                    //        _tbl.NoteCreatedBy = id;
                    //        _tbl.SystemRecordType = 100;
                    //        _tbl.ID_NoteLibrary = 6;
                    //        _db.tbl_Account_Note.Add(_tbl);
                    //        _db.SaveChanges();


                    //        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CMN").FirstOrDefault();  //&& t.NoteCreatedBy == id



                    //    }
                    //    if (_note != null)
                    //    {

                    //        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                    //        _tHist.ID_Note = _note.ID;
                    //        _tHist.NoteDate = DateTime.Now;

                    //        StringBuilder _msgStr8 = new StringBuilder();
                    //        _msgStr8.Append(noteString);
                    //        _msgStr8.Append("Call Regarding = CMN" + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                    //        _tHist.NoteText = _msgStr8.ToString();

                    //        // _tHist.NoteText = noteString + "Call Regarding =  CMN_" + "Note = " + _vm.DocumentationTxt;
                    //        _tHist.ID_Operator = id;

                    //        _db.tbl_Account_Note_History.Add(_tHist);

                    //    }
                    //}

                    //PRIOR AUTH 
                    if ((_vm.PriorAuthorization == true || _vm.PASent == true || _vm.PAReceived == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRIOR AUTH").Take(1).SingleOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "PRIOR AUTH";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 7;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRIOR AUTH").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr9 = new StringBuilder();
                            _msgStr9.Append(noteString);
                            _msgStr9.Append("Call Regarding =");
                            if(_vm.PriorAuthorization == true)
                                _msgStr9.Append("PriorAuthorization");
                            if (_vm.PASent == true)
                                _msgStr9.Append("PA Sent");
                            if (_vm.PAReceived == true)
                                _msgStr9.Append("PA Received");
                            _msgStr9.Append(  Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr9.ToString();


                            //_tHist.NoteText = noteString + "Call Regarding =  PriorAuthorization_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //OVER QUANTITY DOCUMENTATION 
                    if ((_vm.SupportingDoc == true || _vm.ClinicalDocsReceived == true || _vm.ClinicalDocsReq == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "OVER QUANTITY DOCUMENTATION").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "OVER QUANTITY DOCUMENTATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 28;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "OVER QUANTITY DOCUMENTATION").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr10 = new StringBuilder();
                            _msgStr10.Append(noteString);
                            _msgStr10.Append("Call Regarding -" + Environment.NewLine);
                            if (_vm.SupportingDoc == true)
                                _msgStr10.Append("Supporting Document" + Environment.NewLine);
                            if (_vm.ClinicalDocsReceived == true)
                                _msgStr10.Append(" - Clinical Documentation Received" + Environment.NewLine);
                            if (_vm.ClinicalDocsReq == true)
                                _msgStr10.Append(" - Clinical Documentation Requested" + Environment.NewLine);

                            _msgStr10.Append("Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr10.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding = SupportingDoc_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //COMMUNICATOIN
                    if ((_vm.TeacherLetter == true || _vm.Logs == true || _vm.ABN == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();   //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "COMMUNICATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 9;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            StringBuilder _DocStr = new StringBuilder();
                            //string str1 = "";
                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;
                            if (_vm.TeacherLetter)
                                _DocStr.Append("Teacher Letter" + Environment.NewLine);
                            //str1 = str1 + "Teacher Letter_";
                            if (_vm.Logs)
                                _DocStr.Append("Logs" + Environment.NewLine);
                            // str1 = str1 + "Logs_";
                            if (_vm.ABN)
                                _DocStr.Append("ABN" + Environment.NewLine);
                            //  str1 = str1 + "ABN_";


                            StringBuilder _msgStr11 = new StringBuilder();
                            _msgStr11.Append(noteString);
                            _msgStr11.Append("Call Regarding = " + _DocStr + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr11.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding =" + str1 + "Note = " + _vm.DocumentationTxt;


                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //INSURANCE 
                    if ((_vm.InsuarnceChanges == true ||_vm.EligMedicaid == true || _vm.EligMedicare == true || _vm.EligOther == true ) || (_vm.InsuranceChangeTxt != "" && _vm.InsuranceChangeTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "INSURANCE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 3;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr12 = new StringBuilder();
                            _msgStr12.Append(noteString);
                            _msgStr12.Append("Call Regarding = ");
                            if (_vm.InsuarnceChanges == true)
                                _msgStr12.Append(" Insuarnce Changes");
                            if (_vm.EligMedicaid == true)
                            
                                _msgStr12.Append(" - Eligibility Medicaid ");
                            
                            if (_vm.EligMedicare == true)

                                _msgStr12.Append(" - Eligibility Medicare");
                            if (_vm.EligOther == true)
                                _msgStr12.Append("  - Eligibility Other");

                            
                            if (_vm.EligMedicare == true)
                            {
                                _msgStr12.Append(Environment.NewLine +"Medicare Insurance Details are: " + Environment.NewLine);
                                _msgStr12.Append("Name : " + _vm.InsDetail.NameMedicare + Environment.NewLine);
                                _msgStr12.Append("State : " + _vm.InsDetail.StateMedicare + Environment.NewLine);
                                _msgStr12.Append("Verified Through : " + _vm.InsDetail.VerifiedThroughMedicare + Environment.NewLine);
                                if(_vm.InsDetail.NameMatchedMedicare == true)
                                   _msgStr12.Append("Name Matched : " + _vm.InsDetail.NameMatchedMedicare + Environment.NewLine);
                                if (_vm.InsDetail.NameNotMatchedMedicare == true)
                                    _msgStr12.Append("Name Matched : False"  + Environment.NewLine);
                                if (_vm.InsDetail.EffectiveDateMedicare != null)
                                _msgStr12.Append("Effective Date : " + Convert.ToDateTime(_vm.InsDetail.EffectiveDateMedicare).ToShortDateString() + Environment.NewLine);
                                if (_vm.InsDetail.TermDateMedicare != null)
                                    _msgStr12.Append("Term Date : " + Convert.ToDateTime(_vm.InsDetail.TermDateMedicare).ToShortDateString() + Environment.NewLine);
                                _msgStr12.Append("Benefit Plan : " + _vm.InsDetail.BenefitPlanMedicare + Environment.NewLine);
                                _msgStr12.Append("Level of Care : " + _vm.InsDetail.LevelOfCareMedicare + Environment.NewLine);

                                _msgStr12.Append("COB/Other Insurance : " + _vm.InsDetail.COBnOtherInsMedicare + Environment.NewLine);
                                if (_vm.InsDetail.InNetworkMedicare == true)
                                    _msgStr12.Append("In Network : " + _vm.InsDetail.InNetworkMedicare + Environment.NewLine);
                                if (_vm.InsDetail.OutofNetworkMedicare == true)
                                    _msgStr12.Append("Out of Network  "   + Environment.NewLine);
                                _msgStr12.Append("Deductible: " + _vm.InsDetail.DeductibleMedicare + Environment.NewLine);
                                _msgStr12.Append("Out of Pocket Max: " + _vm.InsDetail.OutofPocketMaxMedicare + Environment.NewLine);
                                _msgStr12.Append("Copay/Coinsurance: " + _vm.InsDetail.CopayCoinsMedicare + Environment.NewLine);
                                if (_vm.InsDetail.CopayCoinsWaivedMedicare == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   Yes " + Environment.NewLine);
                                if (_vm.InsDetail.CopayCoinsNotWaivedMedicare == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   No " + Environment.NewLine);
                                _msgStr12.Append("Benefits : ");

                                if (_vm.InsDetail.DMEMedicare == true)
                                    _msgStr12.Append("DME , "  );
                                if (_vm.InsDetail.MedicalMedicare == true)
                                    _msgStr12.Append("Medical , ");
                                if (_vm.InsDetail.PharmacyMedicare == true)
                                    _msgStr12.Append("Pharmacy   ");

                                _msgStr12.Append( Environment.NewLine + "Guidelines : ");
                                if (_vm.InsDetail.MedicaidMedicare == true)
                                    _msgStr12.Append("Medicaid , ");
                                if (_vm.InsDetail.MedicareMedicare == true)
                                    _msgStr12.Append("Medicare , ");
                                if (_vm.InsDetail.OtherMedicare == true)
                                    _msgStr12.Append("Other  ");

                                _msgStr12.Append(Environment.NewLine + "Member enrolled in : ");
                                if (_vm.InsDetail.HHEMedicare == true)
                                    _msgStr12.Append("HHE , "  );
                                if (_vm.InsDetail.HospiceMedicare == true)
                                    _msgStr12.Append("Hospice , "  );
                                if (_vm.InsDetail.HospitalMedicare == true)
                                    _msgStr12.Append("Hospital , "  );
                                if (_vm.InsDetail.Nursing_HomeMedicare == true)
                                    _msgStr12.Append("Nursing Home , "  );
                                if (_vm.InsDetail.NoneMedicare == true)
                                    _msgStr12.Append("None " );

                                _msgStr12.Append(Environment.NewLine + "HCPC/QtyLimitations: " + _vm.InsDetail.HCPCnQtyLimitationsMedicare + Environment.NewLine);

                          
                            }
                            //Medicaid
                            if (_vm.EligMedicaid == true)
                            {
                                _msgStr12.Append(Environment.NewLine + "Medicaid Insurance Details are: " + Environment.NewLine);
                                _msgStr12.Append("Name : " + _vm.InsDetail.NameMedicaid + Environment.NewLine);
                                _msgStr12.Append("State : " + _vm.InsDetail.StateMedicaid + Environment.NewLine);
                                _msgStr12.Append("Verified Through : " + _vm.InsDetail.VerifiedThroughMedicaid + Environment.NewLine);
                                if (_vm.InsDetail.NameMatchedMedicaid == true)
                                    _msgStr12.Append("Name Matched : " + _vm.InsDetail.NameMatchedMedicaid + Environment.NewLine);
                                if (_vm.InsDetail.NameNotMatchedMedicare == true)
                                    _msgStr12.Append("Name Matched : False" + Environment.NewLine);
                                if (_vm.InsDetail.EffectiveDateMedicaid != null)
                                    _msgStr12.Append("Effective Date : " + Convert.ToDateTime(_vm.InsDetail.EffectiveDateMedicaid).ToShortDateString() + Environment.NewLine);
                                if (_vm.InsDetail.TermDateMedicaid != null)
                                    _msgStr12.Append("Term Date : " + _vm.InsDetail.TermDateMedicaid + Environment.NewLine);

                                _msgStr12.Append("Benefit Plan : " + _vm.InsDetail.BenefitPlanMedicaid + Environment.NewLine);
                                _msgStr12.Append("Level of Care : " + _vm.InsDetail.LevelOfCareMedicaid + Environment.NewLine);

                                _msgStr12.Append("COB/Other Insurance : " + _vm.InsDetail.COBnOtherInsMedicaid + Environment.NewLine);
                                if (_vm.InsDetail.InNetworkMedicaid == true)
                                    _msgStr12.Append("In Nnetwork : " + _vm.InsDetail.InNetworkMedicaid + Environment.NewLine);
                                if (_vm.InsDetail.OutofNetworkMedicaid == true)
                                    _msgStr12.Append("Out of Network" + Environment.NewLine);

                                _msgStr12.Append("Deductible: " + _vm.InsDetail.DeductibleMedicaid + Environment.NewLine);
                                _msgStr12.Append("Out of Pocket Max: " + _vm.InsDetail.OutofNetworkMedicaid + Environment.NewLine);

                                _msgStr12.Append("Copay/Coinsurance: " + _vm.InsDetail.CopayCoinsMedicaid + Environment.NewLine);
                             
                                if (_vm.InsDetail.CopayCoinsWaivedMedicaid == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   Yes " + Environment.NewLine);
                                if (_vm.InsDetail.CopayCoinsNotWaivedMedicaid == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   No " + Environment.NewLine);

                                _msgStr12.Append("Benefits : ");
                                if (_vm.InsDetail.DMEMedicaid == true)
                                    _msgStr12.Append("DME , ");
                                if (_vm.InsDetail.MedicalMedicaid == true)
                                    _msgStr12.Append("Medical , ");
                                if (_vm.InsDetail.PharmacyMedicaid == true)
                                    _msgStr12.Append("Pharmacy   ");

                                _msgStr12.Append(Environment.NewLine + "Guidelines : ");
                                if (_vm.InsDetail.MedicaidMedicaid == true)
                                    _msgStr12.Append("Medicaid , ");
                                if (_vm.InsDetail.MedicareMedicaid == true)
                                    _msgStr12.Append("Medicare , ");
                                if (_vm.InsDetail.OtherMedicaid == true)
                                    _msgStr12.Append("Other  ");

                                _msgStr12.Append(Environment.NewLine + "Member enrolled in : ");
                                if (_vm.InsDetail.HHEMedicaid == true)
                                    _msgStr12.Append("HHE , ");
                                if (_vm.InsDetail.HospiceMedicaid == true)
                                    _msgStr12.Append("Hospice , ");
                                if (_vm.InsDetail.HospitalMedicaid == true)
                                    _msgStr12.Append("Hospital , ");
                                if (_vm.InsDetail.Nursing_HomeMedicaid == true)
                                    _msgStr12.Append("Nursing Home , ");
                                if (_vm.InsDetail.NoneMedicaid == true)
                                    _msgStr12.Append("None ");

                                _msgStr12.Append("HCPCnQtyLimitations: " + _vm.InsDetail.HCPCnQtyLimitationsMedicaid + Environment.NewLine);
 
                            }
                            //Other
                            if (_vm.EligOther == true)
                            {
                                _msgStr12.Append(Environment.NewLine + "Other Insurance Details are: " + Environment.NewLine);
                                _msgStr12.Append("Name : " + _vm.InsDetail.Name + Environment.NewLine);
                                _msgStr12.Append("State : " + _vm.InsDetail.State + Environment.NewLine);
                                _msgStr12.Append("Verified Through : " + _vm.InsDetail.VerifiedThrough + Environment.NewLine);
                                if (_vm.InsDetail.NameMatched == true)
                                    _msgStr12.Append("Name Matched : " + _vm.InsDetail.NameMatched + Environment.NewLine);
                                if (_vm.InsDetail.NameNotMatched == true)
                                    _msgStr12.Append("Name Matched :  False"  + Environment.NewLine);
                                if (_vm.InsDetail.EffectiveDate != null)
                                    _msgStr12.Append("Effective Date : " + Convert.ToDateTime(_vm.InsDetail.EffectiveDate).ToShortDateString() + Environment.NewLine);
                                if (_vm.InsDetail.TermDate != null)
                                    _msgStr12.Append("Term Date : " + Convert.ToDateTime(_vm.InsDetail.TermDate).ToShortDateString() + Environment.NewLine);
                                _msgStr12.Append("Benefit Plan : " + _vm.InsDetail.BenefitPlan + Environment.NewLine);
                                _msgStr12.Append("Level of Care : " + _vm.InsDetail.LevelOfCare+ Environment.NewLine);

                                _msgStr12.Append("COB/Other Insurance : " + _vm.InsDetail.COBnOtherIns + Environment.NewLine);
                                if (_vm.InsDetail.InNetwork == true)
                                    _msgStr12.Append("In Nnetwork : " + _vm.InsDetail.InNetwork + Environment.NewLine);
                                if (_vm.InsDetail.OutofNetwork == true)
                                    _msgStr12.Append("Out of Network  "  + Environment.NewLine);

                                _msgStr12.Append("Deductible: " + _vm.InsDetail.Deductible + Environment.NewLine);
                                _msgStr12.Append("Ot of Pocket Max: " + _vm.InsDetail.OutofPocketMax + Environment.NewLine);

                                _msgStr12.Append("Copay/Coinsurance: " + _vm.InsDetail.CopayCoins + Environment.NewLine);
                                if (_vm.InsDetail.CopayCoinsWaived == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   Yes " + Environment.NewLine);
                                if (_vm.InsDetail.CopayCoinsNotWaived == true)
                                    _msgStr12.Append("Is copay/coinsurance waived ?  :   No " + Environment.NewLine);

                                _msgStr12.Append("Benefits : ");
                                if (_vm.InsDetail.DME == true)
                                    _msgStr12.Append("DME , ");
                                if (_vm.InsDetail.Medical == true)
                                    _msgStr12.Append("Medical , ");
                                if (_vm.InsDetail.Pharmacy == true)
                                    _msgStr12.Append("Pharmacy   ");

                                _msgStr12.Append(Environment.NewLine + "Guidelines : ");
                                if (_vm.InsDetail.Medicaid == true)
                                    _msgStr12.Append("Medicaid , ");
                                if (_vm.InsDetail.Medicare == true)
                                    _msgStr12.Append("Medicare , ");
                                if (_vm.InsDetail.Other == true)
                                    _msgStr12.Append("Other  ");

                                _msgStr12.Append(Environment.NewLine + "Member enrolled in : ");
                                if (_vm.InsDetail.HHE == true)
                                    _msgStr12.Append("HHE , ");
                                if (_vm.InsDetail.Hospice == true)
                                    _msgStr12.Append("Hospice , ");
                                if (_vm.InsDetail.Hospital == true)
                                    _msgStr12.Append("Hospital , ");
                                if (_vm.InsDetail.Nursing_Home == true)
                                    _msgStr12.Append("Nursing Home , ");
                                if (_vm.InsDetail.None == true)
                                    _msgStr12.Append("None ");

                                _msgStr12.Append("HCPCnQtyLimitations: " + _vm.InsDetail.HCPCnQtyLimitations + Environment.NewLine);

                                
                            }
                            _msgStr12.Append( Environment.NewLine + "Note = " + _vm.InsuranceChangeTxt);

                            _tHist.NoteText = _msgStr12.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding = InsuarnceChanges_" + "Note = " + _vm.InsuranceChangeTxt;

                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //NEW ACCOUNT 
                    if (_vm.NewAccount == true && (_vm.NewAccountTxtArea != "" && _vm.NewAccountTxtArea != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "NEW ACCOUNT").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "NEW ACCOUNT";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 1;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "NEW ACCOUNT").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _NewaccStr = new StringBuilder();
                            //string newaccStr = "";

                            if (_vm.TypeSupplies1 != null && _vm.TypeSupplies1 != "")
                            {
                                _NewaccStr.Append("Type Supplies = " + _vm.TypeSupplies1 + Environment.NewLine);
                                //newaccStr = newaccStr + "Type Supplies = " + _vm.TypeSupplies1 + "_";
                            }
                            if (_vm.TypeSupplies2 != null && _vm.TypeSupplies2 != "")
                            {

                                _NewaccStr.Append("Type Supplies = " + _vm.TypeSupplies2 + Environment.NewLine);
                                //newaccStr = newaccStr + "Type Supplies = " + _vm.TypeSupplies2 + "_";
                            }

                            if (_vm.TypeSuppliesOther != null && _vm.TypeSuppliesOther != "")
                            {
                                _NewaccStr.Append("Other Type Supplies = " + _vm.TypeSuppliesOther + Environment.NewLine);
                                // newaccStr = newaccStr + "Other Type Supplies = " + _vm.TypeSuppliesOther + "_";
                            }

                            StringBuilder _msgStr13 = new StringBuilder();
                            _msgStr13.Append(noteString);
                            _msgStr13.Append("Call Regarding = New Account" + Environment.NewLine + _NewaccStr + Environment.NewLine + "Note = " + _vm.NewAccountTxtArea);

                            _tHist.NoteText = _msgStr13.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = New Account_" + newaccStr + "Note = " + _vm.NewAccountTxtArea;


                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }
                    //ACTIVE/INACTIVE 
                    if (_vm.Restart == true && (_vm.NewAccountTxtArea != "" && _vm.NewAccountTxtArea != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ACTIVE/INACTIVE").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ACTIVE/INACTIVE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 12;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ACTIVE/INACTIVE").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _RestartStr = new StringBuilder();
                            //string restrtStr = "";

                            if (_vm.TypeSupplies1 != null && _vm.TypeSupplies1 != "")
                            {
                                _RestartStr.Append("Type supplies = " + _vm.TypeSupplies1 + Environment.NewLine);
                                //restrtStr = restrtStr + "Type supplies = " + _vm.TypeSupplies1 + "_";
                            }
                            if (_vm.TypeSupplies2 != null && _vm.TypeSupplies2 != "")
                            {
                                _RestartStr.Append("Type supplies = " + _vm.TypeSupplies2 + Environment.NewLine);
                                // restrtStr = restrtStr + "Type supplies = " + _vm.TypeSupplies2 + "_";
                            }
                            if (_vm.TypeSuppliesOther != null && _vm.TypeSuppliesOther != "")
                            {
                                _RestartStr.Append("Other = " + _vm.TypeSuppliesOther + Environment.NewLine);
                                //restrtStr = restrtStr + "Other = " + _vm.TypeSuppliesOther + "_";
                            }
                            // _RestartStr.Append(_vm.NewAccountTxtArea + Environment.NewLine);
                            //  restrtStr = restrtStr + _vm.NewAccountTxtArea;

                            StringBuilder _msgStr14 = new StringBuilder();
                            _msgStr14.Append(noteString);
                            _msgStr14.Append("Call Regarding = Restart" + Environment.NewLine + _RestartStr + "Note = " + _vm.NewAccountTxtArea);

                            _tHist.NoteText = _msgStr14.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Restart_" + restrtStr + "Note = " + _vm.NewAccountTxtArea;

                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }


                    //ACTIVE/INACTIVE 
                    if (_vm.AccountDeactivated == true && (_vm.OtherHandlingTxt != "" && _vm.OtherHandlingTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ACTIVE/INACTIVE").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ACTIVE/INACTIVE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 12;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ACTIVE/INACTIVE").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;



                            StringBuilder _msgStr14 = new StringBuilder();
                            _msgStr14.Append(noteString);
                            _msgStr14.Append("Call Regarding = Other Call Handling" + Environment.NewLine + "Account Deactivated" + Environment.NewLine + "Note = " + _vm.OtherHandlingTxt);

                            _tHist.NoteText = _msgStr14.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Restart_" + restrtStr + "Note = " + _vm.NewAccountTxtArea;

                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //ORDER CONFIRMATION  
                    if (_vm.OrderConfirmation == true && (_vm.OrderConfirmationTxt != "" && _vm.OrderConfirmationTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ORDER CONFIRMATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 15;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr15 = new StringBuilder();
                            _msgStr15.Append(noteString);
                            _msgStr15.Append("Call Regarding = Order Confirmation" + Environment.NewLine + "Note = " + _vm.OrderConfirmationTxt);

                            _tHist.NoteText = _msgStr15.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Order Confirmation_" + "Note = " + _vm.OrderConfirmationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //COMMUNICATION
                    if ((_vm.FedExOrUSPSTracking == true || _vm.OrderShipped == true ||  _vm.SentReqPurchasing == true|| _vm.OrderDropShipped == true || _vm.OrderETA == true || _vm.RWOCreated == true) && (_vm.OrderStatusTxt != "" && _vm.OrderStatusTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();   //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "COMMUNICATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 9;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            // string str3 = "";
                            StringBuilder _ComStr = new StringBuilder();

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            if (_vm.FedExOrUSPSTracking)
                                _ComStr.Append("FedEx/USPS Tracking" + Environment.NewLine + _vm.FedExOrUSPSTrackingNumber + Environment.NewLine);
                            //str3 = str3 + "FedEx/USPS Tracking_";
                            if (_vm.OrderShipped)
                                _ComStr.Append("Order Shipped" + Environment.NewLine);

                            if (_vm.SentReqPurchasing)
                                _ComStr.Append("Sent request to purchasing" + Environment.NewLine);

                            if (_vm.OrderDropShipped)
                                _ComStr.Append("Order Drop Shipped" + Environment.NewLine);
                            //str3 = str3 + "OrderShipped_";
                            if (_vm.OrderETA)
                                _ComStr.Append("Order ETA" + Environment.NewLine);
                            // str3 = str3 + "Order ETA_";
                            if (_vm.RWOCreated)
                                _ComStr.Append("RWO Created" + Environment.NewLine);
                            // str3 = str3 + "RWO Created_";

                            StringBuilder _msgStr16 = new StringBuilder();
                            _msgStr16.Append(noteString);
                            _msgStr16.Append("Call Regarding = " + _ComStr + Environment.NewLine + "Note = " + _vm.OrderStatusTxt);

                            _tHist.NoteText = _msgStr16.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = " + str3 + "Note_" + _vm.OrderStatusTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //ORDER HELD  
                    if (_vm.OrderHolding == true && (_vm.OrderStatusTxt != "" && _vm.OrderStatusTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER HELD").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "ORDER HELD";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 29;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER HELD").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr17 = new StringBuilder();
                            _msgStr17.Append(noteString);
                            _msgStr17.Append("Call Regarding = Order Holding" + Environment.NewLine + "Note = " + _vm.OrderStatusTxt);

                            _tHist.NoteText = _msgStr17.ToString();

                            // _tHist.NoteText = noteString + " Call Regarding = Order Holding_" + "Note = " + _vm.OrderStatusTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //PRODUCT  
                    if ((_vm.PC_IncreaseOrDecrease == true || _vm.PC_Hold == true || _vm.PC_RemoveOrAdd == true || _vm.ProductChange == true) && (_vm.ProductChangeTxt != "" && _vm.ProductChangeTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRODUCT").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "PRODUCT";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 10;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "PRODUCT").FirstOrDefault(); //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            // string str4 = "";
                            StringBuilder _proStr = new StringBuilder();
                            if (_vm.PC_IncreaseOrDecrease)
                                _proStr.Append("Increase/Decrease" + Environment.NewLine);
                            // str4 = str4 + "Increase/Decrease_";
                            if (_vm.PC_Hold)
                                _proStr.Append("Hold" + Environment.NewLine);
                            //str4 = str4 + "Hold_";
                            if (_vm.PC_RemoveOrAdd)
                                _proStr.Append("Remove/Add" + Environment.NewLine);
                            // str4 = str4 + "Remove/Add_";
                            if (_vm.ProductChange)
                                _proStr.Append("Product Change" + Environment.NewLine);
                            //str4 = str4 + "Product Change";

                            StringBuilder _msgStr18 = new StringBuilder();
                            _msgStr18.Append(noteString);
                            _msgStr18.Append("Call Regarding = " + _proStr + Environment.NewLine + "Note = " + _vm.ProductChangeTxt);

                            _tHist.NoteText = _msgStr18.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = " + str4 + "Note = " + _vm.ProductChangeTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //SAMPLE CHOICE 
                    if ((_vm.SampleChoice == true || _vm.SampleOrderSent == true) && (_vm.SampleTxt != "" && _vm.SampleTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SAMPLE CHOICE").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "SAMPLE CHOICE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 17;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SAMPLE CHOICE").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr19 = new StringBuilder();
                            _msgStr19.Append(noteString);
                            _msgStr19.Append("Call Regarding = Sample Choice " + Environment.NewLine);
                            if(_vm.SampleOrderSent == true)
                            _msgStr19.Append("Sample Order Sent" + Environment.NewLine);

                            _msgStr19.Append("Note = " + _vm.SampleTxt);

                            _tHist.NoteText = _msgStr19.ToString();

                            // _tHist.NoteText = noteString + "Note = " + _vm.SampleTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }


                    //Sample Task
                    if (_vm.SampleTask == true && (_vm.SampleTxt != "" && _vm.SampleTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SAMPLE TASK").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "SAMPLE TASK";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 22;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SAMPLE TASK").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr19 = new StringBuilder();
                            _msgStr19.Append(noteString);
                            _msgStr19.Append("Call Regarding = Sample Task " + Environment.NewLine);
                            _msgStr19.Append("Note = " + _vm.SampleTxt);

                            _tHist.NoteText = _msgStr19.ToString();

                            // _tHist.NoteText = noteString + "Note = " + _vm.SampleTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //SHIPPING ISSUE  
                    if ((_vm.DefectiveProductOrNotUsable == true || _vm.WrongOrExtraProductShipped == true || _vm.MissingProduct == true || _vm.Sh_Other == true) && (_vm.ShippingIssueTxt != "" && _vm.ShippingIssueTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SHIPPING ISSUE").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "SHIPPING ISSUE";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 11;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "SHIPPING ISSUE").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {
                            // string str5 = "";
                            StringBuilder _shipStr = new StringBuilder();
                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            if (_vm.DefectiveProductOrNotUsable)
                                _shipStr.Append("Defective Product/Product not Usable" + Environment.NewLine);
                            //str5 = str5 + "Defective Product/Product not Usable_";
                            if (_vm.WrongOrExtraProductShipped)
                                _shipStr.Append("Wrong/Extra Product Shipped" + Environment.NewLine);
                            //str5 = str5 + "Wrong/Extra Product Shipped_";
                            if (_vm.MissingProduct)
                                _shipStr.Append("Missing Product" + Environment.NewLine);
                            //str5 = str5 + "Missing Product_";
                            if (_vm.Sh_Other)
                                _shipStr.Append("Other =");
                            //str5 = str5 + "Other = ";
                            if (_vm.ShippingOtherName != null && _vm.ShippingOtherName != "")
                                _shipStr.Append(_vm.ShippingOtherName + Environment.NewLine);
                            // str5 = str5 + _vm.ShippingOtherName + "_";

                            StringBuilder _msgStr20 = new StringBuilder();
                            _msgStr20.Append(noteString);
                            _msgStr20.Append("Call Regarding = " + _shipStr + Environment.NewLine + "Note = " + _vm.ShippingIssueTxt);

                            _tHist.NoteText = _msgStr20.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = " + str5 + "Note = " + _vm.ShippingIssueTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //COMMUNICATION
                    if ((_vm.TransferredCall != null && _vm.TransferredCall != "") || (_vm.TransferredCallTxtArea != "" && _vm.TransferredCallTxtArea != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "COMMUNICATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 9;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;


                            StringBuilder _msgStr21 = new StringBuilder();
                            _msgStr21.Append(noteString);
                            _msgStr21.Append("Transferred Call To = " + _vm.TransferredCall + Environment.NewLine + "Note = " + _vm.TransferredCallTxtArea);

                            _tHist.NoteText = _msgStr21.ToString();

                            //_tHist.NoteText = noteString + "Transferred Call To = " + _vm.TransferredCall + "_" + "Note=" + _vm.TransferredCallTxtArea;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //_vm.AccountDeactivated == true  (_vm.OtherHandlingTxt != "" && _vm.OtherHandlingTxt != null)
                    //COMMUNICATION


                    if ((_vm.ReturnedCall_LeftVoicemail == true || _vm.WrongNumber == true  || _vm.Other_CallHandling == true || _vm.AccountDeactivated == true) && (_vm.OtherHandlingTxt != "" && _vm.OtherHandlingTxt != null))
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "COMMUNICATION";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 9;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMUNICATION").FirstOrDefault();  // && t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _callStr = new StringBuilder();

                            //  string str6 = "";
                            if (_vm.ReturnedCall_LeftVoicemail)
                                _callStr.Append("Returned Call & Left Voicemail" + Environment.NewLine);
                            //        str6 = str6 + "Returned Call & Left Voicemail_";

                            if (_vm.WrongNumber)
                                _callStr.Append("Wrong Number" + Environment.NewLine);
                            //str6 = str6 + "Wrong Number_";

                            //  if (_vm.AccountDeactivated)
                            // _callStr.Append("Account Deactivated" + Environment.NewLine);
                            // str6 = str6 + "Account Deactivated_";
                            if (_vm.AccountDeactivated)
                                _callStr.Append("Account Deactivated" + Environment.NewLine);

                       


                            if (_vm.Other_CallHandling)
                                _callStr.Append("Other" + Environment.NewLine);
                            // str6 = str6 + "Nursing / CSRassessment_";

                            StringBuilder _msgStr22 = new StringBuilder();
                            _msgStr22.Append(noteString);
                            _msgStr22.Append("Call Regarding = " + _callStr + Environment.NewLine + "Note = " + _vm.OtherHandlingTxt);

                            _tHist.NoteText = _msgStr22.ToString();


                            // _tHist.NoteText = noteString + "Call Regarding = " + str6 + "Note = " + _vm.OtherHandlingTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
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

            StringBuilder _msgStr = new StringBuilder();
            _msgStr.Append(noteString);
            _msgStr.Append(otherStr + Environment.NewLine);
            _msgStr.Append("OutCome = " + _vm.ComplainOutCome);

            string returnStr = _msgStr.ToString();

            returnStr = returnStr.Replace(Environment.NewLine, "<br />");

            return returnStr;
        }

        public static void sendEmail(string message, Int64? acc, int? reference)
        {


            //  DateTime todaydate = DateTime.Today.Date;



            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");

            mail.To.Add("disteamleader@jandbmedical.com");
            //    mail.To.Add("dvasquez@jandbmedical.com");
            mail.Bcc.Add("grani@jandbmedical.com");


            mail.Subject = "CSR Call Complaint Log";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Issue for account - " + acc + " has not been resolved.</td><td></td>";
            mail.Body += "</tr>";

            //mail.Body += "<tr>";
            //mail.Body += "<td>  Reference Number is:  </td> " + reference + "<td></td>";
            //mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Note:  </td> " + message + "<td></td>";
            mail.Body += "</tr>";


            mail.Body += "<tr>";
            mail.Body += "<td> Link : http://10.10.1.49/JBIntranet/Reports/CustomerServices/CSRComlaintlog </td> <td>  </td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
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
        public static BCNAccInfoVM GetBCNDetails(Int64? Account)
        {
            //  IList<AccountInfoVM> _vm = new List<AccountInfoVM>();
            BCNAccInfoVM tableRec = new BCNAccInfoVM();
            int? account;
            account = Convert.ToInt32(Account);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
               

               // tbl_Account_Member_VM _list = new tbl_Account_Member_VM();
                if (account != null && account != 0)
                {
                    // _list = _db.tbl_Account_Member.Where(t => t.Account == account && t.Member == 1).SingleOrDefault();
                    var _list = (from t in _db.tbl_Account_Member
                                 where t.Account == account && t.Member == 1
                                 select new tbl_Account_Member_VM
                                 {
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Address_1 = t.Address_1 ,
                                     Address_2 = t.Address_2,
                                     City = t.City,
                                     State = t.State,
                                     Zip = t.Zip,
                                     Phone = t.Phone,
                                     BirthDate = t.BirthDate,
                                     EmailAddress = t.EmailAddress,
                                     County = t.County,
                                     AltPhone = t.AltPhone,
                                     Sex = t.Sex, 
                                     ID_Default_Referring_Doctor = t.ID_Default_Referring_Doctor

                                 }).SingleOrDefault();


                    if (_list != null)
                    {
                        tableRec.firstName = _list.First_Name + " " + _list.Last_Name;
                        tableRec.lastName = _list.Last_Name;
                        tableRec.address1 = _list.Address_1;
                        tableRec.address2 = _list.Address_2;
                        tableRec.city = _list.City;
                        tableRec.state = _list.State;
                        tableRec.zipcode = _list.Zip;
                        tableRec.phone = _list.Phone;
                        tableRec.DOB = _list.BirthDate;
                        tableRec.Email = _list.EmailAddress;
                        tableRec.Account2 = account;
                        tableRec.County = _list.County;
                        tableRec.ALtphone = _list.AltPhone;
                        tableRec.Gender = _list.Sex; 
                        tableRec.PrimaryPhysician = _db.tbl_Referral_Source_Table.Where(t => t.ID == _list.ID_Default_Referring_Doctor).Select(t=>t.First_Name + " " +t.Last_Name).SingleOrDefault();

                        

                    }
                    else
                    {
                        tableRec.firstName = "WrongAccount";
                    }

                }
                if (account == null || account == 0)
                {


                    tableRec.firstName = "New Account";

                    tableRec.Account2 = account;

                }
                // if (_list != null)
                //   _vm.Add(tableRec);


            }
            return tableRec;
        }


        public class tbl_Account_Member_VM
        {
            public int Account { get; set; }
            public short Member { get; set; }
          
            public string First_Name { get; set; }
           
            public string Last_Name { get; set; }
            public string Sex { get; set; }
            public Nullable<System.DateTime> BirthDate { get; set; }
        
          
            public string Address_1 { get; set; }
            public string Address_2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Phone { get; set; }
            public string AltPhone { get; set; }
            public string County { get; set; }
            public string EmailAddress { get; set; }
 
            public Nullable<int> ID_Default_Referring_Doctor { get; set; }
            
         
         
        }
        public static IList<ProviderVm> GetProviderList(Int64? acct)
        {
            //int? HDMS_ProviderID = 0;
            //using (HHSQLDBEntities _hdb = new HHSQLDBEntities())
            //{
            //    HDMS_ProviderID = (from t in _hdb.tbl_Account_Member
            //                       where t.Account == acct && t.Member == 1
            //                       select   t.ID_Default_Provider).FirstOrDefault();
            //}
            using (IntranetEntities _db = new IntranetEntities())
            {
                try
                {
                    ProviderVm vn = new ProviderVm();

                    var _list = (from p in _db.BCBS_ProviderList

                                 select new ProviderVm
                                 {
                                     ProviderId = p.ID,
                                     ProviderName = p.Name
                                 }).Distinct().OrderBy(t => t.ProviderId).ToList();


                    _list.Insert(0, vn);

                    return _list;


                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return new List<ProviderVm>();
                }
            }
        }


        public static IList<InFusionSetsVm> GetInfusionSetList()
        {
           
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                try
                {
                    InFusionSetsVm vn = new InFusionSetsVm();

                    var _list = (from p in _db.tbl_Product_Table
                                 join c in _db.tbl_ProductCategory_Table on p.ID_ProductCategory equals c.ID
                                 where ( c.ID == 69 || c.ID == 94) && p.DeletedDate == null && p.Discontinued != true
                                 select new InFusionSetsVm
                                 {
                                     Assmnt_InfusionsettxtCGMID = p.ID,
                                     Assmnt_InfusionsettxtCGM = p.ProductCode
                                 }).Distinct().OrderBy(t => t.Assmnt_InfusionsettxtCGMID).ToList();


                    _list.Insert(0, vn);

                    return _list;


                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return new List<InFusionSetsVm>();
                }
            }
        }

        public static IList<CartridgesSetsVm> GetCartridgesSetList()
        {

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                try
                {
                    CartridgesSetsVm vn = new CartridgesSetsVm();

                    var _list = (from p in _db.tbl_Product_Table
                                 join c in _db.tbl_ProductCategory_Table on p.ID_ProductCategory equals c.ID
                                 where (c.ID == 68) && p.DeletedDate == null && p.Discontinued != true
                                 select new CartridgesSetsVm
                                 {
                                     Assmnt_catridgestxtCGMID = p.ID,
                                     Assmnt_catridgestxtCGM = p.ProductCode
                                 }).Distinct().OrderBy(t => t.Assmnt_catridgestxtCGMID).ToList();


                    _list.Insert(0, vn);

                    return _list;


                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return new List<CartridgesSetsVm>();
                }
            }
        }
    }

    public class ProviderVm
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
    }

    public class InFusionSetsVm
    {
        public int Assmnt_InfusionsettxtCGMID { get; set; }
        public string Assmnt_InfusionsettxtCGM { get; set; }
    }

    public class CartridgesSetsVm
    {
        public int Assmnt_catridgestxtCGMID { get; set; }
        public string Assmnt_catridgestxtCGM { get; set; }
    }
    public class BCNAccInfoVM
    {

     
        public int? Account2 { get; set; }
        public string TimerVal { get; set; }
        public string firstName { get; set; }

        public string Email { get; set; }
        public string Gender { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string phone { get; set; }
        public string ALtphone { get; set; }

        public string County { get; set; }
        public string PrimaryPhysician { get; set; }
      

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

    }

    public class CallLogInsDetail
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string VerifiedThrough { get; set; }
        public bool NameMatched { get; set; }
        public bool NameNotMatched { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TermDate { get; set; }
        public string BenefitPlan { get; set; }
        public string LevelOfCare { get; set; }
        public string COBnOtherIns { get; set; }
        public bool InNetwork { get; set; }
        public bool OutofNetwork { get; set; }


        public double Deductible { get; set; }

        public double OutofPocketMax { get; set; }
        public string CopayCoins { get; set; }

        public bool CopayCoinsWaived { get; set; }
        public bool CopayCoinsNotWaived { get; set; }
        // public bool Benefits { get; set; }
        public bool DME { get; set; }
        public bool Medical { get; set; }
        public bool Pharmacy { get; set; }

        // public bool Guidlines { get; set; }
        public bool Medicaid { get; set; }
        public bool Medicare { get; set; }
        public bool Other { get; set; }
        public string HCPCnQtyLimitations { get; set; }
        public string NameMedicaid { get; set; }
        public string StateMedicaid { get; set; }
        public string VerifiedThroughMedicaid { get; set; }
        public bool NameMatchedMedicaid { get; set; }
        public bool NameNotMatchedMedicaid { get; set; }
        public DateTime? EffectiveDateMedicaid { get; set; }
        public DateTime? TermDateMedicaid { get; set; }
        public string BenefitPlanMedicaid { get; set; }
        public string LevelOfCareMedicaid { get; set; }
        public string COBnOtherInsMedicaid { get; set; }
        public bool InNetworkMedicaid { get; set; }
        public bool OutofNetworkMedicaid { get; set; }

        public double DeductibleMedicaid { get; set; }

        public double OutofPocketMaxMedicaid { get; set; }
        public string CopayCoinsMedicaid { get; set; }

        public bool CopayCoinsWaivedMedicaid { get; set; }
        public bool CopayCoinsNotWaivedMedicaid { get; set; }
        // public bool Benefits { get; set; }
        public bool DMEMedicaid { get; set; }
        public bool MedicalMedicaid { get; set; }
        public bool PharmacyMedicaid { get; set; }

        // public bool Guidlines { get; set; }
        public bool MedicaidMedicaid { get; set; }
        public bool MedicareMedicaid { get; set; }
        public bool OtherMedicaid { get; set; }
        public string HCPCnQtyLimitationsMedicaid { get; set; }

        public string NameMedicare { get; set; }
        public string StateMedicare { get; set; }
        public string VerifiedThroughMedicare { get; set; }
        public bool NameMatchedMedicare { get; set; }
        public bool NameNotMatchedMedicare { get; set; }
        public DateTime? EffectiveDateMedicare { get; set; }
        public DateTime? TermDateMedicare { get; set; }
        public string BenefitPlanMedicare { get; set; }
        public string LevelOfCareMedicare { get; set; }
        public string COBnOtherInsMedicare { get; set; }
        public bool InNetworkMedicare { get; set; }
        public bool OutofNetworkMedicare { get; set; }

        public double DeductibleMedicare { get; set; }

        public double OutofPocketMaxMedicare { get; set; }
        public string CopayCoinsMedicare { get; set; }

        public bool CopayCoinsWaivedMedicare { get; set; }
        public bool CopayCoinsNotWaivedMedicare { get; set; }
        // public bool Benefits { get; set; }
        public bool DMEMedicare { get; set; }
        public bool MedicalMedicare { get; set; }
        public bool PharmacyMedicare { get; set; }

        // public bool Guidlines { get; set; }
        public bool MedicaidMedicare { get; set; }
        public bool MedicareMedicare { get; set; }
        public bool OtherMedicare { get; set; }
        public string HCPCnQtyLimitationsMedicare { get; set; }
        public bool HHE { get; set; }
        public bool Hospice { get; set; }
        public bool Hospital { get; set; }
        public bool Nursing_Home { get; set; }
        public bool None { get; set; }
        public bool HHEMedicare { get; set; }
        public bool HospiceMedicare { get; set; }
        public bool HospitalMedicare { get; set; }
        public bool Nursing_HomeMedicare { get; set; }
        public bool NoneMedicare { get; set; }
        public bool HHEMedicaid { get; set; }
        public bool HospiceMedicaid { get; set; }
        public bool HospitalMedicaid { get; set; }
        public bool Nursing_HomeMedicaid { get; set; }
        public bool NoneMedicaid { get; set; }

    }
    public class BCNCallLogVM
    {
        public bool IsDiab { get; set; }
        public bool IsInsulin { get; set; }
        public CallLogInsDetail InsDetail { get; set; }
        public int? Providerid { get; set; }
        public string ProviderName { get; set; }
        public SelectList ProviderList { get; set; }
        public int? Assmnt_InfusionsettxtCGMID { get; set; }
        //  public SelectList Assmnt_InfusionsettxtCGMDW { get; set; }
        public IList<InFusionSetsVm> Assmnt_InfusionsettxtCGMDW { get; set; }

        public int? Assmnt_catridgestxtCGMID { get; set; }
        //public SelectList Assmnt_catridgestxtCGMDW { get; set; }
        public IList<CartridgesSetsVm> Assmnt_catridgestxtCGMDW { get; set; }
        //-----------------update ---------------------------//
        public bool Newacc { get; set; }
        //public string Product { get; set; }
        public string Mem_FN { get; set; }
        public string Mem_LN { get; set; }
        public string Mem_MN { get; set; }
        public string Mem_Add1 { get; set; }
        public string Mem_Add2 { get; set; }
        public string Mem_City { get; set; }
        public string Mem_State { get; set; }
        public string Mem_ZIp { get; set; }
        public string Mem_County { get; set; }
        public DateTime Mem_DOB { get; set; }

        public string Mem_Gender { get; set; }
        public string Mem_BirthDate { get; set; }
        public string Mem_Phone { get; set; }
        public string Mem_AltPhone { get; set; }
        public string PriPhysician { get; set; }
        public string Provider { get; set; }
        public string WebAccountCreated { get; set; }
        public string Manufacturer { get; set; }
        public string OrderStatus { get; set; }
        //  public string DeliveryCompany { get; set; }

        //   public string BCNProvider { get; set; }
        public bool MadePayment { get; set; }
       // public bool DidntFollowDelIns { get; set; }
      //  public bool VConfirmationCalls { get; set; }
      //  public bool VPaymentCalles { get; set; }

        public bool Julie_VictorCalls { get; set; }
        public bool PhonePrompts_SelfService { get; set; }

       // public bool VirtualCallBack { get; set; }

       // public bool Website { get; set; }

        public bool NoFollowUp { get; set; }

       // public bool ReturnedFromVM { get; set; }
        public bool NoPresORCMN { get; set; }
       // public bool NeverRecivedSupplies { get; set; }

        public bool PhysicianIssue { get; set; }
        //   public bool InsLimitGuidelines { get; set; }

        //   public bool BCNProviderIssue { get; set; }

        public bool assmnt_Diab { get; set; }
        public bool assmnt_Insulin { get; set; }

        public bool Other { get; set; }

        //--------------------------------------------------

        public bool OpPermission { get; set; }
        public string errormsg { get; set; }
        public string empFullName { get; set; }

        //   public string refnum { get; set; }
        public bool firstTime { get; set; }
        public string TimerTxt { get; set; }

        [Required]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public Int64? Account { get; set; }
        public bool VerifiedPHI { get; set; }
        public bool VoicesForHealth { get; set; }

        public bool Call { get; set; }
        public bool Task { get; set; }
        public bool Email { get; set; }
        public bool Fax { get; set; }
        public bool Web { get; set; }
        public bool RxForm { get; set; }
        public bool LogTypeOther { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Relation { get; set; }
        public string otherRelname { get; set; }
        public bool Copay { get; set; }
        public bool Billing { get; set; }

        public bool SampleChoice { get; set; }
        public bool SampleOrderSent { get; set; }
        public bool SampleTask { get; set; }
        public bool Eligibility { get; set; }
        public string BillingTxt { get; set; }
        public bool Phone { get; set; }
        public bool Address { get; set; }

        public bool Physician { get; set; }
        public string DemographicChanges { get; set; }

        public string FedExOrUSPSTrackingNumber { get; set; }

        public bool AOB { get; set; }
        public bool Prescription { get; set; }
        public bool CMN { get; set; }
        public bool PriorAuthorization { get; set; }
        public bool SupportingDoc { get; set; }
        public bool TeacherLetter { get; set; }
        public bool Logs { get; set; }
        public bool ABN { get; set; }

       // public bool LMN { get; set; }

        public bool NewAccount { get; set; }

        public bool InsuarnceChanges { get; set; }
        public bool Restart { get; set; }
        public bool EligMedicare { get; set; }
        public bool EligMedicaid { get; set; }
        public bool EligOther { get; set; }
        public bool OrderConfirmation { get; set; }

        public bool FedExOrUSPSTracking { get; set; }
        public bool OrderShipped { get; set; }
        public bool OrderDropShipped { get; set; }
        
        public bool OrderETA { get; set; }
        public bool OrderHolding { get; set; }
        public bool RWOCreated { get; set; }

        public bool ProductChange { get; set; }
        public bool PC_IncreaseOrDecrease { get; set; }

        public bool PC_Hold { get; set; }
        public bool PC_RemoveOrAdd { get; set; }

        public bool WrongOrExtraProductShipped { get; set; }

        public bool DefectiveProductOrNotUsable { get; set; }
        public bool MissingProduct { get; set; }

        public bool ReturnedCall_LeftVoicemail { get; set; }
        public bool WrongNumber { get; set; }
        public bool SentReqPurchasing { get; set; }

        public bool Other_CallHandling { get; set; }
        public bool Sh_Other { get; set; }
        // public bool Compliant { get; set; }

        public bool AccountDeactivated { get; set; }
        public bool AOBReceived { get; set; }
        public bool RXReceived { get; set; }
        public bool PASent { get; set; }
        public bool PAReceived { get; set; }
        public bool ClinicalDocsReq { get; set; }
        public bool ClinicalDocsReceived { get; set; }

        public string TypeSuppliesOther { get; set; }

        public string NewAccountTxtArea { get; set; }
        public string InsuranceChangeTxt { get; set; }

        public string Documentation { get; set; }
        public string TypeSupplies1 { get; set; }
        public string TypeSupplies2 { get; set; }
        public string OrderConfirmationTxt { get; set; }
        public string OrderStatusTxt { get; set; }
        public string ProductChangeTxt { get; set; }
        public string TransferredCall { get; set; }
        public string TransferredCallTxtArea { get; set; }
        public string ShippingOtherName { get; set; }
        public string ShippingIssueTxt { get; set; }
        public string OtherHandlingTxt { get; set; }

        public string SampleTxt { get; set; }
        public string DocumentationTxt { get; set; }
        public BCNAccInfoVM details { get; set; }
        //Complaint log
        public string _refnum { get; set; }
        public int? _Account { get; set; }
        public bool Damaged { get; set; }

        public bool Driver { get; set; }
        public bool WrongProductShipped { get; set; }

        public bool QualityOfProduct { get; set; }

        public bool WrongArea { get; set; }
        public bool Complain_MissingProduct { get; set; }

        public string TrackingNumber { get; set; }
        public string WorkOrder { get; set; }
        public string FedExTextArea { get; set; }

        public bool Incorrect { get; set; }
        public bool Mispick { get; set; }
        public bool Defective { get; set; }
        public string ProductTextArea { get; set; }

        public bool Impolite_Offensive { get; set; }
        public bool HoldTimes { get; set; }
        public string CustomerServiceTextArea { get; set; }

        public string Others { get; set; }

        public string ComplainOutCome { get; set; }

        public bool HandledConcerns { get; set; }
        public bool Resolved { get; set; }
        public bool NotResolved { get; set; }
        public bool TransferredTeamLeader { get; set; }


        //

        //added pradeep 11/30 
        //Insurance
        public string InsID { get; set; }

        //added end

        //added pradeep 01/30
        public string Assmnt_completedtxt { get; set; }
        public string Assmnt_reltomember { get; set; }

        public string Assmnt_edudoneby { get; set; }
        public bool? Assmnt_memtrained { get; set; }

        public string Assmnt_physicianforsupp { get; set; }
        public string Assmnt_currsuppfrom { get; set; }
        public string Assmnt_lastorderrecieveddt { get; set; }
        public string Assmnt_supporderfrom { get; set; }

        public int? Assmnt_Ord30or90 { get; set; }
        public string Assmnt_remsupplies { get; set; }

        //Diabetic
        public string Assmnt_neworexistdiab { get; set; }
        public int? Assmnt_Testingtimesdiab { get; set; }

        public bool? Assmnt_InsulinTreateddiab { get; set; }
        public string Assmnt_nameofinsulindiab { get; set; }
        public string Assmnt_nameofinsulindiab_Other { get; set; }
        public DateTime? Assmnt_pregduedatediab { get; set; }
        public bool? Assmnt_pregexistingdiab { get; set; }
        public string Assmnt_currmeterdiab { get; set; }
        public string Assmnt_meterservicediab { get; set; }
        public string Assmnt_diffmeterdiab { get; set; }
        public string Assmnt_talkingmeterdiab { get; set; }
        public string Assmnt_lancetsservicediab { get; set; }

        public bool? Assmnt_injsuppliesdiab { get; set; }

        public bool? Assmnt_injfromothersuppdiab { get; set; }
        public string Assmnt_injothersupptxtdiab { get; set; }
        public bool Assmnt_syrwtneedlediab { get; set; }

        public int? Assmnt_syrwtneedle_gaugediab { get; set; }

        public int? Assmnt_syrwtneedle_lendiab { get; set; }

        public int? Assmnt_syrwtneedle_unitsdiab { get; set; }

        public int? Assmnt_syrwtneedle_qtydiab { get; set; }

        public bool Assmnt_needleonly_diab { get; set; }
        public int? Assmnt_needleonly_gauge { get; set; }

        public int? Assmnt_needleonly_len { get; set; }

        public int? Assmnt_needleonly_qty { get; set; }

        public bool Assmnt_alcoholwipes { get; set; }

        public bool? assmnt_ketonediab { get; set; }

        public bool? Assmnt_alcoholwp_othersuppdiab { get; set; }

        public bool Assmnt_UrineKetonediab { get; set; }

        public int? Assmnt_UrineKetone_Freqtestingdiab { get; set; }

        public int? Assmnt_BloodKetone_Freqtestingdiab { get; set; }

        public bool Assmnt_BloodKetonediab { get; set; }

        //CGM / Insulin Pump
        public string Assmnt_diagnosedCGM { get; set; }

        public bool? Assmnt_InsTreatedCGM { get; set; }

        public string Assmnt_InsTreatedTypeCGM { get; set; }

        public bool? Assmnt_reqpumpCGM { get; set; }

        public bool? Assmnt_reqpumpsuppCGM { get; set; }

        public bool? Assmnt_pumpothersuppCGM { get; set; }

        public string Assmnt_pumpothersupptxtCGM { get; set; }

        public string Assmnt_neworreplacementCGM { get; set; }

        public string Assmnt_manufacturerCGM { get; set; }
        public string Assmnt_currpumpnmCGM { get; set; }
        public string Assmnt_serialnumCGM { get; set; }

        public DateTime? Assmnt_outofwarrantydtCGM { get; set; }

        public string Assmnt_InsurancepaidforCGM { get; set; }

        public string Assmnt_NewPumpNameCGM { get; set; }

        public string Assmnt_NewPumpColorCGM { get; set; }

        public string Assmnt_PumpReplacereasonCGM { get; set; }

        public string Assmnt_meterusedCGM { get; set; }

        public bool Assmnt_InfusionsetCGM { get; set; }

        public string Assmnt_InfusionsettxtCGM { get; set; }

        public bool Assmnt_catridgesCGM { get; set; }

        public string Assmnt_catridgestxtCGM { get; set; }

        public string Assmnt_catridgesoftnchngtxtCGM { get; set; }

        public bool Assmnt_BarrierWipesCGM { get; set; }

        public bool Assmnt_RemoverWipesCGM { get; set; }

        public bool Assmnt_AlcoholWipesCGM { get; set; }

        public bool Assmnt_TransparentdressingCGM { get; set; }

        public bool Assmnt_BatteriesCGM { get; set; }

        public bool? Assmnt_memownedorusedCGM { get; set; }

        public bool? Assmnt_memcurronCGM { get; set; }

         public DateTime? Assmnt_transmitter_dtreceivedCGM { get; set; }

      public DateTime? Assmnt_transmitter_outofwarrantydtCGM { get; set; }

        public string Assmnt_transmitter_serialnoCGM { get; set; }
        public string Assmnt_transmitter_ProductCode { get; set; }
        public DateTime? Assmnt_receiver_dtreceivedCGM { get; set; }

        public DateTime? Assmnt_receiver_outofwarrantydtCGM { get; set; }

        public string Assmnt_receiver_serialnoCGM { get; set; }
        public string Assmnt_receiver_ProductCode { get; set; }

        public bool? Assmnt_memneedtransmitterCGM { get; set; }

        public bool? Assmnt_memneedreceiverCGM { get; set; }

        public bool? Assmnt_memneedsensorsCGM { get; set; }

        public bool? Assmnt_othersupplierCGM { get; set; }

        public string Assmnt_othersuppliertxtCGM { get; set; }

        public string Assmnt_transmittertypeCGM { get; set; }

        public string Assmnt_receivertypeCGM { get; set; }

        public string Assmnt_transmitorreceiver_replacementreasonCGM { get; set; }

        public string Assmnt_sensorsforprodcodeCGM { get; set; }

        public bool? Assmnt_memawareofsignCGM { get; set; }


        //added pradeep end 01/30



    }
}