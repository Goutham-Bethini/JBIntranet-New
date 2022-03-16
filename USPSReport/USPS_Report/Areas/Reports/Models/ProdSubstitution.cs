using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net.Mail;
using System.Text;

namespace USPS_Report.Areas.Reports.Models
{
    public class ProdSubstitution
    {

        // check if the old product code and new product code are valid or not
        public static ProductSubModel checkProdCode(ProductSubModel _vm)
        {
            tbl_Product_Table oldpro = new tbl_Product_Table();
            tbl_Product_Table newProd = new tbl_Product_Table();
            IList<listQty> _listQty = new List<listQty>();

            int? totalcountChanged;

            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                oldpro = (from pro in _db.tbl_Product_Table where pro.ProductCode == _vm.oldProd && pro.Discontinued == false select pro).SingleOrDefault();

                if (oldpro == null)
                {
                    _vm.ValidOldProd = false;
                }
                else
                {
                    _vm.ValidOldProd = true;
                }


                //chcek the distinct quantity in tbl_PS_RepeatingOrders for old product code
                if (_vm.ValidOldProd == true)
                {
                    _listQty = (from ps in _db.tbl_PS_RepeatingOrders
                                where ps.ID_Product == oldpro.ID

                                select new listQty
                                {
                                    qty = ps.Qty
                                }).Distinct().ToList();

                    _vm.listQty = _listQty;
                }



                newProd = (from pro in _db.tbl_Product_Table where pro.ProductCode == _vm.NewProd && pro.Discontinued == false select pro).SingleOrDefault();

                if (newProd == null)
                {
                    _vm.ValidNewProd = false;
                }
                else
                {
                    _vm.ValidNewProd = true;
                }


                //if both prod are good add it into rwo_substitution table
                if (_vm.ValidNewProd == true && _vm.ValidOldProd == true)
                {
                    if (_vm.allProd == true) // if all products changes
                    {
                        totalcountChanged = (from ps in _db.tbl_PS_RepeatingOrders where ps.ID_Product == oldpro.ID select ps).Count();

                        var components = HttpContext.Current.User.Identity.Name.Split('\\');

                        var userName = components.Last();
                        using (IntranetEntities _intDB = new IntranetEntities())
                        {
                            RWO_Product_Substitutions rwo_tbl = new RWO_Product_Substitutions();
                            rwo_tbl.subOldProd = oldpro.ID;
                            rwo_tbl.subNewProd = newProd.ID;
                            rwo_tbl.subAddComment = 1;
                            rwo_tbl.subRWOcount = totalcountChanged;
                            //  rwo_tbl.subOldProdQty = _vm.Qty_oldProd;
                            // rwo_tbl.subNewProdQty = _vm.Qty_newProd;
                            rwo_tbl.allProds = true;
                            rwo_tbl.subAdded = DateTime.Now;
                            rwo_tbl.subAddedBy = userName;

                            try
                            {
                                _intDB.RWO_Product_Substitutions.Add(rwo_tbl);
                                _intDB.SaveChanges();

                                sendEmail_RWOProd("MADE", _vm.oldProd, oldpro.ProductDescription, _vm.Qty_oldProd, _vm.NewProd, newProd.ProductDescription, _vm.Qty_newProd, _vm.allProd);
                            }
                            catch (Exception ex)
                            {
                                string msg = ex.Message;
                            }
                        }

                    }

                    else // if products are changed with qty
                    {
                        totalcountChanged = (from ps in _db.tbl_PS_RepeatingOrders where ps.ID_Product == oldpro.ID && ps.Qty == _vm.Qty_oldProd select ps).Count();

                        var components = HttpContext.Current.User.Identity.Name.Split('\\');

                        var userName = components.Last();
                        using (IntranetEntities _intDB = new IntranetEntities())
                        {
                            RWO_Product_Substitutions rwo_tbl = new RWO_Product_Substitutions();
                            rwo_tbl.subOldProd = oldpro.ID;
                            rwo_tbl.subNewProd = newProd.ID;
                            rwo_tbl.subAddComment = 1;
                            rwo_tbl.subRWOcount = totalcountChanged;
                            rwo_tbl.subOldProdQty = _vm.Qty_oldProd;
                            rwo_tbl.subNewProdQty = _vm.Qty_newProd;
                            rwo_tbl.subAdded = DateTime.Now;
                            rwo_tbl.subAddedBy = userName;

                            try
                            {
                                _intDB.RWO_Product_Substitutions.Add(rwo_tbl);
                                _intDB.SaveChanges();

                                sendEmail_RWOProd("MADE", _vm.oldProd, oldpro.ProductDescription, _vm.Qty_oldProd, _vm.NewProd, newProd.ProductDescription, _vm.Qty_newProd, _vm.allProd);
                            }
                            catch (Exception ex)
                            {
                                string msg = ex.Message;
                            }
                        }
                    }
                }
            }
              
