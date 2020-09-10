using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USPS_Report;
using ReportsDatabase;
using System.Data.SqlClient;

namespace USPS_Report.Areas.Reports.Models
{
    public class POTA_Report
    {
        public static IList<POTA_ReportVM> GetPOTAByPayerReport(int? _payerId)
        {

            try
            {
              
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var ParamPayerID = new SqlParameter
                    {
                        ParameterName = "PayerID",
                       
                    };

                    if (_payerId == null)
                        ParamPayerID.Value = 7;
                    else
                        ParamPayerID.Value = DBNull.Value;

                   

                    IList<POTA_ReportVM> _rec = new List<POTA_ReportVM>();
                    _rec = _db.Database.SqlQuery<POTA_ReportVM>("exec sp_POTA_ByPayer @PayerID", ParamPayerID).ToList<POTA_ReportVM>();



                    return _rec;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<POTA_ReportVM>();

            }
        }
    }

    public class POTA_ReportVM
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public int Claim { get; set; }
        public DateTime? From_date { get; set; }
        public string Procedure_Code { get; set; }

        public double? Reimbercment { get; set; }
        
        public int? Eaches { get; set; }
        public double? ShouldPay { get; set; }

        public double? LinePayments { get; set; }
        public double? DiffInPayment { get; set; }
        public double? LineBalance { get; set; }
        public double? LinePayLineBal{ get; set; }

        public double? LineRecindPayments { get; set; }
        public double? ClaimBalance { get; set; }

        public double? ClaimPayments { get; set; }
        public double? ClaimRecindPayments { get; set; }
        
        }

    public class POTA_Report_Model
    {
        public int? PayerId { get; set; }
        public IList<POTA_ReportVM> pota_ReportVM { get; set; }
    }
}