using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class CSRReport
    {
        public static IList<BCNVM> GetClaimList(DateTime? _startDt, DateTime? _endDt, string option, bool others, string prod, double? contract, string serv)
        {
            IList<BCNVM> _list = new List<BCNVM>();

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                if (others != true)
                {
                    if (option == null || option == "Submit")
                    {
                        _list = (from t in _db.BCN_Claims_FTP
                                 where t.Submit_Date >= _startDt &&  
                                 t.Submit_Date < _endDt
                                 select new BCNVM
                                 {
                                     Id = t.Id,
                                     Contract = t.Contract,
                                     PlanGroup = t.PlanGroup,
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Middle = t.Middle_int,
                                     Mbr_Suffix = t.Mbr_suffix,
                                     Mbr_DOB = t.Mbr_DOB,
                                     Claim_Id = t.Claim_id,
                                     Claim_Payee = t.Claim_Payee,
                                     Clm_suffix = t.Clm_suffix,
                                     Serv_Prov = t.Serv_Prov,
                                     Serv_Prov_Name = t.Serv_Prov_Name,
                                     Bcat = t.Bcat,
                                     DOS = t.DOS,
                                     Proc_Code = t.Proc_Code,
                                     Serv_Prov_NPI = t.Serv_Prov_NPI,
                                     Payee_Prov_NPI = t.Payee_Prov_NPI,

                                     Paid_Date = t.Paid_Date,
                                     Submit_Date = t.Submit_Date,
                                     Paid = t.Paid,
                                     Copay = t.Copay,
                                     COB = t.COB,
                                     Allowed = t.Allowed,
                                     Deductible = t.Deductible,
                                     Coinsurance = t.Coinsurance,
                                     Mod1 = t.Mod1,
                                     Mod2 = t.Mod2,
                                     Mod3 = t.Mod3,
                                     Mod4 = t.Mod4,
                                     On_Off_exchange = t.On_Off_exchange,
                                     LoadDate = t.LoadDate,
                                     Units = t.Units,
                                     EX_code = t.EX_code,
                                     EX_Description = t.EX_description,
                                     Charge_Amt = t.Charge_Amt
                                 }).ToList();
                    }
                    else if (option == "Paid")
                    {
                        _list = (from t in _db.BCN_Claims_FTP
                                 where t.Paid_Date >= _startDt &&  
                                 t.Paid_Date < _endDt
                                 select new BCNVM
                                 {
                                     Id = t.Id,
                                     Contract = t.Contract,
                                     PlanGroup = t.PlanGroup,
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Middle = t.Middle_int,
                                     Mbr_Suffix = t.Mbr_suffix,
                                     Claim_Id = t.Claim_id,
                                     Claim_Payee = t.Claim_Payee,
                                     Serv_Prov = t.Serv_Prov,
                                     Serv_Prov_Name = t.Serv_Prov_Name,
                                     Bcat = t.Bcat,
                                     DOS = t.DOS,
                                     Paid_Date = t.Paid_Date,
                                     Submit_Date = t.Submit_Date,
                                     Paid = t.Paid,
                                     Copay = t.Copay,
                                     COB = t.COB,
                                     Allowed = t.Allowed,
                                     Deductible = t.Deductible,
                                     Coinsurance = t.Coinsurance,
                                     Units = t.Units,
                                     EX_code = t.EX_code,
                                     EX_Description = t.EX_description,
                                     Charge_Amt = t.Charge_Amt
                                 }).ToList();
                    }
                    else if (option == "Load")
                    {

                        _list = (from t in _db.BCN_Claims_FTP
                                 where t.LoadDate >= _startDt &&  
                                 t.LoadDate < _endDt
                                 select new BCNVM
                                 {
                                     Id = t.Id,
                                     Contract = t.Contract,
                                     PlanGroup = t.PlanGroup,
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Middle = t.Middle_int,
                                     Mbr_Suffix = t.Mbr_suffix,
                                     Claim_Id = t.Claim_id,
                                     Claim_Payee = t.Claim_Payee,
                                     Serv_Prov = t.Serv_Prov,
                                     Serv_Prov_Name = t.Serv_Prov_Name,
                                     Bcat = t.Bcat,
                                     DOS = t.DOS,
                                     Paid_Date = t.Paid_Date,
                                     Submit_Date = t.Submit_Date,
                                     Paid = t.Paid,
                                     Copay = t.Copay,
                                     COB = t.COB,
                                     Allowed = t.Allowed,
                                     Deductible = t.Deductible,
                                     Coinsurance = t.Coinsurance,
                                     Units = t.Units,
                                     EX_code = t.EX_code,
                                     EX_Description = t.EX_description,
                                     Charge_Amt = t.Charge_Amt
                                 }).ToList();
                    }

                }
                else
                {
                    if (option == null)
                    {
                        _list = (from t in _db.BCN_Claims_FTP
                                 where (prod == null || prod == "" || prod == t.Proc_Code) &&
                                  (serv == null || serv == "" || t.Serv_Prov_Name == serv) &&
                                 (contract == null || contract == 0 || contract == t.Contract)
                                 select new BCNVM
                                 {
                                     Id = t.Id,
                                     Contract = t.Contract,
                                     PlanGroup = t.PlanGroup,
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Middle = t.Middle_int,
                                     Mbr_Suffix = t.Mbr_suffix,
                                     Claim_Id = t.Claim_id,
                                     Claim_Payee = t.Claim_Payee,
                                     Serv_Prov = t.Serv_Prov,
                                     Serv_Prov_Name = t.Serv_Prov_Name,
                                     Bcat = t.Bcat,
                                     DOS = t.DOS,
                                     Paid_Date = t.Paid_Date,
                                     Submit_Date = t.Submit_Date,
                                     Paid = t.Paid,
                                     Copay = t.Copay,
                                     COB = t.COB,
                                     Allowed = t.Allowed,
                                     Deductible = t.Deductible,
                                     Coinsurance = t.Coinsurance,
                                     Units = t.Units,
                                     EX_code = t.EX_code,
                                     EX_Description = t.EX_description,
                                     Charge_Amt = t.Charge_Amt
                                 }).ToList();
                    }

                    else
                    {
                        _list = (from t in _db.BCN_Claims_FTP
                                 where (prod == null || prod == "" || prod == t.Proc_Code) &&
                                  (serv == null || serv == "" || t.Serv_Prov_Name == serv) &&
                                 (contract == null || contract == 0 || contract == t.Contract)
                                 &&(( option == "Load" && t.LoadDate >= _startDt && 
                                 t.LoadDate < _endDt) || (option == "Paid" && t.Paid_Date >= _startDt && 
                                 t.Paid_Date < _endDt) || (option == "Submit" && t.Submit_Date >= _startDt && 
                                 t.Submit_Date < _endDt))  
                                 select new BCNVM
                                 {
                                     Id = t.Id,
                                     Contract = t.Contract,
                                     PlanGroup = t.PlanGroup,
                                     First_Name = t.First_Name,
                                     Last_Name = t.Last_Name,
                                     Middle = t.Middle_int,
                                     Mbr_Suffix = t.Mbr_suffix,
                                     Claim_Id = t.Claim_id,
                                     Claim_Payee = t.Claim_Payee,
                                     Serv_Prov = t.Serv_Prov,
                                     Serv_Prov_Name = t.Serv_Prov_Name,
                                     Bcat = t.Bcat,
                                     DOS = t.DOS,
                                     Paid_Date = t.Paid_Date,
                                     Submit_Date = t.Submit_Date,
                                     Paid = t.Paid,
                                     Copay = t.Copay,
                                     COB = t.COB,
                                     Allowed = t.Allowed,
                                     Deductible = t.Deductible,
                                     Coinsurance = t.Coinsurance,
                                     Units = t.Units,
                                     EX_code = t.EX_code,
                                     EX_Description = t.EX_description,
                                     Charge_Amt = t.Charge_Amt
                                 }).ToList();
                    }
                }
            }

                return _list;
         }

        public static IList<Servlist> GetServName()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    

                    Servlist _bcn = new Servlist();

                    var _list = (from bcn in _db.BCN_Claims_FTP
                                 
                                 select new Servlist
                                 {
                                     Id =bcn.Serv_Prov_Name,
                                     name = bcn.Serv_Prov_Name
                                 }).Distinct().OrderBy(t => t.name).ToList();

                    _list.Insert(0, _bcn);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Servlist>();
            }
        }
        public static IList<CallPerPerson> GetTotalCallPerPerson( DateTime _dt)
        {
            DateTime _nxtdt = _dt.AddDays(1);
            try
            {
                using (IntranetEntities _db = new IntranetEntities())
                {

                    //DateTime today = DateTime.Today.Date;

                    var _list = (from csr in _db.tbl_CSRCallLog
                                 where csr.CreatedOn >= _dt && csr.CreatedOn <= _nxtdt
                                 group csr by new { csr.CreatedBy } into t
                                 select new CallPerPerson {
                                     Name = t.Key.CreatedBy,
                                    Count = t.Count()
                                 }
                                 ).OrderBy (t=>t.Name).ToList();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<CallPerPerson>();
            }

        }

        public static IList<AOBDetail> GetAOBDetail(DateTime _Startdt, DateTime _Enddt)
        {
           
            try
            {
                using (CallAgentDBEntitiesnew _db = new CallAgentDBEntitiesnew())
                {

                    //DateTime today = DateTime.Today.Date;

                    var _list = (from aob in _db.tbl_AOB
                                 where aob.CallTime >= _Startdt && aob.CallTime <= _Enddt
                                
                                 select new AOBDetail
                                 {
                                     Account = aob.Account,
                                     FirstName = aob.FirstName,
                                     LastName = aob.LastName,
                                     isDocSubmitted = aob.isDocSubmitted,
                                     DocSubmittedMethod = aob.DocSubmittedMethod,
                                     ResendMethod = aob.ResendMethod,
                                     ResendEmailAddress = aob.ResendEmailAddress,
                                     ResendFaxNumber = aob.ResendFaxNumber,
                                     IsMailingCorrect = aob.IsMailingCorrect,
                                     UseMailingAsPermBillingAddr = aob.UseMailingAsPermBillingAddr,
                                     MailingAddress= aob.MailingAddress_ID,
                                     CallTime = aob.CallTime,
                                     AOBSubmitDate = aob.AOBSubmitDate
                                     
                                 }
                                 ).OrderBy(t => t.CallTime).ToList();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<AOBDetail>();
            }

        }

        public static IList<CSRAssessment> GetCSRAssessment()
        {
            int?[] _IDs = 
                { 685 ,959 ,287, 713, 686, 548, 773, 616, 891, 863};
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    DateTime? _date = DateTime.Today.Date.AddDays(-21);

                    var _list = (from cli in _db.tbl_Clinical_Assessments
                                 where cli.CreateDate >= _date 
                                &&  _IDs.Contains(cli.ID_CreateBy)
                                 select new CSRAssessment
                                 {
                                     Account = cli.Account,
                                     AssessmentDate = cli.AssessmentDate,
                                     ID_CreateBy = cli.ID_CreateBy,
                                     CreateDate = cli.CreateDate

                                 }).OrderBy(t=>t.AssessmentDate).ToList();
                              
                                

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<CSRAssessment>();
            }

        }

        public static RWOAccountChangesVM GetAccountChanges( int? _acc)
        {
            RWOAccountChangesVM _vm = new RWOAccountChangesVM();

            try
            {
                IList<RWOAccountChanges> _list = new List<RWOAccountChanges>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<RWOAccountChanges>("SELECT DISTINCT mem.Account, "+
                  "  mem.First_Name, " +
                  "  mem.Middle, " +
                  "  mem.Last_name, " +
                   " mem.Last_Updated_Date AS AccountUpdated, " +
                   " op1.LegalName AS AccountUpdatedBy, " +
                   " inf.Last_Updated_Date AS AccountInfUpdated, " +
                  "  op2.LegalName AS AccountInfUpdatedBy, " +
                   " ins.Last_Updated_Date AS InsuranceUpdated, " +
                  "  op6.LegalName AS InsuranceUpdatedBy, " +
                   " pay.Name AS Payer " +
               " FROM tbl_Account_Member                  mem " +
                  "  LEFT JOIN tbl_Operator_Table        op1 ON mem.Last_Updated_User = op1.ID " +
                  "  LEFT JOIN tbl_Account_Information   inf ON inf.Account = mem.Account " +
                   " LEFT JOIN tbl_Operator_Table        op2 ON op2.ID = inf.Last_Updated_User " +
                   " LEFT JOIN tbl_Account_Insurance     ins ON ins.Account = mem.Account " +
                  "  LEFT JOIN tbl_Operator_Table        op6 ON op6.ID = ins.Last_Updated_user " +
                  "  LEFT JOIN tbl_Payer_Table           pay ON pay.id = ins.Id_Payer " +
              "  WHERE mem.Account = "+_acc+"" +
                  "  and mem.member = 1").ToList<RWOAccountChanges>();

                    _vm.Insuranceupdates = _list;
                    _vm.rwoAccountUpdates = (from lst in _list
                                             select lst).Take(1).SingleOrDefault();
                    return _vm; 
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new RWOAccountChangesVM();
            }


        }

        public static RWOAccountChangesModel GetAccountChangesInfo(DateTime? startDt, DateTime? endDt )
        {
            RWOAccountChangesModel _vm = new RWOAccountChangesModel();
            string Mstart = startDt.Value.Month.ToString();
            string Dstrat = startDt.Value.Day.ToString();
            string Ystart = startDt.Value.Year.ToString();

            string Mend = endDt.Value.Month.ToString();
            string Dend = endDt.Value.Day.ToString();
            string Yend = endDt.Value.Year.ToString();

            string stdt = Mstart + "/" + Dstrat + "/" + Ystart;
            string endt = Mend + "/" + Dend + "/" + Yend;
            try
            {
                IList<RWOAccountChanges> _list = new List<RWOAccountChanges>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                   

                    _list = _db.Database.SqlQuery<RWOAccountChanges>("SELECT DISTINCT mem.Account, " +
                  "  mem.First_Name, " +
                  "  mem.Middle, " +
                  "  mem.Last_name, " +
                   " mem.Last_Updated_Date AS AccountUpdated, " +
                   " op1.LegalName AS AccountUpdatedBy, " +
                   " inf.Last_Updated_Date AS AccountInfUpdated, " +
                  "  op2.LegalName AS AccountInfUpdatedBy, " +
                   " ins.Last_Updated_Date AS InsuranceUpdated, " +
                  "  op6.LegalName AS InsuranceUpdatedBy, " +
                   " pay.Name AS Payer " +
               " FROM tbl_Account_Member                  mem " +
                  "  LEFT JOIN tbl_Operator_Table        op1 ON mem.Last_Updated_User = op1.ID " +
                  "  LEFT JOIN tbl_Account_Information   inf ON inf.Account = mem.Account " +
                   " LEFT JOIN tbl_Operator_Table        op2 ON op2.ID = inf.Last_Updated_User " +
                   " LEFT JOIN tbl_Account_Insurance     ins ON ins.Account = mem.Account " +
                  "  LEFT JOIN tbl_Operator_Table        op6 ON op6.ID = ins.Last_Updated_user " +
                  "  LEFT JOIN tbl_Payer_Table           pay ON pay.id = ins.Id_Payer " +
              "    WHERE ins.Last_Updated_Date >= '"+stdt+"' and ins.Last_updated_Date <= '"+endt+"' and mem.member = 1").ToList<RWOAccountChanges>();

                    _vm.Insuranceupdates = _list;
             
                    return _vm;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new RWOAccountChangesModel();
            }


        }

        public static RWOChangesVM GetRWOChanges(int? _acc)
        {
            RWOChangesVM _vm = new RWOChangesVM();

            try
            {
                IList<RWOChanges> _list = new List<RWOChanges>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<RWOChanges>("SELECT "+
                      "  rwo.Account, " +
                       " pro.ProductCode, " +
                        " pro.productdescription,  " +
                       " op1.LegalName AS LastChangedBy, " +
                       " rwo.LastChange, " +
                       " op2.LegalName, " +
                       " rwo.CreateDate AS CreatedBy " +
                   " FROM " +
                    "            tbl_PS_RepeatingOrders  rwo " +
                    "    LEFT JOIN   tbl_Product_Table       pro on pro.ID = rwo.ID_Product " +
                    "    LEFT JOIN   tbl_Operator_Table      op1 on op1.id = rwo.ID_Changed " +
                     "   LEFT JOIN   tbl_Operator_Table      op2 on op2.id = ID_CreateBy " +
                    " WHERE " +
                     "   Account = "+ _acc + "").ToList<RWOChanges>();

                    _vm.rwoChanges = _list;
                    
                    return _vm;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new RWOChangesVM();
            }


        }

        public static IList<callLogReport> GetCalllogReport(DateTime _startDt, DateTime _endDt)
        {


            using (IntranetEntities _db = new IntranetEntities())
            {
                IList<callLogReport> _rec = new List<callLogReport>();
                var list = (from t in _db.tbl_CSRCallLog
                            where t.CreatedOn >= _startDt && t.CreatedOn <= _endDt && t.ComplaintOutcome.Contains("Not Resolved Transferred to Team Lead")

                            select new callLogReport
                            {
                                CreatedBy = t.CreatedBy,
                                CreatedOn = t.CreatedOn,
                                ReferenceNumber = t.id,
                                account = t.Account,
                                Note = t.OtherTxt,
                                Issue = (t.Billing == true || t.Copay == true) ? "Billing/Payments" : (t.Address == true || t.Physician == true || t.Phone == true) ? "Demographic" :
                                         (t.AOB == true || t.Prescription == true || t.CMN == true || t.PriorAuthorization == true || t.SupportingDoc ==true || t.TeacherLetter == true || t.Logs == true || t.ABN == true || t.LMN == true ) ? "Documentation" :
                                         (t.InsuarnceChanges_ == true || t.Eligibility == true ) ? "Insurance" :
                                         (t.NewAcconunt == true || t.Restart == true) ? "New Account/Restart" :
                                         (t.OrderConfirmation == true ) ? "Order Confirmation" :
                                         (t.FedExOrUSPSTracking == true || t.OrderShipped == true || t.OrderETA ==true || t.OrderHolding == true || t.RWOCreated == true) ? "Order Status" :
                                         (t.PC_IncreaseOrDecrease == true || t.PC_Hold == true || t.PC_RemoveOrAdd == true || t.ProductChange == true) ? "RWO Changes" :
                                         (t.SampleChoice == true || t.SampleTask == true) ? "Sample" :
                                         (t.DefectiveProductOrNotUsable == true || t.WrongOrExtraProductShipped == true || t.MissingProduct == true || t.Other == true  ) ? "Shipping" :
                                         (t.Damaged == true || t.WrongProductShipped == true || t.QualityOfProdut == true || t.DefectiveProductOrNotUsable == true ) ? "Product Issues" :
                                         (t.Driver == true || t.DidntFollowDelIns == true || t.WrongArea == true   ) ? "Delivery" :
                                         (t.ImpoliteORoffensive == true ) ?"Impolite CSR" :
                                         (t.VConfirmationCalls == true || t.VPaymentCalles == true || t.SAJamesPhonePromts == true || t.SAJamesSelfService == true ) ? "Victor complaint" :
                                         (t.VirtualCallBack == true ) ? "virtual CallBack" :
                                         (t.Web ==true || t.Website == true) ? "WebSite" :
                                         (t.HoldTimes == true ) ? "Hold Time " :
                                         (t.NoFollowUp == true || t.NoFollowUpWithMem == true || t.ReturnedCall_LeftVoicemail == true ) ? "No Follow UP" :
                                         (t.PhysicianIssue == true || t.Physician == true) ? "Physician Issue" :
                                         (t.NeverRecivedSupplies == true) ? "Never Received Supplies" :
                                         (t.BCNProviderIssue == true ) ? "BCN Provider Issue" :
                                         (t.Other == true) ? "Others" : "No Issue selected",
                                Resolution = t.ComplaintOutcome,
                                ComplaintRecieved = "",

                            }
                        ).ToList();

                var list1 = (from t in _db.tbl_CSRComplaintLog
                             where t.CreatedOn >= _startDt && t.CreatedOn <= _endDt

                             select new callLogReport
                             {
                                 CreatedBy = t.CreatedBy,
                                 CreatedOn = t.CreatedOn,
                                 ReferenceNumber = _db.tbl_CSRCallLog.Where(x => x.Account == t.Account && x.CreatedBy == t.CreatedBy).Count() > 0 ? _db.tbl_CSRCallLog.Where(x => x.Account == t.Account && x.CreatedBy == t.CreatedBy).OrderByDescending(x => x.CreatedOn).FirstOrDefault().id : _db.tbl_CSRCallLog.Where(x => x.Account == t.Account).OrderByDescending(x => x.CreatedOn).FirstOrDefault().id,
                                 account = t.Account,
                                 Note = t.OtherTxt,
                                 Payer = t.PayerType,
                                 Issue = (t.Damaged == true) ? "Damaged" : (t.Driver == true) ? "Driver" :
                                         (t.WrongProductShipped == true) ? "Wrong Product Shipped" :
                                         (t.QualityOfProdut == true) ? "Quanlity of Product" : (t.WrongArea == true) ? "Wrong Area" :
                                         (t.MissingProduct == true) ? "Missing Product" : (t.ProductIncrease == true) ? "Product Increase" :
                                         (t.ProductMispick == true) ? "Product Mispick" : (t.ProductDefective == true) ? "Product Defective" :
                                         (t.ImpoliteORoffensive == true) ? "Impolite or Offensive" : (t.HoldTimes == true) ? "HoldTime" :
                                         (t.Other == true) ? "Other" : (t.BCNProviderIssue == true) ? "BCN Provider Issue" :
                                          (t.InsLimitGuidelines == true) ? "Ins Limit Guidelines" : (t.PhysicianIssue == true) ? "Physician Issue" :
                                          (t.NeverRecivedSupplies == true) ? "Supplies Never Recived" : (t.NoFollowUpWithMem == true) ? "No FollowUp With Mem" :
                                            (t.ReturnedFromVM == true) ? "Returned From VM" : (t.NoFollowUp == true) ? "No Follow Up" :
                                             (t.Website == true) ? "Website" : (t.VirtualCallBack == true) ? "Virtual Call Back" :
                                           (t.SAJamesPhonePromts == true) ? "SA James PhonePromts" : (t.SAJamesSelfService == true) ? "SA James SelfService" :
                                       (t.VPaymentCalles == true) ? "Victor Payment Call" : (t.VConfirmationCalls == true) ? "Victor Confirmation Call" :
                                       (t.DidntFollowDelIns == true) ? "Not follow delivery instruction" : "Issue not listed",
                                 Resolution = t.ComplaintHasBeen,
                                 ComplaintRecieved = (t.SocialMedia == true) ? "Social Medial" : t.Call == true ? "Call" : t.Email == true ? "Email" : t.Fax == true ? "Fax" : t.Website == true ? "WebSite" : t.InsCompany == true ? "Insurance Complany" : t.Other == true ? t.OtherTxt : "No Issue selected"

                             }
                            ).ToList();
                _rec = list;
                foreach (var item in list1)
                {
                    callLogReport Rec = new callLogReport();
                    Rec.CreatedBy = item.CreatedBy;
                    Rec.ReferenceNumber = item.ReferenceNumber;
                    Rec.CreatedOn = item.CreatedOn;
                    Rec.account = item.account;
                    Rec.Note = item.Note;
                    Rec.Payer = item.Payer;
                    Rec.Resolution = item.Resolution;
                    Rec.Issue = item.Issue;
                    Rec.ComplaintRecieved = item.ComplaintRecieved;
                    _rec.Add(Rec);
                }
                return _rec;
            }
            //   return _rec;
        }

        public static IList<pumpsHoldvm> GetHoldReports()
        {

            IList<pumpsHoldvm> list = new List<pumpsHoldvm>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = "Select DISTINCT t1.ID, t1.Account,t1.Request_Date, HoldFromShippingReason,t3.ID_Payer,t3.PayerName from HHSQLDB.dbo.tbl_PS_WorkOrder t1 "+
 " inner join HHSQLDB.dbo.tbl_PS_WorkOrderLine t2 on t1.ID = t2.ID_PS_WorkOrder "+
  " inner join HHSQLDB.dbo.v__AccountMemberEffectiveInsurance_Ins1 t3 on t1.Account = t3.Account "+
"  where t1.HoldFromShipping = 1 and t1.LastPrintDate is null and t1.Cancel_Date is null AND t2.ID_Product in   "+
"  ( "+
 " Select pro1.ID from HHSQLDB.dbo.tbl_Product_Table pro1 "+
 " join HHSQLDB.dbo.tbl_ProductCategory_Table cat  on cat.id = pro1.ID_ProductCategory "+
 " where cat.id in (55,68, 69,72,88,91,92, 94) )and t1.ID > 5000418";
                _db.Database.CommandTimeout = 0;
                list = _db.Database.SqlQuery<pumpsHoldvm>(query).ToList<pumpsHoldvm>();

                return list;
            }
            //   return _rec;
        }

        public static IList<pumpsHoldvm> GetSuperiorHoldReports()
        {

            IList<pumpsHoldvm> list = new List<pumpsHoldvm>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = " select  distinct wos.Account, wos.ID,  wos.HoldFromShippingReason , wos.Request_Date from tbl_PS_WorkOrder wos "+
" join tbl_Account_Insurance ins on wos.Account = ins.Account "+
" where wos.Cancel_Date is null and wos.LastPrintDate is null "+
 " and ins.ID_Payer = 4490 "+
 " and(ins.Effective_Date is null or ins.Effective_Date <= wos.Request_Date) "+
 " and(ins.Expiration_Date is null or ins.Expiration_Date >= wos.Request_Date) "+
 " and wos.HoldFromShipping = 1";
                _db.Database.CommandTimeout = 0;
                list = _db.Database.SqlQuery<pumpsHoldvm>(query).ToList<pumpsHoldvm>();

                return list;
            }
            //   return _rec;
        }


        public static IList<HeldOrdersVM> GetDiabOsUroHeldOrders()
        {

            IList<HeldOrdersVM> _list = new List<HeldOrdersVM>();
            using (ReportsEntities _db = new ReportsEntities())
            {
                              
               _list  = _db.Database.SqlQuery<HeldOrdersVM>("exec sp_DiabOsUroHolds").ToList<HeldOrdersVM>();



            }
            return _list;
        }
    }

    public class CallPerPerson
    {
        public int? Count { get; set; }
        public string Name { get; set; }
    }

    public class AOBDetail
    {
        public int? Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? isDocSubmitted { get; set; }
        public string DocSubmittedMethod { get; set; }
        public string ResendMethod { get; set; }
        public string ResendEmailAddress { get; set; }

        public string ResendFaxNumber { get; set; }
        public bool? IsMailingCorrect { get; set; }
        public bool? UseMailingAsPermBillingAddr { get; set; }
        public int? MailingAddress { get; set; }
        public DateTime? CallTime { get; set; }
        public DateTime? AOBSubmitDate { get; set; }
    }


    public class CSRReportModel
    {
        public DateTime date { get; set; }
        public IList<CallPerPerson> callPerPeroson { get; set; }
    }

    public class AOBReportModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<AOBDetail> AOBDetailList { get; set; }
    }




    public class CSRAssessment
    {
        public int Account { get; set; }
        public DateTime? AssessmentDate { get; set; }

        public int? ID_CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class CSRAssessmentVM
    {
      public IList<CSRAssessment> csrAssessment { get; set; }

    }

    public class RWOAccountChangesVM
    {
        public int? Account { get; set; }
        public IList<RWOAccountChanges>Insuranceupdates  { get; set; }

        public RWOAccountChanges rwoAccountUpdates  { get; set; }
    }


    public class RWOAccountChangesModel
    {
        public DateTime? startDt { get; set; }

        public DateTime? endDt { get; set; }
        public IList<RWOAccountChanges> Insuranceupdates { get; set; }

     
    }


    public class RWOAccountChanges
    {
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Middle { get; set; }
        public string Last_name { get; set; }
        public DateTime? AccountUpdated { get; set; }
        public string AccountUpdatedBy { get; set; }
        public DateTime? AccountInfUpdated { get; set; }
        public string AccountInfUpdatedBy { get; set; }
        public DateTime? InsuranceUpdated { get; set; }
        public string InsuranceUpdatedBy { get; set; }
        public string Payer { get; set; }
      
    }

    public class RWOChangesVM
    {
        public int? Account { get; set; }
        public IList<RWOChanges> rwoChanges { get; set; }

       
    }

    public class RWOChanges
    {
        public int Account { get; set; }
        public string ProductCode { get; set; }
      
    
        public string productdescription { get; set; }
     
        public string LastChangedBy { get; set; }
        public DateTime? LastChange { get; set; }
        public string LegalName { get; set; }
        public DateTime? CreatedBy { get; set; }

    }

    public class callLogReport
    {
        public int? ReferenceNumber { get; set; }
        public int? account { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string Name { get; set; }
        public string Relation { get; set; }

        public string Issue { get; set; }
        public string Payer { get; set; }
        public string PrimaryPayer
        {
            get
            {
                string output = "";
                if (this.Payer != null)
                {
                    if (this.Payer.Trim().Split(',').Count() > 0)
                    {
                        output = this.Payer.Trim().Split(',')[0];
                    }
                    else
                    {
                        output = this.Payer;
                    }
                    
                }
                return output;
            }
                
        }
        public string SecondaryPayer
        {
            get
            {
                string output = "";
                if (this.Payer != null)
                {
                    if (this.Payer.Trim().Split(',').Count() > 1)
                    {
                        output = this.Payer.Trim().Split(',')[1];
                    }

                }
                return output;
            }

        }
        public string TeritiaryPayer
        {
            get
            {
                string output = "";
                if (this.Payer != null)
                {
                    if (this.Payer.Trim().Split(',').Count() >2)
                    {
                        output = this.Payer.Trim().Split(',')[2];
                    }

                }
                return output;
            }

        }
        public string Resolution { get; set; }

        public string ComplaintOutcome { get; set; }
        public string Note { get; set; }
        public string ComplaintRecieved { get; set; }
    }


    public class SpecialHoldsVM
    {
        public IList<pumpsHoldvm> holdlist { get; set; }
    }

    public class pumpsHoldvm {
        public int ID { get; set; }
        public int? Account { get; set; }
        public string HoldFromShippingReason { get; set; }
        public int? ID_Payer { get; set; }
        public string PayerName { get; set; }
        public DateTime? Request_Date { get; set; }

    }
    public class callLogReportVM
    {
        public IList<callLogReport> records { get; set; }
       public DateTime startDt { get; set; }
        public DateTime endDt { get; set; }
        public string ComplaintOutcome { get; set; }
    }

    public class Servlist
    {
        public string name { get; set; }
        public string Id { get; set; }
    }

    public class BCNClaims
    {
        public DateTime? _startDt { get; set; }
        public DateTime? _endDt { get; set; }
      
        public string chk { get; set; }
        public bool others { get; set; }
        public string prod { get; set; }
        public double? contract { get; set; }
        public string ServName { get; set; }
        public IList<BCNVM> BCNClaimList { get; set; }
    }
    public class BCNVM
    {
        public Int32 Id { get; set; }
        public double? Contract { get; set; }
        public double? Clm_suffix { get; set; }
        public double? Serv_Prov_NPI { get; set; }
        public double? Payee_Prov_NPI { get; set; }
        public double? PlanGroup { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }

        public string Middle { get; set; }
        public double? Mbr_Suffix { get; set; }
        public string Claim_Id { get; set; }
        public string Claim_Payee { get; set; }
        public string Serv_Prov { get; set; }
        public string Proc_Code { get; set; }
        public string Mod1 { get; set; }
        public string Mod2 { get; set; }
        public string Mod3 { get; set; }
        public string Mod4 { get; set; }
        public string On_Off_exchange { get; set; }
        public string Serv_Prov_Name { get; set; }
        public string Bcat { get; set; }
        public DateTime? DOS { get; set; }
        public DateTime? LoadDate { get; set; }
        public DateTime? Mbr_DOB { get; set; }
        public DateTime? Paid_Date { get; set; }
        public DateTime? Submit_Date { get; set; }
        public double? Paid { get; set; }
        public double? Copay { get; set; }

        public double? COB { get; set; }
        public double? Allowed { get; set; }
        public double? Deductible { get; set; }
        public double? Coinsurance { get; set; }
        public double? Units { get; set; }
        public string EX_code { get; set; }
        public string EX_Description { get; set; }
        public double? Charge_Amt { get; set; }

    }

}