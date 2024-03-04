using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using USPS_Report.Areas.Reports.Helper;

namespace USPS_Report.Areas.Reports.Models
{
    public class Emails
    {

        public static void AutoEmail(string _email, string _fileName, string name)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");

            string messageHtml = @"<html><body> 
                  <img src=""cid:123"" /> ";
                 //   <img src=""cid:456"" />
         
             
                                          //  </body></html>";
            string messageHtml2 =
           @"

            <table style=""width: 100 % "">
            

          <tr> <td></td>
         <td style = ""text-align:left;  font-size: 15px;width:900px "" > 
       If you have questions, or something is incorrect, we are here to help. You can reach us by any of the following:  </td> 
         </tr> 
       
<tr>  <td style = ""width:3%"" ></td>
        <td style = ""text-align:center"" > </td> </tr>
      
      <tr>
    <td></td>
    <td style = ""text-align:left;  font-size: 15px; "" ><strong> - The ""Message Center"" section of the <a href = ""https://portal.jandbmedical.com"" > Patient Portal </a> </strong>
         </tr>
            <tr>
           <td></td>
            <td style = ""text-align:left;  font-size: 16px; "" ><strong> - Email us at <a href = ""mailto:contact@jandbmedical.com""> contact@jandbmedical.com </a> </strong>
                   </tr>
                 
                   
                        <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 16px; "" ><strong> - Call us at (800) 737 - 0045  </strong>
                            </tr>
                            
                        <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 18px; "" ><strong><br/><br/>Thank You!</strong>
                            </tr>
                            
                            <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 18px; "" ><strong> - J&B Medical Supply </strong>
                            </tr>
                           </table>
                 <img src=""cid:456"" />
                                            </body></html>";

            messageHtml = messageHtml + messageHtml2;

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string Userguide = _fileName;
            string docImg1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/docImg1.JPG");
            string DocImg2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/DocImg2.JPG");



            LinkedResource pic1 = new LinkedResource(docImg1, MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "123";


            LinkedResource pic2 = new LinkedResource(DocImg2, MediaTypeNames.Image.Jpeg);
            pic2.ContentId = "456";

            //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            // pic2.ContentId = "123456";

            //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic1);
            //  view.LinkedResources.Add(text);
              view.LinkedResources.Add(pic2);
            // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");

            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            if(!name.Contains(".pdf"))
            {
                data.Name = name + ".pdf";
            }
            else
            {
                data.Name = name;
            }

            mail.Attachments.Add(data);


            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }

        public static void AutoEmailCSL(string _email, string _fileName, string name)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            string JB_Img = System.Web.HttpContext.Current.Server.MapPath("~/Image/Cover_HMO.JPG");
         
            string messageHtml = @"<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta charset = ""UTF-8"" >
 <style>
 body
{
 font-family:""Times New Roman, serif"";
 }
# footer
  {
 font-size:8pt;
  align: left;
  }
</style>
</head>

<body>
<p align = ""left"" >
 <img width = ""150"" height = ""60"" src = ""cid:999""/> 
      </p>
   <table>
<thead>
<tr>
<td style=""width:500px; height:70px;background-color:#C4A1E9  "">
       <h3> We appreciate your feedback!</h3>
          </td>
          </tr>
          </thead>
          </table>

     
         <div>
         <p>" + "Dear " + name + @", </p>
           <p>
           Thank you for contacting us and sharing your kind words!Your opinion of our service is of great importance to us, and we will always strive to earn your praise.
           <br/><br/>
           As you know, a company's online reputation is extremely important.  We would be grateful if you would share your positive comments on either of these websites by following the provided links:
           </p>
           <p>
           <strong> Yelp:</strong>
           </p>
           <p>
           <a href = ""http://bit.ly/2sZa0jE"" > http://bit.ly/2sZa0jE</a>
</p>
           <p>
           <strong> Facebook:</strong>
           </p>
           <p>
           <a href = ""https://www.facebook.com/jandbmedical"" > https://www.facebook.com/jandbmedical</a>
</p>
           <p>
           <strong > Google:</strong>
           </p>
           <p>
           <a href = ""https://goo.gl/xFamFd"" > https://goo.gl/xFamFd</a>
</p>
           <p>
           From your Smartphone: Search for J & amp; B Medical Supply on google.Scroll down to the heading <strong> Rate and Review </strong>.Choose the number of stars to rate your experience with us, then add any comments.
           <br/><br/>
           From your computer: Search for J & amp; B Medical Supply on google.Look at the information box that's on the right side of your page.  Near the bottom, you' ll see < strong > Write a review </ strong >.If you're not signed in using your google ID and password, you'll be asked to—and then you'll be taken directly to the page to write a review.
           <br/><br/>
           We thank you for your business, and we look forward to caring for you in the future.
           <br/><br/>
           Sincerely,
           <p style = ""font-family:Brush Script MT, Brush Script Std, cursive;font-size:14pt"" > Your friends at J & amp; B Medical Supply, Inc.</p>
           </p>
           
