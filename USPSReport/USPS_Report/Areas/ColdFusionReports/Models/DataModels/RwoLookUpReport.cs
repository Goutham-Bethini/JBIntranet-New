using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.Entity.SqlServer;
using USPS_Report.Models;
using System.ComponentModel.DataAnnotations;
using  System.Globalization;


namespace USPS_Report.Areas.ColdFusionReports.Models.DataModels
{
    public class RwoLookUpReport
    {
        public static IList<ProductListViewModel> GetProducts()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    ProductListViewModel _prod = new ProductListViewModel();

                    var _list = (from pro in _db.tbl_Product_Table
                                 where (pro.Discontinued == false || pro.Discontinued == null)
                                 && pro.DeletedDate == null
                                 select new ProductListViewModel
                                 {
                                     productID = pro.ID,
                                     productCode = pro.ProductCode
                                 }).Distinct().OrderBy(t => t.productCode).ToList();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ProductListViewModel>();
            }
        }

        public static IList<LoactionListViewModel> GetLocations()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    LoactionListViewModel _loc = new LoactionListViewModel();

                    var _list = (from loc in _db.tbl_DeliveryLocation_Table
                                 where loc.DeletedDate == null

                                 select new LoactionListViewModel
                                 {
                                     locationID = loc.ID,
                                     deliveryLocationName = loc.DeliveryLocationName
                                 }).Distinct().OrderBy(t => t.deliveryLocationName).ToList();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<LoactionListViewModel>();
            }
        }

        public static IList<MethodListViewModel> GetMethods()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    MethodListViewModel _met = new MethodListViewModel();

                    var _list = (from met in _db.tbl_DeliveryMethod_Table
                                 where met.DeletedDate == null

                                 select new MethodListViewModel
                                 {
                                     MethodID = met.ID,
                                     MethodName = met.DeliveryMethod
                                 }).Distinct().OrderBy(t => t.MethodName).ToList();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<MethodListViewModel>();
            }
        }

        public static IList<string> GetPayers()
        {

            try
            {

                IList<string> _list = new List<string>();
                string sql = @"SELECT DISTINCT name
				                                FROM tbl_payer_table
				                                WHERE
						                                name NOT LIKE 'DNB%'
					                                AND Discontinued=0
				                                ORDER BY name
                                ";
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _list = _db.Database.SqlQuery<string>(sql).ToList<string>();
                }
                return _list;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<string>();
            }
        }

        public static void setOtherSearchDetails(RwoLookUpSearchViewModel _vm)
        {
            IList<int?> _Yearlist = new List<int?>();
            string _Yearsql = @"SELECT DISTINCT
								DatePart(yyyy,rwo.NextRepeatDate) AS ThisYear
							FROM tbl_ps_repeatingorders	rwo
							WHERE
									DatePart(yyyy,rwo.NextRepeatDate) IS NOT NULL
								AND DatePart(yyyy,rwo.NextRepeatDate) <> ''
							ORDER BY DatePart(yyyy,rwo.NextRepeatDate)
                                ";
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _Yearlist = _db.Database.SqlQuery<int?>(_Yearsql).ToList<int?>();
                _vm.RWOYear = _Yearlist;
            }
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            for (int i = 1; i < names.Length; i++)
            {
                _vm.RWOMonth.Add(i);
            }
            for (int i = 1; i <= 31; i++)
            {
                _vm.RWODay.Add(i);
            }
            for (int i = 0; i <= 2; i++)
            {
                if (i==0)
                {
                    HoldingViewModel _HVM = new HoldingViewModel();
                    _HVM.ID = i;
                    _HVM.Name = "Holding & Not Holding";
                    _vm.RWOHolding.Add(_HVM);
                }
                else if (i==1)
                {
                    HoldingViewModel _HVM = new HoldingViewModel();
                    _HVM.ID = i;
                    _HVM.Name = "Holding Only";
                    _vm.RWOHolding.Add(_HVM);
                }
                else
                {
                    HoldingViewModel _HVM = new HoldingViewModel();
                    _HVM.ID = i;
                    _HVM.Name = "Not Holding Only";
                    _vm.RWOHolding.Add(_HVM);
                }
            }

        }

        public static IList<DeliveryTimeListViewModel> GetDeliveryTimes()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    DeliveryTimeListViewModel _tym = new DeliveryTimeListViewModel();

                    var _list = (from tym in _db.tbl_DeliveryTimes_Table

                                 select new DeliveryTimeListViewModel
                                 {
                                     TimeID = tym.ID,
                                     DeliveryTime = tym.DeliveryTime
                                 }).Distinct().OrderBy(t => t.DeliveryTime).ToList();
                    //_tym.TimeID = -1;
                    //_tym.DeliveryTime = "Any";
                    _list.Insert(0, _tym);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<DeliveryTimeListViewModel>();
            }
        }

        public static IList<FrequencyListViewModel> GetFrequencyTitle()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    FrequencyListViewModel _freq = new FrequencyListViewModel();

                    var _list = (from freq in _db.tbl_Name_Frequency
                                 where freq.Comment != null
                                 select new FrequencyListViewModel
                                 {
                                     freqTitle = freq.Title,
                                     freqComment = freq.Comment
                                 }).Distinct().OrderBy(t => t.freqTitle).ToList();

                    _list.Insert(0, _freq);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<FrequencyListViewModel>();
            }
        }

        public IList<RwoLookUpResultVM> GetDetails(RwoLookUpSearchViewModel _vm)
        {
            IList<RwoLookUpResultVM> _list = new List<RwoLookUpResultVM>();
            string sql = GetDataQuery(_vm);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<RwoLookUpResultVM>(sql).ToList<RwoLookUpResultVM>();
            }
            return _list;
        }

        public void GetDetailsCount(RwoLookUpSearchViewModel _vm)
        {
            IList<RwoLookUpResultVM> _list = new List<RwoLookUpResultVM>();
            string sql = GetDataQuery(_vm);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                _list = _db.Database.SqlQuery<RwoLookUpResultVM>(sql).ToList<RwoLookUpResultVM>();
            }
            _vm.AccountsCount = _list.Count();
        }

        private string GetDataQuery(RwoLookUpSearchViewModel vm)
        {
            string sql = @"SELECT
					rwo.Hold,
					pro.ProductCode,
					rwo.Account,
					mem.First_Name,
					mem.Last_Name,
					rwo.Qty,
					rwo.NextRepeatDate";
            if (!string.IsNullOrEmpty(vm.PayerListSelected))
            {
                sql += ",pay.Name ";
            }
            sql += @"  FROM
								tbl_ps_repeatingorders	rwo
					JOIN		tbl_product_table		pro	ON pro.id=rwo.id_product
					JOIN		tbl_account_member		mem	ON mem.account=rwo.account
																AND mem.member=rwo.member";

            if (!string.IsNullOrEmpty(vm.PayerListSelected))
            {
                sql += @"  LEFT JOIN	tbl_account_insurance	ins	ON ins.account=rwo.account
																	AND (ins.expiration_date IS NULL OR ins.expiration_date > GetDate())
																	AND (ins.effective_date IS NULL OR ins.effective_date < GetDate())
						LEFT JOIN	tbl_payer_table			pay	ON pay.id=ins.id_payer ";
            }

            sql += " WHERE 1 = 1 ";

            if (vm.ProductCodeSelected != 0 && vm.ProductCodeSelected.HasValue == true)
            {
                sql += " AND pro.ID =" + vm.ProductCodeSelected + "";
            }
            if (vm.HoldingSelected == 1)
            {
                sql += " AND rwo.Hold=1 ";
            }
            if (vm.HoldingSelected == 2)
            {
                sql += " AND (rwo.Hold IS NULL OR rwo.Hold=0) ";
            }
            if (vm.RWOMonthSelected != 0 && vm.RWOMonthSelected.HasValue == true)
            {
                sql += " AND DatePart(m,rwo.NextRepeatDate)= " + vm.RWOMonthSelected + "";
            }
            if (vm.RWODaySelected != 0 && vm.RWODaySelected.HasValue == true)
            {
                sql += " AND DatePart(d,rwo.NextRepeatDate)= " + vm.RWODaySelected + "";
            }
            if (vm.RWOYearSelected != 0 && vm.RWOYearSelected.HasValue == true)
            {
                sql += " AND DatePart(yyyy,rwo.NextRepeatDate)= " + vm.RWOYearSelected + "";
            }
            if (vm.PayerListSelected != "" && vm.PayerListSelected != null)
            {
                sql += " AND ltrim(rtrim(pay.Name))= '" + vm.PayerListSelected + "'";
            }
            if (vm.LocationListSelected != 0 && vm.LocationListSelected.HasValue == true)
            {
                sql += " AND ID_DeliveryLocation= " + vm.ProductCodeSelected + "";
            }
            if (vm.MethodListSelected != 0 && vm.MethodListSelected.HasValue == true)
            {
                sql += " AND ID_DeliveryMethod= " + vm.MethodListSelected + "";
            }



            return sql;
        }
    }
    public class HoldingViewModel
    {
        public int? ID { get; set; }
        public string Name { get; set; }
    }
    public class HeldOrdersViewModel
    {
        public int ID { get; set; }
        public int Account { get; set; }
        public DateTime? Request_Date { get; set; }
        public string HoldFromShippingReason { get; set; }
    }

    public class FrequencyListViewModel
    {
        public string freqComment { get; set; }
        public string freqTitle { get; set; }

    }
    public class LoactionListViewModel
    {
        public Int32? locationID { get; set; }
        public string deliveryLocationName { get; set; }

    }

    public class DeliveryTimeListViewModel
    {
        public Int32? TimeID { get; set; }
        public string DeliveryTime { get; set; }

    }
    public class MethodListViewModel
    {
        public Int32? MethodID { get; set; }
        public string MethodName { get; set; }

    }
    public class ProductListViewModel
    {
        public Int32? productID { get; set; }
        public string productCode { get; set; }

    }

    public class RwoLookUpSearchViewModel
    {
        public RwoLookUpSearchViewModel()
        {
            RWODay = new List<int?>();
            RWOHolding = new List<HoldingViewModel>();
            RWOMonth = new List<int?>();
        }
        
        public IList<ProductListViewModel> ProductCodeList { get; set; }
        
        public int? ProductCodeSelected { get; set; }
        public int? HoldingSelected { get; set; }
        public int AccountsCount { get; set; }
        public IList<HoldingViewModel> RWOHolding { get; set; }
        public IList<int?> RWOMonth { get; set; }
        public IList<int?> RWODay { get; set; }
        public IList<int?> RWOYear { get; set; }
        public int? RWOMonthSelected { get; set; }
        public int? RWODaySelected { get; set; }
        public int? RWOYearSelected { get; set; }
        public IList<LoactionListViewModel> LocationList { get; set; }
        public IList<MethodListViewModel> MethodList { get; set; }
        public int? LocationListSelected { get; set; }
        public int? MethodListSelected { get; set; }
        public string PayerListSelected { get; set; }

        public IList<string> PayerList { get; set; }

    }

    public class RwoLookUpResultVM
    {
        public Int16? Hold { get; set; }
        public string ProductCode { get; set; }
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? NextRepeatDate { get; set; }
        public string Name { get; set; }
    }


}