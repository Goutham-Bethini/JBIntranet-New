using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;


namespace USPS_Report.Areas.Reports.Models
{

    public class RWOMultipleTimesReport
    {
        public static IList<RWOMultipleTimesData> GetRWOMultipleTimes()
        {

            try
            {
                IList<RWOMultipleTimesData> _list = new List<RWOMultipleTimesData>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {


                    _list = _db.Database.SqlQuery<RWOMultipleTimesData>("SELECT rwo.Account, mem.First_Name, mem.Last_Name, COUNT(DISTINCT tim.DeliveryTime) AS  Times,  " +
                       "MIN(tim.DeliveryTime) AS Time1," +
   "MAX(tim.DeliveryTime) AS Time2 FROM tbl_ps_repeatingorders  rwo" +

   " JOIN	tbl_DeliveryTimes_Table		tim	ON tim.id=rwo.id_deliveryTime" +
   " JOIN    tbl_account_member          mem on rwo.account=mem.account" +
                                                          " and rwo.member = mem.member" +
" GROUP BY" +
   " rwo.Account, mem.First_Name, mem.Last_Name HAVING COUNT(DISTINCT tim.DeliveryTime) > 1" +
" ORDER BY" +
   " rwo.Account").ToList<RWOMultipleTimesData>();


                    return _list;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<RWOMultipleTimesData>();
            }


        }


    }
        public class RWOMultipleTimesData
        {
            public int Account { get; set; }


            public string First_Name { get; set; }

            public string Last_Name { get; set; }

            public int? Times { get; set; }

            public string Time1 { get; set; }

            public string Time2 { get; set; }

        }


        public class RWOMultipleTimesVM
        {
            public IList<RWOMultipleTimesData> rwoMultipleTimes { get; set; }
        }
    
}