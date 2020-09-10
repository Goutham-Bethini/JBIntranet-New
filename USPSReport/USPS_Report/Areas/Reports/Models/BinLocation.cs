using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{

    public class BinLocationReport {

        public static IList<BinLocationData> GetBinLocation()
        {
            try
            {
                IList<BinLocationTemp> _listTemp = new List<BinLocationTemp>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    _listTemp = _db.Database.SqlQuery<BinLocationTemp>("Select pro.id,pro.productcode,pro.id_productcategory, pro.productdescription,"+
     " (select binlocationcomment From tbl_Inv_ProductLocation_table  Where id_product = pro.id  and id_deliverylocation = 1) as DIS,"+
     " (select binlocationcomment From tbl_Inv_ProductLocation_table Where id_product = pro.id  and id_deliverylocation = 6) as DME" +
       "  From Tbl_Product_Table   pro  JOIN        quantityonhand qoh on qoh.id_product = pro.id  Where" +
    "   pro.discontinued = 0  And ID_ProductCategory not in (52,0)  and qoh.qtyOnhand > 0"+
    "   Order By productcode,pro.id_productcategory").ToList<BinLocationTemp>();

                    var _list = (from lst in _listTemp
                                from pc in _db.tbl_ProductCategory_Table.Where(p=>p.ID == lst.ID_ProductCategory).DefaultIfEmpty()

                                 where (lst.DIS == null ||lst.DIS == "") && ( lst.DME == null || lst.DME == "")
                                 select new BinLocationData
                                 {
                                   ProdCategory = pc.CategoryDescription,
                                     ProdCode = lst.Productcode,
                                     ProdDescription = lst.ProductDescription
                                 }).ToList<BinLocationData>();

                    return _list;

                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<BinLocationData>();
            }

        }
    }
    public class BinLocationData
    {
        public string ProdCategory { get; set; }
        public string ProdCode { get; set; }
        public string ProdDescription { get; set; }
    }

    public class BinLocationVM
    {
        public IList<BinLocationData> binLocationData { get; set; }
    }

    public class BinLocationTemp
    {
        public int ID { get; set; }
        public string  Productcode { get; set; }
        public int? ID_ProductCategory { get; set; }

        public string ProductDescription { get; set; }

        public string DIS { get; set; }
        public string DME { get; set; }
    }
}