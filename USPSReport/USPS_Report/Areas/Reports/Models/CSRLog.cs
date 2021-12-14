using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Mvc;
using System.Text;

namespace USPS_Report.Areas.Reports.Models
{
    public class AddCSRLog
    {



        public static AccountInfoVM GetDetails(Int64? Account)
        {
            //  IList<AccountInfoVM> _vm = new List<AccountInfoVM>();
            AccountInfoVM tableRec = new AccountInfoVM();
            int? account;
            account = Convert.ToInt32(Account);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                //    tbl_Account_Member _list = new tbl_Account_Member();
                if (account != null && account != 0)
                {
                    // _list = _db.tbl_Account_Member.Where(t => t.Account == account && t.Member == 1).SingleOrDefault();
                    var _list = (from t in _db.tbl_Account_Member
                                 where t.Account == account && t.Member == 1
                                 select new tbl_Account_Member_VM
                                 {
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Address_1 = t.Address_1,
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
        public static CSRComplaintVM GetDetailsofAccountByRef(int? id)
        {
            //  IList<AccountInfoVM> _vm = new List<AccountInfoVM>();IssueDate
            CSRComplaintVM tableRec = new CSRComplaintVM();
            // int? account;
            //  account = Convert.ToInt32(Account);
            using (IntranetEntities _db = new IntranetEntities())
            {
                tbl_CSRCallLog _list = new tbl_CSRCallLog();
                tbl_CSRComplaintLog _list2 = new tbl_CSRComplaintLog();
                tbl_CSRComplaintLog_Attachments _list3 = new tbl_CSRComplaintLog_Attachments();
                _list = _db.tbl_CSRCallLog.Where(t => t.id == id).SingleOrDefault();
                _list2 = _db.tbl_CSRComplaintLog.Where(t => t.Account == _list.Account && t.Id_CSRCallLog==id).OrderByDescending(tt=>tt.id).FirstOrDefault();
                
                if (_list != null)
                {
                    tableRec.Account = _list.Account;
                    tableRec.TrackingNumber = _list.TrackingNumber;
                    tableRec.WorkOrder = _list.WorkOrder;
                    tableRec.Compliance = _list.Compliance;
                    tableRec.CustomerService = _list.CustomerService;
                    tableRec.Discrimination = _list.Discrimination;
                    tableRec.SmartAction = _list.SmartAction;
                    tableRec.WebsitePortal = _list.WebsitePortal;
                    tableRec.HealthPlan = _list.HealthPlan;
                    tableRec.ProductDefectiveQuality = _list.ProductDefectiveQuality;
                    tableRec.ShippingUSPS = _list.ShippingUSPS;
                    tableRec.ShippingWarehouse = _list.ShippingWarehouse;
                    tableRec.TextMessaging = _list.TextMessaging;
                    tableRec.ProcessDelay = _list.ProcessDelay;
                    tableRec.PhoneFaxIssues = _list.PhoneFaxIssues;

                    tableRec.BloodPressureMonitors = _list.BloodPressureMonitors;
                    tableRec.BreastPumps = _list.BreastPumps;
                    tableRec.ContGlucoseMonitoring = _list.ContGlucoseMonitoring;
                    tableRec.DiabeticTestSup = _list.DiabeticTestSup;
                    tableRec.EnteralNutrition = _list.EnteralNutrition;
                    tableRec.ExternalDefibrillator = _list.ExternalDefibrillator;
                    tableRec.IncontinenceSupplies = _list.IncontinenceSupplies;
                    tableRec.InsulinPumpsSupplies = _list.InsulinPumpsSupplies;
                    tableRec.InsSyrPenNeed = _list.InsSyrPenNeed;
                    tableRec.OstomySupplies = _list.OstomySupplies;
                    tableRec.PleurXDrainSys = _list.PleurXDrainSys;
                    tableRec.PTINRTesting = _list.PTINRTesting;
                    tableRec.TENSUnitSup = _list.TENSUnitSup;
                    tableRec.UrologicalSupplies = _list.UrologicalSupplies;
                    tableRec.WoundCareSupplies = _list.WoundCareSupplies;
                    tableRec.OtherUnsureSupplies = _list.OtherUnsureSupplies;

                    tableRec.BDI = _list.BDI;
                    tableRec.BPnBPM = _list.BPnBPM;
                    tableRec.CallCenter = _list.CallCenter;
                    tableRec.CSRAssessment = _list.CSRAssessment;
                    tableRec.DynamicSynergy = _list.DynamicSynergy;
                    tableRec.Enteral = _list.Enteral;
                    tableRec.HGS = _list.HGS;
                    tableRec.InsulinPumpCGM = _list.InsulinPumpCGM;
                    tableRec.MedicalDocuments = _list.MedicalDocuments;
                    tableRec.NewAccountTeam = _list.NewAccountTeam;
                    tableRec.Nurses = _list.Nurses;
                    tableRec.QualityAssurance = _list.QualityAssurance;
                    tableRec.Shipping = _list.Shipping;
                    tableRec.THC = _list.THC;
                    tableRec.Troy = _list.Troy;
                    tableRec.Verification = _list.Verification;
                    tableRec.WebSupport = _list.WebSupport;
                    tableRec.WoundCareOstomyTENS = _list.WoundCareOstomyTENS;
                    tableRec.OtherUnsureTeam = _list.OtherUnsureTeam;
                    tableRec.ITHelpDesk = _list.ITHelpDesk;

                    tableRec.Driver = _list.Driver;
                    tableRec.WrongProductShipped = _list.WrongProductShipped;
                    tableRec.WrongArea = _list.WrongArea;
                    tableRec.MissingProduct = _list.OtherIssue_MissingProduct;
                    tableRec.FedExTextArea = _list.FedExTxt;
                    tableRec.ProductIncorrect = _list.ProductIncrease; // chnage to product incorrect
                    tableRec.ProductMispick = _list.ProductMispick;
                    tableRec.ProductDefective = _list.ProductDefective;
                    tableRec.ProductTextArea = _list.ProductTxt;
                    tableRec.ImpoliteORoffensive = _list.ImpoliteORoffensive;
                    tableRec.HoldTimes = _list.HoldTimes;
                    tableRec.Others = _list.OtherTxt;
                    tableRec.CustomerServiceTextArea = _list.customerServiceTxt;
                    tableRec.QualityOfProduct = _list.QualityOfProdut;
                    tableRec.Product = _list.Product;
                    tableRec.DeliveryCompany = _list.DeliveryCompany;
                    //tableRec.BCNProvider = _list.BCNProvider;
                    tableRec.DidntFollowDelIns = _list.DidntFollowDelIns;
                    tableRec.VConfirmationCalls = _list.VConfirmationCalls;
                    tableRec.VPaymentCalles = _list.VPaymentCalles;
                    tableRec.SAJamesPhonePromts = _list.SAJamesPhonePromts;
                    tableRec.SAJamesSelfService = _list.SAJamesSelfService;
                    tableRec.VirtualCallBack = _list.VirtualCallBack;
                    //tableRec.Website = _list.Website;

                    tableRec.NoFollowUp = _list.NoFollowUp;
                    tableRec.ReturnedFromVM = _list.ReturnedFromVM;
                    tableRec.NoFollowUpWithMem = _list.NoFollowUpWithMem;
                    tableRec.NeverRecivedSupplies = _list.NeverRecivedSupplies;
                    tableRec.PhysicianIssue = _list.PhysicianIssue;
                    //tableRec.InsLimitGuidelines = _list.InsLimitGuidelines;
                    //tableRec.BCNProviderIssue = _list.BCNProviderIssue;
                    //tableRec.Other = _list.Other;
                    tableRec.ComplaintProduct = (_list.Damaged == true || _list.QualityOfProdut == true || _list.ProductDefective == true) ? true : false;
                    //tableRec.ComplaintShipping = (_list.OtherIssue_MissingProduct == true || _list.Driver == true || _list.DidntFollowDelIns == true || _list.WrongArea == true || _list.WrongProductShipped == true ) ? true : false;
                    //tableRec.ComplaintService = (_list.ImpoliteORoffensive == true || _list.VirtualCallBack == true || _list.HoldTimes == true || _list.NoFollowUp == true || _list.ReturnedFromVM == true || _list.NoFollowUpWithMem == true || _list.NeverRecivedSupplies == true || _list.PhysicianIssue == true) ? true : false;
                    //tableRec.ComplaintSmartAction = (_list.VConfirmationCalls == true || _list.VPaymentCalles == true ||  _list.SAJamesPhonePromts ==true || _list.SAJamesSelfService == true) ? true : false;


                }
                if (_list2 != null)
                {
                    _list3 = _db.tbl_CSRComplaintLog_Attachments.Where(t => t.Account == _list2.Account && t.ComplaintId == _list2.id).FirstOrDefault();
                    if (_list3 != null)
                    {
                        tableRec.AttachedFile=_list3.FileName;
                    }
                    tableRec.IssueDate = _list2.IssueDate;
                    tableRec.ComplaintDate = _list2.ComplaintDate;
                    tableRec.ResolutionDate = _list2.ResolutionDate;
                    tableRec.InitialRespDate = _list2.InitialRespDate;
                    tableRec.WrittenRespDate = _list2.WrittenRespDate;

                    tableRec.Call = _list2.Call ?? false;
                    tableRec.Email = _list2.Email ?? false;
                    tableRec.Fax = _list2.Fax ?? false;
                    tableRec.CallRcvdWebsite = _list2.CallRcvdWebsite ?? false;
                    tableRec.SocialMedia = _list2.SocialMedia ?? false;
                    tableRec.InsCompany = _list2.InsCompany ?? false;
                    tableRec.Survey = _list2.Survey ?? false;
                    tableRec.CallRcvdOther = _list2.CallRcvdOther ?? false;

                    tableRec.TrackingNumber = _list2.TrackingNumber;
                    tableRec.WorkOrder = _list2.WorkOrder;
                    tableRec.Product = _list2.Product;
                    tableRec.DeliveryCompany = _list2.DeliveryCompany;
                    

                    tableRec.BloodPressureMonitors = _list2.BloodPressureMonitors;
                    tableRec.BreastPumps = _list2.BreastPumps;
                    tableRec.ContGlucoseMonitoring = _list2.ContGlucoseMonitoring;
                    tableRec.DiabeticTestSup = _list2.DiabeticTestSup;
                    tableRec.EnteralNutrition = _list2.EnteralNutrition;
                    tableRec.ExternalDefibrillator = _list2.ExternalDefibrillator;
                    tableRec.IncontinenceSupplies = _list2.IncontinenceSupplies;
                    tableRec.InsulinPumpsSupplies = _list2.InsulinPumpsSupplies;
                    tableRec.InsSyrPenNeed = _list2.InsSyrPenNeed;
                    tableRec.OstomySupplies = _list2.OstomySupplies;
                    tableRec.PleurXDrainSys = _list2.PleurXDrainSys;
                    tableRec.PTINRTesting = _list2.PTINRTesting;
                    tableRec.TENSUnitSup = _list2.TENSUnitSup;
                    tableRec.UrologicalSupplies = _list2.UrologicalSupplies;
                    tableRec.WoundCareSupplies = _list2.WoundCareSupplies;
                    tableRec.OtherUnsureSupplies = _list2.OtherUnsureSupplies;

                    tableRec.BDI = _list2.BDI;
                    tableRec.BPnBPM = _list2.BPnBPM;
                    tableRec.CallCenter = _list2.CallCenter;
                    tableRec.CSRAssessment = _list2.CSRAssessment;
                    tableRec.DynamicSynergy = _list2.DynamicSynergy;
                    tableRec.Enteral = _list2.Enteral;
                    tableRec.HGS = _list2.HGS;
                    tableRec.InsulinPumpCGM = _list2.InsulinPumpCGM;
                    tableRec.MedicalDocuments = _list2.MedicalDocuments;
                    tableRec.NewAccountTeam = _list2.NewAccountTeam;
                    tableRec.Nurses = _list2.Nurses;
                    tableRec.QualityAssurance = _list2.QualityAssurance;
                    tableRec.Shipping = _list2.Shipping;
                    tableRec.THC = _list2.THC;
                    tableRec.Troy = _list2.Troy;
                    tableRec.Verification = _list2.Verification;
                    tableRec.WebSupport = _list2.WebSupport;
                    tableRec.WoundCareOstomyTENS = _list2.WoundCareOstomyTENS;
                    tableRec.OtherUnsureTeam = _list2.OtherUnsureTeam;
                    tableRec.ITHelpDesk = _list2.ITHelpDesk;

                    tableRec.Compliance = _list2.Compliance;
                    tableRec.CustomerService = _list2.CustomerService;
                    tableRec.Discrimination = _list2.Discrimination;
                    tableRec.SmartAction = _list2.SmartAction;
                    tableRec.WebsitePortal = _list2.WebsitePortal;
                    tableRec.HealthPlan = _list2.HealthPlan;
                    tableRec.ProductDefectiveQuality = _list2.ProductDefectiveQuality;
                    tableRec.ShippingUSPS = _list2.ShippingUSPS;
                    tableRec.ShippingWarehouse = _list2.ShippingWarehouse;
                    tableRec.TextMessaging = _list2.TextMessaging;
                    tableRec.ProcessDelay = _list2.ProcessDelay;
                    tableRec.PhoneFaxIssues = _list2.PhoneFaxIssues;

                    tableRec.Others = _list2.OtherTxt;

                    tableRec.PendingResolution = _list2.PendingResolution;

                    tableRec.Resolution = _list2.Resolution;

                    tableRec.ComplaintHasBeen = _list2.ComplaintHasBeen;


                }
            }
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                AccountInfoVM acc = new AccountInfoVM();
                tbl_Account_Member_VM _list = new tbl_Account_Member_VM();

                //  _list = _db.tbl_Account_Member.Where(t => t.Account == tableRec.Account && t.Member == 1).SingleOrDefault();

                _list = (from t in _db.tbl_Account_Member
                         where t.Account == tableRec.Account && t.Member == 1
                         select new tbl_Account_Member_VM
                         {
                             First_Name = t.First_Name,
                             Last_Name = t.Last_Name,
                             Address_1 = t.Address_1,
                             Address_2 = t.Address_2,
                             City = t.City,
                             State = t.State,
                             Zip = t.Zip,
                             Phone = t.Phone,
                             BirthDate = t.BirthDate,
                             EmailAddress = t.EmailAddress



                         }).SingleOrDefault();

                if (_list != null)
                {
                    acc.firstName = _list.First_Name + " " + _list.Last_Name;
                    //  tableRec.lastName = _list.Last_Name;
                    acc.address1 = _list.Address_1;
                    acc.address2 = _list.Address_2;
                    acc.city = _list.City;
                    acc.state = _list.State;
                    acc.zipcode = _list.Zip;
                    acc.phone = _list.Phone;
                    acc.DOB = _list.BirthDate;

                }
                else if (tableRec.Account == 0)
                {
                    acc.firstName = "New Memeber";
                }

                else

                { acc.firstName = "wrong"; }

                tableRec.details = acc;
            }

            return tableRec;
        }

        public static IList<HDMSPayer> HDMSPayerInfo(Int64? Account)
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    HDMSPayer pn = new HDMSPayer();
                    DateTime today = DateTime.Now;
                    //                var _list = (from ins in _db.tbl_Account_Insurance
                    //                             join pt in _db.tbl_Payer_Table
                    //                             on ins.ID_Payer equals pt.ID
                    //                             where ins.Account == Account &&
                    //((ins.Expiration_Date == null || ins.Expiration_Date > today)
                    //    && (ins.Effective_Date == null || ins.Effective_Date < today))
                    //                             where ins.CoverageMet != 100
                    //                             select new HDMSPayer
                    //                             {
                    //                                 payerid = ins.ID_Payer,
                    //                                 payerType = pt.Name,
                    //                                 //coverage = ins.CoverageMet,
                    //                                 // phoneno = pt.Phone_Number

                    //                             }).Distinct().OrderByDescending(t => t.payerType).ToList();
                    string Sql = @"Select ID_Payer as payerid,PayerName as payerType,1 as insOrd from v__AccountMemberEffectiveInsurance_Ins1 where Account = " + Account + " and  ((Expiration_Date is null or Expiration_Date > GETDATE())" +
                                               " and (Effective_Date is null or Effective_Date < GETDATE()))  union  Select ID_Payer as payerid,PayerName as payerType,2 as insOrd from v__AccountMemberEffectiveInsurance_Ins2 where Account = " + Account + " and  ((Expiration_Date is null or Expiration_Date > GETDATE())  and (Effective_Date is null or Effective_Date < GETDATE())) union Select ID_Payer as payerid,PayerName as payerType,3 as insOrd from v__AccountMemberEffectiveInsurance_Ins3 where Account = " + Account + " and  ((Expiration_Date is null or Expiration_Date > GETDATE()) and (Effective_Date is null or Effective_Date < GETDATE())) order by insOrd ";
                    var _list = _db.Database.SqlQuery<HDMSPayer>(Sql).ToList();

                    //  _list.Insert(0, pn);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<HDMSPayer>();
            }
        }
        //public static void AddCallLogComplaints(CSRLogComplaints _vm)
        //{
        //    try
        //    {
        //        using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //        {
        //            //tbl_Complaint _rec = new tbl_Complaint();

        //            //_rec.Account = _vm.Account;

        //            //_rec.CallDuration = _vm.CallDuration;
        //            //_rec.Complaints = _vm.Complaint;
        //            //_rec.C_Corrective = _vm.C_Corrective;
        //            //_rec.C_Description = _vm.C_Description;
        //            //_rec.C_Resolved = _vm.C_Resolved;
        //            //_rec.FedExComplaints = _vm.FedExComplaints;
        //            //_rec.FedExTrack = _vm.FedExTrack;
        //            //_rec.NewAccount = _vm.NewAccount;
        //            //_rec.WICSComplaints = _vm.WICSComplaints;
        //            //_rec.CreatedOn = DateTime.Now;
        //            //_rec.CreatedBy = Environment.UserName1;


        //            //_db.tbl_Complaint.Add(_rec);

        //            //try { } catch (Exception ex) { var msg = ex.Message; }
        //            //_db.SaveChanges();



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //    }

        //}


        //public static void AddResonforCallLog(CSRReasonforCall _vm)
        //{
        //    try
        //    {
        //        using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //        {
        //            //tbl_ReasonForCall _rec = new tbl_ReasonForCall();

        //            //_rec.Account = _vm.Account;

        //            //_rec.CallDuration = _vm.CallDuration;
        //            //_rec.CreatedOn = DateTime.Now;
        //            //_rec.CreatedBy = "admin";
        //            //_rec.Demographic = _vm.Demographic;
        //            //_rec.AStatus = _vm.AStatus;
        //            //_rec.NewAccount = _vm.NewAccount;
        //            //_rec.OrderConfirmation = _vm.OrderConfirmation;
        //            //_rec.OtherCallHandling = _vm.OtherCallHandling;
        //            //_rec.OtherCallHandlingInfo = _vm.OtherCallHandlingInfo;
        //            //_rec.PChange = _vm.PChange;
        //            //_rec.Restart = _vm.Restart;
        //            //_rec.SampleChoice = _vm.SampleChoice;
        //            //_rec.Samples = _vm.Samples;
        //            //_rec.ShippingRelatedIssues = _vm.ShippingRelatedIssues;
        //            //_rec.SState = _vm.SState;
        //            //_rec.TCallOther = _vm.TCallOther;
        //            //_rec.TranferredCall = _vm.TranferredCall;




        //            //_db.tbl_ReasonForCall.Add(_rec);

        //            //try { } catch (Exception ex) { var msg = ex.Message; }
        //            //_db.SaveChanges();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //    }



        //}

        public static int AddCallLog(CallLogVM _vm)
        {
            String _msg = String.Empty;
            int id = 0;
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {
                    // Guid fileid = Guid.NewGuid();
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();


                    tbl_CSRCallLog _rec = new tbl_CSRCallLog();
                    // _rec.RefNum = _vm.refnum;
                    _rec.Account = Convert.ToInt32(_vm.Account);
                    _rec.CreatedOn = DateTime.Now;
                    _rec.CreatedBy = userName;
                    //_rec.CivilRight = false;
                    _rec.CallDuration = _vm.TimerTxt;
                    _rec.Name = _vm.Name;
                    _rec.Relation = _vm.Relation;
                    _rec.OtherRelName = _vm.otherRelname;
                    _rec.BillingTxt = _vm.BillingTxt;
                    _rec.DocumentationTxt = _vm.DocumentationTxt;
                    _rec.TypeSupplies_1 = _vm.TypeSupplies1;
                    _rec.TypeSupplies_2 = _vm.TypeSupplies2;
                    _rec.TypeSuppliesOther = _vm.TypeSuppliesOther;
                    _rec.NewAccountTxt = _vm.NewAccountTxtArea;
                    _rec.InsuranceChangeTxt = _vm.InsuranceChangeTxt;
                    _rec.ShippingOthersName = _vm.ShippingOtherName;
                    _rec.OrderConfirmationTxt = _vm.OrderConfirmationTxt;
                    _rec.OrderStatusTxt = _vm.OrderStatusTxt;
                    _rec.RWOChangeTxt = _vm.ProductChangeTxt;
                    _rec.SampleTxt = _vm.SampleTxt;
                    _rec.ShippingIssueTxt = _vm.ShippingIssueTxt;
                    _rec.TransferredCall = _vm.TransferredCall;
                    _rec.TransferredCallTxt = _vm.TransferredCallTxtArea;
                    _rec.OtherHandlingTxt = _vm.OtherHandlingTxt;
                    _rec.VerifiedPHI = _vm.VerifiedPHI;

                    _rec.NamePHI = _vm.NamePHI;
                    _rec.DOBPHI = _vm.DOBPHI;
                    _rec.AddressPHI = _vm.AddressPHI;
                    _rec.PhoneNumberPHI = _vm.PhoneNumberPHI;
                    _rec.HealthIDPHI = _vm.HealthIDPHI;
                    _rec.DriverLicPHI = _vm.DriverLicPHI;
                    _rec.NotVerifyPHI = _vm.NotVerifyPHI;
                    _rec.OtherPHI = _vm.OtherPHI;
                    _rec.OtherTextPHI = _vm.OtherTextPHI;

                    _rec.VoiceForHealth = _vm.VoicesForHealth;
                    _rec.Copay = _vm.Copay;
                    _rec.Billing = _vm.Billing;
                    _rec.Address = _vm.Address;
                    _rec.Physician = _vm.Physician;
                    _rec.Phone = _vm.Phone;
                    _rec.AOB = _vm.AOB;
                    _rec.Prescription = _vm.Prescription;
                    _rec.CMN = _vm.CMN;
                    _rec.PriorAuthorization = _vm.PriorAuthorization;
                    _rec.SupportingDoc = _vm.SupportingDoc;
                    _rec.TeacherLetter = _vm.TeacherLetter;
                    _rec.Logs = _vm.Logs;
                    _rec.ABN = _vm.ABN;
                    _rec.InsuarnceChanges_ = _vm.InsuarnceChanges;
                    _rec.NewAcconunt = _vm.NewAccount;
                    _rec.Restart = _vm.Restart;
                    _rec.OrderConfirmation = _vm.OrderConfirmation;
                    _rec.FedExOrUSPSTracking = _vm.FedExOrUSPSTracking;
                    _rec.OrderShipped = _vm.OrderShipped;
                    _rec.OrderETA = _vm.OrderETA;
                    _rec.OrderHolding = _vm.OrderHolding;
                    _rec.RWOCreated = _vm.RWOCreated;
                    _rec.PC_IncreaseOrDecrease = _vm.PC_IncreaseOrDecrease;
                    _rec.PC_Hold = _vm.PC_Hold;
                    _rec.PC_RemoveOrAdd = _vm.PC_RemoveOrAdd;
                    _rec.ProductChange = _vm.ProductChange;
                    _rec.DefectiveProductOrNotUsable = _vm.DefectiveProductOrNotUsable;
                    _rec.WrongOrExtraProductShipped = _vm.WrongOrExtraProductShipped;
                    _rec.MissingProduct = _vm.MissingProduct;
                    _rec.ShippingOther = _vm.Sh_Other;
                    _rec.ReturnedCall_LeftVoicemail = _vm.ReturnedCall_LeftVoicemail;
                    _rec.WrongNumber = _vm.WrongNumber;
                    _rec.Nursing_CSRassessment = _vm.Nursing_CSRassessment;
                    _rec.DemographicChangesTxt = _vm.DemographicChanges;
                    _rec.DocumentationTxt = _vm.DocumentationTxt;
                    _rec.Eligibility = _vm.Eligibility;
                    _rec.ComplaintOutcome = _vm.ComplainOutCome;
                    _rec.AccountDeactivate = _vm.AccountDeactivated;
                    _rec.TransferredCallTxt = _vm.TransferredCallTxtArea;
                    _rec.TrackingNumber = _vm.TrackingNumber;
                    _rec.WorkOrder = _vm.WorkOrder;
                    //_rec.Damaged = _vm.Damaged;

                    _rec.BloodPressureMonitors = _vm.BloodPressureMonitors;
                    _rec.BreastPumps = _vm.BreastPumps;
                    _rec.ContGlucoseMonitoring = _vm.ContGlucoseMonitoring;
                    _rec.DiabeticTestSup = _vm.DiabeticTestSup;
                    _rec.EnteralNutrition = _vm.EnteralNutrition;
                    _rec.ExternalDefibrillator = _vm.ExternalDefibrillator;
                    _rec.IncontinenceSupplies = _vm.IncontinenceSupplies;
                    _rec.InsulinPumpsSupplies = _vm.InsulinPumpsSupplies;
                    _rec.InsSyrPenNeed = _vm.InsSyrPenNeed;
                    _rec.OstomySupplies = _vm.OstomySupplies;
                    _rec.PleurXDrainSys = _vm.PleurXDrainSys;
                    _rec.PTINRTesting = _vm.PTINRTesting;
                    _rec.TENSUnitSup = _vm.TENSUnitSup;
                    _rec.UrologicalSupplies = _vm.UrologicalSupplies;
                    _rec.WoundCareSupplies = _vm.WoundCareSupplies;
                    _rec.OtherUnsureSupplies = _vm.OtherUnsureSupplies;

                    _rec.BDI = _vm.BDI;
                    _rec.BPnBPM = _vm.BPnBPM;
                    _rec.CallCenter = _vm.CallCenter;
                    _rec.CSRAssessment = _vm.CSRAssessment;
                    _rec.DynamicSynergy = _vm.DynamicSynergy;
                    _rec.Enteral = _vm.Enteral;
                    _rec.HGS = _vm.HGS;
                    _rec.InsulinPumpCGM = _vm.InsulinPumpCGM;
                    _rec.MedicalDocuments = _vm.MedicalDocuments;
                    _rec.NewAccountTeam = _vm.NewAccountTeam;
                    _rec.Nurses = _vm.Nurses;
                    _rec.QualityAssurance = _vm.QualityAssurance;
                    _rec.Shipping = _vm.Shipping;
                    _rec.THC = _vm.THC;
                    _rec.Troy = _vm.Troy;
                    _rec.Verification = _vm.Verification;
                    _rec.WebSupport = _vm.WebSupport;
                    _rec.WoundCareOstomyTENS = _vm.WoundCareOstomyTENS;
                    _rec.OtherUnsureTeam = _vm.OtherUnsureTeam;
                    _rec.ITHelpDesk = _vm.ITHelpDesk;

                    _rec.Compliance = _vm.Compliance;
                    _rec.CustomerService = _vm.CustomerService;
                    _rec.Discrimination = _vm.Discrimination;
                    _rec.HealthPlan = _vm.HealthPlan;
                    _rec.ProductDefectiveQuality = _vm.ProductDefectiveQuality;
                    _rec.ShippingUSPS = _vm.ShippingUSPS;
                    _rec.ShippingWarehouse = _vm.ShippingWarehouse;
                    _rec.SmartAction = _vm.SmartAction;
                    _rec.TextMessaging = _vm.TextMessaging;
                    _rec.WebsitePortal = _vm.WebsitePortal;
                    _rec.ProcessDelay = _vm.ProcessDelay;
                    _rec.PhoneFaxIssues = _vm.PhoneFaxIssues;

                    _rec.ProductNeeds = _vm.ProductNeeds;
                    _rec.BPM_Needs = _vm.BPM_Needs;
                    _rec.BreastPumps_Needs = _vm.BreastPumps_Needs;
                    _rec.CGM_Needs = _vm.CGM_Needs;
                    _rec.Diabetes_Needs = _vm.Diabetes_Needs;
                    _rec.Nutrition_Needs = _vm.Nutrition_Needs;
                    _rec.Incontinence_Needs = _vm.Incontinence_Needs;
                    _rec.Pump_Needs = _vm.Pump_Needs;
                    _rec.Injectibles_Needs = _vm.Injectibles_Needs;
                    _rec.Ostomy_Needs = _vm.Ostomy_Needs;
                    _rec.Urological_Needs = _vm.Urological_Needs;
                    _rec.Wound_Needs = _vm.Wound_Needs;
                    _rec.Other_Needs = _vm.Other_Needs;
                    _rec.OthersText_Needs = _vm.OthersText_Needs;
                    _rec.Comment_Needs = _vm.Comment_Needs;

                    _rec.ProductSupplies = _vm.ProductSupplies;
                    _rec.BPM_Supplies = _vm.BPM_Supplies;
                    _rec.BreastPumps_Supplies = _vm.BreastPumps_Supplies;
                    _rec.CGM_Supplies = _vm.CGM_Supplies;
                    _rec.Diabetes_Supplies = _vm.Diabetes_Supplies;
                    _rec.Nutrition_Supplies = _vm.Nutrition_Supplies;
                    _rec.Incontinence_Supplies = _vm.Incontinence_Supplies;
                    _rec.Pump_Supplies = _vm.Pump_Supplies;
                    _rec.Injectibles_Supplies = _vm.Injectibles_Supplies;
                    _rec.Ostomy_Supplies = _vm.Ostomy_Supplies;
                    _rec.Urological_Supplies = _vm.Urological_Supplies;
                    _rec.Wound_Supplies = _vm.Wound_Supplies;
                    _rec.Other_Supplies = _vm.Other_Supplies;
                    _rec.OthersText_Supplies = _vm.OthersText_Supplies;

                    _rec.ProductServices = _vm.ProductServices;

                    //_rec.Driver = _vm.Driver;
                    //_rec.WrongProductShipped = _vm.WrongProductShipped;
                    //_rec.QualityOfProdut = _vm.QualityOfProduct;
                    //_rec.WrongArea = _vm.WrongArea;
                    //_rec.OtherIssue_MissingProduct = _vm.other_MissingProduct;
                    _rec.FedExTxt = _vm.FedExTextArea;
                    _rec.ProductIncrease = _vm.Incorrect; // change to product incorrect
                    _rec.ProductMispick = _vm.Mispick;
                    //_rec.ProductDefective = _vm.Defective;
                    _rec.ProductTxt = _vm.ProductTextArea;
                    //_rec.ImpoliteORoffensive = _vm.Impolite_Offensive;
                    //_rec.HoldTimes = _vm.HoldTimes;
                    _rec.customerServiceTxt = _vm.CustomerServiceTextArea;
                    _rec.OtherTxt = _vm.Others;
                    _rec.FedExOrUSPSTrackingNumber = _vm.FedExOrUSPSTrackingNumber;
                    _rec.SampleChoice = _vm.SampleChoice;
                    _rec.SampleTask = _vm.SampleTask;
                    _rec.Other_CallHandling = _vm.Other_CallHandling;
                    _rec.LMN = _vm.LMN;
                    _rec.Call = _vm.Call;
                    _rec.Task = _vm.Task;
                    _rec.Web = _vm.Web;
                    _rec.Survey = _vm.Survey;
                    _rec.Text = _vm.Text;
                    _rec.Portal = _vm.Portal;
                    _rec.SocialMedia = _vm.SocialMedia;
                    _rec.Email = _vm.Email;
                    _rec.Fax = _vm.Fax;
                    _rec.LogTypeOther = _vm.LogTypeOther;

                    //---------------------------new update---------------------------
                    _rec.Product = _vm.Product;
                    _rec.DeliveryCompany = _vm.DeliveryCompany;
                    //_rec.BCNProvider = _vm.BCNProvider;
                    //_rec.DidntFollowDelIns = _vm.DidntFollowDelIns;
                    //_rec.VConfirmationCalls = _vm.VConfirmationCalls;
                    //_rec.VPaymentCalles = _vm.VPaymentCalles;
                    //_rec.SAJamesPhonePromts = _vm.SAJamesPhonePromts;
                    //_rec.SAJamesSelfService = _vm.SAJamesSelfService;
                    //_rec.VirtualCallBack = _vm.VirtualCallBack;
                    //_rec.Website = _vm.Website;
                    //_rec.NoFollowUp = _vm.NoFollowUp;
                    //_rec.ReturnedFromVM = _vm.ReturnedFromVM;
                    //_rec.NoFollowUpWithMem = _vm.NoFollowUpWithMem;
                    //_rec.NeverRecivedSupplies = _vm.NeverRecivedSupplies;
                    //_rec.PhysicianIssue = _vm.PhysicianIssue;
                    //_rec.InsLimitGuidelines = _vm.InsLimitGuidelines;
                    //_rec.BCNProviderIssue = _vm.BCNProviderIssue;
                    //_rec.Other = _vm.Other;
                    //-----------------------------------------------------------------

                    _db.tbl_CSRCallLog.Add(_rec);


                    try { } catch (Exception ex) { var msg = ex.Message; }

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

        public static string AddNote_CallLog(CallLogVM _vm, int reference)
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
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

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

                    if (_vm.NamePHI == true)
                        noteString.Append("PHI Verified - Name " + Environment.NewLine);
                    if (_vm.DOBPHI == true)
                        noteString.Append("PHI Verified - Date of Birth " + Environment.NewLine);
                    if (_vm.AddressPHI == true)
                        noteString.Append("PHI Verified - Address " + Environment.NewLine);
                    if (_vm.PhoneNumberPHI == true)
                        noteString.Append("PHI Verified - Phone Number " + Environment.NewLine);
                    if (_vm.HealthIDPHI == true)
                        noteString.Append("PHI Verified - Health plan ID " + Environment.NewLine);
                    if (_vm.DriverLicPHI == true)
                        noteString.Append("PHI Verified - Driver’s License " + Environment.NewLine);
                    if (_vm.NotVerifyPHI == true)
                        noteString.Append("PHI Verified - Would not verify PHI " + Environment.NewLine);
                    if (_vm.OtherPHI == true)
                        noteString.Append("PHI Verified - Other " + Environment.NewLine);
                    if (_vm.OtherTextPHI != "" && _vm.OtherTextPHI != null)
                    {
                        otherStr.Append(" PHI Verified Other's Note =" + _vm.OtherTextPHI + " " + Environment.NewLine);
                    }

                    if (_vm.Call == true || _vm.Task == true || _vm.Fax == true || _vm.Web == true || _vm.Email == true || _vm.Survey == true || _vm.LogTypeOther == true || _vm.Text == true || _vm.Portal == true || _vm.SocialMedia == true)
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
                        if (_vm.Survey == true)
                            noteString.Append("LogType - Survey " + Environment.NewLine);
                        if (_vm.Text == true)
                            noteString.Append("LogType - Text " + Environment.NewLine);
                        if (_vm.Portal == true)
                            noteString.Append("LogType - Portal " + Environment.NewLine);
                        if (_vm.SocialMedia == true)
                            noteString.Append("LogType - Social Media " + Environment.NewLine);
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
                    if (_vm.Copay == true && (_vm.BillingTxt != "" && _vm.BillingTxt != null))
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
                        if (_note != null)
                        {

                            StringBuilder _msgStr1 = new StringBuilder();
                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            _msgStr1.Append(noteString);
                            _msgStr1.Append("Call Regarding = Copay" + Environment.NewLine + "Note = " + _vm.BillingTxt);

                            _tHist.NoteText = _msgStr1.ToString();

                            //_tHist.NoteText = noteString + "Call Regarding = Copay_" + "Note = " + _vm.BillingTxt;



                            _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //COMMUNICATOIN
                    if (_vm.ProductNeeds != null || _vm.ProductSupplies != null || _vm.ProductServices != null)
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

                            StringBuilder _sbmain = new StringBuilder();
                            _sbmain.Append(noteString);
                            if (_vm.ProductNeeds != null)
                            {
                                StringBuilder _sbNeeds = new StringBuilder();
                                _sbNeeds.Append("Are the products you are receiving meeting your needs? " + Environment.NewLine);
                                _sbNeeds.Append(_vm.ProductNeeds + Environment.NewLine);
                                if (_vm.ProductNeeds == "No")
                                {
                                    _sbNeeds.Append("Products not meeting needs: " + Environment.NewLine);

                                    if (_vm.BPM_Needs)
                                        _sbNeeds.Append("Blood Pressure Monitor " + Environment.NewLine);

                                    if (_vm.BreastPumps_Needs)
                                        _sbNeeds.Append("Breast Pump " + Environment.NewLine);

                                    if (_vm.CGM_Needs)
                                        _sbNeeds.Append("Continuous Glucose Monitor (CGM) " + Environment.NewLine);

                                    if (_vm.Diabetes_Needs)
                                        _sbNeeds.Append("Diabetes Testing " + Environment.NewLine);

                                    if (_vm.Nutrition_Needs)
                                        _sbNeeds.Append("Enteral Nutrition " + Environment.NewLine);

                                    if (_vm.Incontinence_Needs)
                                        _sbNeeds.Append("Incontinence " + Environment.NewLine);

                                    if (_vm.Pump_Needs)
                                        _sbNeeds.Append("Insulin Pump " + Environment.NewLine);

                                    if (_vm.Injectibles_Needs)
                                        _sbNeeds.Append("Insulin Injectibles " + Environment.NewLine);

                                    if (_vm.Ostomy_Needs)
                                        _sbNeeds.Append("Ostomy " + Environment.NewLine);

                                    if (_vm.Urological_Needs)
                                        _sbNeeds.Append("Urological " + Environment.NewLine);

                                    if (_vm.Wound_Needs)
                                        _sbNeeds.Append("Wound Care " + Environment.NewLine);

                                    if (_vm.Other_Needs)
                                        _sbNeeds.Append(_vm.OthersText_Needs + Environment.NewLine);

                                    _sbNeeds.Append("Explanation of how products do not meet members needs: " + Environment.NewLine);
                                    _sbNeeds.Append(_vm.Comment_Needs + Environment.NewLine);

                                }
                                _sbmain.Append(_sbNeeds + Environment.NewLine);
                            }

                            if (_vm.ProductSupplies != null)
                            {
                                StringBuilder _sbSupplies = new StringBuilder();
                                _sbSupplies.Append("Are there any supplies you are receiving too many of? " + Environment.NewLine);
                                _sbSupplies.Append(_vm.ProductSupplies + Environment.NewLine);
                                if (_vm.ProductSupplies == "Yes")
                                {
                                    _sbSupplies.Append("Products receiving too many of: " + Environment.NewLine);

                                    if (_vm.BPM_Supplies)
                                        _sbSupplies.Append("Blood Pressure Monitor " + Environment.NewLine);

                                    if (_vm.BreastPumps_Supplies)
                                        _sbSupplies.Append("Breast Pump " + Environment.NewLine);

                                    if (_vm.CGM_Supplies)
                                        _sbSupplies.Append("Continuous Glucose Monitor (CGM) " + Environment.NewLine);

                                    if (_vm.Diabetes_Supplies)
                                        _sbSupplies.Append("Diabetes Testing " + Environment.NewLine);

                                    if (_vm.Nutrition_Supplies)
                                        _sbSupplies.Append("Enteral Nutrition " + Environment.NewLine);

                                    if (_vm.Incontinence_Supplies)
                                        _sbSupplies.Append("Incontinence " + Environment.NewLine);

                                    if (_vm.Pump_Supplies)
                                        _sbSupplies.Append("Insulin Pump " + Environment.NewLine);

                                    if (_vm.Injectibles_Supplies)
                                        _sbSupplies.Append("Insulin Injectibles " + Environment.NewLine);

                                    if (_vm.Ostomy_Supplies)
                                        _sbSupplies.Append("Ostomy " + Environment.NewLine);

                                    if (_vm.Urological_Supplies)
                                        _sbSupplies.Append("Urological " + Environment.NewLine);

                                    if (_vm.Wound_Supplies)
                                        _sbSupplies.Append("Wound Care " + Environment.NewLine);

                                    if (_vm.Other_Supplies)
                                        _sbSupplies.Append(_vm.OthersText_Supplies + Environment.NewLine);

                                    _sbSupplies.Append("Agent was directed to follow Overstock Process " + Environment.NewLine);
                                }
                                _sbmain.Append(_sbSupplies + Environment.NewLine);
                            }

                            if (_vm.ProductServices != null)
                            {
                                StringBuilder _sbServices = new StringBuilder();
                                _sbServices.Append("J&B Pharmacy Services is now available for any of your medication or pharmacy needs.  You have freedom of choice for which pharmacy you would like to use.  Would you like to obtain any supplies from J&B Pharmacy Services? " + Environment.NewLine);
                                _sbServices.Append(_vm.ProductServices + Environment.NewLine);
                                if (_vm.ProductServices == "Yes")
                                {
                                    _sbServices.Append("Agent was directed to follow Pharmacy Referral Process " + Environment.NewLine);
                                }
                                _sbmain.Append(_sbServices + Environment.NewLine);
                            }

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;
                            _tHist.NoteText = _sbmain.ToString();
                            _tHist.ID_Operator = id;
                            _db.tbl_Account_Note_History.Add(_tHist);
                        }


                    }                    

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

                    //Other Issues- Communication
                    //_vm.Damaged == true
                    //_vm.Driver == true || 
                    //    _vm.WrongProductShipped == true || 
                    //    _vm.QualityOfProduct == true                            || 
                    //    _vm.WrongArea == true || _
                    //        vm.other_MissingProduct == true ||
                    //_vm.Defective ||
                    //|| _vm.Impolite_Offensive == true || 
                    //_vm.HoldTimes == true
                    //|| _vm.DidntFollowDelIns == true || 
                    //_vm.VConfirmationCalls == true || 
                    //_vm.VPaymentCalles == true ||
                    //_vm.SAJamesPhonePromts == true
                    //|| _vm.SAJamesSelfService == true || 
                    //_vm.VirtualCallBack == true || 
                    //_vm.Website == true || 
                    //_vm.NoFollowUp == true || 
                    //_vm.ReturnedFromVM == true || 
                    //_vm.NoFollowUpWithMem == true ||
                    //_vm.NeverRecivedSupplies == true || 
                    //    _vm.PhysicianIssue == true || 
                    //    _vm.InsLimitGuidelines == true || 
                    //    _vm.BCNProviderIssue == true || 
                    //    _vm.Other == true ||
                    //|| (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                    // COMMMUNICATION
                    if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                        || _vm.BloodPressureMonitors == true
                        || _vm.BreastPumps == true
                        || _vm.ContGlucoseMonitoring == true
                        || _vm.DiabeticTestSup == true
                        || _vm.EnteralNutrition == true
                        || _vm.ExternalDefibrillator == true
                        || _vm.IncontinenceSupplies == true
                        || _vm.InsulinPumpsSupplies == true
                        || _vm.InsSyrPenNeed == true
                        || _vm.OstomySupplies == true
                        || _vm.PleurXDrainSys == true
                        || _vm.PTINRTesting == true
                        || _vm.TENSUnitSup == true
                        || _vm.UrologicalSupplies == true
                        || _vm.WoundCareSupplies == true
                        || _vm.OtherUnsureSupplies == true

                        || _vm.BDI == true
                        || _vm.BPnBPM == true
                        || _vm.CallCenter == true
                        || _vm.CSRAssessment == true
                        || _vm.DynamicSynergy == true
                        || _vm.Enteral == true
                        || _vm.HGS == true
                        || _vm.InsulinPumpCGM == true
                        || _vm.MedicalDocuments == true
                        || _vm.NewAccountTeam == true
                        || _vm.Nurses == true
                        || _vm.QualityAssurance == true
                        || _vm.Shipping == true
                        || _vm.THC == true
                        || _vm.Troy == true
                        || _vm.Verification == true
                        || _vm.WebSupport == true
                        || _vm.WoundCareOstomyTENS == true
                        || _vm.OtherUnsureTeam == true
                        || _vm.ITHelpDesk == true

                            || _vm.Compliance == true || _vm.CustomerService == true
                            || _vm.Discrimination == true || _vm.HealthPlan == true
                            || _vm.ProductDefectiveQuality == true || _vm.ShippingUSPS == true
                            || _vm.ShippingWarehouse == true || _vm.SmartAction == true
                            || _vm.TextMessaging == true || _vm.WebsitePortal == true || _vm.ProcessDelay == true || _vm.PhoneFaxIssues == true
                            || (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                            || _vm.Incorrect == true || _vm.Mispick == true || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                            || (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null)
                            || (_vm.Others != "" && _vm.Others != null)
                            || (_vm.Product != "" && _vm.Product != null) || (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null))
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



                            //_vm.Damaged == true
                            //|| _vm.Driver == true || _vm.WrongProductShipped == true || _vm.QualityOfProduct == true
                            //|| _vm.WrongArea == true || _vm.CivilRights == true || _vm.other_MissingProduct == true || _vm.DidntFollowDelIns == true || _vm.VConfirmationCalls == true || _vm.VPaymentCalles == true || _vm.SAJamesPhonePromts == true
                            //|| _vm.SAJamesSelfService == true || _vm.VirtualCallBack == true || _vm.Website == true || _vm.NoFollowUp == true || _vm.ReturnedFromVM == true || _vm.NoFollowUpWithMem == true ||
                            //_vm.NeverRecivedSupplies == true || _vm.PhysicianIssue == true || _vm.InsLimitGuidelines == true || _vm.BCNProviderIssue == true || _vm.Other == true ||
                            //|| (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                            if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                || _vm.BloodPressureMonitors == true
                                || _vm.BreastPumps == true
                                || _vm.ContGlucoseMonitoring == true
                                || _vm.DiabeticTestSup == true
                                || _vm.EnteralNutrition == true
                                || _vm.ExternalDefibrillator == true
                                || _vm.IncontinenceSupplies == true
                                || _vm.InsulinPumpsSupplies == true
                                || _vm.InsSyrPenNeed == true
                                || _vm.OstomySupplies == true
                                || _vm.PleurXDrainSys == true
                                || _vm.PTINRTesting == true
                                || _vm.TENSUnitSup == true
                                || _vm.UrologicalSupplies == true
                                || _vm.WoundCareSupplies == true
                                || _vm.OtherUnsureSupplies == true

                                || _vm.BDI == true
                                || _vm.BPnBPM == true
                                || _vm.CallCenter == true
                                || _vm.CSRAssessment == true
                                || _vm.DynamicSynergy == true
                                || _vm.Enteral == true
                                || _vm.HGS == true
                                || _vm.InsulinPumpCGM == true
                                || _vm.MedicalDocuments == true
                                || _vm.NewAccountTeam == true
                                || _vm.Nurses == true
                                || _vm.QualityAssurance == true
                                || _vm.Shipping == true
                                || _vm.THC == true
                                || _vm.Troy == true
                                || _vm.Verification == true
                                || _vm.WebSupport == true
                                || _vm.WoundCareOstomyTENS == true
                                || _vm.OtherUnsureTeam == true
                                || _vm.ITHelpDesk == true

                                || _vm.Compliance == true || _vm.CustomerService == true
                                || _vm.Discrimination == true || _vm.HealthPlan == true
                                || _vm.ProductDefectiveQuality == true || _vm.ShippingUSPS == true
                                || _vm.ShippingWarehouse == true || _vm.SmartAction == true
                                || _vm.TextMessaging == true || _vm.WebsitePortal == true || _vm.ProcessDelay == true || _vm.PhoneFaxIssues == true
                                || (_vm.FedExTextArea != "" && _vm.FedExTextArea != null) || (_vm.Product != "" && _vm.Product != null) || (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null))
                            {
                                if (_vm.TrackingNumber != "" && _vm.TrackingNumber != null)
                                    otherStr.Append("Tracking Number = " + _vm.TrackingNumber + Environment.NewLine);
                                //  otherStr = otherStr + " Tracking Number = " + _vm.TrackingNumber + "_";

                                if (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                    otherStr.Append("WorkOrder = " + _vm.WorkOrder + Environment.NewLine);
                                // otherStr = otherStr + " WorkOrder = " + _vm.WorkOrder + "_";


                                if (_vm.Product != "" && _vm.Product != null)
                                    otherStr.Append("Product = " + _vm.Product + Environment.NewLine);

                                if (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null)
                                    otherStr.Append("Delivery Company = " + _vm.DeliveryCompany + Environment.NewLine);

                                //if (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                                //    otherStr.Append("BCN Provider = " + _vm.BCNProvider + Environment.NewLine);

                                //if (_vm.Damaged == true)
                                //        otherStr.Append("Damaged"+ Environment.NewLine);
                                //otherStr = otherStr + "Damaged_";

                                if (_vm.BloodPressureMonitors == true)
                                    otherStr.Append("Blood Pressure Monitors" + Environment.NewLine);

                                if (_vm.BreastPumps == true)
                                    otherStr.Append("Breast Pumps" + Environment.NewLine);

                                if (_vm.ContGlucoseMonitoring == true)
                                    otherStr.Append("Continuous Glucose Monitoring" + Environment.NewLine);

                                if (_vm.DiabeticTestSup == true)
                                    otherStr.Append("Diabetic Testing Supplies" + Environment.NewLine);

                                if (_vm.EnteralNutrition == true)
                                    otherStr.Append("Enteral Nutrition" + Environment.NewLine);

                                if (_vm.ExternalDefibrillator == true)
                                    otherStr.Append("External Defibrillator" + Environment.NewLine);

                                if (_vm.IncontinenceSupplies == true)
                                    otherStr.Append("Incontinence Supplies" + Environment.NewLine);

                                if (_vm.InsulinPumpsSupplies == true)
                                    otherStr.Append("Insulin Pumps & Supplies" + Environment.NewLine);

                                if (_vm.InsSyrPenNeed == true)
                                    otherStr.Append("Insulin Syringes & Pen Needles" + Environment.NewLine);

                                if (_vm.OstomySupplies == true)
                                    otherStr.Append("Ostomy Supplies" + Environment.NewLine);

                                if (_vm.PleurXDrainSys == true)
                                    otherStr.Append("PleurX Drainage System" + Environment.NewLine);

                                if (_vm.PTINRTesting == true)
                                    otherStr.Append("PT INR Testing" + Environment.NewLine);

                                if (_vm.TENSUnitSup == true)
                                    otherStr.Append("TENS Unit & Supplies" + Environment.NewLine);

                                if (_vm.UrologicalSupplies == true)
                                    otherStr.Append("Urological Supplies" + Environment.NewLine);

                                if (_vm.WoundCareSupplies == true)
                                    otherStr.Append("Wound Care Supplies" + Environment.NewLine);

                                if (_vm.OtherUnsureSupplies == true)
                                    otherStr.Append("Other/Unsure of Supplies" + Environment.NewLine);

                                if (_vm.BDI == true)
                                    otherStr.Append("BDI" + Environment.NewLine);

                                if (_vm.BPnBPM == true)
                                    otherStr.Append("Breast Pumps & Blood Pressure Monitors" + Environment.NewLine);

                                if (_vm.CallCenter == true)
                                    otherStr.Append("Call Center" + Environment.NewLine);

                                if (_vm.CSRAssessment == true)
                                    otherStr.Append("CSR Assessment" + Environment.NewLine);

                                if (_vm.DynamicSynergy == true)
                                    otherStr.Append("Dynamic Synergy" + Environment.NewLine);

                                if (_vm.Enteral == true)
                                    otherStr.Append("Enteral" + Environment.NewLine);

                                if (_vm.HGS == true)
                                    otherStr.Append("HGS" + Environment.NewLine);

                                if (_vm.InsulinPumpCGM == true)
                                    otherStr.Append("Insulin Pump & CGM" + Environment.NewLine);

                                if (_vm.MedicalDocuments == true)
                                    otherStr.Append("Medical Documents" + Environment.NewLine);

                                if (_vm.NewAccountTeam == true)
                                    otherStr.Append("New Account" + Environment.NewLine);

                                if (_vm.Nurses == true)
                                    otherStr.Append("Nurses" + Environment.NewLine);

                                if (_vm.QualityAssurance == true)
                                    otherStr.Append("Quality Assurance" + Environment.NewLine);

                                if (_vm.Shipping == true)
                                    otherStr.Append("Shipping" + Environment.NewLine);

                                if (_vm.THC == true)
                                    otherStr.Append("THC" + Environment.NewLine);

                                if (_vm.Troy == true)
                                    otherStr.Append("Troy (BCN, BCNA, MAPPO)" + Environment.NewLine);

                                if (_vm.Verification == true)
                                    otherStr.Append("Verification" + Environment.NewLine);

                                if (_vm.WebSupport == true)
                                    otherStr.Append("Web Support" + Environment.NewLine);

                                if (_vm.WoundCareOstomyTENS == true)
                                    otherStr.Append("Wound Care, Ostomy & TENS" + Environment.NewLine);

                                if (_vm.OtherUnsureTeam == true)
                                    otherStr.Append("Other/Unsure of Team" + Environment.NewLine);

                                if (_vm.ITHelpDesk == true)
                                    otherStr.Append("IT / HelpDesk" + Environment.NewLine);

                                if (_vm.Compliance == true)
                                    otherStr.Append("Compliance" + Environment.NewLine);

                                if (_vm.CustomerService == true)
                                    otherStr.Append("Customer Service (CSR Issue, Hold Times,etc)" + Environment.NewLine);

                                if (_vm.Discrimination == true)
                                    otherStr.Append("Discrimination / Civil Rights" + Environment.NewLine);

                                if (_vm.HealthPlan == true)
                                    otherStr.Append("Health Plan (insurance limits, guidelines, etc)" + Environment.NewLine);

                                if (_vm.ProductDefectiveQuality == true)
                                    otherStr.Append("Product (Defective, Quality, etc)" + Environment.NewLine);

                                if (_vm.ShippingUSPS == true)
                                    otherStr.Append("Shipping (USPS / FedEx)" + Environment.NewLine);

                                if (_vm.ShippingWarehouse == true)
                                    otherStr.Append("Shipping (Warehouse: Mispicks, Missing, etc)" + Environment.NewLine);

                                if (_vm.SmartAction == true)
                                    otherStr.Append("SmartAction (Victor / Julie)" + Environment.NewLine);

                                if (_vm.TextMessaging == true)
                                    otherStr.Append("Text Messaging" + Environment.NewLine);

                                if (_vm.WebsitePortal == true)
                                    otherStr.Append("Website / Portal" + Environment.NewLine);

                                if (_vm.ProcessDelay == true)
                                    otherStr.Append("Process Delay (Internal delays regarding follow up, processing, submission, etc)" + Environment.NewLine);

                                if (_vm.PhoneFaxIssues == true)
                                    otherStr.Append("Phone / Fax Issues" + Environment.NewLine);

                                //if (_vm.Driver == true)
                                //       otherStr.Append("Driver" + Environment.NewLine);
                                // otherStr = otherStr + "Driver_";



                                //if (_vm.DidntFollowDelIns == true)
                                //    otherStr.Append("Did not follow delivery instructions" + Environment.NewLine);

                                //if (_vm.VConfirmationCalls == true)
                                //    otherStr.Append("Victor Confirmation Calls " + Environment.NewLine);

                                //if (_vm.VPaymentCalles == true)
                                //    otherStr.Append("Victor Payment Calls " + Environment.NewLine);

                                //if (_vm.SAJamesPhonePromts == true)
                                //    otherStr.Append("Smart Action/James Phone Prompts " + Environment.NewLine);

                                //if (_vm.SAJamesSelfService == true)
                                //    otherStr.Append("Smart Action/James self service " + Environment.NewLine);

                                //if (_vm.VirtualCallBack == true)
                                //    otherStr.Append("Virtual Callback feature " + Environment.NewLine);

                                //if (_vm.NoFollowUp == true)
                                //    otherStr.Append("No follow up on account/on documentation" + Environment.NewLine);

                                //if (_vm.ReturnedFromVM == true)
                                //    otherStr.Append("Call not returned from VM" + Environment.NewLine);

                                //if (_vm.WrongProductShipped == true)
                                //             otherStr.Append("Wrong Product Shipped" + Environment.NewLine);
                                //otherStr = otherStr + "WrongProductShipped_";

                                //if (_vm.NoFollowUpWithMem == true)
                                //    otherStr.Append("No follow up with member " + Environment.NewLine);

                                //if (_vm.NeverRecivedSupplies == true)
                                //    otherStr.Append("Never received supplies" + Environment.NewLine);

                                //if (_vm.PhysicianIssue == true)
                                //    otherStr.Append("Physician Issue" + Environment.NewLine);

                                //if (_vm.InsLimitGuidelines == true)
                                //    otherStr.Append("Insurance Limitations/Guidelines" + Environment.NewLine);

                                //if (_vm.BCNProviderIssue == true)
                                //    otherStr.Append("BCN Provider Issue " + Environment.NewLine);

                                //if (_vm.QualityOfProduct == true)
                                //             otherStr.Append("Quality Of Product" + Environment.NewLine);

                                //if (_vm.CivilRights == true)
                                //    otherStr.Append("Civil Rights" + Environment.NewLine);


                                //if (_vm.Other == true)
                                //    otherStr.Append("Other" + Environment.NewLine);


                                //if (_vm.Website == true)
                                //    otherStr.Append("Website" + Environment.NewLine);

                                //if (_vm.WrongArea == true)
                                //             otherStr.Append("Wrong Area" + Environment.NewLine);
                                //          //otherStr = otherStr + "WrongArea_";

                                //    if (_vm.other_MissingProduct == true)
                                //         otherStr.Append("Missing Product" + Environment.NewLine);
                                //        // otherStr = otherStr + "MissingProduct_";

                                if (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                                    otherStr.Append(" Note =" + _vm.FedExTextArea + Environment.NewLine);
                                // otherStr = otherStr + " FedExNote =" + _vm.FedExTextArea + "_";

                            }

                            //_vm.Defective ||

                            if (_vm.Incorrect == true || _vm.Mispick == true || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null))
                            {

                                if (_vm.Incorrect == true)
                                    otherStr.Append("Incorrect Product" + Environment.NewLine);
                                //otherStr = otherStr + "Incorrect_";

                                if (_vm.Mispick == true)
                                    otherStr.Append("Mispick Product" + Environment.NewLine);
                                //otherStr = otherStr + "Mispick_";

                                //if (_vm.Defective == true)
                                //    otherStr.Append("Defective Product" + Environment.NewLine);
                                //     //otherStr = otherStr + "Defective_";



                                if (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                                    otherStr.Append("Product Note =" + _vm.ProductTextArea + Environment.NewLine);
                                //otherStr = otherStr + " Product Note =" + _vm.ProductTextArea + "_";

                            }
                            //_vm.Impolite_Offensive == true || _vm.HoldTimes == true ||
                            if ((_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null))
                            {

                                //if (_vm.Impolite_Offensive == true)
                                //        otherStr.Append("Impolite or Offensive" + Environment.NewLine);
                                //    //otherStr = otherStr + "Impolite/Offensive_";

                                //if (_vm.HoldTimes == true)
                                //    otherStr.Append("Hold Times" + Environment.NewLine);
                                //        // otherStr = otherStr + "HoldTimes_";


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
                            _msgStr4.Append("Reference Number = " + reference + Environment.NewLine);

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
                    if (_vm.AOB == true && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
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
                            _msgStr6.Append("Call Regarding = AOB" + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr6.ToString();

                            //  _tHist.NoteText = noteString + "Call Regarding = AOB_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }


                    //PRESCRIPTI0N 
                    if (_vm.Prescription == true && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
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
                            _msgStr7.Append("Call Regarding =  Prescription" + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr7.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding = Prescription_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }

                    }

                    //CMN 
                    if (_vm.CMN == true && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
                    {

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CMN").FirstOrDefault();  //&& t.NoteCreatedBy == id

                        if (_note == null)
                        {
                            tbl_Account_Note _tbl = new tbl_Account_Note();
                            _tbl.Account = Convert.ToInt32(_vm.Account);
                            _tbl.Member = 1;
                            _tbl.NoteHeading = "CMN";
                            _tbl.NoteCreateDate = DateTime.Now;
                            _tbl.NoteCreatedBy = id;
                            _tbl.SystemRecordType = 100;
                            _tbl.ID_NoteLibrary = 6;
                            _db.tbl_Account_Note.Add(_tbl);
                            _db.SaveChanges();


                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CMN").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            StringBuilder _msgStr8 = new StringBuilder();
                            _msgStr8.Append(noteString);
                            _msgStr8.Append("Call Regarding = CMN" + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr8.ToString();

                            // _tHist.NoteText = noteString + "Call Regarding =  CMN_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //PRIOR AUTH 
                    if (_vm.PriorAuthorization == true && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
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
                            _msgStr9.Append("Call Regarding = PriorAuthorization" + Environment.NewLine + "Note = " + _vm.DocumentationTxt);

                            _tHist.NoteText = _msgStr9.ToString();


                            //_tHist.NoteText = noteString + "Call Regarding =  PriorAuthorization_" + "Note = " + _vm.DocumentationTxt;
                            _tHist.ID_Operator = id;

                            _db.tbl_Account_Note_History.Add(_tHist);

                        }
                    }

                    //OVER QUANTITY DOCUMENTATION 
                    if ((_vm.SupportingDoc == true || _vm.LMN == true) && (_vm.DocumentationTxt != "" && _vm.DocumentationTxt != null))
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
                            if (_vm.LMN == true)
                                _msgStr10.Append("LMN" + Environment.NewLine);


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
                    if (_vm.InsuarnceChanges == true && (_vm.InsuranceChangeTxt != "" && _vm.InsuranceChangeTxt != null))
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
                            _msgStr12.Append("Call Regarding = Insuarnce Changes" + Environment.NewLine + "Note = " + _vm.InsuranceChangeTxt);

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
                    if ((_vm.FedExOrUSPSTracking == true || _vm.OrderShipped == true || _vm.OrderETA == true || _vm.RWOCreated == true) && (_vm.OrderStatusTxt != "" && _vm.OrderStatusTxt != null))
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
                    if (_vm.SampleChoice == true && (_vm.SampleTxt != "" && _vm.SampleTxt != null))
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


                    if ((_vm.ReturnedCall_LeftVoicemail == true || _vm.WrongNumber == true || _vm.Nursing_CSRassessment == true || _vm.Other_CallHandling == true) && (_vm.OtherHandlingTxt != "" && _vm.OtherHandlingTxt != null))
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


                            if (_vm.Nursing_CSRassessment)
                                _callStr.Append("Nursing / CSRassessment" + Environment.NewLine);


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

        public static int AddComplaintLog(CSRComplaintVM _vm)
        {
            int id = 0;
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    tbl_CSRComplaintLog _rec = new tbl_CSRComplaintLog();
                    _rec.Id_CSRCallLog = _vm.id;
                    //  _rec.RefNum = _vm.refnum;
                    _rec.Account = _vm.Account;
                    _rec.CreatedOn = DateTime.Now;
                    _rec.CreatedBy = userName;
                    _rec.TrackingNumber = _vm.TrackingNumber;
                    _rec.WorkOrder = _vm.WorkOrder;
                    //_rec.Damaged = _vm.Damaged;

                    _rec.BloodPressureMonitors = _vm.BloodPressureMonitors;
                    _rec.BreastPumps = _vm.BreastPumps;
                    _rec.ContGlucoseMonitoring = _vm.ContGlucoseMonitoring;
                    _rec.DiabeticTestSup = _vm.DiabeticTestSup;
                    _rec.EnteralNutrition = _vm.EnteralNutrition;
                    _rec.ExternalDefibrillator = _vm.ExternalDefibrillator;
                    _rec.IncontinenceSupplies = _vm.IncontinenceSupplies;
                    _rec.InsulinPumpsSupplies = _vm.InsulinPumpsSupplies;
                    _rec.InsSyrPenNeed = _vm.InsSyrPenNeed;
                    _rec.OstomySupplies = _vm.OstomySupplies;
                    _rec.PleurXDrainSys = _vm.PleurXDrainSys;
                    _rec.PTINRTesting = _vm.PTINRTesting;
                    _rec.TENSUnitSup = _vm.TENSUnitSup;
                    _rec.UrologicalSupplies = _vm.UrologicalSupplies;
                    _rec.WoundCareSupplies = _vm.WoundCareSupplies;
                    _rec.OtherUnsureSupplies = _vm.OtherUnsureSupplies;

                    _rec.BDI = _vm.BDI;
                    _rec.BPnBPM = _vm.BPnBPM;
                    _rec.CallCenter = _vm.CallCenter;
                    _rec.CSRAssessment = _vm.CSRAssessment;
                    _rec.DynamicSynergy = _vm.DynamicSynergy;
                    _rec.Enteral = _vm.Enteral;
                    _rec.HGS = _vm.HGS;
                    _rec.InsulinPumpCGM = _vm.InsulinPumpCGM;
                    _rec.MedicalDocuments = _vm.MedicalDocuments;
                    _rec.NewAccountTeam = _vm.NewAccountTeam;
                    _rec.Nurses = _vm.Nurses;
                    _rec.QualityAssurance = _vm.QualityAssurance;
                    _rec.Shipping = _vm.Shipping;
                    _rec.THC = _vm.THC;
                    _rec.Troy = _vm.Troy;
                    _rec.Verification = _vm.Verification;
                    _rec.WebSupport = _vm.WebSupport;
                    _rec.WoundCareOstomyTENS = _vm.WoundCareOstomyTENS;
                    _rec.OtherUnsureTeam = _vm.OtherUnsureTeam;
                    _rec.ITHelpDesk = _vm.ITHelpDesk;

                    _rec.Compliance = _vm.Compliance;
                    _rec.CustomerService = _vm.CustomerService;
                    _rec.Discrimination = _vm.Discrimination;
                    _rec.SmartAction = _vm.SmartAction;
                    _rec.WebsitePortal = _vm.WebsitePortal;
                    _rec.HealthPlan = _vm.HealthPlan;
                    _rec.ProductDefectiveQuality = _vm.ProductDefectiveQuality;
                    _rec.ShippingUSPS = _vm.ShippingUSPS;
                    _rec.ShippingWarehouse = _vm.ShippingWarehouse;
                    _rec.TextMessaging = _vm.TextMessaging;
                    _rec.ProcessDelay = _vm.ProcessDelay;
                    _rec.PhoneFaxIssues = _vm.PhoneFaxIssues;

                    _rec.Driver = _vm.Driver;
                    _rec.WrongProductShipped = _vm.WrongProductShipped;
                    _rec.WrongArea = _vm.WrongArea;
                    _rec.QualityOfProdut = _vm.QualityOfProduct;
                    _rec.MissingProduct = _vm.MissingProduct;
                    _rec.FedExTxt = _vm.FedExTextArea;
                    _rec.ProductDefective = _vm.ProductDefective;
                    _rec.ProductIncrease = _vm.ProductIncorrect;
                    _rec.ProductMispick = _vm.ProductMispick;
                    _rec.ProductTxt = _vm.ProductTextArea;
                    _rec.ImpoliteORoffensive = _vm.ImpoliteORoffensive;
                    _rec.HoldTimes = _vm.HoldTimes;
                    _rec.customerServiceTxt = _vm.CustomerServiceTextArea;
                    _rec.OtherTxt = _vm.Others;
                    _rec.ComplaintHasBeen = _vm.ComplaintHasBeen;
                    _rec.PendingResolution = _vm.PendingResolution;
                    _rec.Resolution = _vm.Resolution;
                    string payerids = "";
                    for (int i = 0; i < _vm.payerTypeList.Count(); i++)
                    {
                        payerids += _vm.payerTypeList[i].payerid.ToString();
                        if (i != _vm.payerTypeList.Count() - 1)
                        {
                            payerids += ", ";
                        }
                    }
                    _rec.PayerType = payerids;
                    // _rec.PayerType = _vm.payerid.ToString();
                    // _rec.HandledConcern = _vm.HandledConcerns;
                    // _rec.Resolved = _vm.Resolved;
                    // _rec.NotResolved = _vm.NotResolved;
                    // _rec.TransferredtoTeamLeader = _vm.TransferredTeamLeader;

                    _rec.Product = _vm.Product;
                    _rec.DeliveryCompany = _vm.DeliveryCompany;
                    //_rec.BCNProvider = _vm.BCNProvider;
                    _rec.DidntFollowDelIns = _vm.DidntFollowDelIns;
                    _rec.VConfirmationCalls = _vm.VConfirmationCalls;
                    _rec.VPaymentCalles = _vm.VPaymentCalles;
                    _rec.SAJamesPhonePromts = _vm.SAJamesPhonePromts;
                    _rec.SAJamesSelfService = _vm.SAJamesSelfService;
                    _rec.VirtualCallBack = _vm.VirtualCallBack;
                    //_rec.Website = _vm.Website;
                    _rec.NoFollowUp = _vm.NoFollowUp;
                    _rec.ReturnedFromVM = _vm.ReturnedFromVM;
                    _rec.NoFollowUpWithMem = _vm.NoFollowUpWithMem;
                    _rec.NoFollowUp = _vm.NoFollowUp;
                    _rec.NeverRecivedSupplies = _vm.NeverRecivedSupplies;
                    _rec.PhysicianIssue = _vm.PhysicianIssue;
                    //_rec.BCNProviderIssue = _vm.BCNProviderIssue;
                    //_rec.Other = _vm.Other;
                    //-- added 6/14/2018 ---
                    _rec.IssueDate = _vm.IssueDate;
                    _rec.ComplaintDate = _vm.ComplaintDate;
                    _rec.ResolutionDate = _vm.ResolutionDate;
                    _rec.InitialRespDate = _vm.InitialRespDate;
                    _rec.WrittenRespDate = _vm.WrittenRespDate;
                    _rec.Call = _vm.Call;
                    _rec.Email = _vm.Email;
                    _rec.Fax = _vm.Fax;
                    _rec.Mail = _vm.Mail;
                    _rec.CallRcvdWebsite = _vm.CallRcvdWebsite;
                    _rec.SocialMedia = _vm.SocialMedia;
                    _rec.InsCompany = _vm.InsCompany;
                    _rec.Survey = _vm.Survey;
                    _rec.CallRcvdOther = _vm.CallRcvdOther;
                    _rec.ComplaintProduct = _vm.ComplaintProduct;
                    //_rec.ComplaintShipping = _vm.ComplaintShipping;
                    //_rec.ComplaintService = _vm.ComplaintService;
                    //_rec.ComplaintSmartAction = _vm.ComplaintSmartAction;




                    _db.tbl_CSRComplaintLog.Add(_rec);

                    try { } catch (Exception ex) { var msg = ex.Message; }
                    _db.SaveChanges();

                    id = _rec.id;

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return id;
        }

        public static string AddNote_ComplaintLog(CSRComplaintVM _vm, int? reference)
        {
            StringBuilder otherStr = new StringBuilder();
            StringBuilder noteString = new StringBuilder();

            // string otherStr = "Call Regarding = ";
            // string noteString = "";
            try
            {

                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName1.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //Int32? id = Convert.ToInt32(id_op.ID);

                var components = HttpContext.Current.User.Identity.Name.Split('\\');

                var userName = components.Last();

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);



                    //IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();

                    //_notelist = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && (t.NoteHeading == "COMPLAINTS")).ToList();

                    tbl_Account_Note _note = new tbl_Account_Note();


                    //_vm.Damaged == true
                    //_vm.ComplaintShipping == true
                    //_vm.ComplaintService == true
                    //_vm.ComplaintSmartAction == true
                    //_vm.Website == true
                    //_vm.InsLimitGuidelines == true 
                    //_vm.BCNProviderIssue == true 
                    //_vm.Other == true
                    //|| (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                    // COMPLAINTS
                    if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                        || _vm.BloodPressureMonitors == true
                        || _vm.BreastPumps == true
                        || _vm.ContGlucoseMonitoring == true
                        || _vm.DiabeticTestSup == true
                        || _vm.EnteralNutrition == true
                        || _vm.ExternalDefibrillator == true
                        || _vm.IncontinenceSupplies == true
                        || _vm.InsulinPumpsSupplies == true
                        || _vm.InsSyrPenNeed == true
                        || _vm.OstomySupplies == true
                        || _vm.PleurXDrainSys == true
                        || _vm.PTINRTesting == true
                        || _vm.TENSUnitSup == true
                        || _vm.UrologicalSupplies == true
                        || _vm.WoundCareSupplies == true
                        || _vm.OtherUnsureSupplies == true

                        || _vm.BDI == true
                        || _vm.BPnBPM == true
                        || _vm.CallCenter == true
                        || _vm.CSRAssessment == true
                        || _vm.DynamicSynergy == true
                        || _vm.Enteral == true
                        || _vm.HGS == true
                        || _vm.InsulinPumpCGM == true
                        || _vm.MedicalDocuments == true
                        || _vm.NewAccountTeam == true
                        || _vm.Nurses == true
                        || _vm.QualityAssurance == true
                        || _vm.Shipping == true
                        || _vm.THC == true
                        || _vm.Troy == true
                        || _vm.Verification == true
                        || _vm.WebSupport == true
                        || _vm.WoundCareOstomyTENS == true
                        || _vm.OtherUnsureTeam == true
                        || _vm.ITHelpDesk == true
                             || _vm.Compliance == true || _vm.Driver == true || _vm.WrongProductShipped == true || _vm.QualityOfProduct == true
                             || _vm.WrongArea == true || _vm.MissingProduct == true || (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                             || _vm.ProductIncorrect == true || _vm.Call == true || _vm.Email == true || _vm.Fax == true || _vm.WebsitePortal == true || _vm.SocialMedia == true || _vm.InsCompany == true || _vm.Survey == true || _vm.CallRcvdOther == true || _vm.ProductMispick == true || _vm.ProductDefective || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                             || _vm.ImpoliteORoffensive == true || _vm.HoldTimes == true || (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null)
                             || (_vm.Others != "" && _vm.Others != null) || (_vm.ComplaintHasBeen != "" && _vm.ComplaintHasBeen != null)
                             || _vm.DidntFollowDelIns == true || _vm.VConfirmationCalls == true || _vm.ComplaintProduct == true || _vm.CustomerService == true || _vm.Discrimination == true || _vm.SmartAction == true || _vm.VPaymentCalles == true || _vm.SAJamesPhonePromts == true
                                || _vm.SAJamesSelfService == true || _vm.VirtualCallBack == true || _vm.WebsitePortal == true || _vm.NoFollowUp == true || _vm.ReturnedFromVM == true || _vm.NoFollowUpWithMem == true ||
                                _vm.NeverRecivedSupplies == true || _vm.PhysicianIssue == true || _vm.HealthPlan == true || _vm.ProductDefectiveQuality == true || _vm.ShippingUSPS == true || _vm.ShippingWarehouse == true || _vm.TextMessaging == true || _vm.ProcessDelay == true || _vm.PhoneFaxIssues == true ||
                               (_vm.Product != "" && _vm.Product != null) || (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null)
                               || _vm.IssueDate != null || _vm.ComplaintDate != null || _vm.ResolutionDate != null || _vm.InitialRespDate != null || _vm.WrittenRespDate != null
                               )
                    {
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMPLAINTS").FirstOrDefault();  //&& t.NoteCreatedBy == id

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

                            _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMPLAINTS").FirstOrDefault();  //&& t.NoteCreatedBy == id



                        }
                        if (_note != null)
                        {

                            tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                            _tHist.ID_Note = _note.ID;
                            _tHist.NoteDate = DateTime.Now;

                            //_vm.Damaged == true
                            //_vm.ComplaintShipping == true
                            //_vm.ComplaintService == true
                            //_vm.ComplaintSmartAction == true
                            //_vm.Website == true
                            //_vm.InsLimitGuidelines == true
                            //_vm.BCNProviderIssue == true
                            //_vm.Other == true
                            //|| (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                            if ((_vm.TrackingNumber != "" && _vm.TrackingNumber != null) || (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                || _vm.BloodPressureMonitors == true
                        || _vm.BreastPumps == true
                        || _vm.ContGlucoseMonitoring == true
                        || _vm.DiabeticTestSup == true
                        || _vm.EnteralNutrition == true
                        || _vm.ExternalDefibrillator == true
                        || _vm.IncontinenceSupplies == true
                        || _vm.InsulinPumpsSupplies == true
                        || _vm.InsSyrPenNeed == true
                        || _vm.OstomySupplies == true
                        || _vm.PleurXDrainSys == true
                        || _vm.PTINRTesting == true
                        || _vm.TENSUnitSup == true
                        || _vm.UrologicalSupplies == true
                        || _vm.WoundCareSupplies == true
                        || _vm.OtherUnsureSupplies == true

                        || _vm.BDI == true
                        || _vm.BPnBPM == true
                        || _vm.CallCenter == true
                        || _vm.CSRAssessment == true
                        || _vm.DynamicSynergy == true
                        || _vm.Enteral == true
                        || _vm.HGS == true
                        || _vm.InsulinPumpCGM == true
                        || _vm.MedicalDocuments == true
                        || _vm.NewAccountTeam == true
                        || _vm.Nurses == true
                        || _vm.QualityAssurance == true
                        || _vm.Shipping == true
                        || _vm.THC == true
                        || _vm.Troy == true
                        || _vm.Verification == true
                        || _vm.WebSupport == true
                        || _vm.WoundCareOstomyTENS == true
                        || _vm.OtherUnsureTeam == true
                        || _vm.ITHelpDesk == true
                                || _vm.Compliance == true
                                || _vm.Call == true || _vm.Email == true || _vm.Fax == true || _vm.WebsitePortal == true || _vm.SocialMedia == true || _vm.InsCompany == true || _vm.Survey == true || _vm.CallRcvdOther == true ||
                                _vm.Driver == true || _vm.WrongProductShipped == true || _vm.QualityOfProduct == true
                                || _vm.WrongArea == true || _vm.MissingProduct == true || (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                                  || _vm.DidntFollowDelIns == true || _vm.VConfirmationCalls == true || _vm.VPaymentCalles == true || _vm.SAJamesPhonePromts == true
                                || _vm.SAJamesSelfService == true || _vm.VirtualCallBack == true || _vm.WebsitePortal == true || _vm.NoFollowUp == true || _vm.ReturnedFromVM == true || _vm.NoFollowUpWithMem == true ||
                                _vm.NeverRecivedSupplies == true || _vm.PhysicianIssue == true || _vm.HealthPlan == true || _vm.ProductDefectiveQuality == true || _vm.ShippingUSPS == true || _vm.ShippingWarehouse == true || _vm.TextMessaging == true || _vm.ProcessDelay == true || _vm.PhoneFaxIssues == true ||
                               (_vm.Product != "" && _vm.Product != null) || (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null)
                               || _vm.IssueDate != null || _vm.ComplaintDate != null || _vm.ResolutionDate != null || _vm.InitialRespDate != null || _vm.WrittenRespDate != null ||
                                _vm.ComplaintProduct == true || _vm.CustomerService == true || _vm.Discrimination == true || _vm.SmartAction == true
                               )
                            {
                                otherStr.Append("Complaint Received By = ");
                                if (_vm.Call == true)
                                    otherStr.Append(" Call " + Environment.NewLine);
                                if (_vm.Email == true)
                                    otherStr.Append(" Email " + Environment.NewLine);
                                if (_vm.Fax == true)
                                    otherStr.Append(" Fax " + Environment.NewLine);
                                //if (_vm.Website == true)
                                //    otherStr.Append(" Website " + Environment.NewLine);
                                if (_vm.WebsitePortal == true)
                                    otherStr.Append(" Website / Portal " + Environment.NewLine);
                                if (_vm.SocialMedia == true)
                                    otherStr.Append(" Social Media " + Environment.NewLine);
                                if (_vm.InsCompany == true)
                                    otherStr.Append(" Insurance Company " + Environment.NewLine);
                                if (_vm.Survey == true)
                                    otherStr.Append(" Survey " + Environment.NewLine);
                                if (_vm.CallRcvdOther == true)
                                    otherStr.Append(" Other  " + Environment.NewLine);

                                if (_vm.IssueDate != null)
                                    otherStr.Append(" Issue Date  " + Convert.ToDateTime(_vm.IssueDate).ToShortDateString() + Environment.NewLine);
                                if (_vm.ComplaintDate != null)
                                    otherStr.Append(" Complaint Date  " + Convert.ToDateTime(_vm.ComplaintDate).ToShortDateString() + Environment.NewLine);
                                if (_vm.ResolutionDate != null)
                                    otherStr.Append(" Resolution Date  " + Convert.ToDateTime(_vm.ResolutionDate).ToShortDateString() + Environment.NewLine);
                                if (_vm.InitialRespDate != null)
                                    otherStr.Append(" Initital Response Date  " + Convert.ToDateTime(_vm.InitialRespDate).ToShortDateString() + Environment.NewLine);
                                if (_vm.WrittenRespDate != null)
                                    otherStr.Append(" Written Response Date  " + Convert.ToDateTime(_vm.WrittenRespDate).ToShortDateString() + Environment.NewLine);


                                if (_vm.TrackingNumber != "" && _vm.TrackingNumber != null)
                                    otherStr.Append("Tracking Number =" + _vm.TrackingNumber + Environment.NewLine);
                                //otherStr = otherStr + " Tracking Number = " + _vm.TrackingNumber + "_";

                                if (_vm.WorkOrder != "" && _vm.WorkOrder != null)
                                    otherStr.Append("WorkOrder = " + _vm.WorkOrder + Environment.NewLine);
                                // otherStr = otherStr + " WorkOrder = " + _vm.WorkOrder + "_";

                                if (_vm.Product != "" && _vm.Product != null)
                                    otherStr.Append("Product = " + _vm.Product + Environment.NewLine);

                                if (_vm.DeliveryCompany != "" && _vm.DeliveryCompany != null)
                                    otherStr.Append("Delivery Company = " + _vm.DeliveryCompany + Environment.NewLine);

                                //if (_vm.BCNProvider != "" && _vm.BCNProvider != null)
                                //    otherStr.Append("BCN Provider = " + _vm.BCNProvider + Environment.NewLine);

                                //if (_vm.Damaged == true)
                                //    otherStr.Append("Damaged" + Environment.NewLine);

                                if (_vm.BloodPressureMonitors == true)
                                    otherStr.Append("Blood Pressure Monitors" + Environment.NewLine);

                                if (_vm.BreastPumps == true)
                                    otherStr.Append("Breast Pumps" + Environment.NewLine);

                                if (_vm.ContGlucoseMonitoring == true)
                                    otherStr.Append("Continuous Glucose Monitoring" + Environment.NewLine);

                                if (_vm.DiabeticTestSup == true)
                                    otherStr.Append("Diabetic Testing Supplies" + Environment.NewLine);

                                if (_vm.EnteralNutrition == true)
                                    otherStr.Append("Enteral Nutrition" + Environment.NewLine);

                                if (_vm.ExternalDefibrillator == true)
                                    otherStr.Append("External Defibrillator" + Environment.NewLine);

                                if (_vm.IncontinenceSupplies == true)
                                    otherStr.Append("Incontinence Supplies" + Environment.NewLine);

                                if (_vm.InsulinPumpsSupplies == true)
                                    otherStr.Append("Insulin Pumps & Supplies" + Environment.NewLine);

                                if (_vm.InsSyrPenNeed == true)
                                    otherStr.Append("Insulin Syringes & Pen Needles" + Environment.NewLine);

                                if (_vm.OstomySupplies == true)
                                    otherStr.Append("Ostomy Supplies" + Environment.NewLine);

                                if (_vm.PleurXDrainSys == true)
                                    otherStr.Append("PleurX Drainage System" + Environment.NewLine);

                                if (_vm.PTINRTesting == true)
                                    otherStr.Append("PT INR Testing" + Environment.NewLine);

                                if (_vm.TENSUnitSup == true)
                                    otherStr.Append("TENS Unit & Supplies" + Environment.NewLine);

                                if (_vm.UrologicalSupplies == true)
                                    otherStr.Append("Urological Supplies" + Environment.NewLine);

                                if (_vm.WoundCareSupplies == true)
                                    otherStr.Append("Wound Care Supplies" + Environment.NewLine);

                                if (_vm.OtherUnsureSupplies == true)
                                    otherStr.Append("Other/Unsure of Supplies" + Environment.NewLine);

                                if (_vm.BDI == true)
                                    otherStr.Append("BDI" + Environment.NewLine);

                                if (_vm.BPnBPM == true)
                                    otherStr.Append("Breast Pumps & Blood Pressure Monitors" + Environment.NewLine);

                                if (_vm.CallCenter == true)
                                    otherStr.Append("Call Center" + Environment.NewLine);

                                if (_vm.CSRAssessment == true)
                                    otherStr.Append("CSR Assessment" + Environment.NewLine);

                                if (_vm.DynamicSynergy == true)
                                    otherStr.Append("Dynamic Synergy" + Environment.NewLine);

                                if (_vm.Enteral == true)
                                    otherStr.Append("Enteral" + Environment.NewLine);

                                if (_vm.HGS == true)
                                    otherStr.Append("HGS" + Environment.NewLine);

                                if (_vm.InsulinPumpCGM == true)
                                    otherStr.Append("Insulin Pump & CGM" + Environment.NewLine);

                                if (_vm.MedicalDocuments == true)
                                    otherStr.Append("Medical Documents" + Environment.NewLine);

                                if (_vm.NewAccountTeam == true)
                                    otherStr.Append("New Account" + Environment.NewLine);

                                if (_vm.Nurses == true)
                                    otherStr.Append("Nurses" + Environment.NewLine);

                                if (_vm.QualityAssurance == true)
                                    otherStr.Append("Quality Assurance" + Environment.NewLine);

                                if (_vm.Shipping == true)
                                    otherStr.Append("Shipping" + Environment.NewLine);

                                if (_vm.THC == true)
                                    otherStr.Append("THC" + Environment.NewLine);

                                if (_vm.Troy == true)
                                    otherStr.Append("Troy (BCN, BCNA, MAPPO)" + Environment.NewLine);

                                if (_vm.Verification == true)
                                    otherStr.Append("Verification" + Environment.NewLine);

                                if (_vm.WebSupport == true)
                                    otherStr.Append("Web Support" + Environment.NewLine);

                                if (_vm.WoundCareOstomyTENS == true)
                                    otherStr.Append("Wound Care, Ostomy & TENS" + Environment.NewLine);

                                if (_vm.OtherUnsureTeam == true)
                                    otherStr.Append("Other/Unsure of Team" + Environment.NewLine);

                                if (_vm.ITHelpDesk == true)
                                    otherStr.Append("IT / HelpDesk" + Environment.NewLine);

                                if (_vm.Compliance == true)
                                    otherStr.Append("Compliance" + Environment.NewLine);

                                if (_vm.ComplaintProduct == true)
                                    otherStr.Append("Product" + Environment.NewLine);

                                if (_vm.CustomerService == true)
                                    otherStr.Append("Customer Service (CSR Issue, Hold Times,etc)" + Environment.NewLine);

                                //if (_vm.ComplaintShipping == true)
                                //    otherStr.Append("Shipping" + Environment.NewLine);

                                //if (_vm.ComplaintService == true)
                                //    otherStr.Append("Service" + Environment.NewLine);

                                if (_vm.Discrimination == true)
                                    otherStr.Append("Discrimination / Civil Rights" + Environment.NewLine);

                                //if (_vm.ComplaintSmartAction == true)
                                //    otherStr.Append("Smart Action" + Environment.NewLine);
                                //otherStr = otherStr + "Damaged_";

                                if (_vm.SmartAction == true)
                                    otherStr.Append("Smart Action" + Environment.NewLine);

                                if (_vm.HealthPlan == true)
                                    otherStr.Append("Health Plan (insurance limits, guidelines, etc)" + Environment.NewLine);

                                if (_vm.ProductDefectiveQuality == true)
                                    otherStr.Append("Product (Defective, Quality, etc)" + Environment.NewLine);

                                if (_vm.ShippingUSPS == true)
                                    otherStr.Append("Shipping (USPS / FedEx)" + Environment.NewLine);

                                if (_vm.ShippingWarehouse == true)
                                    otherStr.Append("Shipping (Warehouse: Mispicks, Missing, etc)" + Environment.NewLine);

                                if (_vm.TextMessaging == true)
                                    otherStr.Append("Text Messaging" + Environment.NewLine);

                                if (_vm.ProcessDelay == true)
                                    otherStr.Append("Process Delay (Internal delays regarding follow up, processing, submission, etc)" + Environment.NewLine);

                                if (_vm.PhoneFaxIssues == true)
                                    otherStr.Append("Phone / Fax Issues" + Environment.NewLine);

                                if (_vm.Driver == true)
                                    otherStr.Append("Driver" + Environment.NewLine);
                                //otherStr = otherStr + "Driver_";


                                if (_vm.DidntFollowDelIns == true)
                                    otherStr.Append("Did not follow delivery instructions" + Environment.NewLine);

                                if (_vm.VConfirmationCalls == true)
                                    otherStr.Append("Victor Confirmation Calls " + Environment.NewLine);

                                if (_vm.VPaymentCalles == true)
                                    otherStr.Append("Victor Payment Calls " + Environment.NewLine);

                                if (_vm.SAJamesPhonePromts == true)
                                    otherStr.Append("Smart Action/James Phone Prompts " + Environment.NewLine);

                                if (_vm.SAJamesSelfService == true)
                                    otherStr.Append("Smart Action/James self service " + Environment.NewLine);

                                if (_vm.VirtualCallBack == true)
                                    otherStr.Append("Virtual Callback feature " + Environment.NewLine);

                                if (_vm.NoFollowUp == true)
                                    otherStr.Append("No follow up on account/on documentation" + Environment.NewLine);

                                if (_vm.ReturnedFromVM == true)
                                    otherStr.Append("Call not returned from VM" + Environment.NewLine);

                                if (_vm.WrongProductShipped == true)
                                    otherStr.Append("Wrong Product Shipped" + Environment.NewLine);
                                //otherStr = otherStr + "WrongProductShipped_";

                                if (_vm.NoFollowUpWithMem == true)
                                    otherStr.Append("No follow up with member " + Environment.NewLine);

                                if (_vm.NeverRecivedSupplies == true)
                                    otherStr.Append("Never received supplies" + Environment.NewLine);

                                if (_vm.PhysicianIssue == true)
                                    otherStr.Append("Physician Issue" + Environment.NewLine);

                                //if (_vm.InsLimitGuidelines == true)
                                //    otherStr.Append("Insurance Limitations/Guidelines" + Environment.NewLine);

                                //if (_vm.BCNProviderIssue == true)
                                //    otherStr.Append("BCN Provider Issue " + Environment.NewLine);


                                if (_vm.QualityOfProduct == true)
                                    otherStr.Append("Quality Of Product" + Environment.NewLine);
                                //otherStr = otherStr + "QualityOfProduct_";

                                if (_vm.WrongArea == true)
                                    otherStr.Append("Wrong Area" + Environment.NewLine);
                                //otherStr = otherStr + "WrongArea_";

                                if (_vm.MissingProduct == true)
                                    otherStr.Append("Missing Product" + Environment.NewLine);
                                // otherStr = otherStr + "MissingProduct_";

                                //if (_vm.Other == true)
                                //    otherStr.Append("Other" + Environment.NewLine);


                                //if (_vm.Website == true)
                                //    otherStr.Append("Website" + Environment.NewLine);
                                if (_vm.WebsitePortal == true)
                                    otherStr.Append("Website / Portal" + Environment.NewLine);

                                if (_vm.FedExTextArea != "" && _vm.FedExTextArea != null)
                                    otherStr.Append("Note =" + _vm.FedExTextArea + Environment.NewLine);
                                //  otherStr = otherStr + " FedExNote =" + _vm.FedExTextArea + "_";

                            }

                            if (_vm.ProductIncorrect == true || _vm.ProductMispick == true || _vm.ProductDefective || (_vm.ProductTextArea != "" && _vm.ProductTextArea != null))
                            {

                                if (_vm.ProductIncorrect == true)
                                    otherStr.Append("Incorrect Product" + Environment.NewLine);
                                // otherStr = otherStr + "Incorrect_";

                                if (_vm.ProductMispick == true)
                                    otherStr.Append("Mispick Product" + Environment.NewLine);
                                // otherStr = otherStr + "Mispick_";

                                if (_vm.ProductDefective == true)
                                    otherStr.Append("Defective Product" + Environment.NewLine);
                                // otherStr = otherStr + "Defective_";



                                if (_vm.ProductTextArea != "" && _vm.ProductTextArea != null)
                                    otherStr.Append("Product Note =" + _vm.ProductTextArea + Environment.NewLine);
                                //otherStr = otherStr + " Product Note =" + _vm.ProductTextArea + "_";

                            }

                            if (_vm.ImpoliteORoffensive == true || _vm.HoldTimes == true || (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null))
                            {

                                if (_vm.ImpoliteORoffensive == true)
                                    otherStr.Append("Impolite/Offensive" + Environment.NewLine);
                                //otherStr = otherStr + "Impolite/Offensive_";

                                if (_vm.HoldTimes == true)
                                    otherStr.Append("Hold Times" + Environment.NewLine);
                                //otherStr = otherStr + "HoldTimes_";


                                if (_vm.CustomerServiceTextArea != "" && _vm.CustomerServiceTextArea != null)
                                    otherStr.Append("Customer Service Note =" + _vm.CustomerServiceTextArea + Environment.NewLine);
                                //otherStr = otherStr + " Customer Service Note =" + _vm.CustomerServiceTextArea + "_";

                            }

                            if (_vm.Others != "" && _vm.Others != null)
                            {
                                otherStr.Append("Others Note =" + _vm.Others + Environment.NewLine);
                                // otherStr = otherStr + " Others Note =" + _vm.Others + "_";

                            }

                            if (_vm.PendingResolution != "" && _vm.PendingResolution != null)
                            {
                                otherStr.Append("Pending Resolution =" + _vm.PendingResolution + Environment.NewLine);
                                // otherStr = otherStr + " Others Note =" + _vm.Others + "_";

                            }

                            if (_vm.Resolution != "" && _vm.Resolution != null)
                            {
                                otherStr.Append("Resolution =" + _vm.Resolution + Environment.NewLine);
                                // otherStr = otherStr + " Others Note =" + _vm.Others + "_";

                            }

                            StringBuilder _msgStr23 = new StringBuilder();
                            _msgStr23.Append(noteString);
                            _msgStr23.Append(otherStr);
                            _msgStr23.Append("Complaint Has Been = " + _vm.ComplaintHasBeen + Environment.NewLine);

                            if (reference != null)
                            {
                                _msgStr23.Append("Reference Number = " + reference + Environment.NewLine);
                            }


                            _tHist.NoteText = _msgStr23.ToString();

                            //_tHist.NoteText = noteString + otherStr + "Complaint Has Been = " + _vm.ComplaintHasBeen;
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

            StringBuilder _retStr = new StringBuilder();
            _retStr.Append(noteString);
            _retStr.Append(otherStr);
            _retStr.Append("Complaint has been = " + _vm.ComplaintHasBeen + Environment.NewLine);

            string returnStr = _retStr.ToString();

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
            //  mail.To.Add("gbethini@jandbmedical.com");
            mail.Bcc.Add("grani@jandbmedical.com");


            mail.Subject = "CSR Call Complaint Log";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Issue for account - " + acc + " has not been resolved.</td><td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Reference Number is:  </td> " + reference + "<td></td>";
            mail.Body += "</tr>";

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

        public static void sendComplainLogEmail(string message, Int64? acc, int? reference)
        {


            //  DateTime todaydate = DateTime.Today.Date;



            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");

            mail.To.Add("CustomerServiceManager@jandbmedical.com");
            //mail.CC.Add("maheshkattamuribpl@jandbmedical.com");

            mail.Subject = "CSR Call Complaint Log";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Issue for account - " + acc + " has not been resolved.</td><td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  Reference Number is:  </td> " + reference + "<td></td>";
            mail.Body += "</tr>";

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

        public static void sendComplainLogEmailToSupervisors(string message, Int64? acc, int? reference)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");
            mail.To.Add("DISTeamLeader@jandbmedical.com");
            //mail.CC.Add("maheshkattamuribpl@jandbmedical.com");
            mail.Subject = "CSR Call Complaint Log To Supervisors";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Issue for account - " + acc + " has not been resolved.</td><td></td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>  Reference Number is:  </td> " + reference + "<td></td>";
            mail.Body += "</tr>";
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
            SmtpServer.Send(mail);
        }

        public static void sendComplainLogEmailToManagers(string message, Int64? acc, int? reference, CSRComplaintVM _vm)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");
            mail.To.Add("bserra@jandbmedical.com");
            mail.To.Add("tkemp@jandbmedical.com");
            mail.To.Add("tmoore@jandbmedical.com");
            mail.To.Add("tsymanski@jandbmedical.com");
            mail.To.Add("rpetrashko@jandbmedical.com");
            mail.To.Add("kdick@jandbmedical.com");
            mail.To.Add("jujones@jandbmedical.com");
            mail.To.Add("gbarragan@jandbmedical.com");
            mail.To.Add("jmudge@jandbmedical.com");
            //mail.CC.Add("maheshkattamuribpl@jandbmedical.com");
            mail.Subject = "CSR Call Complaint Log To Managers";
            StringBuilder complaintReceived = new StringBuilder();
            if (_vm.Call == true)
                complaintReceived.Append(" Call " + "<br />");
            if (_vm.Email == true)
                complaintReceived.Append(" Email " + "<br />");
            if (_vm.Fax == true)
                complaintReceived.Append(" Fax " + "<br />");
            if (_vm.CallRcvdWebsite == true)
                complaintReceived.Append(" Website " + "<br />");
            if (_vm.SocialMedia == true)
                complaintReceived.Append(" Social Media " + "<br />");
            if (_vm.InsCompany == true)
                complaintReceived.Append(" Insurance Company " + "<br />");
            if (_vm.Survey == true)
                complaintReceived.Append(" Survey " + "<br />");
            if (_vm.CallRcvdOther == true)
                complaintReceived.Append(" Other " + "<br />");

            StringBuilder productLine = new StringBuilder();
            if (_vm.BloodPressureMonitors == true)
                productLine.Append(" Blood Pressure Monitors " + "<br />");
            if (_vm.BreastPumps == true)
                productLine.Append(" Breast Pumps " + "<br />");
            if (_vm.ContGlucoseMonitoring == true)
                productLine.Append(" Continuous Glucose Monitoring " + "<br />");
            if (_vm.DiabeticTestSup == true)
                productLine.Append(" Diabetic Testing Supplies " + "<br />");
            if (_vm.EnteralNutrition == true)
                productLine.Append(" Enteral Nutrition " + "<br />");
            if (_vm.ExternalDefibrillator == true)
                productLine.Append(" External Defibrillator " + "<br />");
            if (_vm.IncontinenceSupplies == true)
                productLine.Append(" Incontinence Supplies " + "<br />");
            if (_vm.InsulinPumpsSupplies == true)
                productLine.Append(" Insulin Pumps & Supplies " + "<br />");
            if (_vm.InsSyrPenNeed == true)
                productLine.Append(" Insulin Syringes & Pen Needles " + "<br />");
            if (_vm.OstomySupplies == true)
                productLine.Append(" Ostomy Supplies " + "<br />");
            if (_vm.PleurXDrainSys == true)
                productLine.Append(" PleurX Drainage System " + "<br />");
            if (_vm.PTINRTesting == true)
                productLine.Append(" PT INR Testing " + "<br />");
            if (_vm.TENSUnitSup == true)
                productLine.Append(" TENS Unit & Supplies " + "<br />");
            if (_vm.UrologicalSupplies == true)
                productLine.Append(" Urological Supplies " + "<br />");
            if (_vm.WoundCareSupplies == true)
                productLine.Append(" Wound Care Supplies " + "<br />");
            if (_vm.OtherUnsureSupplies == true)
                productLine.Append(" Other/Unsure of Supplies " + "<br />");

            StringBuilder team = new StringBuilder();
            if (_vm.BDI == true)
                team.Append(" BDI " + "<br />");
            if (_vm.BPnBPM == true)
                team.Append(" Breast Pumps & Blood Pressure Monitors " + "<br />");
            if (_vm.CallCenter == true)
                team.Append(" Call Center " + "<br />");
            if (_vm.CSRAssessment == true)
                team.Append(" CSR Assessment " + "<br />");
            if (_vm.DynamicSynergy == true)
                team.Append(" Dynamic Synergy " + "<br />");
            if (_vm.Enteral == true)
                team.Append(" Enteral " + "<br />");
            if (_vm.HGS == true)
                team.Append(" HGS " + "<br />");
            if (_vm.InsulinPumpCGM == true)
                team.Append(" Insulin Pump & CGM " + "<br />");
            if (_vm.MedicalDocuments == true)
                team.Append(" Medical Documents " + "<br />");
            if (_vm.NewAccountTeam == true)
                team.Append(" New Account " + "<br />");
            if (_vm.Nurses == true)
                team.Append(" Nurses " + "<br />");
            if (_vm.QualityAssurance == true)
                team.Append(" Quality Assurance " + "<br />");
            if (_vm.Shipping == true)
                team.Append(" Shipping " + "<br />");
            if (_vm.THC == true)
                team.Append(" THC " + "<br />");
            if (_vm.Troy == true)
                team.Append(" Troy (BCN, BCNA, MAPPO) " + "<br />");
            if (_vm.Verification == true)
                team.Append(" Verification " + "<br />");
            if (_vm.WebSupport == true)
                team.Append(" Web Support " + "<br />");
            if (_vm.WoundCareOstomyTENS == true)
                team.Append(" Wound Care, Ostomy & TENS " + "<br />");
            if (_vm.OtherUnsureTeam == true)
                team.Append(" Other/Unsure of Team " + "<br />");
            if (_vm.ITHelpDesk == true)
                team.Append(" IT / HelpDesk " + "<br />");

            StringBuilder complaintType = new StringBuilder();
            if (_vm.Compliance == true)
                complaintType.Append(" Compliance " + "<br />");
            if (_vm.CustomerService == true)
                complaintType.Append(" Customer Service (CSR Issue, Hold Times,etc) " + "<br />");
            if (_vm.Discrimination == true)
                complaintType.Append(" Discrimination / Civil Rights " + "<br />");
            if (_vm.HealthPlan == true)
                complaintType.Append(" Health Plan (insurance limits, guidelines, etc) " + "<br />");
            if (_vm.ProductDefectiveQuality == true)
                complaintType.Append(" Product (Defective, Quality, etc) " + "<br />");
            if (_vm.ShippingUSPS == true)
                complaintType.Append(" Shipping (USPS / FedEx) " + "<br />");
            if (_vm.ShippingWarehouse == true)
                complaintType.Append(" Shipping (Warehouse: Mispicks, Missing, etc) " + "<br />");
            if (_vm.SmartAction == true)
                complaintType.Append(" SmartAction (Victor / Julie) " + "<br />");
            if (_vm.TextMessaging == true)
                complaintType.Append(" Text Messaging " + "<br />");
            if (_vm.WebsitePortal == true)
                complaintType.Append(" Website / Portal " + "<br />");
            if (_vm.ProcessDelay == true)
                complaintType.Append(" Process Delay (Internal delays regarding follow up, processing, submission, etc) " + "<br />");
            if (_vm.PhoneFaxIssues == true)
                complaintType.Append(" Phone / Fax Issues " + "<br />");

            mail.Body += " <html>";
            mail.Body += " <head><style>table, th, td {border: 1px solid black;border - collapse: collapse;}</style></head> ";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Account: </td><td>" + _vm.Account + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Reference Number: </td><td>" + _vm.id + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Complaint Received: </td><td>" + complaintReceived.ToString() + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Product Line: </td><td>" + productLine.ToString() + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Team: </td><td>" + team.ToString() + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Complaint Type: </td><td>" + complaintType.ToString() + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Comments: </td><td>" + _vm.Others + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td>Complaint Status: </td><td>" + _vm.ComplaintHasBeen + "</td>";
            mail.Body += "</tr>";
            mail.Body += "<tr>";
            mail.Body += "<td> Link to Complaint Log: </td><td>http://10.10.1.49/JBIntranet/Reports/CustomerServices/CSRComlaintlog</td>";
            mail.Body += "</tr>";
            mail.Body += "</table>";
            mail.Body += "<br />";
            mail.Body += "<span>Thank You!</span>";
            mail.Body += "</body>";
            mail.Body += "</html>";
            mail.IsBodyHtml = true;
            SmtpServer.Send(mail);
        }
    }
    //    public class CSRLogComplaints
    //{

    //    public int? Account { get; set; }



    //    public DateTime CreatedOn { get; set; }
    //    public string CreatedBy { get; set; }
    //    public int CallDuration { get; set; }
    //    public string SState { get; set; }
    //    public bool NewAccount { get; set; }

    //    public bool HandlingConcern { get; set; }
    //    public string Complaint { get; set; }

    //    public bool FedExComplaints { get; set; }
    //    public string FedExTrack { get; set; }
    //    public bool WICSComplaints { get; set; }

    //    public string C_Description { get; set; }
    //    public string C_Corrective { get; set; }

    //    public bool C_Resolved { get; set; }
    //    public bool firsttime { get; set; }

    //    public AccountInfoVM details { get; set; }
    //}

    public class AccountInfoVM
    {
        public int? Account2 { get; set; }
        public string TimerVal { get; set; }
        public string firstName { get; set; }

        public string Email { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

    }

    //public class CSRReasonforCall
    //{

    //    public string Account { get; set; }

    //    public DateTime CreatedOn { get; set; }
    //    public string CreatedBy { get; set; }
    //    public int CallDuration { get; set; }
    //    public string SState { get; set; }
    //    public bool NewAccount { get; set; }

    //    public bool Demographic { get; set; }
    //    public string PChange { get; set; }

    //    public bool Samples { get; set; }
    //    public bool SampleChoice { get; set; }

    //    public string AStatus { get; set; }
    //    public bool OrderConfirmation { get; set; }
    //    public bool Restart { get; set; }


    //    public string TranferredCall { get; set; }
    //    public string TCallOther { get; set; }

    //    public string ShippingRelatedIssues { get; set; }


    //    public bool OtherCallHandling { get; set; }

    //    public string OtherCallHandlingInfo { get; set; }
    //    public bool firsttime { get; set; }
    //}

    public class AccountStatusVM
    {
        public bool Eligibility { get; set; }
        public bool Rx { get; set; }
        public bool CMN { get; set; }
        public bool AOB { get; set; }
        public bool NNA { get; set; }
        public bool Copay { get; set; }
        public bool OrderStatus { get; set; }


    }

    public class ID_VM
    {
        public double? ID { get; set; }
        public string name { get; set; }
    }
    public class CallLogVM
    {
        public bool BPM_Supplies { get; set; }
        public bool BreastPumps_Supplies { get; set; }
        public bool CGM_Supplies { get; set; }
        public bool Diabetes_Supplies { get; set; }
        public bool Nutrition_Supplies { get; set; }
        public bool Incontinence_Supplies { get; set; }
        public bool Pump_Supplies { get; set; }
        public bool Injectibles_Supplies { get; set; }
        public bool Ostomy_Supplies { get; set; }
        public bool Urological_Supplies { get; set; }
        public bool Wound_Supplies { get; set; }
        public bool Other_Supplies { get; set; }
        public string OthersText_Supplies { get; set; }
        public string ProductSupplies { get; set; }
        public string ProductServices { get; set; }
        public bool isPayer7or3739 { get; set; }
        public string ProductNeeds { get; set; }
        public bool BPM_Needs { get; set; }
        public bool BreastPumps_Needs { get; set; }
        public bool CGM_Needs { get; set; }
        public bool Diabetes_Needs { get; set; }
        public bool Nutrition_Needs { get; set; }
        public bool Incontinence_Needs { get; set; }
        public bool Pump_Needs { get; set; }
        public bool Injectibles_Needs { get; set; }
        public bool Ostomy_Needs { get; set; }
        public bool Urological_Needs { get; set; }
        public bool Wound_Needs { get; set; }
        public bool Other_Needs { get; set; }
        public string OthersText_Needs { get; set; }
        public string Comment_Needs { get; set; }

        //-----------------update ---------------------------//
        public string Product { get; set; }
        public string DeliveryCompany { get; set; }

        //public string BCNProvider { get; set; }

        //public bool DidntFollowDelIns { get; set; }
        //public bool VConfirmationCalls { get; set; }
        //public bool VPaymentCalles { get; set; }

        //public bool SAJamesPhonePromts { get; set; }
        //public bool SAJamesSelfService{ get; set; }

        //public bool VirtualCallBack { get; set; }

        //public bool Website { get; set; }

        //public bool NoFollowUp { get; set; }

        //public bool CivilRights { get; set; }
        //public bool ReturnedFromVM { get; set; }
        //public bool NoFollowUpWithMem { get; set; }
        //public bool NeverRecivedSupplies { get; set; }

        //public bool PhysicianIssue { get; set; }
        //public bool InsLimitGuidelines { get; set; }

        //public bool BCNProviderIssue { get; set; }

        //public bool Other { get; set; }

        //--------------------------------------------------

        public bool OpPermission { get; set; }
        public string errormsg { get; set; }
        public string empFullName { get; set; }

        //   public string refnum { get; set; }
        public bool firstTime { get; set; }
        public string TimerTxt { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public Int64? Account { get; set; }
        public bool VerifiedPHI { get; set; }

        public bool NamePHI { get; set; }
        public bool DOBPHI { get; set; }
        public bool AddressPHI { get; set; }
        public bool PhoneNumberPHI { get; set; }
        public bool HealthIDPHI { get; set; }
        public bool DriverLicPHI { get; set; }
        public bool NotVerifyPHI { get; set; }
        public bool OtherPHI { get; set; }
        public string OtherTextPHI { get; set; }

        public bool VoicesForHealth { get; set; }

        public bool Call { get; set; }
        public bool Task { get; set; }
        public bool Email { get; set; }
        public bool Fax { get; set; }
        public bool Web { get; set; }
        public bool LogTypeOther { get; set; }
        public bool Survey { get; set; }
        public bool Text { get; set; }
        public bool Portal { get; set; }
        public bool SocialMedia { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public string Relation { get; set; }
        public string otherRelname { get; set; }
        public bool Copay { get; set; }
        public bool Billing { get; set; }

        public bool SampleChoice { get; set; }

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

        public bool LMN { get; set; }

        public bool NewAccount { get; set; }

        public bool InsuarnceChanges { get; set; }
        public bool Restart { get; set; }
        public bool OrderConfirmation { get; set; }

        public bool FedExOrUSPSTracking { get; set; }
        public bool OrderShipped { get; set; }
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
        public bool Nursing_CSRassessment { get; set; }

        public bool Other_CallHandling { get; set; }
        public bool Sh_Other { get; set; }
        // public bool Compliant { get; set; }

        public bool AccountDeactivated { get; set; }

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
        public AccountInfoVM details { get; set; }
        public List<HDMSPayer> payerTypeList { get; set; }
        //Complaint log
        public string _refnum { get; set; }
        public int? _Account { get; set; }
        //public bool Damaged { get; set; }

        //Product Line
        public bool BloodPressureMonitors { get; set; }
        public bool BreastPumps { get; set; }
        public bool ContGlucoseMonitoring { get; set; }
        public bool DiabeticTestSup { get; set; }
        public bool EnteralNutrition { get; set; }
        public bool ExternalDefibrillator { get; set; }
        public bool IncontinenceSupplies { get; set; }
        public bool InsulinPumpsSupplies { get; set; }
        public bool InsSyrPenNeed { get; set; }
        public bool OstomySupplies { get; set; }
        public bool PleurXDrainSys { get; set; }
        public bool PTINRTesting { get; set; }
        public bool TENSUnitSup { get; set; }
        public bool UrologicalSupplies { get; set; }
        public bool WoundCareSupplies { get; set; }
        public bool OtherUnsureSupplies { get; set; }

        //Team
        public bool BDI { get; set; }
        public bool BPnBPM { get; set; }
        public bool CallCenter { get; set; }
        public bool CSRAssessment { get; set; }
        public bool DynamicSynergy { get; set; }
        public bool Enteral { get; set; }
        public bool HGS { get; set; }
        public bool InsulinPumpCGM { get; set; }
        public bool MedicalDocuments { get; set; }
        public bool NewAccountTeam { get; set; }
        public bool Nurses { get; set; }
        public bool QualityAssurance { get; set; }
        public bool Shipping { get; set; }
        public bool THC { get; set; }
        public bool Troy { get; set; }
        public bool Verification { get; set; }
        public bool WebSupport { get; set; }
        public bool WoundCareOstomyTENS { get; set; }
        public bool OtherUnsureTeam { get; set; }
        public bool ITHelpDesk { get; set; }

        //Complaint Type
        public bool Compliance { get; set; }
        public bool CustomerService { get; set; }
        public bool Discrimination { get; set; }
        public bool HealthPlan { get; set; }
        public bool ProductDefectiveQuality { get; set; }
        public bool ShippingUSPS { get; set; }
        public bool ShippingWarehouse { get; set; }
        public bool SmartAction { get; set; }
        public bool TextMessaging { get; set; }
        public bool WebsitePortal { get; set; }
        public bool ProcessDelay { get; set; }
        public bool PhoneFaxIssues { get; set; }

        //public bool Driver { get; set; }
        //public bool WrongProductShipped { get; set; }
        //public bool QualityOfProduct { get; set; }

        //public bool WrongArea { get; set; }
        //public bool other_MissingProduct { get; set; }

        public string TrackingNumber { get; set; }
        public string WorkOrder { get; set; }
        public string FedExTextArea { get; set; }

        public bool Incorrect { get; set; }
        public bool Mispick { get; set; }
        //public bool Defective { get; set; }
        public string ProductTextArea { get; set; }

        //public bool Impolite_Offensive { get; set; }
        //public bool HoldTimes { get; set; }
        public string CustomerServiceTextArea { get; set; }

        public string Others { get; set; }

        public string ComplainOutCome { get; set; }

        public bool HandledConcerns { get; set; }
        public bool Resolved { get; set; }
        public bool NotResolved { get; set; }
        public bool TransferredTeamLeader { get; set; }


        //



    }
    //public class ProductSurveyVM
    //{
    //    public string ProductNeeds { get; set; }

    //}

    public class CSRComplaintVM
    {

        //-----------------update ---------------------------//

        public bool newAccount { get; set; }
        public string Product { get; set; }
        public string DeliveryCompany { get; set; }

        //public string BCNProvider { get; set; }

        public bool DidntFollowDelIns { get; set; }
        public bool VConfirmationCalls { get; set; }
        public bool VPaymentCalles { get; set; }

        public bool SAJamesPhonePromts { get; set; }
        public bool SAJamesSelfService { get; set; }

        public bool VirtualCallBack { get; set; }

        //public bool Website { get; set; }

        public bool NoFollowUp { get; set; }

        public bool ReturnedFromVM { get; set; }
        public bool NoFollowUpWithMem { get; set; }
        public bool NeverRecivedSupplies { get; set; }

        public bool PhysicianIssue { get; set; }
        //public bool InsLimitGuidelines { get; set; }

        //public bool BCNProviderIssue { get; set; }

        //public bool Other { get; set; }

        //--------------------------------------------------

        public bool OpPermission { get; set; }
        public int? payerid { get; set; }
        public SelectList payerType { get; set; }
        public List<HDMSPayer> payerTypeList { get; set; }


        public int? id { get; set; }

        public bool ProductIncorrect { get; set; }
        public bool ProductMispick { get; set; }
        public bool ProductDefective { get; set; }
        public bool firstTime { get; set; }
        public bool ImpoliteORoffensive { get; set; }
        public string refnum { get; set; }
        public int? Account { get; set; }
        //public bool Damaged { get; set; }

        //Product Line
        public bool BloodPressureMonitors { get; set; }
        public bool BreastPumps { get; set; }
        public bool ContGlucoseMonitoring { get; set; }
        public bool DiabeticTestSup { get; set; }
        public bool EnteralNutrition { get; set; }
        public bool ExternalDefibrillator { get; set; }
        public bool IncontinenceSupplies { get; set; }
        public bool InsulinPumpsSupplies { get; set; }
        public bool InsSyrPenNeed { get; set; }
        public bool OstomySupplies { get; set; }
        public bool PleurXDrainSys { get; set; }
        public bool PTINRTesting { get; set; }
        public bool TENSUnitSup { get; set; }
        public bool UrologicalSupplies { get; set; }
        public bool WoundCareSupplies { get; set; }
        public bool OtherUnsureSupplies { get; set; }

        //Team
        public bool BDI { get; set; }
        public bool BPnBPM { get; set; }
        public bool CallCenter { get; set; }
        public bool CSRAssessment { get; set; }
        public bool DynamicSynergy { get; set; }
        public bool Enteral { get; set; }
        public bool HGS { get; set; }
        public bool InsulinPumpCGM { get; set; }
        public bool MedicalDocuments { get; set; }
        public bool NewAccountTeam { get; set; }
        public bool Nurses { get; set; }
        public bool QualityAssurance { get; set; }
        public bool Shipping { get; set; }
        public bool THC { get; set; }
        public bool Troy { get; set; }
        public bool Verification { get; set; }
        public bool WebSupport { get; set; }
        public bool WoundCareOstomyTENS { get; set; }
        public bool OtherUnsureTeam { get; set; }
        public bool ITHelpDesk { get; set; }

        //Complaint Type
        public bool Compliance { get; set; }
        public bool CustomerService { get; set; }
        public bool Discrimination { get; set; }
        public bool HealthPlan { get; set; }
        public bool ProductDefectiveQuality { get; set; }
        public bool ShippingUSPS { get; set; }
        public bool ShippingWarehouse { get; set; }
        public bool SmartAction { get; set; }
        public bool TextMessaging { get; set; }
        public bool WebsitePortal { get; set; }
        public bool ProcessDelay { get; set; }
        public bool PhoneFaxIssues { get; set; }


        public bool Driver { get; set; }
        public bool WrongProductShipped { get; set; }
        public bool QualityOfProduct { get; set; }

        public bool WrongArea { get; set; }
        public bool MissingProduct { get; set; }
        public string WorkOrder { get; set; }
        public string TrackingNumber { get; set; }
        public string FedExTextArea { get; set; }


        public string ProductTextArea { get; set; }


        public bool HoldTimes { get; set; }
        public string CustomerServiceTextArea { get; set; }

        public string Others { get; set; }

        //  public bool HandledConcerns { get; set; }
        //  public bool Resolved { get; set; }
        //  public bool NotResolved { get; set; }
        //  public bool TransferredTeamLeader { get; set; }

        public string PendingResolution { get; set; }
        public string Resolution { get; set; }

        public string ComplaintHasBeen { get; set; }
        public AccountInfoVM details { get; set; }

        //-- New columns added on 6/14/2018
        public DateTime? IssueDate { get; set; }
        public DateTime? ComplaintDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime? InitialRespDate { get; set; }
        public DateTime? WrittenRespDate { get; set; }

        public bool Call { get; set; }
        public bool Email { get; set; }
        public bool Fax { get; set; }
        public bool Mail { get; set; }
        public bool CallRcvdWebsite { get; set; }
        public bool SocialMedia { get; set; }
        public bool InsCompany { get; set; }
        public bool Survey { get; set; }
        public bool CallRcvdOther { get; set; }
        public bool ComplaintProduct { get; set; }
        public string AttachedFile { get; set; }
        //public bool ComplaintShipping { get; set; }
        //public bool ComplaintService { get; set; }
        //public bool ComplaintSmartAction { get; set; }
    }

    public class HDMSPayer
    {
        //  
        public int? payerid { get; set; }
        public string payerType { get; set; }
        public int insOrd { get; set; }

        // public short? coverage { get; set; }
        // public string phoneno { get; set; }


    }

}