using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{

    public class WareHouseBoard
    {
        public static IList<WareHouseStatusVM> GetWareHouseStatus()
        {
            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    DateTime today = DateTime.Now.Date;

                    var _list = (from wos in _db.tbl_PS_WorkOrder
                                 join wol in _db.tbl_PS_WorkOrderLine on wos.ID equals wol.ID_PS_WorkOrder
                                 join pro in _db.tbl_Product_Table on wol.ID_Product equals pro.ID
                                 join cat in _db.tbl_ProductCategory_Table on pro.ID_ProductCategory equals cat.ID
                                 join bct in _db.tbl_Billing_Code_Table on pro.ID_BillingCode equals bct.ID

                                 where wos.Completed_Date != null &&
                                        wos.Completed_Date == today &&
                                       !bct.Code.Contains("DELIVERY") &&
                                       !bct.Code.Contains("Noninv")

                                 select new WareHouseStatusVM
                                 {
                                     ID = wos.ID,
                                     Diab = cat.ID
                                 }



                               ).ToList();

                                
                                  
                                 
                    
                    return new List<WareHouseStatusVM>();

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<WareHouseStatusVM>();
            }

        }

        public static IList<scheduleOrders> GetEligScheduleStatus()
        {
            int[] _CatIDArry = { 68, 70, 67, 71, 65, 74, 69, 66, 45, 85 };
            IList<scheduleOrders> scheduleList = new List<scheduleOrders>();
            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    for (int i = 1; i <= 13; i++)
                    {
                    
                        DateTime tomorrow = DateTime.Today.AddDays(i);
                        DateTime dayaftertmw = DateTime.Today.AddDays(i + 1);
                        string dayName = tomorrow.DayOfWeek.ToString();
                   
                        if (dayName == "Friday" || dayName == "Sunday" )
                        {
                            dayaftertmw = DateTime.Today.AddDays(i+2);
                        }
                        if (dayName != "Saturday" && dayName != "Monday")
                            
                        {
                            var _list = (from rwo in _db.tbl_PS_RepeatingOrders
                                             join pro in _db.tbl_Product_Table on rwo.ID_Product equals pro.ID
                                             join cat in _db.tbl_ProductCategory_Table on  pro.ID_ProductCategory equals cat.ID
                                         where rwo.NextRepeatDate >= tomorrow && rwo.NextRepeatDate < dayaftertmw

                                         select new scheduleOrders
                                         {
                                             ID = rwo.Account,

                                             Diab = (_CatIDArry.Contains(cat.ID) && cat.DeletedDate == null) ? 1 : 0,
                                             NonDiab = (!_CatIDArry.Contains(cat.ID) && cat.DeletedDate == null) ? 1 : 0
                                         }



                                       ).ToList();


                            scheduleOrders _order = new scheduleOrders();
                            _order.count = _list.Select(t => t.ID).Distinct().Count();
                            _order.Diab = _list.Where(t => t.Diab > 0 && t.NonDiab == 0).Select(t => t.ID).Distinct().Count();
                            _order.NonDiab = _list.Where(t => t.NonDiab > 0 && t.Diab == 0).Select(t => t.ID).Distinct().Count();
                            _order.Diab_NonDiab = _list.Where(t => t.Diab > 0 && t.NonDiab > 0).Select(t => t.ID).Distinct().Count();

                            scheduleList.Add(_order);
                        }
                    }
                    return  scheduleList;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<scheduleOrders>();
            }

        }
    }
    public class WareHouseStatusVM
    {

        public int? ID { get; set; }

     
        public int? Diab { get; set; }
        public int? NonDiab { get; set; }
    }

    public class OrdersGenerated
    {
        public int? ID { get; set; }

        public int? catid { get; set; }
    }

    public class scheduleOrders
    {
        public int? ID { get; set; }
        public int? count { get; set; }
        public DateTime? nextDate { get; set; }
        public int? Diab { get; set; }
        public int? NonDiab { get; set; }
        public int? Diab_NonDiab { get; set; }
        
    }

    public class EligSchVM
    {
        public IList<scheduleOrders> _schOrders { get; set; }
    }
}