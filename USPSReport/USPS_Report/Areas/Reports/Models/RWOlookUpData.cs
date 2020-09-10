using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Data.SqlClient;
using USPS_Report.Models;

namespace USPS_Report.Areas.Reports.Models
{

    public class RWOLookUPReport
    {
        //public static IList<RWOlookUpData> GetRWOLookUP(DateTime? _startDt, DateTime? _endDt)
        //{
        //    try
        //    {

        //        using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //        {
                  
        //            DateTime? todayDate = DateTime.Now;
        //            var _list = (from rwo in _db.tbl_PS_RepeatingOrders
        //                         join pro in _db.tbl_Product_Table
        //                         on rwo.ID_Product equals pro.ID
        //                         join mem in _db.tbl_Account_Member
        //                         on rwo.Account equals mem.Account
        //                         from ins in _db.tbl_Account_Insurance.Where(w => w.Account == rwo.Account && (w.Expiration_Date == null || w.Expiration_Date > todayDate)).DefaultIfEmpty()
        //                         from pay in _db.tbl_Payer_Table.Where(p => p.ID == ins.ID_Payer).DefaultIfEmpty()
        //                         from loc in _db.tbl_DeliveryLocation_Table.Where(l => l.ID == rwo.ID_DeliveryLocation).DefaultIfEmpty()
        //                         from meth in _db.tbl_DeliveryMethod_Table.Where(m => m.ID == rwo.ID_DeliveryMethod).DefaultIfEmpty()
        //                             // where (rwo.NextRepeatDate >= _startDt && rwo.NextRepeatDate <= _endDt) and 
        //                         select new RWOlookUpData
        //                         {
        //                             Hold = rwo.Hold,
        //                             ProductCode = pro.ProductCode,
        //                             Account = rwo.Account,
        //                             First_Name = mem.First_Name,
        //                             Last_Name = mem.Last_Name,
        //                             Name = pay.Name,
        //                            // Location = loc.DeliveryLocationName,
        //                           //  Method = meth.MethodType

        //                         }).ToList();


        //            return _list;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        string msg = ex.Message;
        //        return new List<RWOlookUpData>();

        //    }
        //}

