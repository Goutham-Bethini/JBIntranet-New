using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{
    public class WeightCalculator
    {
        public static IList<WorkOrderDetail> GetWODetailByAccount(Int32? account, string operatorName)
        {
            DateTime _dt = DateTime.Today.AddDays(-180);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                try { 
                var _woList = (from p in _db.tbl_PS_WorkOrder
                               where p.Account == account &&
                               p.Cancel_Date == null &&
                               p.Request_Date > _dt
                               select new WorkOrderDetail
                               {
                                 Request_Date = p.Request_Date.Value,
                                 
                                 WorkOrderID=  p.ID,


                                   productDetails = (from wol in _db.tbl_PS_WorkOrderLine
                                                     join pro in _db.tbl_Product_Table
                                                     on wol.ID_Product equals pro.ID
                                                     join uom in _db.tbl_Inv_UOM_Table
                                                     on pro.ID_UOM equals uom.ID
                                                     //  from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                                     //  from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode).DefaultIfEmpty()
                                                     where wol.ID_PS_WorkOrder == p.ID
                                                     select new ProductDetails
                                                     {
                                                         Product = pro.ProductCode,
                                                         Description = pro.ProductDescription,
                                                         Ordered = wol.QtyOrdered,
                                                         Shipped = wol.QtyShipped,
                                                         UOM = uom.UOMName + " of " + pro.PerUnitQty,
                                                         UnitWeight = pro.UnitWeight

                                                     }).OrderBy(t=>t.Product).ToList()
                               }).OrderByDescending(t=>t.Request_Date).ToList();

                    string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',20,GETDATE())";

                    int rowsinsert = _db.Database.ExecuteSqlCommand(query);

                    return _woList;

                }
                catch (Exception ex) { string msg = ex.Message;
                    return new List<WorkOrderDetail>();

                }

           
            }
        }

        public static IList<ProductDetails> GetProduct_With_No_UW(DateTime? _start, DateTime? _end, string operatorName)
        {
            DateTime _dt = DateTime.Today.AddDays(-90);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                try
                {
                    var _proList = (from pro in _db.tbl_Product_Table
                                   from   wol in _db.tbl_PS_WorkOrderLine.Where(w =>w.ID_Product == pro.ID).DefaultIfEmpty()
                                   // on pro.ID equals wol.ID_Product
                                    from uom in _db.tbl_Inv_UOM_Table.Where(u=>u.ID == pro.ID_UOM)
                                 //   on pro.ID_UOM equals uom.ID
                                    //  from wo in _db.ERP_OrdersSent.Where(w => w.woWorkOrder == wol.ID_PS_WorkOrder).DefaultIfEmpty()
                                    //  from lin in _db.ERP_OrderLines.Where(w => w.linWOid == wo.woID && w.linProductCode == pro.ProductCode).DefaultIfEmpty()
                                    where (pro.UnitWeight == 0 || pro.UnitWeight == null) && pro.CreateDate >= _start && pro.CreateDate <= _end
                                    select new ProductDetails
                                    {
                                        Product = pro.ProductCode,
                                        Description = pro.ProductDescription,
                                        Ordered = wol.QtyOrdered,
                                        Shipped = wol.QtyShipped,
                                        UOM = uom.UOMName + " of " + pro.PerUnitQty,
                                        UnitWeight = pro.UnitWeight,
                                        CreateDate = pro.CreateDate

                                    }).OrderBy(t => t.CreateDate).ToList();
                    string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',32,GETDATE())";

                    int rowsinsert = _db.Database.ExecuteSqlCommand(query);

                    return _proList;

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return new List<ProductDetails>();

                }


            }
        }

    }



    public class WeightCal_Vm
    {
        public double? totalWt { get; set; }
        public WeightCal_Vm()
        {
            workOrderDetail = new List<WorkOrderDetail>();
        }
        [Required]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public Int64? Account { get; set; }

        public IList<WorkOrderDetail> workOrderDetail { get; set; }
        public AccountInfoVM details { get; set; }

        public IList<string> tabs { get; set; }
    

    }


    public class WorkOrderDetail
    {
        public bool checkbox { get; set; }
        public int WorkOrderID { get; set; }

        public int? Account { get; set; }
        public DateTime? Request_Date { get; set; }


        public double? totalProductsWt { get; set; }

        public IList<ProductDetails> productDetails { get; set; }
    }

    public class ProductWith_No_UW
    {
        public DateTime? _start { get; set; }
        public DateTime? _end { get; set; }
        public IList<ProductDetails> productDetails { get; set; }
    }

}