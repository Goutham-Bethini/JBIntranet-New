using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.ComponentModel.DataAnnotations;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Web.Mvc;
using System.ComponentModel;
using System.Net.Mail;

namespace USPS_Report.Areas.Reports.Models
{
    public class AOBGenerator
    {
        public static AccountDetailsVM GetAccountDetails(Int64? Account)
        {
            AccountDetailsVM tableRec = new AccountDetailsVM();
            Int64 account;
            account = Convert.ToInt64(Account);
            DateTime today = DateTime.Now;

            int[] THC = { 4510, 3100, 4446, 4511, 4346 };
            int[] BCN = { 4344, 3679 };

            int[] IN = { 3179 };
            int[] WI = { 3739 };
            int[] KS = { 4122, 4212 };
            int[] AH = { 4176, 4123, 4336 };


            int[] policyTHC = { 4510, 4511, 4446 };


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                //  tbl_Account_Member _list = new tbl_Account_Member();

                // _list = _db.tbl_Account_Member.Where(t => t.Account == account && t.Member == 1).SingleOrDefault();



                var _list = (from mem in _db.tbl_Account_Member
                             where mem.Account == account && mem.Member == 1
                             select new
                             {
                                 mem.First_Name,
                                 mem.Last_Name,
                                 mem.Address_1,
                                 mem.Address_2,
                                 mem.City,
                                 mem.State,
                                 mem.Zip,
                                 mem.Phone,
                                 mem.BirthDate,
                                 mem.Sex,
                                 mem.EmailAddress,
                                 
                               
                                 physicianFN = mem.ID_Default_Referring_Doctor != null ? (from rs in _db.tbl_Referral_Source_Table
                                                                                          where mem.ID_Default_Referring_Doctor == rs.ID
                                                                                          select rs.First_Name).FirstOrDefault() : String.Empty,
                                 physicianLN = mem.ID_Default_Referring_Doctor != null ? (from rs in _db.tbl_Referral_Source_Table
                                                                                          where mem.ID_Default_Referring_Doctor == rs.ID
                                                                                          select rs.Last_Name).Take(1).FirstOrDefault() : String.Empty,
                             }).SingleOrDefault();

                //var _payerId = (from pay in _db.v__AccountMemberEffectiveInsurance_Ins1
                //                where pay.Account == account
                //                select new AccountDetailsVM
                //                {
                //                    PayerType = pay.PayerName
                //                }).Take(1).SingleOrDefault();


                //var _payerId = (from ins in _db.tbl_Account_Insurance

                //                join pt in _db.tbl_Payer_Table
                //                on ins.ID_Payer equals pt.ID
                //                where ins.Account == Account &&
                //               ((ins.Expiration_Date == null || ins.Expiration_Date > today)
                //                   && (ins.Effective_Date == null || ins.Effective_Date < today))
                //                where ins.CoverageMet != 100
                //                select new PayerNameVm
                //                {
                //                    payerid = ins.ID_Payer,
                //                    PayerName = pt.Name,

                //                }).Take(1).SingleOrDefault();
                //.Distinct().OrderByDescending(t => t.PayerName).ToList();



                var _payerId = _db.Database.SqlQuery<PayerNameVm>("select pay.payerName,pay.contactPhone,ins.ID_Payer, pt.Name,cast (mem.Sequence_Number as int) as Sequence_Number,  mem.Policy_Number  from tbl_Account_Insurance ins join  " +
"tbl_Account_Member_Insurance mem on ins.Account = mem.Account and ins.Entry_Number = mem.Entry_Number " +

 "join " +
 " tbl_Payer_Table pt on ins.ID_Payer = pt.id " +
  " left join[Intranet].[dbo].[AOB_Payer_Contact_Info] pay on pay.payerID = ins.ID_Payer " +
 " where((ins.Expiration_Date is null or ins.Expiration_Date > GETDATE()) and(ins.Effective_Date is null or ins.Effective_Date < GETDATE())) " +
  " and ins.account =" + account.ToString() +
  " ORDER BY  mem.Sequence_Number ASC").ToList<PayerNameVm>();

                PayerNameVm pt = new PayerNameVm();

                if (_list != null)
                {
                    tableRec.firstName = _list.First_Name + " " + _list.Last_Name;
                     tableRec.lastName = _list.Last_Name + ", " + _list.First_Name;
                    tableRec.address1 = _list.Address_1;
                    tableRec.address2 = _list.Address_2;
                    tableRec.city = _list.City;
                    tableRec.state = _list.State;
                    tableRec.zipcode = _list.Zip;
                    tableRec.gender = _list.Sex;
                    tableRec.Email = _list.EmailAddress != null ?( _list.EmailAddress.Contains("@")?(_list.EmailAddress.Contains(".")? _list.EmailAddress : ""): ""): "";
                    tableRec.phone = _list.Phone;
                    tableRec.DOB = _list.BirthDate;
                 
                    tableRec.PhysicianName = _list.physicianFN + " " + _list.physicianLN;

                    var _thc = (from t in _payerId where THC.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();
                    var _bcn = (from t in _payerId where BCN.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();

                    var _in = (from t in _payerId where IN.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();
                    var _wi = (from t in _payerId where WI.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();
                    var _keystone = (from t in _payerId where KS.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();
                    var _amrihealth = (from t in _payerId where AH.Contains(t.ID_Payer) select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone, Sequence_Number = t.Sequence_Number }).ToList();



                    var firstPreferList = new List<PayerNameVm>(_thc.Count +
                                     _bcn.Count);
                    firstPreferList.AddRange(_thc);
                    firstPreferList.AddRange(_bcn);

                    firstPreferList.OrderBy(t => t.Sequence_Number);

                    var secPreferList = new List<PayerNameVm>(_in.Count +
                                   _wi.Count +
                                   _keystone.Count +
                                   _amrihealth.Count);
                    secPreferList.AddRange(_in);
                    secPreferList.AddRange(_wi);
                    secPreferList.AddRange(_keystone);
                    secPreferList.AddRange(_amrihealth);

                    secPreferList.OrderBy(t => t.Sequence_Number);

                    if (firstPreferList.Count != 0)
                    {
                        var selectfromfirstlistPayer = (from t in firstPreferList
                                                        select t).Take(1).SingleOrDefault();
                        tableRec.PayerType = selectfromfirstlistPayer.Name;
                        tableRec.Payerid = selectfromfirstlistPayer.ID_Payer;
                        tableRec.payername = selectfromfirstlistPayer.payerName;
                        tableRec.contactpayer = selectfromfirstlistPayer.contactPhone;

                    }
                    else if (secPreferList.Count != 0)
                    {
                        var selectfromSeclistPayer = (from t in secPreferList
                                                      select t).Take(1).SingleOrDefault();
                        tableRec.PayerType = selectfromSeclistPayer.Name;
                        tableRec.Payerid = selectfromSeclistPayer.ID_Payer;
                        tableRec.payername = selectfromSeclistPayer.payerName;
                        tableRec.contactpayer = selectfromSeclistPayer.contactPhone;
                    }
                    else
                    {
                        //  var _payerListwithSeqNoDesc = (from t in _payerId  select t).ToList().OrderByDescending(t => t.Sequence_Number);

                        var _highestSeqNum = (from t in _payerId
                                              select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone }).Take(1).SingleOrDefault();

                        if (_highestSeqNum != null)
                        {
                            tableRec.PayerType = _highestSeqNum.Name;
                            tableRec.Payerid = _highestSeqNum.ID_Payer;
                            tableRec.payername = _highestSeqNum.payerName;
                            tableRec.contactpayer = "(1800) 737-0045";
                        }
                        else
                        {
                            tableRec.PayerType = "No Payer in Account";
                            tableRec.Payerid = 0;
                            tableRec.payername = "";
                            tableRec.contactpayer = "(1800) 737-0045";
                        }
                    }


                    // to get the right policy number
                    var _policyNum = (from t in _payerId where policyTHC.Contains(t.ID_Payer) select t.Policy_Number).Take(1).SingleOrDefault();

                    if (_policyNum != null)
                        tableRec.PolicyNumber = _policyNum;
                    else
                        tableRec.PolicyNumber = "No Policy Number";

                    var _subscribeNamePCP = (from t in _payerId select t).OrderByDescending(t => t.Sequence_Number).Take(1).SingleOrDefault();

                    if (_subscribeNamePCP != null)
                        tableRec.SubsNumberPCP = _subscribeNamePCP.Policy_Number;
                    else
                        tableRec.SubsNumberPCP = "No Subscriber ID";
                    //var chkpayer3679 = (from t in _payerId where t.ID_Payer == 3679 select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone }).Take(1).SingleOrDefault();
                    //var chkpayer4344 = (from t in _payerId where t.ID_Payer == 4344 select new PayerNameVm {ID_Payer =  t.ID_Payer, Name= t.Name, payerName = t.payerName, contactPhone  = t.contactPhone }).Take(1).SingleOrDefault();
                    //var chkpayer4345 = (from t in _payerId where t.ID_Payer == 4345 select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone }).Take(1).SingleOrDefault();

                    //var BCNPayer = (from t in _payerId where t.ID_Payer == 3679 || t.ID_Payer == 4344 || t.ID_Payer == 4345 select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone }).Take(1).SingleOrDefault();


                    //var lowestCov = (from t in _payerId select new PayerNameVm { ID_Payer = t.ID_Payer, Name = t.Name, payerName = t.payerName, contactPhone = t.contactPhone }).Take(1).SingleOrDefault();

                    //if (BCNPayer != null)
                    //{
                    //    tableRec.PayerType = BCNPayer.Name;
                    //    tableRec.Payerid = BCNPayer.ID_Payer;
                    //    tableRec.payername = BCNPayer.payerName;
                    //    tableRec.contactpayer = BCNPayer.contactPhone;
                    //}

                    //else if (lowestCov != null)
                    //{
                    //    tableRec.PayerType = lowestCov.Name;
                    //    tableRec.Payerid = lowestCov.ID_Payer;
                    //    tableRec.payername = lowestCov.payerName;
                    //    tableRec.contactpayer = lowestCov.contactPhone;

                    //}


                    tableRec.Account2 = Convert.ToInt32(account);
                    tableRec.PrimaryPayer = (from v in _db.v__AccountMemberEffectiveInsurance_Ins1
                                             where v.Account == account
                                             select v.PayerName).FirstOrDefault();

                    tableRec.RefNo = (from n in _db.tbl_Account_Information
                                      where n.Account == account
                                      select n.Reference).FirstOrDefault();



                }
                else
                {
                    tableRec.firstName = "WrongAccount";
                }




            }
            return tableRec;
        }

        public static IList<prolist> GetItems(Int64? account)
        {

            IList<prolist> prodlist = new List<prolist>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                DateTime? dt = _db.tbl_PS_RepeatingOrders.Where(t => t.Account == account).OrderBy(t => t.NextRepeatDate).Select(t => t.NextRepeatDate).Take(1).FirstOrDefault();
                prodlist = (from t in _db.tbl_PS_RepeatingOrders
                            join p in _db.tbl_Product_Table on t.ID_Product equals p.ID
                            where t.NextRepeatDate == dt && t.Account == account
                            select new prolist
                            {
                                Prod = p.ProductDescription,
                                qty = t.Qty,
                            }).ToList();

            }


            return prodlist;
        }

        public static IList<prolist> GetAllowableAmount(Int64? account)
        {
            DateTime today = DateTime.Today.Date;
            IList<prolist> prodlist = new List<prolist>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                DateTime? dt = _db.tbl_PS_RepeatingOrders.Where(t => t.Account == account).OrderBy(t => t.NextRepeatDate).Select(t => t.NextRepeatDate).Take(1).FirstOrDefault();
                prodlist = (from p in _db.tbl_PS_RepeatingOrders
                            join p1 in _db.tbl_Account_Insurance
                            on p.Account equals p1.Account
                            join p2 in _db.tbl_Payer_Table
                            on p1.ID_Payer equals p2.ID

                            join p3 in _db.tbl_Product_Table

                            on p.ID_Product equals p3.ID
                            join inv in _db.tbl_Inv_UOM_Table on p3.ID_UOM equals inv.ID
                            join cat in _db.tbl_ProductCategory_Table
                            on p3.ID_ProductCategory equals cat.ID
                            from p6 in _db.tbl_Procedure_Groups_Table
                            where p.Account == account && p.Member == 1 &&
                            (p.StartDate == null || p.StartDate <= DateTime.Now) &&
                            (p.EndDate == null || p.EndDate > DateTime.Now) && p6.ID_Billing_Code == p3.ID_BillingCode
                            && p6.ID_Group_No == p2.ID_Procedure_Group && (p1.Expiration_Date == null || p1.Expiration_Date > today) &&
                                        (p1.Effective_Date == null || p1.Effective_Date < today)
                            select new prolist
                            {


                                Prod = p3.ProductDescription,
                              
                                qty = p.Qty,
                                UOM = inv.UOMName,
                                qtycal = p.Qty,

                                UnitPrice = (from t in _db.tbl_Allowable_Amounts
                                             where t.ID_AllowableSet == p2.ID_Allowable &&
                                     t.Procedure_Code == p6.Procedure_Code
                                             orderby t.AsOfDateOfService descending
                                             select t.Purchase).Take(1).FirstOrDefault(),
                                // Date = tbl.AsOfDateOfService,
                              
                                perQty = p3.PerUnitQty,
                                Multiplier = p6.Multiplier,

                            }
                       ).Distinct().ToList();

            }


            return prodlist;
        }

