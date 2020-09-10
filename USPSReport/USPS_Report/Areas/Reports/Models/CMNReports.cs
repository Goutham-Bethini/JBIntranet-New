using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class CMNReports
    {
        public static IList<CMNWithDuration> GetCMNWithDuration99()
        {

            try
            {
                IList<CMNWithDuration> _list = new List<CMNWithDuration>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<CMNWithDuration>("SELECT  "+
     "   t1.Account,  " +
      "  t1.DocTypeDescription,  " +
       " t1.EffectiveDate,  " +
       " cast(t1.Duration as int) as Duration  " +
     " FROM  " +
                   " [dbo].[v_CMN_CurrentCMN]            t1  " +

       " LEFT JOIN[dbo].[tbl_Account_Information]     t2  ON  t2.Account = t1.Account  " +

   " WHERE(t1.EffectiveDate + (30 * t1.Duration)) >= GETDATE()  " +
    "    AND     t1.Duration = 99  " +
     "   AND     t2.InActiveAccount != 1  " +

   " ORDER BY  " +

    "        t1.Account").ToList<CMNWithDuration>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<CMNWithDuration>();
            }


        }

    }

    public class CMNReportsVM
    {
        public IList<CMNWithDuration> cmnWithDuration { get; set; }
    }
    public class CMNWithDuration
    {
        public int Account { get; set; }
        public string DocTypeDescription { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? Duration { get; set; }
    }
}