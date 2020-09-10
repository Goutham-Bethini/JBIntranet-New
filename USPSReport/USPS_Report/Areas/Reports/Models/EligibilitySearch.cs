using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{
    public class EligibilitySearchMIString
    {

        public static IList<EligSerachMI> GetEligibilityString_MI(int? _claim, int? _workOrder, int? _account, DateTime? _dos)
        {

            try
            {
                // @ProductCode varchar(100),@HoldCode int,@startDt Datetime, @endDt Datetime
                using (ReportsEntities _db = new ReportsEntities())
                {

                    var ParamClaim = new SqlParameter
                    {
                        ParameterName = "Claim",
                       // Value =   _claim   != null ? _claim : 0
                    };

                    if (_claim != null)
                        ParamClaim.Value = _claim;
                    else
                        ParamClaim.Value = DBNull.Value;

                    var ParamWorkOrder = new SqlParameter
                    {
                        ParameterName = "WorkOrder",
                       // Value = _workOrder  != null ? _workOrder : 0
                    };

                    if (_workOrder != null)
                        ParamWorkOrder.Value = _workOrder;
                    else
                        ParamWorkOrder.Value = DBNull.Value;

                    var ParamAccount = new SqlParameter
                    {
                        ParameterName = "Account",
                        //Value = _account   != null ? _account : 0
                    };

                    if (_account != null)
                        ParamAccount.Value = _account;
                    else
                        ParamAccount.Value = DBNull.Value;

                    var ParamDOS = new SqlParameter
                    {
                        ParameterName = "DOS",
                      //  Value = _dos 
                    };
                    if (_dos != null)
                        ParamDOS.Value = _dos;
                    else
                        ParamDOS.Value = DBNull.Value;

                   

                    IList<EligSerachMI> _rec = new List<EligSerachMI>();
                    _db.Database.CommandTimeout = 0;
                    _rec = _db.Database.SqlQuery<EligSerachMI>("exec sp_EligibilitySearch_MI @Claim,@WorkOrder,@Account,@DOS ", ParamClaim, ParamWorkOrder, ParamAccount, ParamDOS).ToList<EligSerachMI>();



                    return _rec;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<EligSerachMI>();

            }
        }


      

    }

    public class EligibilitySearchWIString
    {
        public static IList<EligSerach_WI> GetEligibilityString_WI(string _stringSreach, string _parameter)
        {


            try
            {
                // @ProductCode varchar(100),@HoldCode int,@startDt Datetime, @endDt Datetime
                using (ReportsEntities _db = new ReportsEntities())
                {

                    var ParamString = new SqlParameter
                    {
                        ParameterName = "SearchString",
                    };

                    if (_stringSreach != null)
                        ParamString.Value = _stringSreach;
                    else
                        ParamString.Value = DBNull.Value;

                    var ParamString2 = new SqlParameter
                    {
                        ParameterName = "SearchString",
                    };

                    ParamString2.Value = ParamString.Value;
//----------------------------------------------------------------------------------------
                    var ParamWorkOrder = new SqlParameter
                    {
                        ParameterName = "WorkOrder",

                    };

                    if (_parameter != null && _stringSreach != null && _stringSreach.Contains("workorder") == true)
                        ParamWorkOrder.Value = Convert.ToInt32(_parameter);
                    else
                        ParamWorkOrder.Value = DBNull.Value;

                    var ParamWorkOrder2 = new SqlParameter
                    {
                        ParameterName = "WorkOrder",

                    };

                    ParamWorkOrder2.Value = ParamWorkOrder.Value;

  //----------------------------------------------------------------------------------------------------
                    var ParamAccount = new SqlParameter
                    {
                        ParameterName = "Account",

                    };

                    if (_parameter != null && _stringSreach != null && _stringSreach.Contains("account") == true)
                        ParamAccount.Value = Convert.ToInt32(_parameter);
                    else
                        ParamAccount.Value = DBNull.Value;

                    var ParamAccount2 = new SqlParameter
                    {
                        ParameterName = "Account",

                    };
                    ParamAccount2.Value = ParamAccount.Value;
  //--------------------------------------------------------------------------------------------------------

                    var ParamLastName = new SqlParameter
                    {
                        ParameterName = "LastName",

                    };
                    if (_parameter != null && _stringSreach != null && _stringSreach.Contains("lastname") == true)
                        ParamLastName.Value = _parameter;
                    else
                        ParamLastName.Value = DBNull.Value;
                    var ParamLastName2 = new SqlParameter
                    {
                        ParameterName = "LastName",

                    };
                    ParamLastName2.Value = ParamLastName.Value;
 //-------------------------------------------------------------------------------------------------------

                    IList<EligSerach_WI> _recDB = new List<EligSerach_WI>();
                    _recDB = _db.Database.SqlQuery<EligSerach_WI>("exec sp_EligibilitySearchDB_WI @SearchString,@WorkOrder,@Account,@LastName ", ParamString, ParamWorkOrder, ParamAccount, ParamLastName).ToList<EligSerach_WI>();

                    IList<EligSerach_WI> _recLegacy = new List<EligSerach_WI>();
                    _recLegacy = _db.Database.SqlQuery<EligSerach_WI>("exec sp_EligibilitySearchLegacy_WI @SearchString,@WorkOrder,@Account,@LastName ", ParamString2, ParamWorkOrder2, ParamAccount2, ParamLastName2).ToList<EligSerach_WI>();


                    foreach (var item in _recLegacy)
                    {
                        _recDB.Add(item);
                    }

                    List<EligSerach_WI> _rec = _recDB.OrderBy(t => t.werAccount).ThenByDescending(t => t.werDOS).ToList();

                    return _recDB;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<EligSerach_WI>();

            }
        }

    }

        public class EligSerachMI
    {
        public string DB { get; set; }
        public int? Elig_Response_ID { get; set; }

        public int? Account { get; set; }
        public int? WONum { get; set; }
        public DateTime? request_date { get; set; }
        public int? ClaimNum { get; set; }
        public DateTime? completed_date { get; set; }
        public DateTime? cancel_date { get; set; }
        public DateTime? Invoice_Date { get; set; }
        public string DateChecked { get; set; }
        public string Elig_Response_String { get; set; }
        public bool? Is_Eligible { get; set; }
        public string Is_Eligible_Reason { get; set; }
        public string LegalName { get; set; }
        public string PrintUser { get; set; }
    }

    public class EligSearchMIVM
    {
        public int? Claim { get; set; }
        public int? WorkOrder { get; set; }
        public int? Account { get; set; }
        public DateTime? DateOfService { get; set; }
        public bool? noCriteria { get; set; }
        public IList<EligSerachMI> eligSerachMIVM { get; set; }
    }

    public class EligSearchWIVM
    {
        public string Parameter { get; set; }
       
        public string SearchString { get; set; }
        public IList<EligSerach_WI> eligSerach_WI { get; set; }

        public BatchVM batchVM { get; set; }
    }

    public class EligSerach_WI
    {
        public string DB { get; set; }
        public int? ID { get; set; }

     
        public DateTime? completed_date { get; set; }
        public string Completed_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
        public string Cancel_by { get; set; }
        public string Cancel_Note { get; set; }
        public DateTime? Last_Updated_Date { get; set; }
        public string Last_Updated_By { get; set; }

        public int? werID { get; set; }
        public int? werWEFid { get; set; }
        public int? werAccount { get; set; }
        public int? werWorkOrder { get; set; }
        public DateTime? werDOS { get; set; }
        public string werLastName { get; set; }
        public string werFirstName { get; set; }
        public string werTrackingNum { get; set; }
        public string werHoldReason { get; set; }
        public string werEligString { get; set; }
        public DateTime? werDateRecorded { get; set; }
        public int? wefID { get; set; }
        public string wefFileName { get; set; }
        public DateTime? wefStarted { get; set; }
        public DateTime? wefFinished { get; set; }
    }
}