            <p id = ""footer"" >
            The information contained in this e - mail is intended solely for its authorized recipient(s), and is protected by federal law(HIPAA).
If you are not an intended recipient, please contact us immediately by telephone(800) 737-0045 or by e-mail at info @jandbmedical.com and delete the original and all copies of this transmission (including any attachments).
 </p>
</div>
</body>
</html>
 ";
            //   <img src=""cid:456"" />


            //  </body></html>";
           

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string Userguide = _fileName;
         //   string docImg1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/docImg1.JPG");
          //  string DocImg2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/DocImg2.JPG");


//
      LinkedResource pic1 = new LinkedResource(JB_Img, MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "999";

//
         //   LinkedResource pic2 = new LinkedResource(DocImg2, MediaTypeNames.Image.Jpeg);
          //  pic2.ContentId = "456";

            //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            // pic2.ContentId = "123456";

            //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

           view.LinkedResources.Add(pic1);
            //  view.LinkedResources.Add(text);
          //  view.LinkedResources.Add(pic2);
            // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");

            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            data.Name = name + ".pdf";

          //  mail.Attachments.Add(data);


            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }

        public static void AutoEmailDRL(string _email, string _fileName, string name, string memberName)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");

            string messageHtml = @"<html><body> 
                  <img src=""cid:123"" /> ";
            //   <img src=""cid:456"" />


            //  </body></html>";
            string messageHtml2 =
           @"
<table><tr><td style=""background-color:#9999ff;height:60px;width:600px;font-weight:800;font-size:20px""><strong>You have a new letter</strong></td></tr>
 <tr><td style=""background-color:#003366;height:5px;width:600px;font-weight:800;font-size:20px""> </hr> </td></tr>
</table>
           
 
<h2>Dear " + memberName + @", </h2>
  <p style=""font-size:20px""> We are sending the attached letter about your account</p>
     <h2> Need more help?</h2>
        <p style=""font-size:20px;""> If you have any questions, or something is incorrect, we are here to help.You can reach us by any of the following:</p>
          <p style=""font-size:20px""> -The “Message Center” section of the <a href = ""https://portal.jandbmedical.com"" > Patient Portal </a> </p>
            <p style=""font-size:20px""> -Email us at <a href = ""mailto:contact@jandbmedical.com""> contact@jandbmedical.com </a> </p>
               <p style=""font-size:20px""> -Call us at (800) 737 - 0045 </p>
                 <br/>
                 <p style=""font-size:25px""> Thank you! </p>
                 <p style=""font-size:25px""> -J & B Medical Supply </p>
                 <p>
                 <br/>
                 <small><b> The information contained in this e - mail is intended solely for its authorized recipient(s), and is protected by federal law(HIPAA).
If you are not an intended recipient, please contact us immediately by telephone(800) 737-0045 or by e-mail at info @jandbmedical.com and delete the original and all copies of this transmission (including any attachments).
</b>
</small>
</p>
                                            </body></html>";

            messageHtml = messageHtml + messageHtml2;

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string Userguide = _fileName;
            string docImg1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/Cover_HMO.JPG");
            string DocImg2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/DocImg2.JPG");



