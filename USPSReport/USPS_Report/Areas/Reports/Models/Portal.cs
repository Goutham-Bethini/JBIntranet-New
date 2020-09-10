using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ReportsDatabase;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;

namespace USPS_Report.Areas.Reports.Models
{
    public class Portal
    {
        public static bool GetActiveAcct(Int64? Account)
        {
            bool IsActive = true;
            int? account;
            account = Convert.ToInt32(Account);
            using (HHSQLDBEntities _db = new HHSQLDBEntities())
            {
                tbl_Account_Information _list = new tbl_Account_Information();
                if (account != null && account != 0)
                {
                    _list = _db.tbl_Account_Information.Where(t => t.Account == account).SingleOrDefault();

                    if (_list != null)
                    {
                        if (_list.InActiveAccount == 1)
                        { IsActive = false; }

                    }

                }

            }
            return IsActive;
        }

        public static string GetWebAcct(Int64? Account)
        {
      //  StringBuilder username = new StringBuilder();
        string username = "";
            int? account;
            account = Convert.ToInt32(Account);

            IList<WebAcctUserName> _usernameList = new List<WebAcctUserName>();
            using (JBInteractivewebDBEntities _jb = new JBInteractivewebDBEntities())
            {

                if (account != null && account != 0)
                {
                    _usernameList = (from tbl in _jb.UserAccountRelations
                                     where tbl.AccountNum == account
                                     select new WebAcctUserName
                                     {
                                         username = tbl.UserName
                                     }).ToList();
                    if (_usernameList.Count != 0)
                    {
                        foreach (var t in _usernameList)
                        {
                        //    username.Append(t.username)
                          //    username.Append("<br/>")
                            username = username + t.username + ",";

                        }

                    }

                }

            }
            return username;
        }

        public static void WelcomeEmailforInteractiveWebSite(string _email , string username)
        {
            SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");

            string messageHtml = @"<html><body> 
                  <img src=""cid:12345"" />
           <table style=""width: 100 % "">
 <tr>
          <td style = ""width:10%"" ></td>
          <td style = ""text-align:center"" > </td>
        </tr>
 <tr>
   <td style = ""width:10%""></td>
    <td style = ""text-align:center"" > Select the link to get started:  <a href = ""https://portal.jandbmedical.com/SGAccount/MyLogin?ReturnUrl=%2f"" > www.jandbportal.com </a> </td>
  </tr>
 <tr>
   <td style = ""width:10%""></td>
    <td style = ""text-align:center"" > Username: </t>" + username+ @" </br> *Case sensitive as written* </td>
  </tr>
 <tr>
          <td style = ""width:10%"" ></td>
          <td style = ""text-align:center"" > </td>
        </tr>
 
            <tr>
   <td style = ""width:10%""></td>
    <td style = ""text-align:center"" > If you have any technical problem with the website,</td>
  </tr>
  <tr>
    <td style = ""width:10%"" ></td>
    <td style = ""text-align:center"" > please send an email to
      <a href = ""mailto:websupport@jandbmedical.com"" > websupport@jandbmedical.com </a> </td>
          </tr>
      <tr>
          <td style = ""width:10%"" ></td>
          <td style = ""text-align:center"" > </td>
        </tr>
        <tr>
          <td style = ""width:10%"" ></td>
          <td style = ""text-align:center"" > If you need to communicate with us for account issues, please use the </td>
               </tr>
             <tr>
              <td style = ""width:10%""></td>
             <td style = ""text-align:center"" > ""Message Center"" on the website, or call us at(800) 737 - 0045x151.</td>
               </tr>
             <tr>
               <td style = ""width:10%"" ></td>
               <td style = ""text-align:center""> </td>
            
                </tr>
             <tr>
               <td style = ""width:10%"" ></td>
                <td style = ""text-align:center""> **PLEASE DO NOT E-MAIL WEBSUPPORT WITH ACCOUNT QUESTIONS * *</td>
              </tr>
            
             <tr>
              <td style = ""width:10%"" ></td>
              <td style = ""text-align:center""> This is for technical support only </td>
                    </tr>
                 </table>
                                    <img src=""cid:123456"" />
            <table style=""width: 100 % "">
           
          <tr> <td style = ""width:3%"" ></td>
         <td style = ""text-align:left;  font-size: 17px; "" ><strong>
        If you have questions, we are here to help.You can reach us by any of the following: </strong> </tr> 
        <tr>  <td style = ""width:3%"" ></td>
        <td style = ""text-align:center"" > </td> </tr>
      
      <tr>
    <td style = ""width:3%"" ></td>
    <td style = ""text-align:left;  font-size: 15px; "" ><strong> -The ""Message Center"" section of the <a href = ""https://portal.jandbmedical.com"" > Patient Portal </a> </strong>
         </tr>
            <tr>
           <td style = ""width:3%"" ></td>
            <td style = ""text-align:left;  font-size: 18px; "" ><strong> -Email us at <a href = ""mailto:websupport@jandbmedical.com""> websupport@jandbmedical.com </a> </strong>
                   </tr>
                   <tr>
                     <td style = ""width:3%"" ></td>
                      <td style = ""text-align:left;  font-size: 14px; "" > ""Technial support only, no account questions please""
                     </tr>
                   
                        <tr>
                         <td style = ""width:3%"" ></td>
                          <td style = ""text-align:left;  font-size: 16px; "" ><strong> -Call us at (800) 737 - 0045 Ext. 151 </strong>
                            </tr>
                           </table>
                  <img src=""cid:6767"" />
                                            </body></html>";

            //string messageHtml = @"<html><body> 
            //      <img src=""cid:12345"" />

            //       <img src=""cid:123456"" />
            //        <img src=""cid:6767"" />
            //        <img src=""cid:8989"" /></body></html>";

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string welcomeImage1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/welcome1.jpg");
            string welcomeImage2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/welcome3.jpg");
            string welcomeImage3 = System.Web.HttpContext.Current.Server.MapPath("~/Image/welcome4.jpg");
            string Image2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/welcome2.jpg");

            LinkedResource pic = new LinkedResource(welcomeImage1, MediaTypeNames.Image.Jpeg);
            pic.ContentId = "12345";

            LinkedResource pic2 = new LinkedResource(welcomeImage2, MediaTypeNames.Image.Jpeg);
            pic2.ContentId = "123456";

            LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic);
            //  view.LinkedResources.Add(text);
            view.LinkedResources.Add(pic2);
            view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "Welcome to J&B Medical Supply";
            mail.From = new MailAddress("noreply@jandbmedical.com");
            string fileName = @"C:\EmailBlast\Userguide.pdf";
            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            mail.Attachments.Add(data);
            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }


        public static void DeclineEmailforInteractiveWebSite(string _email)
        {
            SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");

            string messageHtml = @"<html><body> 
                  <img src=""cid:12345"" />
         
                                  
            <table style=""width: 100 % "">
           
          <tr> <td style = ""width:3%"" ></td>
         <td style = ""text-align:left;  font-size: 17px; "" ><strong>
        If you have questions, we are here to help.You can reach us by any of the following: </strong> </tr> 
        <tr>  <td style = ""width:3%"" ></td>
        <td style = ""text-align:center"" > </td> </tr>
      
      <tr>
    <td style = ""width:3%"" ></td>
    <td style = ""text-align:left;  font-size: 15px; "" ><strong> -The ""Message Center"" section of the <a href = ""https://portal.jandbmedical.com"" > Patient Portal </a> </strong>
         </tr>
            <tr>
           <td style = ""width:3%"" ></td>
            <td style = ""text-align:left;  font-size: 18px; "" ><strong> -Email us at <a href = ""mailto:websupport@jandbmedical.com""> websupport@jandbmedical.com </a> </strong>
                   </tr>
                   <tr>
                     <td style = ""width:3%"" ></td>
                      <td style = ""text-align:left;  font-size: 14px; "" > ""Technial support only, no account questions please""
                     </tr>
                   
                        <tr>
                         <td style = ""width:3%"" ></td>
                          <td style = ""text-align:left;  font-size: 16px; "" ><strong> -Call us at (800) 737 - 0045 Ext. 151 </strong>
                            </tr>
                           </table>
                   <img src=""cid:123456"" />
                                            </body></html>";

            //string messageHtml = @"<html><body> 
            //      <img src=""cid:12345"" />

            //       <img src=""cid:123456"" />
            //        <img src=""cid:6767"" />
            //        <img src=""cid:8989"" /></body></html>";

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string NoPortal1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/NoPortal1.jpg");
            string NoPortal2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/NoPortal2.jpg");
          
          

            LinkedResource pic = new LinkedResource(NoPortal1, MediaTypeNames.Image.Jpeg);
            pic.ContentId = "12345";

            LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            pic2.ContentId = "123456";

          //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
           // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic);
            //  view.LinkedResources.Add(text);
            view.LinkedResources.Add(pic2);
           // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "Welcome to J&B Medical Supply";
            mail.From = new MailAddress("noreply@jandbmedical.com");
            string fileName = @"C:\EmailBlast\Userguide.pdf";
            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            mail.Attachments.Add(data);
            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }
        public static void sendEmailToAcctHolder(string _email)
        {
            //_email = "cchaffee@jandbmedical.com";
           // _email = "grani@jandbmedical.com";
            try {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("jandbmedical-com.mail.protection.outlook.com");
                mail.From = new MailAddress("noreply@jandbmedical.com");

                mail.To.Add(_email);

                mail.Subject = "Welcome to JBPortal";
                mail.Body += " <html>";
                mail.Body += "<body>";
                mail.Body += "<table>";
                mail.Body += "<tr>";
                mail.Body += "<td></td><td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>	Thank you for registering on the J&B Medical Supply Account Portal!</td><td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>	Your account has been activated, and you can log in at your convenience at <br /> www.jandbportal.com</td><td></td>";
                mail.Body += "</tr>";


                mail.Body += "<tr>";
                mail.Body += "<td>               </td> <td> </td>";
                mail.Body += "</tr>";
                mail.Body += "<tr>";
                mail.Body += "<td>	Thank you for being a valued customer of J&B Medical Supply!</td><td></td>";
                mail.Body += "</tr>";

                mail.Body += "<tr>";
                mail.Body += "<td>	The J&B Medical Supply Websupport Team</td><td></td>";
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
            catch (Exception ex)
            { }
        }

        public static void AddNoteforNewWebAcct(int? _acc, string UN)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');
                    var userName = components.Last();
                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();
                    tbl_Account_Note _note = new tbl_Account_Note();
                     _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "WEBSITE").FirstOrDefault(); 
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_acc);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "WEBSITE";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 33;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                    _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "WEBSITE").FirstOrDefault(); 
                     }
                    if (_note != null)
                    {
                        string noteString = "Web account is created for Account =" + _acc + Environment.NewLine + "Username = "+UN + Environment.NewLine+" Password = " + UN + Environment.NewLine+" by  "+userName+" ";



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

        public static void AddDecliningNoteforNewWebAcct(int? _acc)
        {
            try
            {
                ID_VM id_op = new ID_VM();

                using (HHSQLDBEntities _db = new HHSQLDBEntities())
                {
                    var components = HttpContext.Current.User.Identity.Name.Split('\\');
                    var userName = components.Last();
                    id_op = (from emp in _db.tbl_Operator_Table
                             where emp.OperatorName.ToUpper() == userName.ToUpper() && emp.InactiveDate == null && emp.InactiveDate == null
                             select new ID_VM
                             {
                                 ID = emp.ID
                             }).Take(1).SingleOrDefault();

                    Int32? id = Convert.ToInt32(id_op.ID);
                    IList<tbl_Account_Note> _notelist = new List<tbl_Account_Note>();
                    tbl_Account_Note _note = new tbl_Account_Note();
                    _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "WEBSITE").FirstOrDefault();
                    if (_note == null)
                    {
                        tbl_Account_Note _tbl = new tbl_Account_Note();
                        _tbl.Account = Convert.ToInt32(_acc);
                        _tbl.Member = 1;
                        _tbl.NoteHeading = "WEBSITE";
                        _tbl.NoteCreateDate = DateTime.Now;
                        _tbl.NoteCreatedBy = id;
                        _tbl.SystemRecordType = 100;
                        _tbl.ID_NoteLibrary = 33;
                        _db.tbl_Account_Note.Add(_tbl);
                        try { _db.SaveChanges(); }
                        catch (Exception ex)
                        {
                            string str = ex.Message;
                        }

                        _note = _db.tbl_Account_Note.Where(t => t.Account == _acc && t.NoteHeading == "WEBSITE").FirstOrDefault();
                    }
                    if (_note != null)
                    {
                        string noteString = "WEB PORTAL ACCESS WAS DECLINED WHEN RECOMMEND BY CSR";



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
    }

    public class WebAcctCreatorVm
    {
        public bool? decline { get; set; }
        public bool MissingEmail { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSubmit { get; set; }
        public bool firstTime { get; set; }
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Please enter intergers only")]
        public int? Account { get; set; }
        public bool ISActive { get; set; }
        public AccountInfoVM details { get; set; }
        public string webUsername { get; set; }
        public bool? IsAcctExists { get; set; }
        public bool? IsAcctCreated { get; set; }
        public string errorMsg { get; set; }

    }
    public class WebAcctUserName
    {
        public string username { get; set; }
    }
}