        public static IList<RWOlookUpData> RWOLookUPDetail(DateTime? _startDt, DateTime? _endDt, string _holdCode, string _productCode, string _payerId, int? _locationId, int? _methodId, string _InactiveCode, int? _delTimeId, string _ferqID, string _IsAssigned, string _MakeRWOIncomplete, string _ServiceType, string _HCPC, bool all)
        {
           
            try
            {
               // @ProductCode varchar(100),@HoldCode int,@startDt Datetime, @endDt Datetime
                using (ReportsEntities _db = new ReportsEntities())
                {
                    Int32 _serviceTypeId = 0;
                    Int32 _hold = 2;
                    Int32 _assigned = 2;
                    //  Int32 _PayId = 0;
                    string _PayId = "NULL";
                    Int32 _Inactive = 2;
                    Int32 _makeRwoIncomplete = 2;

                    if (_InactiveCode != null)
                        _Inactive = Convert.ToInt32(_InactiveCode);

                    if (_holdCode != null)
                         _hold = Convert.ToInt32(_holdCode) ;

                    if (_ServiceType != null)
                        _serviceTypeId = Convert.ToInt32(_ServiceType);

                    if (_IsAssigned != null)
                        _assigned = Convert.ToInt32(_IsAssigned);

                    if (_MakeRWOIncomplete != null)
                        _makeRwoIncomplete = Convert.ToInt32(_makeRwoIncomplete);

                    if (_productCode != null)
                        _productCode = _productCode.ToUpper();
                    else
                        _productCode = "NULL";

                    if (_payerId != null)
                        _PayId = _payerId;
                      //  _PayId = Convert.ToInt32(_payerId);

                    if (_locationId == null)
                        _locationId = 0;

                    if (_methodId == null)
                        _methodId = 0;

                    if (_delTimeId == null)
                        _delTimeId = 0;

                    if (_ferqID != null)
                        _ferqID = _ferqID.ToUpper();
                    else
                        _ferqID = "NULL";

                    if (_HCPC != null)
                        _HCPC = _HCPC.ToUpper();
                    else
                        _HCPC = "NULL";


                    if (all == true)
                    {
                        _startDt = DateTime.Now;
                        _endDt = DateTime.Now;
                    }
                    var ParamProductCode = new SqlParameter
                    {
                        ParameterName = "ProductCode",
                        Value = _productCode      // "COV7176"
                    };

                    var ParamHoldCode = new SqlParameter
                    {
                        ParameterName = "HoldCode",
                        Value = _hold  //"0"
                    };

                    var ParamStartDt = new SqlParameter
                    {
                        ParameterName = "startDt",
                        Value = _startDt //"9/23/2014"
                    };

                    var ParamEndDt = new SqlParameter
                    {
                        ParameterName = "endDt",
                        Value = _endDt  // "7/23/2015"
                    };

                    var ParamPayerId = new SqlParameter
                    {
                        ParameterName = "PayerID",
                        Value = _PayId  //"0"
                    };

                    var ParamlocationId = new SqlParameter
                    {
                        ParameterName = "locationID",
                        Value = _locationId  //"0"
                    };

                    var ParamMethodId = new SqlParameter
                    {
                        ParameterName = "MethodID",
                        Value = _methodId  //"0"
                    };

                    var ParamInactiveId = new SqlParameter
                    {
                        ParameterName = "Inactive",
                        Value = _Inactive  //"0"
                    };

                    var ParamDeliveryTimeId = new SqlParameter
                    {
                        ParameterName = "DelTimeID",
                        Value = _delTimeId  //"0"
                    };

                    var ParamFrequencyId = new SqlParameter
                    {
                        ParameterName = "FerqID",
                        Value = _ferqID  //"0"
                    };

                    var ParamIsAssigned = new SqlParameter
                    {
                        ParameterName = "IsAssigned",
                        Value = _assigned  //"0"
                    };

                    var ParamMakeRWoIncomplete = new SqlParameter
                    {
                        ParameterName = "MakeRWOInComplete",
                        Value = _makeRwoIncomplete  //"0"
                    };
                    var ParamServiceType = new SqlParameter
                    {
                        ParameterName = "ServiceTypeId",
                        Value = _serviceTypeId  //"0"
                    };

                    var ParamHCPC = new SqlParameter
                    {
                        ParameterName = "HCPCCode",
                        Value = _HCPC     
                    };

                    _db.Database.CommandTimeout = 0;
                    IList<RWOlookUpData> _rwoVM = new List<RWOlookUpData>();
                    if (all != true)
                    {
                     
                        _rwoVM = _db.Database.SqlQuery<RWOlookUpData>("exec sp_RWO_LookupWithDates @ProductCode,@HoldCode,@startDt, @endDt, @PayerID, @locationID, @MethodID , @Inactive, @DelTimeID, @FerqID, @IsAssigned,@MakeRWOInComplete,@ServiceTypeId,@HCPCCode", ParamProductCode, ParamHoldCode, ParamStartDt, ParamEndDt, ParamPayerId, ParamlocationId, ParamMethodId, ParamInactiveId, ParamDeliveryTimeId, ParamFrequencyId, ParamIsAssigned, ParamMakeRWoIncomplete, ParamServiceType, ParamHCPC).ToList<RWOlookUpData>();
                    }
                    else {
                       
                        _rwoVM = _db.Database.SqlQuery<RWOlookUpData>("exec sp_RWO_Lookup @ProductCode,@HoldCode,@startDt, @endDt, @PayerID, @locationID, @MethodID , @Inactive, @DelTimeID, @FerqID, @IsAssigned,@MakeRWOInComplete,@ServiceTypeId,@HCPCCode", ParamProductCode, ParamHoldCode, ParamStartDt, ParamEndDt, ParamPayerId, ParamlocationId, ParamMethodId, ParamInactiveId, ParamDeliveryTimeId, ParamFrequencyId, ParamIsAssigned, ParamMakeRWoIncomplete, ParamServiceType, ParamHCPC).ToList<RWOlookUpData>();

                    }

                    int? Totalcount = _rwoVM.Count;
                    int? accountCount = _rwoVM.Select(t => t.Account).Distinct().Count();
                    int rec = 1;
                    foreach (var item in _rwoVM)
                    {
                        if (rec == 1)
                        {
                            item.RecordCount = Totalcount;
                            item.DistinctAccCount = accountCount;
                        }
                        rec++;

                    }
                    return _rwoVM;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<RWOlookUpData>();

            }
        }

        public static IList<ProductListVm> GetProducts()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                //    SELECT ID, DeliveryLocationName
                //FROM tbl_deliverylocation_table
                //WHERE DeletedDate IS NULL
                //ORDER BY DeliveryLocationName


                    ProductListVm _prod = new ProductListVm();

                    var _list = (from pro in _db.tbl_Product_Table
                                 where (pro.Discontinued == false || pro.Discontinued == null) 
                                 && pro.DeletedDate == null
                                 select new ProductListVm
                                 {
                                     productID = pro.ID,
                                     productCode = pro.ProductCode
                                 }).Distinct().OrderBy(t => t.productCode).ToList();

                    _list.Insert(0, _prod);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ProductListVm>();
            }
        }

