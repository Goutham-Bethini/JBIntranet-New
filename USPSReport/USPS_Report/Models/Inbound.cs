using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Text;
using System.Data.Entity;

namespace USPS_Report.Models
{
    public class Inbound
    {
        public static void AddNoteInsuranceChange(InsChangeVM _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();
                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "INSURANCE";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 3;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "INSURANCE").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_note != null)
                    {
                        StringBuilder noteString = new StringBuilder();

                        noteString = noteString.Append("Member provided insurance through smart action self service to the following:");
                        noteString = noteString.Append(Environment.NewLine + "Insurance Provider :" + _vm.InsuranceProvider  );
                        noteString = noteString.Append(Environment.NewLine + "Insurance Provider ID:" + _vm.InsuranceProviderID);
                        noteString = noteString.Append(Environment.NewLine + "Insurance Provider Phone:" + _vm.InsuranceProviderPhone);
                        if (_vm.UpdateStatus == true)
                        {
                            noteString = noteString.Append(Environment.NewLine + "Insurance updated in HDMS");
                        }
                        else
                        {
                            noteString = noteString.Append(Environment.NewLine + "Insurance infomartion is not complete : did not update in HDMS");
                        }
                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;

                        _tHist.NoteText = noteString.ToString();

                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }

        public static void AddNoteDoctorChange(DoctorChangeVM _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();
                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "DEMOGRAPHICS";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 4;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_note != null)
                    {
                        StringBuilder noteString = new StringBuilder();

                        noteString = noteString.Append("Member provided Doctor information through smart action self service to the following:");
                        noteString = noteString.Append(Environment.NewLine + "Doctor Name :" + _vm.DoctorName);
                        noteString = noteString.Append(Environment.NewLine + "Doctor Phone : " + _vm.DoctorPhone);
                        noteString = noteString.Append(Environment.NewLine + "Doctor Location :" + _vm.DoctorLocation);
                        if (_vm.UpdateStatus == true)
                        {
                            noteString = noteString.Append(Environment.NewLine + "Doctor updated in HDMS");
                        }
                        else
                        {
                            noteString = noteString.Append(Environment.NewLine + "Doctor infomartion is not complete : did not update in HDMS");
                        }
                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;

                        _tHist.NoteText = noteString.ToString();

                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }
        public static void AddNoteAddressChange(NewAdd _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "DEMOGRAPHICS";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 4;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_note != null)
                    {
                        StringBuilder noteString = new StringBuilder();

                        noteString = noteString.Append("Member changed address through smart action self service to the following:");
                        noteString = noteString.Append(Environment.NewLine + Environment.NewLine);

                        if ((_vm.B_City != "" && _vm.B_City != null) || (_vm.B_State != "" && _vm.B_State != null)
                             || (_vm.B_Zip != "" && _vm.B_Zip != null))
                        {
                            noteString = noteString.Append("Billing Address" + Environment.NewLine + Environment.NewLine);
                            noteString = noteString.Append(_vm.B_Address1+ " "+ _vm.B_Address2 + Environment.NewLine);
                            noteString = noteString.Append(_vm.B_City + ", " + _vm.B_State + ", " + _vm.B_Zip + Environment.NewLine + Environment.NewLine);
 
                        }


                        if ((_vm.S_City != "" && _vm.S_City != null) || (_vm.S_State != "" && _vm.S_State != null)
                          || (_vm.S_Zip != "" && _vm.S_Zip != null))
                        {
                            noteString = noteString.Append("Shipping Address" + Environment.NewLine + Environment.NewLine);
                            noteString = noteString.Append(_vm.S_Address1 + " " + _vm.S_Address2 + Environment.NewLine);
                            noteString = noteString.Append(_vm.S_City + ", " + _vm.S_State + ", " + _vm.S_Zip + Environment.NewLine + Environment.NewLine);
  
                        }

                        noteString = noteString.Append("Verified through USPS.com");

                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                         
                        _tHist.NoteText = noteString.ToString();
                         
                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);
                       
                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }
        public static void AddNoteInboundSupplies(ReorderSuppliesVM _vm)
        {
            try
            {

                string SpokeWith = "";
                string RelationShip = "";
                IList<ProductReq> _prodList = new List<ProductReq>();


                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    _prodList = (from t in _CallDB.InboundProductConfirmations
                                 where t.OrderID == _vm.id
                                 select new ProductReq
                                 {
                                     Id = t.Id,
                                     ProdCategoryId = t.ProductCategoryId,
                                     ProductDescription = t.ProductDescription,
                                     ProdNeeded = t.NeedsProduct == true ? true : false,
                                     Qty = t.Qty_Number

                                 }
                               ).OrderByDescending(t => t.ProductDescription).ToList();

                    RelationShip = _CallDB.InboundCalls.Where(t => t.Id == _vm.id).Select(t => t.RelationShip).SingleOrDefault();
                    if ((RelationShip).TrimEnd().TrimStart() == "Memeber")
                    {
                        SpokeWith = _vm.First_Name + " " + _vm.Last_Name;

                    }
                    else
                    {
                        SpokeWith = _CallDB.InboundCalls.Where(t => t.Id == _vm.id).Select(t => t.SpokeWith).SingleOrDefault();
                    }


                }

                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);
                   
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "ORDER CONFIRMATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 15;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }
                         
                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                          
                    }
                    if (_note != null)
                    {

                        StringBuilder noteString = new StringBuilder();
                        noteString = noteString.Append("SpokeWith: " + SpokeWith +Environment.NewLine);
                        noteString = noteString.Append("RelationShip: " + RelationShip +Environment.NewLine);
                        noteString = noteString.Append("ReOrder supplies has been  "); 
                         

                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;

                       
                        if (_vm.IsDeleted == true)
                        {
                            noteString = noteString.Append(" is deleted  through smart action self service for the following items:");
                        }
                        else {
                            if (_vm.ISConfirmed == true )
                            { noteString = noteString.Append(" is confirmed  through smart action self service for the following items: "); }
                            else if (_vm.ISConfirmed == false )
                            {
                                noteString = noteString.Append(" is not confirmed  through smart action self service for the following items: ");
                            }

                        }

                        noteString = noteString.Append(Environment.NewLine);
                        noteString = noteString.Append(Environment.NewLine);

                        foreach (var prod in _prodList)
                        {
                            noteString = noteString.Append(prod.ProductDescription + Environment.NewLine);
                            noteString = noteString.Append("IsNeeded = "+prod.ProdNeeded.ToString() + Environment.NewLine);
                            noteString = noteString.Append("Qty Remaining"+prod.Qty.ToString() + Environment.NewLine + Environment.NewLine);
                        }


                        noteString = noteString.Append("Shipping address verified" + Environment.NewLine + Environment.NewLine);
                        noteString = noteString.Append("Are you in a nursing home or hospital ? NO" + Environment.NewLine);
                        noteString = noteString.Append("Do you receive hospice care? NO" + Environment.NewLine);
                        noteString = noteString.Append("Is there a nursing coming to your home to provide care for you? NO" + Environment.NewLine);


                        _tHist.NoteText = noteString.ToString()  ;
                        


                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);
                        //try
                        //{
                        //    _db.SaveChanges();
                        //}
                        //catch (Exception Ex)
                        //{
                        //    string msg = Ex.Message;

                        //}
                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }

        public static void AddNoteInboundReassessment(int id)
        {
            try
            {
                int? acc;
                
                IList<ProductReq> _prodList = new List<ProductReq>();
                StringBuilder _note = new StringBuilder();

                InboundReassessment _reass = new InboundReassessment();

                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                      _reass = (from c in _CallDB.InboundReassessments
                                  where c.id == id
                                  select c).Take(1).SingleOrDefault();
                    acc = _reass.Account;

                    _prodList = (from t in _CallDB.InboundReassmentProdConfirmations
                                 where t.CallId == id && t.NeedsProduct == true
                                 select new ProductReq
                                 {
                                     Id = t.Id,
                                     ProdCategoryId = t.ProductCategoryId,
                                     ProductDescription = t.ProductCategoryDescription,
                                     ProdNeeded = t.NeedsProduct == true ? true : false,
                                   
                                 }
                               ).OrderByDescending(t => t.ProductDescription).ToList();

                    _note = _note.Append("SMART ACTION REASSESSMENT SELF-SERVICE" + Environment.NewLine+ Environment.NewLine+" Assessment Completed With: SpokeWith = ");
                    _note = _note.Append(_reass.SpokeWith + Environment.NewLine + "RelationShip to Memeber : " + _reass.Relationship +Environment.NewLine + Environment.NewLine);
                    _note = _note.Append("Member phone changed ?  " );

                    if (_reass.HasPhoneChanged == true)
                        _note = _note.Append("Yes" + Environment.NewLine + Environment.NewLine);
                    else
                        _note = _note.Append("No" + Environment.NewLine + Environment.NewLine);

                    _note = _note.Append("Member address  changed ?  ");

                    if (_reass.HasAddressChanged == true)
                        _note = _note.Append("Yes" + Environment.NewLine + Environment.NewLine);
                    else
                        _note = _note.Append("No" + Environment.NewLine + Environment.NewLine);

                    _note = _note.Append("Member insurance changed ?  ");

                    if (_reass.HasInsuranceChanged == true)
                        _note = _note.Append("Yes" + Environment.NewLine + "New Insurance Provider : " +_reass.NewInsuranceProvider + Environment.NewLine+ "New Insurance Provider Id: "  + _reass.NewInsuranceProviderId + Environment.NewLine+"New insurance provider phone : " + _reass.NewInsuranceProviderPhone+ Environment.NewLine + "Treating Physician: " + _reass.DoctorName +Environment.NewLine );
                    else
                        _note = _note.Append("No" + Environment.NewLine + Environment.NewLine);

                    _note = _note.Append("Incontinence/Urological " + Environment.NewLine);
                    _note = _note.Append("Is the member incontinent or urine, bowel or both? " +_reass.IncontinentType + Environment.NewLine);
                    _note = _note.Append("Is the member incontinent during the day ? ");
                    if (_reass.DayContType != "" && _reass.DayContType != null)
                    {
                        _note = _note.Append(  _reass.DayContType + "  " );
                    }

                    if (_reass.IsDayBowelIncontinent ==true)
                    {
                        _note = _note.Append("Is Day Bowel Incontinent : " + _reass.IsDayBowelIncontinent + "  ");
                    }

                    if (_reass.IsDayUrineIncontinent == true)
                    {
                        _note = _note.Append("IS Day Urine Incontinen : " + _reass.IsDayUrineIncontinent + "  ");
                    }
                    if ((_reass.DayContType == "" || _reass.DayContType == null) && _reass.IsDayBowelIncontinent != true && _reass.IsDayUrineIncontinent != true)
                    {
                        _note = _note.Append("False  ");
                    }

                    _note = _note.Append(Environment.NewLine);
                    _note = _note.Append("Is the member incontinent during the evening ?");

                    if (_reass.NiteContType != "" && _reass.NiteContType != null)
                    {
                        _note = _note.Append(   _reass.NiteContType + "  ");
                    }
                    if (_reass.IsNightBowelIncontinent == true)
                    {
                        _note = _note.Append("Is Night Bowel Incontinent : " + _reass.IsNightBowelIncontinent + "  ");
                    }

                    if (_reass.IsNightUrineIncontinent == true)
                    {
                        _note = _note.Append("IS Night Urine Incontinent : " + _reass.IsNightUrineIncontinent + "  ");
                    }
                    if ((_reass.NiteContType == "" || _reass.NiteContType == null) && _reass.IsNightBowelIncontinent != true && _reass.IsNightUrineIncontinent != true)
                    {
                        _note = _note.Append(  "False  ");
                    }

                    _note = _note.Append( Environment.NewLine);

                    _note = _note.Append("Is usage during evening only? " + _reass.IsUsageEvening + Environment.NewLine);
                    _note = _note.Append("Is the member mentally impaired?  " + _reass.IsMentallyImpaired + Environment.NewLine);
                    _note = _note.Append("Is the member in a diapering program during the day?  " + _reass.IsDiapering + Environment.NewLine);

                    _note = _note.Append("Does the member have a seizure disorder?  " + _reass.HasSeizureDisorder + Environment.NewLine);
                    _note = _note.Append("Is the member able to walk?  " + _reass.CanWalk + Environment.NewLine);

                    _note = _note.Append("Does the member use a walker, wheelchair or cane?  " + _reass.UseWalkAssist + Environment.NewLine);

                    _note = _note.Append("Does the member have any allergy to latex, plastic, rubber or vinyl?  " + _reass.HasAllergy + "AllergyMaterials: " +_reass.AllergyMaterials+Environment.NewLine);
                    _note = _note.Append("Does the member have any skin sores or rashes in the diaper area?  " + _reass.HasSoreOrRash + Environment.NewLine);

                    _note = _note.Append("How much does the member weigh?  " + _reass.Weight + Environment.NewLine);
                    _note = _note.Append("Does the member eat by mouth?  " + _reass.EatByMouth + Environment.NewLine);

                    _note = _note.Append("Is the member tube feed?  " + _reass.IsTubeFed + Environment.NewLine);
                    _note = _note.Append("Is the member verbal?  " + _reass.IsVerbal + Environment.NewLine);

                    _note = _note.Append(Environment.NewLine +"Diabetic" +  Environment.NewLine);
                    _note = _note.Append("Meter working properly?  " +_reass.IsMeterWorking+ Environment.NewLine);
                    _note = _note.Append("Number of times per day member is testing:   " + _reass.TestingTimes + Environment.NewLine);
                    _note = _note.Append("Member injecting with insulin?  " + _reass.IsInjectInsulin + Environment.NewLine);
                    _note = _note.Append("Type of Insulin:  " + _reass.InsulinType + Environment.NewLine);

                    _note = _note.Append(Environment.NewLine + "Current supplies reviewed: Yes" + Environment.NewLine);

                    foreach (var pro in _prodList)
                    {
                        _note = _note.Append(pro.ProductDescription + Environment.NewLine);
                    }

                    _note = _note.Append(Environment.NewLine +"Overstock of Products ? " + _reass.HasProductOverstock + Environment.NewLine);
                    _note = _note.Append("Additional Supplies needed? " + _reass.AdditionalSupplies);
                }

                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? Ids = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _noteHMS = new tbl_Account_Note();

                    _noteHMS = _db.tbl_Account_Note.Where(t => t.Account == acc && t.NoteHeading == "ASSESSMENT").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_noteHMS == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(acc);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "ASSESSMENT";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 8;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _noteHMS = _db.tbl_Account_Note.Where(t => t.Account == acc && t.NoteHeading == "ASSESSMENT").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_noteHMS != null)
                    {

                    

                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _noteHMS.ID;
                        _tHist.NoteDate = DateTime.Now;
                         

                        _tHist.NoteText = _note.ToString();



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);
                 
                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                    // add to clinical assessment 
                    tbl_Clinical_Assessments cliAs = new tbl_Clinical_Assessments();
                    cliAs.Account = Convert.ToInt32(_reass.Account);
                    cliAs.AssessmentDate = DateTime.Today;
                    cliAs.Duration = 12;

                    cliAs.Member = 1;
                    if (_reass.IsDiapering == true)
                    {
                        cliAs.BowelBladder_Program = 1;
                    }
                    if (_reass.HasSoreOrRash == true)
                    {
                        cliAs.SkinIntegrityRed = 1;
                    }
                    if (_reass.AllergyMaterials.Contains("Plastic"))
                    {
                        cliAs.PlasticAllergy = 1;
                    }
                    if (_reass.AllergyMaterials.Contains("Rubber"))
                    {
                        cliAs.RubberAllergy = 1;
                    }
                    if (_reass.AllergyMaterials.Contains("Vinyl"))
                    {
                        cliAs.VinylAllergy = 1;
                    }
                    if (_reass.AllergyMaterials.Contains("Drug"))
                    {
                        cliAs.DrugAllergy = 1;
                    }
                    if (_reass.AllergyMaterials.Contains("Latex"))
                    {
                        cliAs.LatexAllergy = 1;
                    }
                    if (_reass.HasAllergy != true)
                    {
                        cliAs.Allergies = "None";
                    }
                    cliAs.Weight = Convert.ToInt16( _reass.Weight);
                    if (_reass.EatByMouth == true)
                    {
                        cliAs.Diet = "Regular";
                    }
                    if (_reass.IsTubeFed == true)
                    {
                        cliAs.TubeFeeding = 1;
                    }
                    if (_reass.CanWalk == true)
                    {
                        cliAs.Ambulatory = 1;
                    }
                    if (_reass.UseWalkAssist == true)
                    {
                        cliAs.AssistDevice1 = "Yes";
                    }
                    if (_reass.IsVerbal == true)
                    {
                        cliAs.CommunicationSkills = "Verbal";
                    }
                    if (_reass.IsVerbal != true)
                    {
                        cliAs.CommunicationSkills = "Non Verbal";
                    }
                    if (_reass.IsMentallyImpaired == true)
                    {
                        cliAs.MentalStatus = "Mentally Impared";
                    }
                    if (_reass.IncontinentType.Contains("both"))
                    {
                        cliAs.Bowel_Incontinent = 1;
                        cliAs.Bladder_Incontinent = 1;
                    }
                    if (_reass.IncontinentType.Contains("Bowel"))
                    {
                        cliAs.Bowel_Incontinent = 1;
                    }
                    if (_reass.IncontinentType.Contains("Bladder"))
                    {
                        cliAs.Bladder_Incontinent = 1;
                    }
                    cliAs.AssessmentCompletedWith = _reass.SpokeWith;
                    cliAs.Comments = _note.ToString();
                    cliAs.CreateDate = DateTime.Today;
                    cliAs.ID_CreateBy = Convert.ToInt16(id_op.ID); 
                    _db.tbl_Clinical_Assessments.Add(cliAs);
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }
                    // end of adding clinical assessment to hdms

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }

        public static void AddNotePharmaInboundSupplies(ReorderSuppliesVM _vm)
        {
            try
            {

                string SpokeWith = "";
                string RelationShip = "";
                IList<ProductReq> _prodList = new List<ProductReq>();


                using (CallAgentDBEntitiesnew _CallDB = new CallAgentDBEntitiesnew())
                {
                    _prodList = (from t in _CallDB.PharmacyConfirmations
                                 where t.OrderID == _vm.id
                                 select new ProductReq
                                 {
                                     Id = t.Id,
                                     ProdCategoryId = t.ProductCategoryId,
                                     ProductDescription = t.ProductDescription,
                                     ProdNeeded = t.NeedsProduct == true ? true : false,
                                     Qty = t.Qty_Number

                                 }
                               ).OrderByDescending(t => t.ProductDescription).ToList();

                    RelationShip = _CallDB.PharmacyCalls.Where(t => t.Id == _vm.id).Select(t => t.RelationShip).SingleOrDefault();
                    if ((RelationShip).TrimEnd().TrimStart() == "Memeber")
                    {
                        SpokeWith = _vm.First_Name + " " + _vm.Last_Name;

                    }
                    else
                    {
                        SpokeWith = _CallDB.InboundCalls.Where(t => t.Id == _vm.id).Select(t => t.SpokeWith).SingleOrDefault();
                    }


                }

                ID_VM id_op = new ID_VM();
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "ORDER CONFIRMATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 15;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "ORDER CONFIRMATION").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_note != null)
                    {

                        StringBuilder noteString = new StringBuilder();
                        noteString = noteString.Append("SpokeWith: " + SpokeWith + Environment.NewLine);
                        noteString = noteString.Append("RelationShip: " + RelationShip + Environment.NewLine);
                        noteString = noteString.Append("ReOrder supplies  ");


                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;


                        if (_vm.IsDeleted == true)
                        {
                            noteString = noteString.Append("has been  deleted  through smart action self service for the following items:");
                        }
                        else
                        {
                            if (_vm.ISConfirmed == true)
                            { noteString = noteString.Append(" has been  confirmed  through smart action self service for the following items: "); }
                            else if (_vm.ISConfirmed == false)
                            {
                                noteString = noteString.Append(" has not been confirmed  through smart action self service for the following items: ");
                            }

                        }

                        noteString = noteString.Append(Environment.NewLine);
                        noteString = noteString.Append(Environment.NewLine);

                        foreach (var prod in _prodList)
                        {
                            noteString = noteString.Append(prod.ProductDescription + Environment.NewLine);
                            noteString = noteString.Append("IsNeeded = " + prod.ProdNeeded.ToString() + Environment.NewLine);
                            noteString = noteString.Append("Qty Remaining =  " + prod.Qty.ToString() + Environment.NewLine + Environment.NewLine);
                        }


                        noteString = noteString.Append("Shipping address verified" + Environment.NewLine + Environment.NewLine);
                        noteString = noteString.Append("Are you in a nursing home or hospital ? NO" + Environment.NewLine);
                        noteString = noteString.Append("Do you receive hospice care? NO" + Environment.NewLine);
                        noteString = noteString.Append("Is there a nursing coming to your home to provide care for you? NO" + Environment.NewLine);


                        _tHist.NoteText = noteString.ToString();



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);
                        //try
                        //{
                        //    _db.SaveChanges();
                        //}
                        //catch (Exception Ex)
                        //{
                        //    string msg = Ex.Message;

                        //}
                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

        }

        public static void AddPhoneinHDMS(int ID)
        {

            // update the status 1 in phone inbound table
            InboundChangePhone rec = new InboundChangePhone();
            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                  rec = (from t in _callDB.InboundChangePhones
                            where t.id == ID
                            select t 
                            ).SingleOrDefault();

                

                    var _data = (from t in _callDB.InboundChangePhones
                                 where t.Account == rec.Account && t.UpdateStatus != true
                                 select t
                             ).ToList();

                if (_data.Count() > 0)
                {
                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = true;

                        _callDB.Entry(_item).State = EntityState.Modified;
                    }
                }

                try
                {
                    _callDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

            // update the phone in HDMS
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                var _mem = (from t in _db.tbl_Account_Member
                            where t.Account == rec.Account && t.Member == 1
                            select t
                           ).SingleOrDefault();

                if (rec.PriPhone != null && rec.PriPhone != "")
                {
                    _mem.Phone = rec.PriPhone;
                }
                if (rec.AltPhone != null && rec.AltPhone != "")
                {
                    _mem.AltPhone = rec.AltPhone;
                }

                _db.Entry(_mem).State = EntityState.Modified;
                try { _db.SaveChanges(); }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }

                //add note in HDMS
                ID_VM id_op = new ID_VM();
                try
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.DeletedDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();


                    Int32? id = Convert.ToInt32(id_op.ID);

                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == rec.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(rec.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "DEMOGRAPHICS";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 4;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == rec.Account && t.NoteHeading == "DEMOGRAPHICS").FirstOrDefault(); // && t.NoteCreatedBy == id

                    }
                    if (_note != null)
                    {
                        StringBuilder noteString = new StringBuilder();
                        noteString.Append("Member changed phone number through Smart action self service to the following:" + Environment.NewLine);
                        if (rec.PriPhone != null && rec.PriPhone != "")
                        {
                            noteString.Append("Primary Phone : " + rec.PriPhone + Environment.NewLine);
                        }
                        if (rec.AltPhone != null && rec.AltPhone != "")
                        {
                            noteString.Append("Alternative Phone : " + rec.AltPhone + Environment.NewLine);
                        }

                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;

                        _tHist.NoteText = noteString.ToString();

                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                    }


                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        string msg = Ex.Message;

                    }

                }
                catch (Exception ex1) { }
            }
        }

        public static void DeletePhoneInboundReq(int ID)
        {

            // update the status 1 in phone inbound table
            InboundChangePhone rec = new InboundChangePhone();
            using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
            {
                rec = (from t in _callDB.InboundChangePhones
                       where t.id == ID
                       select t
                          ).SingleOrDefault();



                var _data = (from t in _callDB.InboundChangePhones
                             where t.Account == rec.Account && t.UpdateStatus != true
                             select t
                         ).ToList();

                if (_data.Count() > 0)
                {
                    foreach (var _item in _data)
                    {
                        _item.UpdateStatus = true;

                        _callDB.Entry(_item).State = EntityState.Modified;
                    }
                }

                try
                {
                    _callDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }

          
             
        }


        public static void ConfirmInboundSupplies(ReorderSuppliesVM _vm)
        {
            try
            {
                DateTime _orderdate = Convert.ToDateTime(_vm.OrderDate);
                using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
                {
                    
                   // var list = _callDB.OrderConfirmations.Where(t => t.OrderDate == _orderdate && t.AccountNumber == _vm.Account && t.IsOrderConfirmed == true).ToList();
                   /* if (list.Count() > 0)
                    {
                        foreach (var rec in list)
                        {
                          
                                DelOrderConfirmation deleteConfirmation = new DelOrderConfirmation();
                                deleteConfirmation.AccountNumber = rec.AccountNumber;
                                deleteConfirmation.OrderDate = rec.OrderDate;
                                deleteConfirmation.IsOrderConfirmed = rec.IsOrderConfirmed;
                                deleteConfirmation.IsOrderReleasedFromHold = rec.IsOrderReleasedFromHold;
                                deleteConfirmation.LoadedDate = rec.LoadedDate;
                                deleteConfirmation.Source = rec.Source;



                            _callDB.DelOrderConfirmations.Add(deleteConfirmation);
                            _callDB.OrderConfirmations.Remove(rec);
                                try
                                {
                                _callDB.SaveChanges();
                                _callDB.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                string msg = ex.Message;
                                }
                            }


                        } */

                    OrderConfirmation orderConfTbl = new OrderConfirmation();
                    orderConfTbl.AccountNumber = _vm.Account;
                    orderConfTbl.LoadedDate = DateTime.Now;
                    orderConfTbl.IsOrderConfirmed = _vm.ISConfirmed;
                    orderConfTbl.OrderDate = _orderdate;
                    orderConfTbl.IsOrderReleasedFromHold = false;
                    orderConfTbl.Source = "InboundSS";
                    _callDB.OrderConfirmations.Add(orderConfTbl);
                    try
                    {
                        _callDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }



                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }

        public static void ConfirmPharmacySupplies(ReorderSuppliesVM _vm)
        {
            try
            {
                DateTime _orderdate = Convert.ToDateTime(_vm.RefillDate);
                using (CallAgentDBEntitiesnew _callDB = new CallAgentDBEntitiesnew())
                {

                    var list = _callDB.OrderConfirmations.Where(t => t.OrderDate == _orderdate && t.AccountNumber == _vm.Account && t.IsOrderConfirmed == true).ToList();
                    if (list.Count() > 0)
                    {
                        foreach (var rec in list)
                        {

                            DelOrderConfirmation deleteConfirmation = new DelOrderConfirmation();
                            deleteConfirmation.AccountNumber = rec.AccountNumber;
                            deleteConfirmation.OrderDate = rec.OrderDate;
                            deleteConfirmation.IsOrderConfirmed = rec.IsOrderConfirmed;
                            deleteConfirmation.IsOrderReleasedFromHold = rec.IsOrderReleasedFromHold;
                            deleteConfirmation.LoadedDate = rec.LoadedDate;
                            deleteConfirmation.Source = rec.Source;



                            _callDB.DelOrderConfirmations.Add(deleteConfirmation);
                            _callDB.OrderConfirmations.Remove(rec);
                            try
                            {
                                _callDB.SaveChanges();
                                _callDB.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                string msg = ex.Message;
                            }
                        }


                    }

                    OrderConfirmation orderConfTbl = new OrderConfirmation();
                    orderConfTbl.AccountNumber = _vm.Account;
                    orderConfTbl.LoadedDate = DateTime.Now;
                    orderConfTbl.IsOrderConfirmed = _vm.ISConfirmed;
                    orderConfTbl.OrderDate = _orderdate;
                    orderConfTbl.IsOrderReleasedFromHold = false;
                    orderConfTbl.Source = "InboundSS";
                    _callDB.OrderConfirmations.Add(orderConfTbl);
                    try
                    {
                        _callDB.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }



                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }



    }
    public class ID_VM
    {
        public double? ID { get; set; }
        public string name { get; set; }
    }
    public class AddressChangeVM
    {

        public int id { get; set; }
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle { get; set; }

        public string EmailAddress { get; set; }
          public DateTime? BirthDate { get; set; }
        public DateTime? UpdateTime { get; set; }
          public bool UpdateStatus { get; set; }

        public ChangeAddReq chngAdd { get; set; }
    }


    public class DoctorChangeVM
    { 
        public int id { get; set; }
        public int Account { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorLocation { get; set; }
         public DateTime? UpdateTime { get; set; }
        public bool UpdateStatus { get; set; }

        
    }


    public class InsChangeVM
    {
        public int id { get; set; }
        public int Account { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceProviderPhone { get; set; }
        public string InsuranceProviderID { get; set; }
         public DateTime? UpdateTime { get; set; }
        public bool UpdateStatus { get; set; }


    }
    public class ReorderSuppliesVM
    {

        public int id { get; set; }
        public int Account { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle { get; set; }

        public string EmailAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? RefillDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool ISConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public string service { get; set; }


    }

    public class InboundReassessmentVM
    {
        public int id { get; set;}
        public int? Account { get; set; }
        public string Relationship { get; set; }
        public bool? HasPhoneChanged { get; set; }
        public bool? HasAddressChanged { get; set; }
        public bool? HasInsuranceChanged { get; set; }
        public string NewInsuranceProvider { get; set; }
        public string NewInsuranceProviderId { get; set; }
        public string NewInsuranceProviderPhone { get; set; }
        public string DoctorName { get; set; }
        public bool? IsMeterWorking { get; set; }
        public int? TestingTimes { get; set; }
        public bool? IsInjectInsulin { get; set; }
        public string DayContType { get; set; }
        public string NiteContType { get; set; }
        public bool? IsDayBowelIncontinent { get; set; }
        public bool? IsNightBowelIncontinent { get; set; }
        public bool? IsDayUrineIncontinent { get; set; }
        public bool? IsNightUrineIncontinent { get; set; }
        public bool? IsUsageEvening { get; set; }
        public bool? IsMentallyImpaired { get; set; }
        public bool? IsDiapering { get; set; }
        public bool? HasSeizureDisorder { get; set; }
        public bool? CanWalk { get; set; }
        public bool? UseWalkAssist { get; set; }
        public bool? HasAllergy { get; set; }
        public string AllergyMaterials { get; set; }
        public bool? HasSoreOrRash { get; set; }
        public int? Weight { get; set; }
        public bool? EatByMouth { get; set; }
        public bool? IsTubeFed { get; set; }
        public bool? IsVerbal { get; set; }
        public bool? HasProductOverstock { get; set; }
        public string AdditionalSupplies { get; set; }
        public bool? IsReassessmentComplete { get; set; }
        
        public DateTime? UploadTime { get; set; }
        public bool Uploaded { get; set; }
        public string IncontinentType { get; set; }
         

    }

    public class phoneModel
    {
        public IList<UpdatePhoneVm> updatePhone { get; set; }
    }
    public class UpdatePhoneVm
    {
        public int id { get; set; }
        public int Account { get; set; }
        public string PriPhone { get; set; }
        public string AltPhone { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool UpdateStatus { get; set; }

    }

    public class ChangeAddReq
    {
        public DateTime? UpdateTime { get; set; }
        public int Account { get; set; }
        public string DB { get; set; }
        public string Billing { get; set; }
        public string Shipping { get; set; }
        public string HDMSBAdd { get; set; }
        public string HDMSSAdd { get; set; }
         public string ReqBAdd { get; set; }
         public string ReqSAdd { get; set; }
 
    }

    public class NewAdd
    {
        public int Account { get; set; }
        public string B_Address1 { get; set; }
        public string B_Address2 { get; set; }
        public string B_State { get; set; }
        public string B_City { get; set; }
        public string B_Zip { get; set; }

        public string S_Address1 { get; set; }
        public string S_Address2 { get; set; }
        public string S_State { get; set; }
        public string S_City { get; set; }
        public string S_Zip { get; set; }
    }

    public class ProductReq
    {
       
        public int Id { get; set; }
        public int  ProdCategoryId { get; set; }
        public string ProductDescription  { get; set; }
        public bool ProdNeeded { get; set; }
        public int?  Qty { get; set; }
       

    }

    public class ProdReq
    {

        public int CallId { get; set; }
        public int ProductCategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public bool NeedsProduct { get; set; }
       

    }




}