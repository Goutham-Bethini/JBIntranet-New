using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.SqlClient;
using USPS_Report.Models;
//using ReportsEntities = USPS_Report.Models.ReportsEntities;

namespace USPS_Report.Areas.Reports.Models
{
    public class warehouse
    {
        public static IList<UM_TrackingLookup> Get_UM_TrackingInfo(string _searchby, string _searchValue)
        {


            try
            {
                // @ProductCode varchar(100),@HoldCode int,@startDt Datetime, @endDt Datetime
                using (ReportsEntities _db = new ReportsEntities())
                {

                    var ParamAccount= new SqlParameter
                    {
                        ParameterName = "Account",
                    };

                    if (_searchby != null && _searchby == "account")
                        ParamAccount.Value = Convert.ToInt32(_searchValue);
                    else
                        ParamAccount.Value = DBNull.Value;
//----------------------------------------------------------------------------------
                    var ParamPONumber = new SqlParameter
                    {
                        ParameterName = "POnumber",
                    };

                    if (_searchby != null && _searchby == "ponumber")
                        ParamPONumber.Value = _searchValue;
                    else
                        ParamPONumber.Value = DBNull.Value;
   //----------------------------------------------------------------------------------------
                    var ParamLastName = new SqlParameter
                    {
                        ParameterName = "LastName",
                    };

                    if (_searchby != null && _searchby == "lastname")
                        ParamLastName.Value = _searchValue;
                    else
                        ParamLastName.Value = DBNull.Value;
                    //-------------------------------------------------------------------------------------------------------

                    IList<UM_TrackingLookup> _recDB = new List<UM_TrackingLookup>();
                    _recDB = _db.Database.SqlQuery<UM_TrackingLookup>("exec sp_UM_TrackingLookUP @Account,@POnumber,@LastName ", ParamAccount, ParamPONumber, ParamLastName).ToList<UM_TrackingLookup>();


                    return _recDB;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<UM_TrackingLookup>();

            }
        }
    }

    public class UM_TrackingLookup
    {
        public string last_name { get; set; }
        public int id { get; set; }
        public DateTime? Request_Date { get; set; }
        public int? account { get; set; }
        public string confirmationNumber { get; set; }
        public DateTime? DateShipped { get; set; }
        public DateTime? completed_date { get; set; }
        public DateTime? cancel_date { get; set; }
        public string Cancel_Note { get; set; }
        public int ID { get; set; }
        public string PONumber { get; set; }
    }

    public class UM_TrackingLookupVM
    {
      
        public string SearchBy { get; set; }
        public string SearchValue { get; set; }
        public IList<UM_TrackingLookup> um_TrackingLookup { get; set; }
    }
}