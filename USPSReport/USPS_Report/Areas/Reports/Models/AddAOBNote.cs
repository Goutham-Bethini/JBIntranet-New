using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;


namespace USPS_Report.Areas.Reports.Models
{
    public class AddAOBNote
    {
        public static void AddNote_AOBGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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
                    _vm.testName = userName;
                    _vm.testid = id;
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "AOB").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "AOB";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 16;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "AOB").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                      // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 


                        string noteString = " ";
                        if (_vm.spanish == true)
                        {

                            noteString = "AOB (in Spanish) generated to( ";
                        }
                        else if (_vm.chinese == true)
                        {

                            noteString = "AOB (in Chinese) generated to( ";
                        }
                     else   if (_vm.russian == true)
                        {

                            noteString = "AOB (in Russian) generated to( ";
                        }
                        else
                        {
                            noteString = "AOB (in English) generated to( ";
                        }

                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNotePaymentCollectionGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "CO-PAY COLLECTIONS";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 14;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                     // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Payment Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        //  _tHist.NoteText
                        noteString = noteString + ") for " + " Account = " + _vm.Account + Environment.NewLine + "Items included in the letter are: " + Environment.NewLine;

                        foreach (var item in _vm.prodlist)
                        {
                            if (item.include == true)
                            { noteString = noteString + item.Prod + ",  Qty: " + item.qty + ",  Amount: " + item.amt + Environment.NewLine; }

                        }

                        noteString = noteString + "CoInsurance: " + _vm.Coins + Environment.NewLine + "Remaining Deductible: " + _vm.Deduc + Environment.NewLine + "Total Amount Due: " + _vm.AmountDue;



                        _tHist.NoteText = noteString;
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

        public static void AddNoteDetailedReceiptLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "CO-PAY COLLECTIONS";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 14;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "CO-PAY COLLECTIONS").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                     // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Detailed Receipt Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        //  _tHist.NoteText
                        noteString = noteString + ") for " + " Account = " + _vm.Account + Environment.NewLine + "Date of Service /Order : " +_vm.DateOfService +Environment.NewLine+"Detailed Receipt for Copay Collected on :" +_vm.DetailedReceiptDate+Environment.NewLine+Environment.NewLine  + "Items included in the letter are: " + Environment.NewLine;

                        foreach (var item in _vm.prodlist)
                        {
                            if (item.include == true)
                            { noteString = noteString + item.Prod + ",  Qty: " + item.qty +", Insurance Rate: " +item.insRt + ",  Amount: " + item.amt + Environment.NewLine + Environment.NewLine; }

                        }

                   //     noteString = noteString + "CoInsurance: " + _vm.Coins + Environment.NewLine + "Remaining Deductible: " + _vm.Deduc + Environment.NewLine + "Total Amount Due: " + _vm.AmountDue;



                        _tHist.NoteText = noteString;
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
        public static void AddNoteCoverLetterGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Contact Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteSympathyLetterGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Symapthy Contact Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteOffLableLetterGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Off Label Member Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddMedicateAsSecondaryLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Medicaid is Secondary Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account +Environment.NewLine + "Primary Insurance Allows : " + _vm.PriInsAllow1 
                            + Environment.NewLine + "Secondary Insurance Allows : "+ _vm.SecInsAllow1 + Environment.NewLine + "Estimate Cost :" + _vm.EstimateCost ;



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

        public static void AddNoteLibreLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Libre New Member Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteLibreTrainingLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Libre Training Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteDexcomG5TrainingLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Dexcom-G5 Training Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteDexcomG6TrainingLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Dexcom-G6 Training Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteOutOfStateLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Out Of State BCBS Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteDexcomLetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Dexcom New Member Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteDexcomG6LetterGenerator(GeneratorModel _vm)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Dexcom-G6 New Member Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddAOBTrack(GeneratorModel _vm)
        {
            try
            {
              
                using (IntranetEntities _rdb = new IntranetEntities())
                {
                    AOB_track rec = new AOB_track();
                    rec.Account = Convert.ToInt16(_vm.Account);
                    rec.Type = "AOB";
                    rec.CreateDate = DateTime.Now;
                    rec.Uploaded = true;
                    _rdb.AOB_track.Add(rec);
                    _rdb.SaveChanges();
                }
            }
            catch (Exception ex) { }
            }
        


        public static void AddNoteBCNNOConfirmGenerator(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "BCN Order Confirmation Notice generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteNotEligibleLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.DeletedDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Not Eligible letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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

        public static void AddNoteTeacherLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();


                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.DeletedDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Teacher Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddNoteReassessmentLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();


                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.DeletedDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Reassessment Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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
        public static void AddNoteDiabeticReassessmentLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

               
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();


                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.DeletedDate == null
                             select new ID_VM
                             {
                                 
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Diabetic Reassessment Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;



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
        public static void AddNoteMemUnabletoServiceLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Member unable to service Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }



                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account + Environment.NewLine + "Reason:" + _vm.Reason;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddConcernContactLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Concern Contact Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }



                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account + Environment.NewLine + "Contact Person with Ext.#:" + _vm.CRL_Text3;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddComplaintResolutionLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Complaint Resolution Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }



                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account + Environment.NewLine + "Result about Investigation :" + _vm.CRL_Text1+
                            Environment.NewLine+ "Resolution : " + _vm.CRL_Text2 + Environment.NewLine + "Contact person with Ext# : " + _vm.CRL_Text3;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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


        public static void AddNoteTestingTimesLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.UserName.ToUpper()
                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');

                    var userName = components.Last();

                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 // name = emp.empFullName,
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Testing Times Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }



                        _tHist.NoteText = noteString + ") for " + " Account = " + _vm.Account;
                        if(_vm.TestStripsFrom != 0 && _vm.TestStripsTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Test Strips tested from " + _vm.TestStripsFrom + " To " + _vm.TestStripsTo;
                        }
                        if (_vm.LancetsFrom != 0 && _vm.LancetsTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Lancets from " + _vm.LancetsFrom + " To " + _vm.LancetsTo;
                        }
                        if (_vm.SyringesFrom != 0 && _vm.SyringesTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Syringes from " + _vm.SyringesFrom + " To " + _vm.SyringesTo;
                        }
                        if (_vm.PenNeedlesFrom != 0 && _vm.PenNeedlesTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Pen Needles from " + _vm.PenNeedlesFrom + " To " + _vm.PenNeedlesTo;
                        }
                        if (_vm.InfusionSetsFrom != 0 && _vm.InfusionSetsTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "InfusionSets from " + _vm.InfusionSetsFrom + " To " + _vm.InfusionSetsTo;
                        }
                        if (_vm.PodsFrom != 0 && _vm.PodsTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Pods from " + _vm.PodsFrom + " To " + _vm.PodsTo;
                        }
                        if (_vm.ReservoirsFrom != 0 && _vm.ReservoirsTo != 0)
                        {
                            _tHist.NoteText = _tHist.NoteText + Environment.NewLine + "Reservoirs from " + _vm.ReservoirsFrom + " To " + _vm.ReservoirsTo;
                        }


                        _tHist.NoteText = _tHist.NoteText+ Environment.NewLine + "Effective Date  =   " + _vm.RXDate.Month + "/" + _vm.RXDate.Day + "/" + _vm.RXDate.Year;   



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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
        public static void AddNotePhysicianUnabletoServiceLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.

                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "Physician unable to service Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for Dr." + _vm.details.PhysicianName + " for  Account = " + _vm.Account + Environment.NewLine + _vm.Reason;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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


        public static void AddNoteUnabletoReorderLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.

                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "THC Unable to Reorder Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for Dr." + _vm.details.PhysicianName + " for  Account = " + _vm.Account + Environment.NewLine + _vm.Reason;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddJBCustomerSatisfactionLetter(GeneratorModel _vm)
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

                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault();


                    }
                    if (_note != null)
                    {


                        string noteString = "JB Customer Satisfaction Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ")  for  Account = " + _vm.Account + Environment.NewLine;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddNotePCPLetter(GeneratorModel _vm)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                //using (IntranetEntities _in = new IntranetEntities())
                //{
                //    id_op = (from emp in _in.Employees_New
                //             where emp.empLogin.ToUpper() == Environment.

                //             select new ID_VM
                //             {
                //                 // name = emp.empFullName,
                //                 ID = emp.empPOSid
                //             }).Take(1).SingleOrDefault();
                //}
                //()
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //	AOB generated to (mail, Fax with fax #, email with email address) by user name 

                        string noteString = "PCP Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for  Account = " + _vm.Account + Environment.NewLine + _vm.Reason;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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


        public static void AddNoteDiabeticCoinsAuthLetter(GeneratorModel _vm)
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

                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault();
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "COMMMUNICATION";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == "COMMMUNICATION").FirstOrDefault();


                    }
                    if (_note != null)
                    {


                        string noteString = "Diabetic Coinsurance Authorization Letter generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";



                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for  Account = " + _vm.Account + Environment.NewLine + "Physician Times Testing:" + _vm.PhysicianTimesTesting +
                            Environment.NewLine + "Secondary Insurance Allowed Amt:" + _vm.SecInsAllowedAmt +
                            Environment.NewLine + "Primary Insurance coverage Amt (%):" + _vm.PriInsCovAmt +
                            Environment.NewLine + "Member Owed Amt ($):" + _vm.MemOwedAmt;



                        _tHist.ID_Operator = Convert.ToInt16(id_op.ID);

                        _db.tbl_Account_Note_History.Add(_tHist);

                        //try
                        //{  
                        //    _db.SaveChanges();
                        //} 
                        //catch(Exception Ex)
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

        public static void AddNoteForOtherLanguages(GeneratorModel _vm,string letterType)
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

                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();


                    tbl_Account_Note _note = new tbl_Account_Note();
                    string noteHeading = (letterType == "AOB") ? "AOB" : "COMMMUNICATION";
                    _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == noteHeading).FirstOrDefault();
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_vm.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = noteHeading;
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 9;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _vm.Account && t.NoteHeading == noteHeading).FirstOrDefault();


                    }
                    if (_note != null)
                    {
                        string OtherLanguageOrBraille = "";
                        if (!string.IsNullOrEmpty(_vm.OtherLanguage) && !_vm.NeedBrailleLetter)
                        {
                            OtherLanguageOrBraille = _vm.OtherLanguage;
                        }
                        else if (string.IsNullOrEmpty(_vm.OtherLanguage) && _vm.NeedBrailleLetter)
                        {
                            OtherLanguageOrBraille = "Braille";
                        }
                        else if (!string.IsNullOrEmpty(_vm.OtherLanguage) && _vm.NeedBrailleLetter)
                        {
                            OtherLanguageOrBraille = _vm.OtherLanguage + " " + " Braille";
                        }

                        string noteString = letterType+ " (in "+ OtherLanguageOrBraille + ") "+ "generated to( ";
                        if (_vm.FileFax == false && _vm.FileEmail == false)

                            noteString = noteString + "Mail ,";


                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;
                        if (_vm.FileFax == true)
                        { noteString = noteString + " Fax with " + _vm.FedEx + ","; }
                        if (_vm.FileEmail == true)
                        { noteString = noteString + " Email with " + _vm.Email; }

                        _tHist.NoteText = noteString + ") for  Account = " + _vm.Account;



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

    }
}