            LinkedResource pic1 = new LinkedResource(docImg1, MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "123";


            LinkedResource pic2 = new LinkedResource(DocImg2, MediaTypeNames.Image.Jpeg);
            pic2.ContentId = "456";

            //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            // pic2.ContentId = "123456";

            //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic1);
            //  view.LinkedResources.Add(text);
            view.LinkedResources.Add(pic2);
            // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");

            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            data.Name = name + ".pdf";

            mail.Attachments.Add(data);


            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }

        public static void AutoEmailSpanish(string _email, string _fileName, string name)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");

            string messageHtml = @"<html><body> 
                  <img src=""cid:123"" /> ";
            //   <img src=""cid:456"" />


            //  </body></html>";
            string messageHtml2 =
           @"

            <table style=""width: 100 % "">
            

          <tr> <td></td>
         <td style = ""text-align:left;  font-size: 15px;width:900px "" > 
      Si tienes alguna duda o algo es incorrecto, estamos aquí para ayudar. Puede comunicarse con nosotros por cualquiera de los siguientes:  </td> 
         </tr> 
       
<tr>  <td style = ""width:3%"" ></td>
        <td style = ""text-align:center"" > </td> </tr>
      
      <tr>
    <td></td>
    <td style = ""text-align:left;  font-size: 15px; "" ><strong> - La sección ""Centro de mensajes"" del <a href = ""https://portal.jandbmedical.com"" > portal del paciente </a> </strong>
         </tr>
            <tr>
           <td></td>
            <td style = ""text-align:left;  font-size: 16px; "" ><strong> - Envíanos un email a <a href = ""mailto:contact@jandbmedical.com""> contact@jandbmedical.com </a> </strong>
                   </tr>
                 
                   
                        <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 16px; "" ><strong> - Llámenos al (800) 737 - 0045  </strong>
                            </tr>
                            
                        <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 18px; "" ><strong><br/><br/>¡Gracias!</strong>
                            </tr>
                            
                            <tr>
                         <td></td>
                          <td style = ""text-align:left;  font-size: 18px; "" ><strong> - Suministros médicos J & B </strong>
                            </tr>
                           </table>
                 <img src=""cid:456"" />
                                            </body></html>";

            messageHtml = messageHtml + messageHtml2;

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string Userguide = _fileName;
            string docImg1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/docImg1_S.JPG");
            string DocImg2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/DocImg2_S.JPG");



            LinkedResource pic1 = new LinkedResource(docImg1, MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "123";


            LinkedResource pic2 = new LinkedResource(DocImg2, MediaTypeNames.Image.Jpeg);
            pic2.ContentId = "456";

            //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            // pic2.ContentId = "123456";

            //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic1);
            //  view.LinkedResources.Add(text);
            view.LinkedResources.Add(pic2);
            // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");

            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            data.Name = name + ".pdf";

            mail.Attachments.Add(data);


            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }

        /*   public static void AutoEmailAOB(string _email, string _fileName, string name)

            {
                SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");

                string messageHtml = @"<html><body> 
                      <img src=""cid:12345"" />


                                                </body></html>";

                //string messageHtml = @"<html><body> 
                //      <img src=""cid:12345"" />

                //       <img src=""cid:123456"" />
                //        <img src=""cid:6767"" />
                //        <img src=""cid:8989"" /></body></html>";

                AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


                // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
                string Userguide = _fileName;
                string NoPortal1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/LetterTemplate.JPG");
                // string NoPortal2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/NoPortal2.jpg");



                LinkedResource pic = new LinkedResource(NoPortal1, MediaTypeNames.Image.Jpeg);
                pic.ContentId = "12345";

                //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
                // pic2.ContentId = "123456";

                //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
                // pic3.ContentId = "6767";

                //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
                //pic4.ContentId = "8989";

                //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
                //pic4.ContentId = "89891";

                view.LinkedResources.Add(pic);
                //  view.LinkedResources.Add(text);
                //  view.LinkedResources.Add(pic2);
                // view.LinkedResources.Add(pic3);
                //  view.LinkedResources.Add(pic4);

                MailMessage mail = new MailMessage();
                mail.Subject = "J&B Medical Supply";
                mail.From = new MailAddress("noreply@jandbmedical.com");

                mail.To.Add(_email);

                Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
                data.Name = name + ".pdf";

                mail.Attachments.Add(data);


                mail.AlternateViews.Add(view);
                SmtpServer.Send(mail);

            }
            */

