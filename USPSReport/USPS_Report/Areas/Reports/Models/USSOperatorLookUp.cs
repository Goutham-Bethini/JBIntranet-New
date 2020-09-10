using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{

    public class USSOperatorLookUPReport
    {

        public static IList<USSOperatorLookUp> GetUSSOperatorLoopUp()
        {
            try {

                IList<USSOperatorLookUp> _list = new List<USSOperatorLookUp>();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    _list = _db.Database.SqlQuery<USSOperatorLookUp>("SELECT "+
               " op.ID, "+
               " LegalName, "+
              "  opd.DeptName, "+
              "  CASE "+
                   " WHEN DeletedDate IS NOT NULL OR InactiveDate IS NOT NULL THEN 'No'  "+
                   " ELSE 'Yes' "+
             "   END AS Active "+
           " FROM "+
                           " tbl_operator_table              op  "+
              "  LEFT JOIN   tbl_operatordepartments_table   opd ON opd.id = op.id_dept  "+
          "  ORDER BY op.id").ToList<USSOperatorLookUp>();

                    return _list;
                }
            }
            catch(Exception ex) {
                string var = ex.Message;
                return new List<USSOperatorLookUp>();
            }
        }
    }
    public class USSOperatorLookUp
    {
        public int ID { get; set; }
        public string LegalName { get; set; }
        public string DeptName { get; set; }
        public string Active { get; set; }
    }

    public class USSOperatorLookUpVM
    {
        public IList<USSOperatorLookUp> uSSOperatorLookUp { get; set; }
    }
}