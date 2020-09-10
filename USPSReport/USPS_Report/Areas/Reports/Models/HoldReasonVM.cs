using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using DotNet.Highcharts;

namespace USPS_Report.Areas.Reports.Models
{
    public class HoldCountByReasonReport
    {
        public static IList<HoldReasonList> GetHoldCountByReason()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    DateTime? _dt = DateTime.Now.AddDays(-10).Date;
                    var _list = (from t in _db.tbl_PS_WorkOrder
                                 where 
                                  t.Cancel_Date == null
                                  && t.Completed_Date == null
                                  &&t.LastPrintDate == null
                                 && t.HoldFromShipping == 1
                                 

                                 select t
                               ).ToList();

                    int? TotalCount = _list.Count();
                    int? TotalCounttemp = TotalCount;

                    string[] holdArr = {"AOB","FREQ","PA_NOT_ATT","AAA","CONFIRM"};

                    IList<HoldReasonList> _lst4 = new List<HoldReasonList>();
                    
                    foreach (var Item in holdArr)
                    {
                        HoldReasonList _vm = new HoldReasonList();
                        _vm.HoldReason = Item;
                        _vm.totalCount = TotalCounttemp;
                        _vm.Count = _list.Where(t=>t.HoldFromShippingReason!=null && t.HoldFromShippingReason.Contains(Item)).Count();
                        TotalCount = TotalCount - _vm.Count;
                          
                        _lst4.Add(_vm);
                    }

                    HoldReasonList _h = new HoldReasonList();
                    _h.HoldReason = "Others";
                    _h.Count = TotalCount;
                    _lst4.Add(_h);

                
                    return _lst4; 
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<HoldReasonList>();
            }


        }


    }

    public class HoldReasonVM
    {
       
       public  IList<HoldReasonList> GetHoldReason { get; set; }
        public Highcharts HoldReasonPieChart { get; set; }
    }
    public class HoldReasonList
    {
        public int? totalCount { get; set; }
        public int? Count { get; set; }
        public string  HoldReason { get; set; }

    }


}