        public static IList<LoactionListVm> GetLocations()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    //    SELECT ID, DeliveryLocationName
                    //FROM tbl_deliverylocation_table
                    //WHERE DeletedDate IS NULL
                    //ORDER BY DeliveryLocationName


                    LoactionListVm _loc = new LoactionListVm();

                    var _list = (from loc in _db.tbl_DeliveryLocation_Table
                                 where loc.DeletedDate == null
                                
                                 select new LoactionListVm
                                 {
                                    locationID  = loc.ID,
                                    deliveryLocationName = loc.DeliveryLocationName
                                 }).Distinct().OrderBy(t => t.deliveryLocationName).ToList();

                    _list.Insert(0, _loc);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<LoactionListVm>();
            }
        }
       
        public static IList<MethodListVm> GetMethods()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    MethodListVm _met = new MethodListVm();

                    var _list = (from met in _db.tbl_DeliveryMethod_Table
                                 where met.DeletedDate == null

                                 select new MethodListVm
                                 {
                                     MethodID = met.ID,
                                     MethodName = met.DeliveryMethod
                                 }).Distinct().OrderBy(t => t.MethodName).ToList();

                    //_met.MethodID = -1;
                    //_met.MethodName = "Any";
                    _list.Insert(0, _met);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<MethodListVm>();
            }
        }

        public static IList<DeliveryTimeListVm> GetDeliveryTimes()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                   
                    DeliveryTimeListVm _tym = new DeliveryTimeListVm();

                    var _list = (from tym in _db.tbl_DeliveryTimes_Table
                               
                                 select new DeliveryTimeListVm
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
                return new List<DeliveryTimeListVm>();
            }
        }

        public static IList<FrequencyListVm> GetFrequencyTitle()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    FrequencyListVm _freq = new FrequencyListVm();

                    var _list = (from freq in _db.tbl_Name_Frequency
                                 where freq.Comment != null
                                 select new FrequencyListVm
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
                return new List<FrequencyListVm>();
            }
        }
    }
    public class RWOlookUpData
    {
        public int? RecordCount { get; set; }
        public int? DistinctAccCount { get; set; }
        public string Hold { get; set; }
        public string ProductCode { get; set; }
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int? Qty { get; set; }
        public DateTime? NextRepeatDate { get; set; }
        public string UOM { get; set; }
      //  public int? PerUnitQty { get; set; }
        //public string Procedure_Code { get; set; }
        //public string PayerName { get; set; }
        public int InActive { get; set; }
        public int? ServiceType { get; set; }
        public string DeliveryLocationName { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryTime { get; set; }
        public int? AcceptAssignment { get; set; }
        public int? MakeRwoIncomplete { get; set; }
        public string PriIns { get; set; }


    }


    public class RWOLookUPVM

    {
        public bool all { get; set; }
        public int? TotalRecord { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProductCode { get; set; }

        public string HCPC { get; set; }
        public string HoldCode { get; set; }
        public string ServiceType { get; set; }
        public string IsAssigned { get; set; }
        public string makeRwoIncomplete { get; set; }
        public string FreqID { get; set; }
        public string InactiveORActive { get; set; }

        public int? locationId { get;set; }
        public int? methodId { get; set; }
        public int? delTimeId { get; set; }
        public string  PayerId { get; set; } //int?
        //public IList<ProductListVm> ProductList { get; set; }
        public IList<MethodListVm> MethodList { get; set; }
        public IList<DeliveryTimeListVm> DeliveryTimeList { get; set; }
        public IList<LoactionListVm> locationList { get; set; }

        public IList<FrequencyListVm> FrequencyList { get; set; }
        public IList<RWOlookUpData> rwoLookUp { get; set; }

       
    }

    public class HeldOrdersVM
    { 
        public int ID { get; set; }
        public int Account { get; set; }
        public DateTime? Request_Date { get; set; }
        public string HoldFromShippingReason { get; set; }
    }

    public class FrequencyListVm
    {
        public string freqComment { get; set; }
        public string freqTitle { get; set; }

    }
    public class LoactionListVm
    {
        public Int32? locationID { get; set; }
        public string deliveryLocationName { get; set; }

    }

    public class DeliveryTimeListVm
    {
        public Int32? TimeID { get; set; }
        public string DeliveryTime { get; set; }

    }
    public class MethodListVm
    {
        public Int32? MethodID {get; set; }
        public string MethodName { get; set; }

    }
    public class ProductListVm
    {
        public Int32? productID { get; set; }
        public string productCode { get; set; }

    }
}