using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    public class AccessRequest
    {
        public static void sendEmail()
        {

            var components = HttpContext.Current.User.Identity.Name.Split('\\');

            var userName = components.Last();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("AccessRequest@jandbmedical.com");

            mail.To.Add("grani@jandbmedical.com");
            mail.To.Add("helpdesk@jandbmedical.com");


            mail.Subject = "JBIntranet Access";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";
            mail.Body += "<tr>";
            mail.Body += "<td>Please provide access of new Intranet Tool to the following user: </td><td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td></td><td></td>";
            mail.Body += "</tr>";

            mail.Body += "<tr>";
            mail.Body += "<td></td>  " + userName + "<td></td>";
            mail.Body += "</tr>";


            mail.Body += "<tr>";
            mail.Body += "<td> </td> <td>  </td>";
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