            return _vm;
        }


        public static ProductSubModel displayProdSubsList(ProductSubModel _vm, string operatorName)
        {
            int year = DateTime.Now.AddYears(-2).Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            //ProductSubModel _vm = new ProductSubModel();

            IList<RwoProSub> proSublist = new List<RwoProSub>();


            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                proSublist = _db.Database.SqlQuery<RwoProSub>(" SELECT " +
          "  CASE " +
             "   WHEN subApproved IS NOT NULL THEN 'Approved' " +
              "  WHEN subDenied IS NOT NULL THEN 'Denied' " +
              "  WHEN subDeleted IS NOT NULL THEN 'Deleted' " +
              "  ELSE 'Pending' " +
         "   END AS Status, " +
          "  sub.subAdded, " +
          "  sub.subAddedBy, " +
          "  sub.subApproved, " +
           " sub.subApprovedBy, " +
           " sub.subDeleted, " +
          "  sub.subDeletedBy," +
           " sub.subDenied, " +
           " sub.subDeniedBy, " +
           " sub.subRwoCount, " +
             " sub.subOldProdQty, " +
               " sub.subNewProdQty,  sub.allProds, " +
           " CASE " +
            "    WHEN pr1.ProductCode IS NOT NULL THEN pr1.ProductCode " +
             "   ELSE null " + 
           " END AS OldProd, " +
           " CASE " +
            "    WHEN pr2.ProductCode IS NOT NULL THEN pr2.ProductCode " +
             "   ELSE null " + 
            " END AS NewProd " +
       " FROM Intranet..RWO_Product_Substitutions sub " +
     "   LEFT JOIN HHSQLDB..tbl_product_table            pr1 ON pr1.id = sub.subOldProd " +
      "  LEFT JOIN HHSQLDB..tbl_product_table            pr2 ON pr2.id = sub.subNewProd " +
   

     "   WHERE " +
          "  sub.subDeleted IS NULL and sub.subAdded >= '1/1/2014' " +
     "   ORDER BY " +
          "  CASE " +
             "   WHEN sub.subApproved IS NOT NULL THEN 1 " +
             "   WHEN sub.subDenied IS NOT NULL THEN 2 " +
             "   WHEN sub.subDeleted IS NOT NULL THEN 3 " +
             "   ELSE 0 " +
         "   END, " +
          "  sub.subAdded DESC " +
          "insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',21,GETDATE())").ToList<RwoProSub>();

            }

            var pendingReqList = (from list in proSublist
                                  where list.subDeleted == null
                                  && list.subApproved == null
                                  && list.subDenied == null
                                  select list).OrderByDescending(t => t.subAdded).ToList();

            _vm.pendingList = pendingReqList;

            var ProcessedReqList = (from list in proSublist
                                    where list.subAdded != null
                                    || list.subApproved != null
                                    || list.subDenied != null
                                     || list.subDeleted != null
                                    select list).OrderByDescending(t => t.subAdded).ToList();

            _vm.processedList = ProcessedReqList;

