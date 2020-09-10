using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using DotNet.Highcharts;

namespace USPS_Report.Areas.Reports.Models
{

    public class ShippingOrderReport
    {
        public static IList<ShippingOrderData> GetShippingOrders()
        {

            try
            {
         
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                   
                    DateTime? _dt = DateTime.Now.AddDays(-7).Date;
                    DateTime? todayDate = DateTime.Now;

                    var _list = (from ups in _db.tbl_UPS_WorkOrders
                                 where ups.DatePosted >= _dt
                                 select new testVM {

                                     postdate= ups.DatePosted,
                                 workorder = ups.ID_WorkOrder}).Distinct().ToList<testVM>();




                    var _list2 = (from p in _list
                                  group p by new {
                                      Date = p.postdate.Value.Date,
                                      Hour = p.postdate.Value.Hour
                                  } into g

                                  select new ShippingOrderData
                                  {
                                      Date = g.Key.Date,
                                      Worked = g.Count(),
                                      Hours = g.Key.Hour 
                                  }).OrderBy(p=>p.Date).ToList();



                    return _list2;
                }
            }
            catch (Exception ex)
            {
               string msg=  ex.Message;
                return new List<ShippingOrderData>();
            }


        }


    }
    public class ShippingOrderVM
    {
      public IList<ShippingOrderData> ShipData { get; set; }
        public Highcharts ShippingChart { get; set; }

    }

    public class ShippingOrderData
    {
        public DateTime? Date { get; set; }
        public int? Hours { get; set; }

        public int? Worked { get; set; }

    }

    public class testVM
    {
        public DateTime? postdate { get; set; }
        public int? workorder { get; set; }
    }
}