        public static void AutoEmailAOB(string _email, string _fileName, string name)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");

          //  string messageHtml = @"<html><body> 
                //  <img src=""cid:123"" /> ";
            //   <img src=""cid:456"" />


            //  </body></html>";
            string messageHtml =
           @"
<html><body> 
            <table style=""width: 100 % "">
            

          <tr> <td></td>
         <td style = ""text-align:left;font-size: 20px;width:1200px"" > 
      Welcome to J&B Medical! </br> </br> </td> 
         </tr> 
       
<tr>  <td></td>
        <td style = ""text-align:left;font-size: 20px;width:1500px"" > Please complete the attached Assignment of Benefits form that has been provided.</br></br> You may return the form via email <a href = ""mailto:JBDocs@jandbmedical.com"" >JBDocs@jandbmedical.com </a>, mail, or by fax 800-737-0012.  </br> </br></td> </tr>
    

<tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
    You can now electronically sign the Assignment of Benefits on our Patient Portal! </br></br></br></td> 
         </tr> 

<tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
    Don’t have an account for our patient portal? It’s easy and simple, go to <a href = ""http://www.jandbportal.com"" >www.jandbportal.com </a> and select Register to get started  </br></br></br></td> 
         </tr>   

  
<tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
      Any questions or concerns please contact us.   </br></br></br></td> 
         </tr> 



      </table>
                 <img src=""cid:123"" />

 <table style=""width: 100 % "">
           <tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
        </td> 
         </tr> 
 <tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
     </td> 
         </tr>   

          <tr> <td></td>
         <td style = ""text-align:left;  font-size: 20px;width:900px "" > 
      50496 W. Pontiac Trail </br>
Wixom, MI 48393 </br>
Phone: 800-737-0045 </br>
Fax: 800- 737-0012   </br>
<a href = ""https://www.jandbmedical.com/"" > www.jandbmedical.com</a> </br>
<a href = ""http://www.jandbportal.com/"" > www.jandbportal.com</a> </br></td> 
         </tr> 
       
<tr>  <td></td>
        <td style = ""text-align:left;font-size: 20px;width:1500px"" >
The information contained in this e-mail is intended solely for its authorized recipient(s), and is protected by federal law (HIPAA). If you are not an intended recipient, please contact us immediately by telephone (800) 737-0045 or by e-mail at<a href = ""mailto:info@jandbmedical.com"" > info@jandbmedical.com </a> and delete the original and all copies of this transmission (including any attachments).
</td> </tr>
      
 
                                            </body></html>";

            

            AlternateView view = AlternateView.CreateAlternateViewFromString(messageHtml, null, MediaTypeNames.Text.Html);


            // string Userguide = System.Web.HttpContext.Current.Server.MapPath("~/Image/Userguide.pdf");
            string Userguide = _fileName;
            string docImg1 = System.Web.HttpContext.Current.Server.MapPath("~/Image/logo.JPG");
          //  string DocImg2 = System.Web.HttpContext.Current.Server.MapPath("~/Image/DocImg2.JPG");



            LinkedResource pic1 = new LinkedResource(docImg1, MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "123";


        //    LinkedResource pic2 = new LinkedResource(DocImg2, MediaTypeNames.Image.Jpeg);
         //   pic2.ContentId = "456";

            //  LinkedResource pic2 = new LinkedResource(NoPortal2, MediaTypeNames.Image.Jpeg);
            // pic2.ContentId = "123456";

            //  LinkedResource pic3 = new LinkedResource(welcomeImage3, MediaTypeNames.Image.Jpeg);
            // pic3.ContentId = "6767";

            //LinkedResource pic4 = new LinkedResource(Image1, MediaTypeNames.Image.Jpeg);
            //pic4.ContentId = "8989";

            //LinkedResource text = new LinkedResource("text.txt", MediaTypeNames.Text.Plain);
            //pic4.ContentId = "89891";

            view.LinkedResources.Add(pic1);
            //  view.LinkedResources.Add(text);
         //   view.LinkedResources.Add(pic2);
            // view.LinkedResources.Add(pic3);
            //  view.LinkedResources.Add(pic4);

            MailMessage mail = new MailMessage();
            mail.Subject = "J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");

