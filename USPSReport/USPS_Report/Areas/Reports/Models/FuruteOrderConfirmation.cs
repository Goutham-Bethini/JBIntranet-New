using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsDatabase;
using System.Data.SqlClient;

namespace USPS_Report.Areas.Reports.Models
{

    public class FutureOrderConfirmationReport
    {
        public static IList<InsTypeVm> GetInsType()
        {
            try
            {

                IList<InsTypeVm> _list = new List<InsTypeVm>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _list = _db.Database.SqlQuery<InsTypeVm>(" SELECT "+
                  "  'Rush Orders' AS InsType UNION SELECT "+
                  "  'Eligibility Tests'  UNION  SELECT 'MI-Mcaid' "+
                  "   UNION SELECT 'IN-Mcaid' UNION SELECT "+
                   " 'WI-Mcaid' UNION SELECT 'CHP' UNION "+
                   "  SELECT 'HMO' UNION SELECT "+ 
                  "  'BCN' UNION SELECT "+
                  "  DeliveryLocationName FROM tbl_deliverylocation_table "+
                   "  WHERE id NOT IN(1, 6) AND DeletedDate IS NULL "+ 
                    " ORDER BY InsType").ToList<InsTypeVm>();

                    InsTypeVm _list2 = new InsTypeVm();
                    _list2.InsType = "all";

                    InsTypeVm _list3 = new InsTypeVm();
                    _list3.InsType = "blank";

                    _list.Insert(0,_list2);
                    _list.Insert(1,_list3);

                    return _list;
                }
            }
            catch (Exception ex)
            {
                string var = ex.Message;
                return new List<InsTypeVm>();
            }
        }

        public static IList<FutureOredreConfirmationData> GetFutureOredreConfirmation(DateTime? _startDt, DateTime? _endDt, string insType)
        {
            try
            {

                IList<FutureOredreConfirmationData> _list = new List<FutureOredreConfirmationData>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var idParam = new SqlParameter
                    {
                        ParameterName = "RangeStart",
                        Value = _startDt
                    };
                    var idParam2 = new SqlParameter
                    {
                        ParameterName = "RangeEnd",
                        Value = _endDt
                    };

                    var idParam3 = new SqlParameter
                    {
                        ParameterName = "InsType",
                        Value = insType
                    };

                    _list = _db.Database.SqlQuery<FutureOredreConfirmationData>("exec sp_OrderConfirmations @RangeStart,@RangeEnd,@InsType ", idParam, idParam2,idParam3).ToList<FutureOredreConfirmationData>();
                    return _list;
                }
            }
            catch (Exception ex)
            {
                string var = ex.Message;
                return new List<FutureOredreConfirmationData>();
            }
        }
    }

    public class FutureOredreConfirmationData
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime? NextRepeatDate { get; set; }
        public int Account { get; set; }
        public string InsType { get; set; }
    }
    public class FuruteOrderConfirmationVM
    {
      
        public string InsType { get; set; }
        public SelectList InsTypeList { get; set; }

        public IList<FutureOredreConfirmationData> futureOredreConfirmationData { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class InsTypeVm
    {
        public string InsType { get; set; }
        

    }
   
}