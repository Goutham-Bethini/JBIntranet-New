using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;

namespace USPS_Report.Areas.Reports.Models
{

    public class RWOMultipleLocationsReport
    {
        public static IList<RWOMultipleLocationsData> GetRWOMultipleLocations()
        {

            try
            {

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {

                    var _list = (from rwo in _db.tbl_PS_RepeatingOrders
                                 join loc in _db.tbl_DeliveryLocation_Table
                                 on rwo.ID_DeliveryLocation equals loc.ID
                                 from mem in _db.tbl_Account_Member.Where(m=>m.Account==rwo.Account && m.Member == rwo.Member).DefaultIfEmpty()
                            
                                
                                 select new
                                 {
                                     mem.First_Name,
                                     mem.Last_Name,
                                     rwo.Account,
                                     loc.DeliveryLocationName,
                                  
                                 }
                                 ).Distinct().ToList();

                    var _list2 = (from a in _list
                                  group a by new
                                  {
                                  
                                      a.Account,
                                      a.First_Name,
                                      a.Last_Name
                                     
                                  } into t
                                  where t.Count() > 1
                                  select new RWOMultipleLocationsData
                                  {
                                      Account = t.Key.Account,
                                      FirstName = t.Key.First_Name,
                                      LastName = t.Key.Last_Name,
                                      Locations = t.Count(),
                                      Location1 = t.Min(p => p.DeliveryLocationName),
                                      Location2 = t.Max(p=>p.DeliveryLocationName)
                                  }).OrderBy(t=>t.Account).ToList();


                    return _list2;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<RWOMultipleLocationsData>();
            }


        }


    }
    public class RWOMultipleLocationsData
    {
        public int? Account { get; set; }


        public string  FirstName { get; set; }

        public string LastName { get; set; }

        public int? Locations { get; set; }

        public string Location1 { get; set; }

        public string Location2 { get; set; }

    }


    public class RWOMultipleLocationsVM
    {
        public IList<RWOMultipleLocationsData> rwoMultipleLocations { get; set; }
    }
}