            return _vm;
        }

        //   public static ProductSubModel displayProdSubsList(ProductSubModel _vm, string operatorName)
        //   {
        //       int year = DateTime.Now.AddYears(-2).Year;
        //       DateTime firstDay = new DateTime(year, 1, 1);
        //       //ProductSubModel _vm = new ProductSubModel();

        //       IList<RwoProSub> proSublist = new List<RwoProSub>();


        //           using (HHSQLDBEntities _db = new HHSQLDBEntities())
        //           {
        //           proSublist = _db.Database.SqlQuery<RwoProSub>(" SELECT " +
        //     "  CASE " +
        //        "   WHEN subApproved IS NOT NULL THEN 'Approved' " +
        //         "  WHEN subDenied IS NOT NULL THEN 'Denied' " +
        //         "  WHEN subDeleted IS NOT NULL THEN 'Deleted' " +
        //         "  ELSE 'Pending' " +
        //    "   END AS Status, " +
        //     "  sub.subAdded, " +
        //     "  sub.subAddedBy, " +
        //     "  sub.subApproved, " +
        //      " sub.subApprovedBy, " +
        //      " sub.subDeleted, " +
        //     "  sub.subDeletedBy," +
        //      " sub.subDenied, " +
        //      " sub.subDeniedBy, " +
        //      " sub.subRwoCount, " +
        //        " sub.subOldProdQty, " +
        //          " sub.subNewProdQty,  sub.allProds, " +
        //      " CASE " +
        //       "    WHEN pr1.ProductCode IS NOT NULL THEN pr1.ProductCode " +
        //        "   ELSE 'prd3.ProductCode' " + //prd3.ProductCode 
        //      " END AS OldProd, " +
        //      " CASE " +
        //       "    WHEN pr2.ProductCode IS NOT NULL THEN pr2.ProductCode " +
        //        "   ELSE 'prd4.ProductCode' " + //prd4.ProductCode
        //       " END AS NewProd " +
        //  " FROM Intranet..RWO_Product_Substitutions sub " +
        //"   LEFT JOIN HHSQLDB..tbl_product_table            pr1 ON pr1.id = sub.subOldProd " +
        // "  LEFT JOIN HHSQLDB..tbl_product_table            pr2 ON pr2.id = sub.subNewProd " +
        //"   LEFT JOIN HHSQLDB_Legacy..tbl_product_table     prd3    ON prd3.id = sub.subOldProd " +
        //"   LEFT JOIN HHSQLDB_Legacy..tbl_product_table     prd4    ON prd4.ID = sub.subNewProd " +

        //"   WHERE " +
        //     "  sub.subDeleted IS NULL and sub.subAdded >= '1/1/2014' " +
        //"   ORDER BY " +
        //     "  CASE " +
        //        "   WHEN sub.subApproved IS NOT NULL THEN 1 " +
        //        "   WHEN sub.subDenied IS NOT NULL THEN 2 " +
        //        "   WHEN sub.subDeleted IS NOT NULL THEN 3 " +
        //        "   ELSE 0 " +
        //    "   END, " +
        //     "  sub.subAdded DESC " +
        //     "insert into Reports.dbo.tbl_ReportsAuditLine values('" + operatorName + "',21,GETDATE())").ToList<RwoProSub>();

        //           }

        //       var pendingReqList = (from list in proSublist
        //                             where list.subDeleted == null
        //                             && list.subApproved == null
        //                             && list.subDenied == null
        //                             select list).OrderByDescending(t=>t.subAdded).ToList();

        //       _vm.pendingList = pendingReqList;

        //       var ProcessedReqList = (from list in proSublist
        //                             where list.subAdded != null
        //                             || list.subApproved != null
        //                             || list.subDenied != null
        //                              || list.subDeleted != null
        //                               select list).OrderByDescending(t => t.subAdded).ToList();

        //       _vm.processedList = ProcessedReqList;

        //       return _vm;
        //   }


        public static void updateProdSubTable(ProductSubModel _vm)
        {
            IList<products> prodList = new List<products>();
            int? oldProID;
            int? newProID;

            string oldProDescription;
            string newProDescription;

            var components = HttpContext.Current.User.Identity.Name.Split('\\');

            var userName = components.Last();
            foreach (var item in _vm.pendingList)
            {
             
                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    prodList = (from pro in _db.tbl_Product_Table
                                join pri in _db.tbl_Pricing_Table
                                on pro.ID equals pri.ID_Product
                                where pri.ID_PricingNo == 2 && pro.Discontinued == false
                                select new products
                                {
                                    ProductCode = pro.ProductCode,
                                    PrductId = pro.ID,
                                    UnitPrice = pri.Purchase,
                                    ProductDescription = pro.ProductDescription
                                }).ToList();




                    oldProID = (from pro in prodList where pro.ProductCode == item.oldProd select pro.PrductId).SingleOrDefault();

                    newProID = (from pro in prodList where pro.ProductCode == item.newProd select pro.PrductId).SingleOrDefault();


                    oldProDescription = (from pro in prodList where pro.ProductCode == item.oldProd select pro.ProductDescription).SingleOrDefault();

                    newProDescription = (from pro in prodList where pro.ProductCode == item.newProd select pro.ProductDescription).SingleOrDefault();

                    //update the tbl_ps_repeatingOrders table
                    if (item.actionVal == true)
                    {
                        if (item.allProds == true) // if all products changes at a time
                        {
                            string errMsg;
                            sendEmail_RWOProd("APPROVED", _vm.oldProd, oldProDescription, _vm.Qty_oldProd, _vm.NewProd, newProDescription, _vm.Qty_newProd, _vm.allProd);

                            var tbl = (from RO in _db.tbl_PS_RepeatingOrders
                                       where RO.ID_Product == oldProID
                                       select RO).ToList();

                            foreach (var rec in tbl)
                            {
                                AccountDetail _acc = new AccountDetail();

                                _acc.Account = rec.Account;
                                _acc.oldCode = item.oldProd;
                                _acc.newCode = item.newProd;
                                _acc.qtyOldProd = item.subOldProdQty;
                                _acc.qtyNewProd = item.subNewProdQty;
                                _acc.allProds = item.allProds;

                                rec.ID_Product = newProID;
                               // rec.Qty = item.subNewProdQty;
                                rec.UnitPrice = prodList.Where(t => t.PrductId == newProID).Select(t => t.UnitPrice).SingleOrDefault() ?? 0;

                                try
                                {
                                    _db.Entry(rec).State = EntityState.Modified;
                                    _db.SaveChanges();
                                    AddNoteforProdSubstitution(_acc);

                                }
                                catch (Exception ex)
                                { errMsg = ex.Message; }


                                //discontined the product
                                var product = (from pro in _db.tbl_Product_Table where pro.ID == oldProID && pro.Discontinued == false select pro).Take(1).SingleOrDefault();
                                if (product != null)
                                {
                                    product.Discontinued = true;
                                    try {
                                        _db.Entry(product).State = EntityState.Modified;
                                        _db.SaveChanges();
                                    }

                                    catch (Exception ex)
                                    {
                                        errMsg = ex.Message;
                                    }


                                }

                            }
                        }

                        //if not all products changed at a time
                        else { 

                        sendEmail_RWOProd("APPROVED", _vm.oldProd, oldProDescription, _vm.Qty_oldProd, _vm.NewProd, newProDescription, _vm.Qty_newProd, _vm.allProd);

                        var tbl = (from RO in _db.tbl_PS_RepeatingOrders
                                   where RO.ID_Product == oldProID &&
                                   RO.Qty == item.subOldProdQty
                                   select RO).ToList();

                        foreach (var rec in tbl)
                        {
                            AccountDetail _acc = new AccountDetail();

                            _acc.Account = rec.Account;
                            _acc.oldCode = item.oldProd;
                            _acc.newCode = item.newProd;
                            _acc.qtyOldProd = item.subOldProdQty;
                            _acc.qtyNewProd = item.subNewProdQty;

                            rec.ID_Product = newProID;
                            rec.Qty = item.subNewProdQty;
                            rec.UnitPrice = prodList.Where(t => t.PrductId == newProID).Select(t => t.UnitPrice).SingleOrDefault() ?? 0;

                            try
                            {
                                _db.Entry(rec).State = EntityState.Modified;
                                _db.SaveChanges();
                                AddNoteforProdSubstitution(_acc);

                            }
                            catch (Exception ex)
                            { string msg = ex.Message; }

                        }
                    }
                    }
                    else if (item.actionVal == false)
                    {
                        sendEmail_RWOProd("DENIED", _vm.oldProd, oldProDescription, _vm.Qty_oldProd, _vm.NewProd, newProDescription, _vm.Qty_newProd, _vm.allProd);
                    }


                }

                using (IntranetEntities _intDB = new IntranetEntities())
                {

                 


                    var _subRWO = (from sub in _intDB.RWO_Product_Substitutions
                                   where sub.subOldProd == oldProID && sub.subNewProd == newProID
                                   && sub.subOldProdQty == item.subOldProdQty && sub.subNewProdQty == item.subNewProdQty
                                 //  && sub.subAdded  == item.subAdded

                                 && sub.subApproved == null && sub.subDenied == null
                                 && sub.subAddedBy.ToUpper() == item.subAddedBy.ToUpper()
                                   select sub
                               ).Take(1).SingleOrDefault();

                    if (_subRWO != null)
                    {

                       
                        if (item.actionVal == true)
                        {
                            _subRWO.subApproved = DateTime.Now;
                            _subRWO.subApprovedBy = userName;

                        }
                        else
                        {
                            _subRWO.subDenied = DateTime.Now;
                            _subRWO.subDeniedBy = userName;
                        }


                        try
                        {
                            _intDB.Entry(_subRWO).State = EntityState.Modified;
                            _intDB.SaveChanges();
                        }
                        catch (Exception ex)
                        { string msg = ex.Message; }
                    }

                    

                   
                }

            }

            
        }


        //Add note to HDMS under Products heading
        public static void AddNoteforProdSubstitution(AccountDetail _detail)
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

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _detail.Account && t.NoteHeading == "PRODUCT").FirstOrDefault(); //&& t.NoteCreatedBy == id
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_detail.Account);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "PRODUCT";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 10;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }


                        _note = _db.tbl_Account_Note.Where(t => t.Account == _detail.Account && t.NoteHeading == "PRODUCT").FirstOrDefault(); // && t.NoteCreatedBy == id
                                                                                                                                                 // Environment.UserName


                    }
                    if (_note != null)
                    {
                        //PRODUCT SUBSTITUTION: ( old product code ) changed to ( new product code ) on RWO.

                        string noteString = "PRODUCT SUBSTITUTION: (" + _detail.oldCode +" ) with qty = "+_detail.qtyOldProd+" changed to ( " +_detail.newCode+ " ) with qty = "+_detail.qtyNewProd+" on RWO";
                     
                        tbl_Account_Note_History _tHist = new tbl_Account_Note_History();
                        _tHist.ID_Note = _note.ID;
                        _tHist.NoteDate = DateTime.Now;


                        _tHist.NoteText = noteString;



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

        //send email alert to ProdSubs <ProdSubs@jandbmedical.com>; disteamleader disteamleader@jandbmedical.com
        public static void sendEmail_RWOProd(string action, string oldProd, string oldProdDescription, int? qty_oldProd, string newProd, string newProdDescription, int? qty_newProd, bool all)
        {
            if (all == true)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
                mail.From = new MailAddress("noreply@jandbmedical.com");

                mail.To.Add("grani@jandbmedical.com");

                // mail.To.Add("grani@jandbmedical.com");
                mail.To.Add("ProdSubs@jandbmedical.com");
                mail.To.Add("disteamleader@jandbmedical.com");

                mail.Subject = "RWO Product Substitution";
                mail.Body += " <html>";
                mail.Body += "<body>";
                mail.Body += "<table>";

                mail.Body += "<tr>";
                mail.Body += " <td>Request " + action + " for Product Substitution:</td><td>  </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";


                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  ----------- OLD PRODUCT -----------</td><td>  </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += " <td>" + oldProd + " : (" + oldProdDescription + ") with Qty = All </td> <td>  </td>";
                mail.Body += "</tr>";


                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  ----------- NEW PRODUCT -----------</td><td>  </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>" + newProd + " : (" + newProdDescription + ") with Qty =  All </td><td>  </td> ";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  </td> <td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>  link:   http://JBMAZWeb02/JBIntranet/Reports/RWO/ProductSub </td> <td> </td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td></td><td></td>";
                mail.Body += "</tr>";


                mail.Body += "<tr>";
                mail.Body += "<td>Thank You!</td><td></td>";
                mail.Body += "</tr>";


                mail.Body += "</table>";
                mail.Body += "</body>";
                mail.Body += "</html>";
                mail.IsBodyHtml = true;
                // SmtpServer.Port = 25;
                //  SmtpServer.Credentials = new System.Net.NetworkCredential("geeta.arora2006@gmail.com", "GEETUgeet1");
                //  SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            else { 

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("noreply@jandbmedical.com");

            mail.Bcc.Add("grani@jandbmedical.com");

             mail.To.Add("grani@jandbmedical.com");
            mail.To.Add("ProdSubs@jandbmedical.com");
            mail.To.Add("disteamleader@jandbmedical.com");

            mail.Subject = "RWO Product Substitution ";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";

            mail.Body += "<tr>";
            mail.Body += " <td>Request " + action + " for Product Substitution:</td><td>  </td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";


            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  ----------- OLD PRODUCT -----------</td><td>  </td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += " <td>" + oldProd + " : (" + oldProdDescription + ") with Qty = " + qty_oldProd + "</td> <td>  </td>";
            mail.Body += "</tr>";


            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  ----------- NEW PRODUCT -----------</td><td>  </td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>" + newProd + " : (" + newProdDescription + ") with Qty = " + qty_newProd + "</td><td>  </td> ";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  </td> <td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td>  link:   http://JBMAZWeb02/JBIntranet/Reports/RWO/ProductSub </td> <td> </td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td></td><td></td>";
            mail.Body += "</tr>";


            mail.Body += "<tr>";
            mail.Body += "<td>Thank You!</td><td></td>";
            mail.Body += "</tr>";


            mail.Body += "</table>";
            mail.Body += "</body>";
            mail.Body += "</html>";
            mail.IsBodyHtml = true;
            // SmtpServer.Port = 25;
            //  SmtpServer.Credentials = new System.Net.NetworkCredential("geeta.arora2006@gmail.com", "GEETUgeet1");
            //  SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
        }
    }

    public class ProductSubModel
    {

        public bool allProd { get; set; }
        public string oldProd { get; set; }
        public bool ValidOldProd { get; set; }
        public bool ValidNewProd { get; set; }

        [Required]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public int Qty_oldProd { get; set; }

       
        public string NewProd { get; set; }

        [Required]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public int Qty_newProd { get; set; }

        public IList<RwoProSub> pendingList { get; set; }
        public IList<RwoProSub> processedList { get; set; }

      //  public string proCode { get; set; }

        public IList<listQty> listQty { get; set; }
    }

    public class RwoProSub
    {
        [Required]
        public Boolean actionVal { get; set; }
        public string Status { get; set; }
        public string oldProd { get; set; }
        public string newProd { get; set; }
        public string subAddedBy { get; set; } //RequestedBY
        public DateTime? subAdded { get; set; } //RequestedON
        public string subApprovedBy { get; set; } //ApprovedBy
        public DateTime? subApproved { get; set; } //ApprovedOn
        public string subDeniedBy { get; set; } //DeniedBy
        public DateTime? subDenied { get; set; } //DeniedON
        public string subDeletedBy { get; set; } //DeletedBy
     
        public DateTime? subDeleted { get; set; } //DeletedON
      
        public int? subRwoCount { get; set; }
        public int? subOldProdQty { get; set; }
        public int? subNewProdQty { get; set; }

        public bool allProds { get; set; }


    }

    public class AccountDetail
    {
        public int Account { get; set; }
        public string oldCode { get; set; }
        public int? qtyOldProd { get; set; }
        public string newCode { get; set; }
        public int? qtyNewProd { get; set; }

        public bool allProds { get; set; }
    }

    public class products
    {
        public int PrductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class listQty
    {
      
        public int? qty { get; set; }
    }
}