        public static IList<PayerNameVm> GetPayerType(Int64? Account)
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    PayerNameVm pn = new PayerNameVm();
                    DateTime today = DateTime.Today;
                    var _list = (from ins in _db.tbl_Account_Insurance
                                 join pt in _db.tbl_Payer_Table
                                 on ins.ID_Payer equals pt.ID
                                 where ins.Account == Account &&
    ((ins.Expiration_Date == null || ins.Expiration_Date > today)
        && (ins.Effective_Date == null || ins.Effective_Date < today))
                                 where ins.CoverageMet != 100
                                 select new PayerNameVm
                                 {
                                     ID_Payer = ins.ID_Payer,
                                     Name = pt.Name,
                                     //coverage = ins.CoverageMet,
                                     // phoneno = pt.Phone_Number

                                 }).Distinct().OrderByDescending(t => t.Name).ToList();

                    _list.Insert(0, pn);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<PayerNameVm>();
            }
        }


        public static bool CheckIfBCN(Int64? Account)
        {
            bool IsBCN = false;
            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    DateTime today = DateTime.Today;
                    var _list = (from ins in _db.tbl_Account_Insurance
                                 join pt in _db.tbl_Payer_Table
                                 on ins.ID_Payer equals pt.ID
                                 where ins.Account == Account &&
    ((ins.Expiration_Date == null || ins.Expiration_Date > today)
        && (ins.Effective_Date == null || ins.Effective_Date <= today)) && (ins.ID_Payer == 4345 || ins.ID_Payer == 3679)

                                 select new
                                 {
                                     ins.ID_Payer,
                                 }).Distinct().ToList();

                    if (_list.Count() > 0)
                    {
                        IsBCN = true;
                    }

                    return IsBCN;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return IsBCN;
            }
        }

        public static bool CheckIfMAPPO(Int64? Account)
        {
            bool IsBCN = false;
            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    DateTime today = DateTime.Today;
                    var _list = (from ins in _db.tbl_Account_Insurance
                                 join pt in _db.tbl_Payer_Table
                                 on ins.ID_Payer equals pt.ID
                                 where ins.Account == Account &&
    ((ins.Expiration_Date == null || ins.Expiration_Date > today)
        && (ins.Effective_Date == null || ins.Effective_Date <= today)) && (ins.ID_Payer == 3148  )

                                 select new
                                 {
                                     ins.ID_Payer,
                                 }).Distinct().ToList();

                    if (_list.Count() > 0)
                    {
                        IsBCN = true;
                    }

                    return IsBCN;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return IsBCN;
            }
        }

        //public static void sendEmail()
        //{

        //    DateTime todaydate = DateTime.Today.Date;
        //    int? total = 0;

        //    DateTime? todayDate = DateTime.Now;


        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        //    mail.From = new MailAddress("geeta.arora2006@gmail.com");
        //    mail.To.Add("grani@jandbmedical.com");
        //    //  mail.To.Add("satmakuri@jandbmedical.com");
        //    mail.Subject = "WareHouse Report";
        //  //  mail.Attachments();
        //    mail.Body += " <html>";
        //    mail.Body += "<body>";
        //    mail.Body += "<table>";
        //    mail.Body += "<tr>";
        //    mail.Body += "<td>Order Completed for " + todayDate + ".</td><td></td>";
        //    mail.Body += "</tr>";

        //    mail.Body += "<tr>";
        //    mail.Body += "<td> <b>" + total + "</b> Workorders has been removed manually.</td><td></td>";
        //    mail.Body += "</tr>";

        //    mail.Body += "<tr>";
        //    mail.Body += "<td>Workorders are :</td><td> </td>";
        //    mail.Body += "</tr>";




        //    mail.Body += "<tr>";
        //    mail.Body += "<td>This is test Email.</td><td></td>";
        //    mail.Body += "</tr>";

        //    mail.Body += "</table>";
        //    mail.Body += "</body>";
        //    mail.Body += "</html>";
        //    mail.IsBodyHtml = true;
        //    SmtpServer.Port = 587;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("geeta.arora2006@gmail.com", "GEETUgeet1");
        //    SmtpServer.EnableSsl = true;
        //    SmtpServer.Send(mail);
        //}
         
    }
    public class prolist
    {
        public string UOM { get; set; }
        public bool include { get; set; }
        public string Prod { get; set; }
        public int? qty { get; set; }
        public int? qtycal { get; set; }
        public Int16? perQty { get; set; }
        public double? Multiplier { get; set; }
       
        public decimal? UnitPrice { get; set; }


        [Required(ErrorMessage = "Insurance Rate is required")]
        //   [RegularExpression("^[0-9,.]*$", ErrorMessage = "Insurance rate must be numeric of length 9 or less, example $00,000.00")]
        [RegularExpression(@"^\-?\(?\$?\s*\-?\s*\(?(((\d{1,3}((\,\d{3})*|\d*))?(\.\d{1,4})?)|((\d{1,3}((\,\d{3})*|\d*))(\.\d{0,4})?))\)?", ErrorMessage = "Insurance Rate must be numeric of length 9 or less and should have proper delimated comman, example $100,000.00")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "Insurance rate should be of length 9 or less")]
        public string insRt { get; set; }
         

        [Required(ErrorMessage = "Amount Rate is required")]
        //  [RegularExpression("^[0-9,.]*$", ErrorMessage = "Amount Rate must be numeric of length 9 or less, example $100,000.00")]
        [RegularExpression(@"^\-?\(?\$?\s*\-?\s*\(?(((\d{1,3}((\,\d{3})*|\d*))?(\.\d{1,4})?)|((\d{1,3}((\,\d{3})*|\d*))(\.\d{0,4})?))\)?", ErrorMessage = "Amount Rate must be numeric of length 9 or less and should have proper delimated comman, example $100,000.00")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "Amount Rate should be of length 9 or less")]
        public string amt { get; set; }
    }
    public class GeneratorModel
    {
        public bool? IsPage { get; set; }
        public IList<prolist> prodlist { get; set; }

        [Required(ErrorMessage = "Co-Insurance is required")]
        //  [RegularExpression("^[0-9,.]*$", ErrorMessage = "Amount Rate must be numeric of length 9 or less, example $100,000.00")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Co-Insurance must be numeric of length 2 or less")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "Co-Insurance should be of length 2 or less")]
        public string Coins { get; set; }

        [Required(ErrorMessage = "Deductible is required")]
        //  [RegularExpression("^[0-9,.]*$", ErrorMessage = "Amount Rate must be numeric of length 9 or less, example $100,000.00")]
        [RegularExpression(@"^\-?\(?\$?\s*\-?\s*\(?(((\d{1,3}((\,\d{3})*|\d*))?(\.\d{1,4})?)|((\d{1,3}((\,\d{3})*|\d*))(\.\d{0,4})?))\)?", ErrorMessage = "Deductible must be numeric of length 9 or less and should have proper delimated comman, example $100,000.00")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "Deductible should be of length 9 or less")]

        public string Deduc { get; set; }

        [Required(ErrorMessage = "Amount Due is required")]
        //  [RegularExpression("^[0-9,.]*$", ErrorMessage = "Amount Rate must be numeric of length 9 or less, example $100,000.00")]
        [RegularExpression(@"^\-?\(?\$?\s*\-?\s*\(?(((\d{1,3}((\,\d{3})*|\d*))?(\.\d{1,4})?)|((\d{1,3}((\,\d{3})*|\d*))(\.\d{0,4})?))\)?", ErrorMessage = "Amount Due must be numeric of length 9 or less and should have proper delimated comman, example $100,000.00")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "Amount Due should be of length 9 or less")]

        public string AmountDue { get; set; }

        public DateTime RXDate { get; set; }
        public string PhysicianTimesTesting { get; set; }
        public string SecInsAllowedAmt { get; set; }
        public string PriInsCovAmt { get; set; }
        public string MemOwedAmt { get; set; }
        public string testName { get; set; }
        public Int32? testid { get; set; }
        public bool English { get; set; }
        public bool spanish { get; set; }
        public bool chinese { get; set; }
        public bool russian { get; set; }
        public bool armenian { get; set; }
        public bool arabic { get; set; }

        // public int? payerid { get; set; }
        // public SelectList payerType { get; set; }

        [Required]
        public string Reason { get; set; }
        [Required(ErrorMessage = "Please enter investigation results")]
        public string CRL_Text1 { get; set; }

        [Required(ErrorMessage = "Please enter what Primary Insurnace allows")]
        public string PriInsAllow1 { get; set; }

        [Required(ErrorMessage = "Please enter what Secondary Insurnace allows")]
        public string SecInsAllow1 { get; set; }
        

 
        [Required(ErrorMessage = "Please enter medical supplies Dr has ordered")]
        public string OrderedSupplies { get; set; }


        [Required(ErrorMessage = "Please enter Secondary Insurnace name")]
        public string SecInsName { get; set; }

        [Required(ErrorMessage = "Please enter estimate cost")]
        public string EstimateCost { get; set; }

        [Required(ErrorMessage = "Resolution text required")]
        public string CRL_Text2 { get; set; }
        [Required(ErrorMessage = "Contact person text required")]
        public string CRL_Text3 { get; set; }
        public bool OpPermission { get; set; }
        //[Required(ErrorMessage = "Please enter text ")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Text should be length 100 or less words")]
        public string OffLableText { get; set; }

        [Required]
        public Int32? TestStripsFrom { get; set; }
        [Required]
        public Int32? TestStripsTo { get; set; }
        [Required]
        public Int32? LancetsFrom { get; set; }
        [Required]
        public Int32? LancetsTo { get; set; }
        [Required]
        public Int32? InfusionSetsFrom { get; set; }
        [Required]
        public Int32? InfusionSetsTo { get; set; }
        [Required]
        public Int32? PodsFrom { get; set; }
        [Required]
        public Int32? PodsTo { get; set; }
        [Required]
        public Int32? ReservoirsFrom { get; set; }
        [Required]
        public Int32? ReservoirsTo { get; set; }

        [Required]
        public Int32? SyringesFrom { get; set; }
        [Required]
        public Int32? SyringesTo { get; set; }
        [Required]
        public Int32? PenNeedlesFrom { get; set; }
        [Required]
        public Int32? PenNeedlesTo { get; set; }


        [Required]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public Int64? Account { get; set; }
        [Required]
        public string Option { get; set; }
        public AccountDetailsVM details { get; set; }

        public bool? FileExists { get; set; }

        public bool FileFax { get; set; }

        [Required(ErrorMessage = "Fax field is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Fax number must be numeric of length 11 and the first numeric should be 1, example 18007370012")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Fax number should be of length 11")]
        public string FedEx { get; set; }

        public bool FileEmail { get; set; }


        [Required(ErrorMessage = "Email field is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public bool IsChineseFormAvail { get; set; }
        public bool IsRussianFormAvail { get; set; }

        public bool IsArmenianFormAvail { get; set; }
        public bool IsArabicFormAvail { get; set; }
        //[Required(ErrorMessage = "Dateiled Receipt Date for Copay Collected is required.")]
        //[RegularExpression("~(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)d~")]
        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "Detailed Reciept date field is required")]
        public string DetailedReceiptDate { get; set; }
        [Required(ErrorMessage = "Date of Service /Order is required")]
        public string DateOfService { get; set; }
        public string OtherLanguage { get; set; }
        public bool NeedBrailleLetter { get; set; }
        public string EmailSuccessMsg { get; set; }

    }


    public class Uro_InconItems
    {
       
        public string ProdDes { get; set; }
        public string UOM { get; set; }
        public int? Qty { get; set; }
    }

    public class PayerNameVm
    {
        public int ID_Payer { get; set; }
        public int Sequence_Number { get; set; }
        public string Name { get; set; }

        public string payerName { get; set; }
        public string contactPhone { get; set; }

        public string Policy_Number { get; set; }

        // public short? coverage { get; set; }
        // public string phoneno { get; set; }


    }

    public class AccountDetailsVM
    {
        public string SubsNumberPCP { get; set; }
        public string PolicyNumber { get; set; }
        public string PhysicianName { get; set; }
        [Required]
        public int? Account2 { get; set; }
        public string TimerVal { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }

        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string phone { get; set; }

        public string gender { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
      

        public string PayerType { get; set; }
        public int Payerid { get; set; }

        public string payername { get; set; }
        public string contactpayer { get; set; }

        public string PrimaryPayer { get; set; }

        public string RefNo { get; set; }

    

    }

  

}