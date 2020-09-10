using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Web.Mvc;

namespace USPS_Report.Areas.Reports.Models
{

    public class ProductsReport
    {
        public static IList<ProductsData> GetProducts(DateTime? _startDt, DateTime? _endDt, Int32? id, string prodCode)
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    if (prodCode != null && prodCode != "")
                    {
                        var _list = (from pro in _db.tbl_Product_Table
                                     join ro in _db.tbl_PS_RepeatingOrders
                                     on pro.ID equals ro.ID_Product
                                    
                                     where ro.NextRepeatDate >= _startDt && ro.NextRepeatDate <= _endDt
                                        && pro.ProductCode == prodCode
                                   
                                    
                                     select new
                                     {
                                         pro.ID,
                                         pro.ProductCode,
                                         pro.ProductDescription,
                                         ro.Qty

                                     }
                                    ).ToList();

                        var _list1 = (from p in _list
                                      group p by new
                                      {
                                          p.ProductCode,
                                          p.ProductDescription,
                                          p.ID

                                      } into t
                                      from qt in _db.QuantityOnHand.Where(q => q.ID_Product == t.Key.ID)

                                      //join qt in _db.QuantityOnHands 
                                      //on t.Key.ID equals qt.ID_Product
                                      select new ProductsData
                                      {
                                          Productcode = t.Key.ProductCode,
                                          ProductDescription = t.Key.ProductDescription,
                                          ROQty = t.Sum(p => p.Qty),
                                          ProdonHand = qt.QtyOnHand,
                                          Difference = qt.QtyOnHand - t.Sum(p => p.Qty)



                                                   }).OrderBy(g => g.Productcode).ToList<ProductsData>();
                       
                      
                       
                       
                        return _list1;
                    }
                    else if (id > 0) {
                        var _list = (from pro in _db.tbl_Product_Table
                                     join ro in _db.tbl_PS_RepeatingOrders
                                     on pro.ID equals ro.ID_Product
                                     
                                     where ro.NextRepeatDate >= _startDt && ro.NextRepeatDate <= _endDt

                                     select new
                                     {
                                         pro.ID,
                                         pro.ProductCode,
                                         pro.ProductDescription,
                                         ro.Qty
                                     }
                                   ).ToList();

                        var _list1 = (from t in _list
                                      join vp in _db.tbl_Inv_VendorProduct_Table
                                      on t.ID equals vp.ID_Product
                                      join inv in _db.tbl_Inv_Vendor_Table
                                      on vp.ID_Vendor equals inv.ID
                                      where inv.ID == id
                                      select new
                                      {
                                          t.ProductCode,
                                          t.ProductDescription,
                                          t.Qty,
                                          inv.VendorName

                                      }).ToList();

                        var _list2 = (from p in _list1
                                      group p by new
                                      {
                                          p.ProductCode,
                                          p.ProductDescription,
                                         p.VendorName

                                      } into t
                                      select new ProductsData
                                      {
                                          Productcode = t.Key.ProductCode,
                                          ProductDescription = t.Key.ProductDescription,
                                          ROQty = t.Sum(p => p.Qty),
                                          vendorName = t.Key.VendorName
                                      }).OrderBy(g => g.Productcode).ToList<ProductsData>();

                        return _list2;
                    }
                    else
                    {
                        var _list = (from pro in _db.tbl_Product_Table
                                     join ro in _db.tbl_PS_RepeatingOrders
                                     on pro.ID equals ro.ID_Product
                                     where ro.NextRepeatDate >= _startDt && ro.NextRepeatDate <= _endDt

                                     select new
                                     {
                                         pro.ProductCode,
                                         pro.ProductDescription,
                                         ro.Qty
                                     }
                                     ).ToList();

                        var _list2 = (from p in _list
                                      group p by new
                                      {
                                          p.ProductCode,
                                          p.ProductDescription,

                                      } into t
                                      select new ProductsData
                                      {
                                          Productcode = t.Key.ProductCode,
                                          ProductDescription = t.Key.ProductDescription,
                                          ROQty = t.Sum(p => p.Qty)
                                          
                                      }).OrderBy(g => g.Productcode).ToList<ProductsData>();

                        return _list2;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<ProductsData>();
            }


        }

        public static IList<VendorNameVm> GetVendorName()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    VendorNameVm vn = new VendorNameVm();
                    
                    var _list = (from v in _db.tbl_Inv_Vendor_Table
                                 select new VendorNameVm
                                 {
                                     vendorid = v.ID,
                                     VendorName = v.VendorName
                                 }).Distinct().OrderBy(t => t.VendorName).ToList();

                    _list.Insert(0, vn);

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<VendorNameVm>();
            }
        }
    }

    public class ProductsData
    {
        public string  Productcode { get; set; }
        public string ProductDescription { get; set; }

        public string vendorName { get; set; }

        public int? ROQty { get; set; }

        public int ID { get; set; }

        public int? ProdonHand { get; set; }

        public int? Difference { get; set; }
    }

    public class ProductVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string productCode { get; set; }
        public IList<ProductsData> productData { get; set; }

        public int? vendorid { get; set; }

        
        public SelectList VendorList { get; set; }
     
    }

        public class VendorNameVm
        {
            public Int32? vendorid { get; set; }
            public string VendorName { get; set; }
        
        }
}