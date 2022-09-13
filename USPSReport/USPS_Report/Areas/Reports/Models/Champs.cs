using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace USPS_Report.Areas.Reports.Models
{
    public class ChampsTools
    {

        //   public static BatchVM GetBatchVM()
        //   {
        //       using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //       {


        //           var _list = _db.Database.SqlQuery<BatchVM>("SELECT csbID, csbBillingDate " +
        //"   FROM ClaimsToSubmit_Batches " +
        // "  WHERE " +
        //      " csbID = ( " +
        //         "  SELECT MAX(csbID) " +
        //         "  FROM ClaimsToSubmit_Batches)").SingleOrDefault();
        //           return _list;
        //       }
        //       }

        public static IList<ClaimVM> GetClaimsByPayer(int? payerId, DateTime startDt, DateTime endDt, string NPCode1, bool allNpCodes, string HCPC, string operatorName)
        {
           using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = "";
                string HCPCs = "";
                string appender1_HCPC = "";
                string appender2_HCPC = "";

                if (!string.IsNullOrEmpty(HCPC))
                {
                    List<string> HCPC_List = new List<string>();
                    string[] HCPCList = HCPC.Split(',');
                    foreach (var hcpc in HCPCList)
                    {
                        HCPC_List.Add(string.Format("'{0}'", hcpc));
                    }
                    HCPCs = string.Join(",", HCPC_List);

                    if (allNpCodes)
                    {
                        appender2_HCPC = " where t4.HCPC in(" + HCPCs + ")";
                    }
                    else
                    {
                        appender2_HCPC = " and t4.HCPC in(" + HCPCs + ")";
                    }
                    appender1_HCPC = " and Procedure_Code in("+ HCPCs +")";                   
                }
                if (allNpCodes == true)
                {                                              
                   query = ";With table1 as (Select Distinct t1.ID_Claim,t1.ID_Payer, " +
    " t1.Account as Acct,  t1.ID_Bill, Billing_Date as DOS, Procedure_Code as HCPC,mem.First_Name, mem.Last_Name,  " +
  " t1.ClaimBalance, ins.Policy_Number, t3.RespDate, t1.SumAllowed as AllowableAmt , t1.sumpay as PaymentAmt " +
   " from[dbo].[v_AR_ClaimDetail_Selection] t1 Left join tbl_Account_Insurance t2 on t1.Account = t2.Account AND t2.ID_Payer = " + payerId + " AND " +
     " ((t2.Effective_Date is NULL OR t2.Effective_Date < Billing_Date) " +
    " AND(t2.Expiration_Date is NULL or t2.Expiration_Date > Billing_Date))  " +
  " left join tbl_Account_Member_Insurance ins on t2.Account = ins.Account and t2.Entry_Number = ins.Entry_Number " +
  "left join tbl_account_member mem on t1.Account = mem.Account and  mem.Member = 1" +
  " Left JOIN(Select ID_Claim, ID_Bill, MIN(RESPONSEDate) as RespDate FROM dbo.v__Util_ClaimBillRejectionInfo  where ResponseDate >= '1/1/2015' " +
   " Group by ID_Claim, ID_Bill  ) t3 on t1.ID_Claim = t3.ID_Claim AND t1.ID_Bill = t3.ID_Bill " +
   " where t1.ID_Payer = " + payerId + "and t1.ClaimBalance != 0" + appender1_HCPC +" and Billing_Date >= '" + startDt + "' and Billing_Date <= '" + endDt + "' )" +

  " Select t4.*, t5.NPCode1, t5.NPDesc1, t5.NPCode2, t5.NPDesc2, t5.NPCode3, t5.NPDesc3, t5.NPCode4, t5.NPDesc4,  t5.DocumentNumber as DocNum, t7.MaxDateTime as lastClaimDt from table1 t4 " +
   " left join dbo.v_Rejection_Report t5 on t4.ID_Claim = t5.Current_ClaimID AND t4.ID_Bill = t5.ID_Bill AND " +
   "RejectionDate = t4.RespDate left join (  SELECT ID_Claim, MAX(DateOfHistory)AS MaxDateTime FROM v_ClaimHistoryDisplay_Report where  HistoryType = 'Submitted Claim'   GROUP BY ID_Claim ) as t7 on t7.ID_Claim = t4.ID_Claim"
   + appender2_HCPC;
                                   
                }

                else {
                    //orders.Add(String.Format("'{0}'", all_orders[i]));

                    List<string> NPCodes_List = new List<string>();
                    string NPCodes = "";
                    string[] NPCodesList = NPCode1.Split(',');
                    foreach(var npcode in NPCodesList)
                    {
                        NPCodes_List.Add(string.Format("'{0}'", npcode));
                    }
                    NPCodes = string.Join(",", NPCodes_List);

                   query = @";With table1 as (Select Distinct t1.ID_Claim,t1.ID_Payer, " +
     " t1.Account as Acct,  t1.ID_Bill, Billing_Date as DOS, Procedure_Code as HCPC,mem.First_Name, mem.Last_Name,  " +
   " t1.ClaimBalance, ins.Policy_Number, t3.RespDate, t1.SumAllowed as AllowableAmt , t1.sumpay as PaymentAmt " +
    " from [dbo].[v_AR_ClaimDetail_Selection] t1 Left join tbl_Account_Insurance t2 on t1.Account = t2.Account AND t2.ID_Payer = " + payerId + " AND " +
      " ((t2.Effective_Date is NULL OR t2.Effective_Date < Billing_Date) " +
     " AND(t2.Expiration_Date is NULL or t2.Expiration_Date > Billing_Date))  " +
   " left join tbl_Account_Member_Insurance ins on t2.Account = ins.Account and t2.Entry_Number = ins.Entry_Number " +
   "left join tbl_account_member mem on t1.Account = mem.Account and  mem.Member = 1" +
   " Left JOIN(Select ID_Claim, ID_Bill, MIN(RESPONSEDate) as RespDate FROM dbo.v__Util_ClaimBillRejectionInfo  where ResponseDate >= '1/1/2015' " +
    " Group by ID_Claim, ID_Bill  ) t3 on t1.ID_Claim = t3.ID_Claim AND t1.ID_Bill = t3.ID_Bill " +
    " where t1.ID_Payer = " + payerId + " and t1.ClaimBalance != 0"+appender1_HCPC+" and Billing_Date >= '" + startDt + "' and Billing_Date <= '" + endDt + "' ) " +

   " Select t4.*, t5.NPCode1, t5.NPDesc1, t5.NPCode2, t5.NPDesc2, t5.NPCode3, t5.NPDesc3, t5.NPCode4, t5.NPDesc4,  t5.DocumentNumber as DocNum, t7.MaxDateTime as lastClaimDt from table1 t4 " +
    " left join dbo.v_Rejection_Report t5 on t4.ID_Claim = t5.Current_ClaimID AND t4.ID_Bill = t5.ID_Bill AND t5.NPCode1 in(" + NPCodes + ") and " +
    "RejectionDate = t4.RespDate left join (  SELECT ID_Claim, MAX(DateOfHistory)AS MaxDateTime FROM v_ClaimHistoryDisplay_Report where  HistoryType = 'Submitted Claim'   GROUP BY ID_Claim ) as t7 on t7.ID_Claim = t4.ID_Claim" 
    + " where t5.NPCode1 in("+ NPCodes + ")" + appender2_HCPC;

                }

                _db.Database.CommandTimeout = 0;
                var _list = _db.Database.SqlQuery<ClaimVM>(query).ToList();

                string query2 = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',23,GETDATE())";

                int rowsinsert = _db.Database.ExecuteSqlCommand(query2);

                //specific auditing for Claim By Payer report.
                string query3 = @"insert into Reports.dbo.ReportsAuditLine_ClaimByPayer(OperatorName, ReportId, HCPCS, NPCode1, ResultCount, ReportAccessDate)
                                 values('" + operatorName + "',23,'" + HCPC + "','" + NPCode1 + "'," + _list.Count + ", GETDATE())";

                rowsinsert = _db.Database.ExecuteSqlCommand(query3);

                return _list;
            }
        }


        public static IList<CollectionVM> GetCollectionByOp(int Op, DateTime startDt, DateTime endDt, string operatorName)
        {
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                string query = " select distinct ID_Claim, TemplateName, cast(ActivityDate as Date) as ActivityDate from [dbo].[tbl_Claim_Worksheet_Activity] activity join " +
 " tbl_Claims_CollectionsTemplate template on activity.ID_ClaimsWorkSheet = template.ID  where ID_Operator = "+ Op + " "+
 " and ActivityDate >= '"+ startDt + "' and ActivityDate < '" + endDt + "'" + " insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',24,GETDATE())";




                _db.Database.CommandTimeout = 0;
                var _list = _db.Database.SqlQuery<CollectionVM>(query).ToList();


                return _list;
            }
        }
        public static IList<Champs> GetClaimstoTransmitToday(string willTransmit, string show)
        {

            try
            {
                BatchVM _list = new BatchVM();
                IList<Champs> _list1 = new List<Champs>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<BatchVM>("SELECT csbID, csbBillingDate " +
         "   FROM ClaimsToSubmit_Batches " +
          "  WHERE " +
               " csbID = ( " +
                  "  SELECT MAX(csbID) " +
                  "  FROM ClaimsToSubmit_Batches)").SingleOrDefault();




                    var _list2 = (from cs in _db.ClaimsToSubmit
                                  from csf in _db.ClaimsToSubmit_Fixed.Where(c => c.csfClaim == cs.ctsClaim && c.csfUBReason == cs.ctsUBReason).DefaultIfEmpty()

                                  where cs.ctsCSBid == _list.csbID &&
                                  cs.ctsWillSubmit == (willTransmit == "yes" ? 1 : willTransmit == "no" ? null : cs.ctsWillSubmit)
                                  && cs.ctsIssues == (show == "1" ? 1 : show == "0" ? null : cs.ctsIssues)
                                  && cs.ctsTimesSubmitted >= (show == "trans" ? 1 : cs.ctsTimesSubmitted)

                                  && cs.ctsOtherClaimsQty >= (show == "dup" ? 1 : cs.ctsOtherClaimsQty)

                                  && cs.ctsPaymentsReceived >= (show == "pay" ? 1 : cs.ctsPaymentsReceived)
                                  // && cs.ctsUBReason != (show == "ub" ? null : )
                                  && cs.ctsEligibilityDate == (show == "elg" ? null : cs.ctsEligibilityDate)

                                  select new { cs.ctsClaim, cs.ctsAccount, cs.ctsUBReason, cs.ctsUBFixedBy, cs.ctsManuallyCreatedBy,
                                      cs.ctsReleasedBy, cs.ctsEligibilityDate, cs.ctsHoldReason, cs.ctsOtherClaims, cs.ctsOtherClaimsQty,
                                      cs.ctsPaymentsReceived, cs.ctsDaysSinceCrossOver, cs.ctsNotes, cs.ctsPaymentPostedBy, cs.ctsResubmittedBy,
                                      cs.ctsWillSubmit, cs.ctsTimesSubmitted, cs.ctsIssues }).ToList();

                    if (show == "xelig")
                    {
                        _list1 = (from cs in _list2
                                  where cs.ctsIssues == 1 && (cs.ctsTimesSubmitted > 0 || cs.ctsOtherClaimsQty > 0
                                                               || cs.ctsHoldReason != null || cs.ctsPaymentsReceived > 0
                                                               || cs.ctsUBReason != null)


                                  select new Champs
                                  {
                                      ctsClaim = cs.ctsClaim,
                                      ctsAccount = cs.ctsAccount,
                                      ctsUBReason = cs.ctsUBReason,
                                      ctsUBFixedby = cs.ctsUBFixedBy,
                                      ctsManuallyCreatedBy = cs.ctsManuallyCreatedBy,
                                      ctsRealsedBy = cs.ctsReleasedBy,
                                      ctsEligibilityDate = cs.ctsEligibilityDate,
                                      ctsHoldReason = cs.ctsHoldReason,
                                      ctsOtherClaims = cs.ctsOtherClaims,
                                      ctsOtherClaimsQyt = cs.ctsOtherClaimsQty,
                                      ctsPaymentReceived = cs.ctsPaymentsReceived,
                                      ctsDaysincecrossOver = cs.ctsDaysSinceCrossOver,
                                      ctsNotes = cs.ctsNotes,
                                      ctsPaymentPostedBy = cs.ctsPaymentPostedBy,
                                      ctsResubmittedBy = cs.ctsResubmittedBy,
                                      ctsTimesSubmitted = cs.ctsTimesSubmitted,
                                      ctsIssue = cs.ctsIssues,
                                      ctsWillTransmit = cs.ctsWillSubmit




                                  }



                        ).OrderBy(t => t.ctsUBReason != null ? 0 : 1).ThenBy(t => t.ctsHoldReason != null ? 0 : 1).ThenBy(t => t.ctsOtherClaimsQyt > 0 ? 0 : 1)
                        .ThenBy(t => t.ctsPaymentReceived > 0 ? 0 : 1).ThenBy(t => t.ctsTimesSubmitted > 0 ? 0 : 1).ThenBy(t => t.ctsEligibilityDate == null ? 0 : 1).
                        ThenBy(t => t.ctsEligibilityDate == null ? t.ctsManuallyCreatedBy : null).ThenBy(t => t.ctsEligibilityDate == null ? t.ctsRealsedBy : null).ThenBy(t => t.ctsClaim).ToList<Champs>();
                    }
                    //if (show == "cross")
                    //{
                    //    _list1 = (from cs in _list2
                    //              where cs.ctsDaysSinceCrossOver != null


                    //              select new Champs
                    //              {
                    //                  ctsClaim = cs.ctsClaim,
                    //                  ctsAccount = cs.ctsAccount,
                    //                  ctsUBReason = cs.ctsUBReason,
                    //                  ctsUBFixedby = cs.ctsUBFixedBy,
                    //                  ctsManuallyCreatedBy = cs.ctsManuallyCreatedBy,
                    //                  ctsRealsedBy = cs.ctsReleasedBy,
                    //                  ctsEligibilityDate = cs.ctsEligibilityDate,
                    //                  ctsHoldReason = cs.ctsHoldReason,
                    //                  ctsOtherClaims = cs.ctsOtherClaims,
                    //                  ctsOtherClaimsQyt = cs.ctsOtherClaimsQty,
                    //                  ctsPaymentReceived = cs.ctsPaymentsReceived,
                    //                  ctsDaysincecrossOver = cs.ctsDaysSinceCrossOver,
                    //                  ctsNotes = cs.ctsNotes,
                    //                  ctsPaymentPostedBy = cs.ctsPaymentPostedBy,
                    //                  ctsResubmittedBy = cs.ctsResubmittedBy,
                    //                  ctsTimesSubmitted = cs.ctsTimesSubmitted,
                    //                  ctsIssue = cs.ctsIssues,
                    //                  WillTransmit = willTransmit,
                    //                  Show = show



                    //              }



                    //          ).OrderBy(t => t.ctsUBReason != null ? 0 : 1).ThenBy(t => t.ctsHoldReason != null ? 0 : 1).ThenBy(t => t.ctsOtherClaimsQyt > 0 ? 0 : 1)
                    //          .ThenBy(t => t.ctsPaymentReceived > 0 ? 0 : 1).ThenBy(t => t.ctsTimesSubmitted > 0 ? 0 : 1).ThenBy(t => t.ctsEligibilityDate == null ? 0 : 1).
                    //          ThenBy(t => t.ctsEligibilityDate == null ? t.ctsManuallyCreatedBy : null).ThenBy(t => t.ctsEligibilityDate == null ? t.ctsRealsedBy : null).ThenBy(t => t.ctsClaim).ToList<Champs>();

                    //}
                    else if (show == "held")
                    {
                        _list1 = (from cs in _list2
                                  where cs.ctsHoldReason != null


                                  select new Champs
                                  {
                                      ctsClaim = cs.ctsClaim,
                                      ctsAccount = cs.ctsAccount,
                                      ctsUBReason = cs.ctsUBReason,
                                      ctsUBFixedby = cs.ctsUBFixedBy,
                                      ctsManuallyCreatedBy = cs.ctsManuallyCreatedBy,
                                      ctsRealsedBy = cs.ctsReleasedBy,
                                      ctsEligibilityDate = cs.ctsEligibilityDate,
                                      ctsHoldReason = cs.ctsHoldReason,
                                      ctsOtherClaims = cs.ctsOtherClaims,
                                      ctsOtherClaimsQyt = cs.ctsOtherClaimsQty,
                                      ctsPaymentReceived = cs.ctsPaymentsReceived,
                                      ctsDaysincecrossOver = cs.ctsDaysSinceCrossOver,
                                      ctsNotes = cs.ctsNotes,
                                      ctsPaymentPostedBy = cs.ctsPaymentPostedBy,
                                      ctsResubmittedBy = cs.ctsResubmittedBy,
                                      ctsTimesSubmitted = cs.ctsTimesSubmitted,
                                      ctsIssue = cs.ctsIssues,
                                      ctsWillTransmit = cs.ctsWillSubmit




                                  }



                        ).OrderBy(t => t.ctsUBReason != null ? 0 : 1).ThenBy(t => t.ctsHoldReason != null ? 0 : 1).ThenBy(t => t.ctsOtherClaimsQyt > 0 ? 0 : 1)
                        .ThenBy(t => t.ctsPaymentReceived > 0 ? 0 : 1).ThenBy(t => t.ctsTimesSubmitted > 0 ? 0 : 1).ThenBy(t => t.ctsEligibilityDate == null ? 0 : 1).
                        ThenBy(t => t.ctsEligibilityDate == null ? t.ctsManuallyCreatedBy : null).ThenBy(t => t.ctsEligibilityDate == null ? t.ctsRealsedBy : null).ThenBy(t => t.ctsClaim).ToList<Champs>();
                    }
                    else if (show == "ub")
                    {
                        _list1 = (from cs in _list2
                                  where cs.ctsUBReason != null


                                  select new Champs
                                  {
                                      ctsClaim = cs.ctsClaim,
                                      ctsAccount = cs.ctsAccount,
                                      ctsUBReason = cs.ctsUBReason,
                                      ctsUBFixedby = cs.ctsUBFixedBy,
                                      ctsManuallyCreatedBy = cs.ctsManuallyCreatedBy,
                                      ctsRealsedBy = cs.ctsReleasedBy,
                                      ctsEligibilityDate = cs.ctsEligibilityDate,
                                      ctsHoldReason = cs.ctsHoldReason,
                                      ctsOtherClaims = cs.ctsOtherClaims,
                                      ctsOtherClaimsQyt = cs.ctsOtherClaimsQty,
                                      ctsPaymentReceived = cs.ctsPaymentsReceived,
                                      ctsDaysincecrossOver = cs.ctsDaysSinceCrossOver,
                                      ctsNotes = cs.ctsNotes,
                                      ctsPaymentPostedBy = cs.ctsPaymentPostedBy,
                                      ctsResubmittedBy = cs.ctsResubmittedBy,
                                      ctsTimesSubmitted = cs.ctsTimesSubmitted,
                                      ctsIssue = cs.ctsIssues,
                                      ctsWillTransmit = cs.ctsWillSubmit




                                  }



                        ).OrderBy(t => t.ctsUBReason != null ? 0 : 1).ThenBy(t => t.ctsHoldReason != null ? 0 : 1).ThenBy(t => t.ctsOtherClaimsQyt > 0 ? 0 : 1)
                        .ThenBy(t => t.ctsPaymentReceived > 0 ? 0 : 1).ThenBy(t => t.ctsTimesSubmitted > 0 ? 0 : 1).ThenBy(t => t.ctsEligibilityDate == null ? 0 : 1).
                        ThenBy(t => t.ctsEligibilityDate == null ? t.ctsManuallyCreatedBy : null).ThenBy(t => t.ctsEligibilityDate == null ? t.ctsRealsedBy : null).ThenBy(t => t.ctsClaim).ToList<Champs>();
                    }

                    else {
                        _list1 = (from cs in _list2

                                  select new Champs
                                  {
                                      ctsClaim = cs.ctsClaim,
                                      ctsAccount = cs.ctsAccount,
                                      ctsUBReason = cs.ctsUBReason,
                                      ctsUBFixedby = cs.ctsUBFixedBy,
                                      ctsManuallyCreatedBy = cs.ctsManuallyCreatedBy,
                                      ctsRealsedBy = cs.ctsReleasedBy,
                                      ctsEligibilityDate = cs.ctsEligibilityDate,
                                      ctsHoldReason = cs.ctsHoldReason,
                                      ctsOtherClaims = cs.ctsOtherClaims,
                                      ctsOtherClaimsQyt = cs.ctsOtherClaimsQty,
                                      ctsPaymentReceived = cs.ctsPaymentsReceived,
                                      ctsDaysincecrossOver = cs.ctsDaysSinceCrossOver,
                                      ctsNotes = cs.ctsNotes,

                                      ctsPaymentPostedBy = cs.ctsPaymentPostedBy,
                                      ctsResubmittedBy = cs.ctsResubmittedBy,
                                      ctsTimesSubmitted = cs.ctsTimesSubmitted,
                                      ctsIssue = cs.ctsIssues,
                                      ctsWillTransmit = cs.ctsWillSubmit




                                  }



                              ).OrderBy(t => t.ctsUBReason != null ? 0 : 1).ThenBy(t => t.ctsHoldReason != null ? 0 : 1).ThenBy(t => t.ctsOtherClaimsQyt > 0 ? 0 : 1)
                              .ThenBy(t => t.ctsPaymentReceived > 0 ? 0 : 1).ThenBy(t => t.ctsTimesSubmitted > 0 ? 0 : 1).ThenBy(t => t.ctsEligibilityDate == null ? 0 : 1).
                              ThenBy(t => t.ctsEligibilityDate == null ? t.ctsManuallyCreatedBy : null).ThenBy(t => t.ctsEligibilityDate == null ? t.ctsRealsedBy : null).ThenBy(t => t.ctsClaim).ToList<Champs>();

                    }



                    return _list1;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<Champs>();
            }


        }

        public static void IgnoreError(string _errCode)
        {

            CHAMPS_Ignore_Errors _rec = new CHAMPS_Ignore_Errors();
            var components = HttpContext.Current.User.Identity.Name.Split('\\');

            var userName = components.Last();
            try {
                using (IntranetEntities _db = new IntranetEntities())
                {

               
                    _rec.cieCode = _errCode;
                   _rec.cieAdded = DateTime.Now;
                    _rec.cieAddedBy = userName;


                   _db.CHAMPS_Ignore_Errors.Add(_rec);


                    _db.SaveChanges();
                }
            } catch (Exception ex)
            {
                string msg = ex.Message;
            }
           
        }

        public static BatchVM GetSubmitBatch()
        {
            BatchVM _list = new BatchVM();

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                _list = _db.Database.SqlQuery<BatchVM>("SELECT * " +
                 "   FROM ClaimsToSubmit_Batches " +
                "  WHERE " +
                " csbID = ( " +
              "  SELECT MAX(csbID) " +
              "  FROM ClaimsToSubmit_Batches)").SingleOrDefault();
            }

            return _list;
        }


        public static IList<submittedClaims> GetSubmittedClaims(DateTime? _Dt, int responceType)
        {
            try
            {

                IList<submittedClaims> _list = new List<submittedClaims>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var idParam = new SqlParameter
                    {
                        ParameterName = "PayerID",
                        Value = 7
                    };
                    var idParam2 = new SqlParameter
                    {
                        ParameterName = "SubmittedDate",
                        Value = _Dt
                    };


                    var idParam3 = new SqlParameter
                    {
                        ParameterName = "ResponseType",
                        Value = responceType

                    };



                    _list = _db.Database.SqlQuery<submittedClaims>("exec sp_Submitted_Claims @PayerID,@SubmittedDate,@ResponseType ", idParam, idParam2, idParam3).ToList<submittedClaims>();

                    foreach (var item in _list)
                    {
                        if (item.Claims > 1)
                        {
                            IList<ClaimIds> otherClaims = new List<ClaimIds>();

                            IList<int> id_claim = new List<int>();


                            id_claim = _db.Database.SqlQuery<int>("SELECT DISTINCT " +
                              "  trn.id_claim " +
                          "  FROM " +
                                     "   tbl_encounters              enc " +
                             "   JOIN    tbl_transaction_file        trn ON trn.id_encounter = enc.id " +
                              "  JOIN    tbl_claims                  clm ON clm.id = trn.id_claim " +
                              "  JOIN    tbl_items                   itm ON itm.id = trn.id_item " +
                                " JOIN    tbl_payer_table             pay ON pay.id = clm.id_payer " +
                               " JOIN    tbl_procedure_groups_table  pcd ON pcd.id_group_no = pay.id_procedure_group " +
                                                                         "   AND pcd.id_billing_code = itm.id_billing_code " +
                          "  WHERE " +
                                   " enc.id_workorder = ( " +
                                      "  SELECT MAX(enc.id_workorder) " +
                                      "  FROM " +
                                                  "  tbl_encounters          enc " +
                                         "   JOIN    tbl_transaction_file    trn ON trn.id_encounter = enc.id " +
                                       " WHERE " +
                                               " trn.id_claim = " + item.ClaimID + ") " +
                              "  AND clm.id_payer = " + 7 + " " +
                              "  AND pcd.Procedure_Code IN ( " +
                                  "  SELECT procedure_code " +
                                 "   FROM " +
                                            "    tbl_claims                  clm " +
                                       " JOIN    tbl_transaction_file        trn ON clm.id = trn.id_claim " +
                                       " JOIN    tbl_items                   itm ON itm.id = trn.id_item " +
                                       " JOIN    tbl_payer_table             pay ON pay.id = clm.id_payer " +
                                     "   JOIN    tbl_procedure_groups_table  pcd ON pcd.id_group_no = pay.id_procedure_group " +
                                                                          "  AND pcd.id_billing_code = itm.id_billing_code " +
                                 "   where clm.ID = " + item.ClaimID + ") " +
                                " AND clm.ID<> " + item.ClaimID + "").ToList<int>();

                            item.otherClaims = String.Join(",", id_claim);
                        }
                    }


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string var = ex.Message;
                return new List<submittedClaims>();
            }
        }

        public static ErrorDecorderVM GetErrorDecorder(string _remarkCode, string _reasonCode)
        {

            ErrorDecorderVM errorDecorderVm = new ErrorDecorderVM();
            errorDecorderVm.ReasonCode = _reasonCode;
            errorDecorderVm.RemarkCode = _remarkCode;
            //    List<string> reasonCode = "16,22,23,45".Split(',').ToList<string>();
            //    List<string> remarkCode = "N290,MA125,N598".Split(',').ToList<string>();
               List<string> reasonCode = _reasonCode.Split(',').ToList<string>();
               List<string> remarkCode = _remarkCode.Split(',').ToList<string>();
            try
            {

                using (IntranetEntities _db = new IntranetEntities())
                {



                    var _list = (from err in _db.CHAMPS_Error_Codes
                                 join adj in _db.CHAMPS_Adj_Codes
                                 on err.errAdjCode equals adj.adjCode
                               join rem in _db.CHAMPS_Remit_Codes
                               on err.errRemitCode equals rem.remCode
                               from  cie in _db.CHAMPS_Ignore_Errors.Where(t=>t.cieCode == err.errCode).DefaultIfEmpty()
                                 where reasonCode.Contains(adj.adjCode) &&
                                 remarkCode.Contains(rem.remCode)
                                 select new ErrorDecorder
                                 {
                                    errCode  = err.errCode,
                                    errRemitCode = err.errRemitCode,
                                   errAdjCode = err.errAdjCode,
                                  errHighlight = err.errHighlight != null ? err.errHighlight : null,
                                  cieID  = cie.cieID ,
                                   Error = err.errShortDesc

                                 }).ToList();

                  

                    var _HideIgnoreList = (from t in _list
                                  where t.cieID == null
                                       select new ErrorDecorder
                                       {
                                           errCode = t.errCode,
                                           errRemitCode = t.errRemitCode,
                                           errAdjCode = t.errAdjCode,
                                           errHighlight = t.errHighlight != null ? t.errHighlight : null,
                                           cieID = t.cieID,
                                           Error = t.Error

                                       }).ToList();
                    errorDecorderVm.Count = _list.Count() - _HideIgnoreList.Count();
                    errorDecorderVm.withoutIgnoreList = _HideIgnoreList;


                    // _list = _list.Except(_ignoreList);

                    return errorDecorderVm;
                }

                }
           
            catch (Exception ex)
            {
                string var = ex.Message;
                return new ErrorDecorderVM();
            }
        }

        public static ErrorDecorderVM GetErrorDecorderShowAll(string _remarkCode, string _reasonCode)
        {
            if (_reasonCode == null)
            {
                _reasonCode = ",";
            }

            if (_remarkCode == null)
            {
                _remarkCode = ",";
            }


            ErrorDecorderVM errorDecorderVm = new ErrorDecorderVM();
            errorDecorderVm.ReasonCode = _reasonCode;
            errorDecorderVm.RemarkCode = _remarkCode;
            //    List<string> reasonCode = "16,22,23,45".Split(',').ToList<string>();
            //    List<string> remarkCode = "N290,MA125,N598".Split(',').ToList<string>();
            List<string> reasonCode = _reasonCode.Split(',').ToList<string>();
            List<string> remarkCode = _remarkCode.Split(',').ToList<string>();
            try
            {

                using (IntranetEntities _db = new IntranetEntities())
                {



                    var _list = (from err in _db.CHAMPS_Error_Codes
                                 join adj in _db.CHAMPS_Adj_Codes
                                 on err.errAdjCode equals adj.adjCode
                                 join rem in _db.CHAMPS_Remit_Codes
                                 on err.errRemitCode equals rem.remCode
                                 from cie in _db.CHAMPS_Ignore_Errors.Where(t => t.cieCode == err.errCode && t.cieDeleted == null).DefaultIfEmpty()
                                 where reasonCode.Contains(adj.adjCode) &&
                                 remarkCode.Contains(rem.remCode) 
                                 select new ErrorDecorder
                                 {
                                     errCode = err.errCode,
                                     errRemitCode = err.errRemitCode,
                                     errAdjCode = err.errAdjCode,
                                     errHighlight = err.errHighlight != null ? err.errHighlight : null,
                                     cieID = cie.cieID,
                                     Error = err.errShortDesc

                                 }).OrderBy(t=>t.cieID).ToList();


                    var _WithoutIgnoreList = (from t in _list
                                           where t.cieID == null
                                           select new ErrorDecorder
                                           {
                                               errCode = t.errCode,
                                               errRemitCode = t.errRemitCode,
                                               errAdjCode = t.errAdjCode,
                                               errHighlight = t.errHighlight != null ? t.errHighlight : null,
                                               cieID = t.cieID,
                                               Error = t.Error

                                           }).ToList();


                    var _ShowIgnoreList = (from t in _list
                                           where t.cieID != null
                                           select new ErrorDecorder
                                           {
                                               errCode = t.errCode,
                                               errRemitCode = t.errRemitCode,
                                               errAdjCode = t.errAdjCode,
                                               errHighlight = t.errHighlight != null ? t.errHighlight : null,
                                               cieID = t.cieID,
                                               Error = t.Error

                                           }).ToList();
                    errorDecorderVm.withoutIgnoreList = _WithoutIgnoreList;

                    errorDecorderVm.IgnoreList = _ShowIgnoreList;

                    errorDecorderVm.Count = _ShowIgnoreList.Count();

                   

                    return errorDecorderVm;
                }

            }

            catch (Exception ex)
            {
                string var = ex.Message;
                return new ErrorDecorderVM();
            }
        }
        
        //public static ErrorDecorderVM GetErrorDecorderHideIgnore(string _remarkCode, string _reasonCode)
        //{

        //    ErrorDecorderVM errorDecorderVm = new ErrorDecorderVM();
        //    errorDecorderVm.ReasonCode = _reasonCode;
        //    errorDecorderVm.RemarkCode = _remarkCode;
        //    //    List<string> reasonCode = "16,22,23,45".Split(',').ToList<string>();
        //    //    List<string> remarkCode = "N290,MA125,N598".Split(',').ToList<string>();
        //    List<string> reasonCode = _reasonCode.Split(',').ToList<string>();
        //    List<string> remarkCode = _remarkCode.Split(',').ToList<string>();
        //    try
        //    {

        //        using (IntranetEntities _db = new IntranetEntities())
        //        {



        //            var _list = (from err in _db.CHAMPS_Error_Codes
        //                         join adj in _db.CHAMPS_Adj_Codes
        //                         on err.errAdjCode equals adj.adjCode
        //                         join rem in _db.CHAMPS_Remit_Codes
        //                         on err.errRemitCode equals rem.remCode
        //                         from cie in _db.CHAMPS_Ignore_Errors.Where(t => t.cieCode == err.errCode).DefaultIfEmpty()
        //                         where reasonCode.Contains(adj.adjCode) &&
        //                         remarkCode.Contains(rem.remCode)
        //                         select new ErrorDecorder
        //                         {
        //                             errCode = err.errCode,
        //                             errRemitCode = err.errRemitCode,
        //                             errAdjCode = err.errAdjCode,
        //                             errHighlight = err.errHighlight != null ? err.errHighlight : null,
        //                             cieID = cie.cieID,
        //                             Error = err.errShortDesc

        //                         }).ToList();

        //            errorDecorderVm.errorDecorder = _list;



        //            return errorDecorderVm;
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        string var = ex.Message;
        //        return new ErrorDecorderVM();
        //    }
        //}


    }

    public class Champs
    {


        public DateTime? csbBillingDate { get; set; }
        public int? ctsWillTransmit { get; set; }
        public int? ctsClaim { get; set; }
        public int? ctsAccount { get; set; }
        public string ctsUBReason { get; set; }
        public string ctsUBFixedby { get; set; }
        public string ctsManuallyCreatedBy { get; set; }
        public string ctsRealsedBy { get; set; }
        public string ctsHoldReason { get; set; }
        public DateTime? ctsEligibilityDate { get; set; }

        public int? ctsOtherClaimsQyt { get; set; }
        public string ctsOtherClaims { get; set; }
        public int? ctsPaymentReceived { get; set; }
        public string ctsPaymentPostedBy { get; set; }

        public int? ctsTimesSubmitted { get; set; }
        public string ctsResubmittedBy { get; set; }

        public int? ctsDaysincecrossOver { get; set; }
        public string ctsNotes { get; set; }
        public int? ctsIssue { get; set; }


    }

    public class BatchVM
    {

        public int csbID { get; set; }
        public int csbPayerID { get; set; }
        public DateTime? csbBillingDate { get; set; }
        public DateTime? csbDateRun { get; set; }
    }


    public class ChampsVM
    {
        public string WillTransmit { get; set; }
        public string Show { get; set; }
        public IList<Champs> champs { get; set; }

        public BatchVM batchVM { get; set; }

    }


    public class submittedClaimVM
    {
        public int ResponseType { get; set; }
        public DateTime? Date { get; set; }

        public IList<submittedClaims> submittedClaims { get; set; }

    }


    public class ClaimIds
    { public int id_claim { get; set; } }
    public class submittedClaims
    {
        public int ClaimID { get; set; }
        public int Account { get; set; }

        public int? id_workorder { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public int? TimesSubmitted { get; set; }

        public int? PaymentsPreviouslyReceived { get; set; }

        public string ClaimNotes { get; set; }

        public string LastSubmittedBy { get; set; }

        public int? Claims { get; set; }

        public int? CrossOver { get; set; }

        public string otherClaims { get; set; }
    }

    public class ErrorDecorder
   {
        public string errCode { get; set; }
        public string errRemitCode { get; set; }
        public string Error { get; set; }
        public string errAdjCode { get; set; }
       // public string errShortDesc { get; set; }
        public int? cieID { get; set; }
        public int? errHighlight { get; set; }

        public bool ignore { get; set; }
    }

    public class ErrorDecorderVM
    {
        public bool NotfirstTime { get; set; }
        public bool showAll { get; set; }
        public string ReasonCode { get; set; }
        public string RemarkCode { get; set; }

        public int? Count { get; set; }
        public IList<ErrorDecorder> withoutIgnoreList{ get; set; }
        public IList<ErrorDecorder> IgnoreList{ get; set; }
    }


    public class ClaimModel
    {
        public IList<ClaimVM> claimVM { get; set; }
        [Required]
        public int? PayerId { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime EndDt { get; set; }
        public string NPcode { get; set; }
        public bool allNPcode { get; set; }
        public string HCPC { get; set; }
    }

    public class CollectionModel
    {
        public IList<CollectionVM> collectionVM { get; set; }
        
        public DateTime ActivityStartDt { get; set; }
        public DateTime ActivityEndDt { get; set; }
        [Required]
        public int? OperatorID { get; set; }
        
    }
    public class CollectionVM
    {
        public Int32 ID_Claim { get; set; }
        public string TemplateName { get; set; }
        public DateTime ActivityDate { get; set; }
    }
        public class ClaimVM
    {
        public int? ID_Claim { get; set; }
        public int? ID_Payer { get; set; }
        public int? Acct { get; set; }
        public string DocNum { get; set; }
        public DateTime? lastClaimDt { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int? ID_Bill { get; set; }
        public DateTime? DOS { get; set; }
        public string HCPC { get; set; }
        public decimal? ClaimBalance { get; set; }

        public string Policy_Number { get; set; }
        public DateTime? RespDate { get; set; }
        public string NPCode1 { get; set; }
        public string NPDesc1 { get; set; }
        public string NPCode2 { get; set; }
        public string NPDesc2 { get; set; }
        public string NPCode3 { get; set; }
        public string NPDesc3 { get; set; }
        public string NPCode4 { get; set; }
        public string NPDesc4 { get; set; }
        public decimal? AllowableAmt { get; set; }
        public decimal? PaymentAmt { get; set; }

    }
}