using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.SqlClient;

namespace USPS_Report.Areas.Reports.Models
{
    public class MultipleshipDatesVM
    {
       
        public int Account { get; set; }

        public string dates { get; set; }
        public DateTime? NextRepeatDate { get; set; }

    }

    public class multidatesVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int  AutoCount { get; set; }

        public bool NotFirstTime { get; set; }
        public IList<MultipleshipDatesVM> details { get; set; }
    }


  
    public class DatesVM
    {
       
        public string dates { get; set; }

    }

    public class MultipleshipsReport
    {
        public static IList<MultipleshipDatesVM> GetMultipleshipDates(DateTime? _startDt, DateTime? _endDt)
        {
            try
            {
                

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

      

                    var _list2 = _db.Database.SqlQuery<MultipleshipDatesVM>("SELECT DISTINCT Account, NextRepeatDate "+
       " FROM tbl_PS_RepeatingOrders ro "+
      "  WHERE(SELECT COUNT(DISTINCT NextRepeatDate) "+
                " FROM tbl_PS_RepeatingOrders "+
                "  WHERE NextRepeatDate BETWEEN '" + _startDt + "'AND '"+ _endDt + "' "+
              "  AND Account = ro.Account) > 1 and NextRepeatDate BETWEEN '" + _startDt + "'AND '" + _endDt + "' " +
        " ORDER BY Account").ToList<MultipleshipDatesVM>();



                    IList<MultipleshipDatesVM> _rec = new List<MultipleshipDatesVM>();
                    foreach (var item in _list2.Select(t => t.Account).Distinct())
                    {
                        MultipleshipDatesVM _vm = new MultipleshipDatesVM();

                        _vm.Account = item;
                        _vm.dates = String.Join("|", _list2.Where(t => t.Account == item).Select(t=>t.NextRepeatDate).ToList());
                        _rec.Add(_vm);

                    }

                    var resut = _rec.Where(t=>t.dates.Contains("|")).ToList();



                    


                    return resut;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<MultipleshipDatesVM>();
            }


        }


        public static int GetAutoCorrcetCount(DateTime? _startDt, DateTime? _endDt)
        {
            try
            {

            
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var count = _db.Database.SqlQuery<int>("SELECT COUNT(*) " +
                                             "  FROM(  SELECT   DISTINCT elg.msdAccount, elg.msdNewDate " +

                                               " FROM Intranet..Eligibility_MultipleShipDates elg " +
                                                "  WHERE  msdOldDate <> msdNewDate " +
                                            " AND  msdOldDate BETWEEN '" + _startDt + "'AND '" + _endDt + "' " +
                                          ") as count").First();

                    //  var total = _db.Database.SqlQuery<int>(count).First();


                    var sum = count;

                    return count;
                }
            }
            catch (Exception ex)
            {
                int _count = 0;
                string msg = ex.Message;
                return _count;
            }


        }


    }
}