using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using DotNet.Highcharts;

namespace USPS_Report.Areas.Reports.Models
{
    public class ReportWOHolds
    {
        public static IList<WoOnHoldVM> GetAllWoHolds(string Instype)
        {

            if (Instype == "Others")
                Instype = "";



            IList<WoOnHoldVM> _woVM = new List<WoOnHoldVM>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                if (Instype == "AllHolds")
                {
                    _woVM = _db.Database.SqlQuery<WoOnHoldVM>("exec GetWoHolds").ToList<WoOnHoldVM>();
                }
                else
                {
                    _woVM = _db.Database.SqlQuery<WoOnHoldVM>("exec GetWoHolds").Where(t => t.InsType == Instype).ToList<WoOnHoldVM>();

                }
                return _woVM;
            }
        }


        public static IList<woHoldTypes_Qty> GetAllWoHoldTypes_Qty()
        {
            IList<woHoldTypes_Qty> _woInsTandQVM = new List<woHoldTypes_Qty>();
            IList<WoOnHoldVM> _woVM = new List<WoOnHoldVM>();
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {

                //_woVM = _db.Database.SqlQuery<WoOnHoldVM>("exec GetWoHolds").ToList<WoOnHoldVM>();

                //var _list = (from a in _woVM
                //             group a by new { a.InsType } into t
                //             select new woHoldTypes_Qty
                //             {
                //                 // InsType = t.Key.InsType!= null ? t.Key.InsType : "Others",
                //                 InsType = ((t.Key.InsType == null) || (t.Key.InsType == "") ? "Others" : t.Key.InsType),
                //                 Count = t.Count()
                //             }).OrderBy(a => a.InsType).ToList();

                //_woInsTandQVM = (from t in _list
                //                 group t by new { t.InsType } into p
                //                 select new woHoldTypes_Qty
                //                 {
                //                     InsType = p.Key.InsType,
                //                     Count = p.Sum(t => t.Count)
                //                 }).OrderBy(a => a.InsType).ToList<woHoldTypes_Qty>();
                return _woInsTandQVM;


        
            }

        }

    }

    public class WoOnHoldVM
    {
        public int? Account { get; set; }
        public int ID { get; set; }
        public string HoldFromShippingReason { get; set; }
        public DateTime? Request_date { get; set; }
        public string FullName { get; set; }
        public string InsType { get; set; }
        public string Reference { get; set; }
        public string PayerName { get; set; }



    }

    public class woHoldTypes_Qty
    {
        public string InsType { get; set; }
        public int Count { get; set; }
    }


    public class HoldPayerVM
    {
        public IList<woHoldTypes_Qty> GetHoldPayer { get; set; }
        public Highcharts HoldPayerPieChart { get; set; }
    }

    public class DisconProdVM
    {
        public int Account { get; set; }
        public string Description { get; set; }
    }

    public class DiscontinuedProductReport
    {
        public static IList<DisconProdVM> GetDisconProd()
        {
            try
            {
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from p in _db.tbl_Product_Table
                                 join ro in _db.tbl_PS_RepeatingOrders
                                 on p.ID equals ro.ID_Product

                                 where p.Discontinued == true
                                 select new DisconProdVM
                                 {
                                     Account = ro.Account,
                                     Description = p.ProductDescription
                    
                                     
                                 }).ToList();

                    return _list;
                }
            }
            catch (Exception ex)
            {
                string var = ex.Message;
                return new List<DisconProdVM>();
            }


        }
    }
}