            mail.To.Add(_email);

            Attachment data = new Attachment(Userguide, MediaTypeNames.Application.Octet);
            data.Name = name + ".pdf";

            mail.Attachments.Add(data);


            mail.AlternateViews.Add(view);
            SmtpServer.Send(mail);

        }

        public static void AutoEmailAOBEnglishSpanish(string _email,string emailTemplate,string filePathToSave)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            string contents = File.ReadAllText(emailTemplate);

            AlternateView view = AlternateView.CreateAlternateViewFromString(contents, null, MediaTypeNames.Text.Html);
 
            // Create linkedresources for images
            string logoImg = System.Web.HttpContext.Current.Server.MapPath("~/Image/logo.JPG");
            string twitterImg = System.Web.HttpContext.Current.Server.MapPath("~/Image/Twitter.JPG");
            string facebookImg = System.Web.HttpContext.Current.Server.MapPath("~/Image/Facebook.JPG");
            string linkedInImg = System.Web.HttpContext.Current.Server.MapPath("~/Image/LinkedIn.JPG");
            string wbencImg = System.Web.HttpContext.Current.Server.MapPath("~/Image/CertifiedWBENC.JPG");

            LinkedResource logoRes = new LinkedResource(logoImg, MediaTypeNames.Image.Jpeg);
            logoRes.ContentId = "logo";
            view.LinkedResources.Add(logoRes);

            LinkedResource twitterRes = new LinkedResource(twitterImg, MediaTypeNames.Image.Jpeg);
            twitterRes.ContentId = "twitter";
            view.LinkedResources.Add(twitterRes);

            LinkedResource facebookRes = new LinkedResource(facebookImg, MediaTypeNames.Image.Jpeg);
            facebookRes.ContentId = "facebook";
            view.LinkedResources.Add(facebookRes);

            LinkedResource linkedInRes = new LinkedResource(linkedInImg, MediaTypeNames.Image.Jpeg);
            linkedInRes.ContentId = "linkedIn";
            view.LinkedResources.Add(linkedInRes);

            LinkedResource wbencRes = new LinkedResource(wbencImg, MediaTypeNames.Image.Jpeg);
            wbencRes.ContentId = "wbenc";
            view.LinkedResources.Add(wbencRes);


            MailMessage mail = new MailMessage();
            mail.Subject = "Welcome to J&B Medical Supply";
            mail.From = new MailAddress("JBDocs@jandbmedical.com");
            mail.To.Add(_email);
            // Add the final html to mail views
            mail.AlternateViews.Add(view);
            // Save message to local folder
            mail.Save(filePathToSave);
            SmtpServer.Send(mail);

        }       
        public static void AutoFax(string _fax, string _fileName)
        {


            //  DateTime todaydate = DateTime.Today.Date;

            string faxNo = "fax=" + _fax + "@10.10.2.19";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.jandbmedical.com");
            mail.From = new MailAddress("autofax@jandbmedical.com");

            mail.To.Add(faxNo);
            //    mail.To.Add("dvasquez@jandbmedical.com");
            // mail.Bcc.Add("grani@jandbmedical.com");
            Attachment data = new Attachment(_fileName, MediaTypeNames.Application.Octet);
            mail.Attachments.Add(data);

            mail.Subject = "J and B Document-Auto Fax";
            mail.Body += " <html>";
            mail.Body += "<body>";
            mail.Body += "<table>";




            mail.Body += "<tr>";
            mail.Body += "<td>Thank You!</td> ";
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