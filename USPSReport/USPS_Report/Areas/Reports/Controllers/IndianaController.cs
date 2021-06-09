using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsDatabase;
using USPS_Report.Areas.Reports.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace USPS_Report.Areas.Reports.Controllers
{
    public class IndianaController : Controller
    {
        // GET: Reports/Indiana
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Indiana_Survey()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Indiana_Survey(FormCollection form1)
        {
            try
            {
                ViewBag.message = "";
                DateTime dt = System.DateTime.Now;

                var components = User.Identity.Name.Split('\\');

                var userName = components.Last();

              //  string addby = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
               // string b = addby.Split('\\')[1];
                string u_addby = userName;

                string qa = "";
                string qb = "";
                string qc = "";
                string qd = "";
                string qe = "";
                string q1ans = "";
                string q2ans = "";
                string q3ans = "";
                string q4ans = "";
                string q5ans = "";
                string q6ans = "";

                //get Name,Address and Comment
                string name = Request.Form["u_name"];
                string address = Request.Form["u_addr"];
                string comm = Request.Form["u_comm"];
                //q1        
                qa = Request.Form["q1a"];
                qb = Request.Form["q1b"];
                qc = Request.Form["q1c"];
                qd = Request.Form["q1d"];
                qe = Request.Form["q1e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q1ans = qa;
                else if (qb != null)
                    q1ans = qb;
                else if (qc != null)
                    q1ans = qc;
                else if (qd != null)
                    q1ans = qd;
                else
                    q1ans = qe;

                //q2       
                qa = Request.Form["q2a"];
                qb = Request.Form["q2b"];
                qc = Request.Form["q2c"];
                qd = Request.Form["q2d"];
                qe = Request.Form["q2e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q2ans = qa;
                else if (qb != null)
                    q2ans = qb;
                else if (qc != null)
                    q2ans = qc;
                else if (qd != null)
                    q2ans = qd;
                else
                    q2ans = qe;

                //q3        
                qa = Request.Form["q3a"];
                qb = Request.Form["q3b"];
                qc = Request.Form["q3c"];
                qd = Request.Form["q3d"];
                qe = Request.Form["q3e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q3ans = qa;
                else if (qb != null)
                    q3ans = qb;
                else if (qc != null)
                    q3ans = qc;
                else if (qd != null)
                    q3ans = qd;
                else
                    q3ans = qe;

                //q4       
                qa = Request.Form["q4a"];
                qb = Request.Form["q4b"];
                qc = Request.Form["q4c"];
                qd = Request.Form["q4d"];
                qe = Request.Form["q4e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q4ans = qa;
                else if (qb != null)
                    q4ans = qb;
                else if (qc != null)
                    q4ans = qc;
                else if (qd != null)
                    q4ans = qd;
                else
                    q4ans = qe;
                //q5        
                qa = Request.Form["q5a"];
                qb = Request.Form["q5b"];
                qc = Request.Form["q5c"];
                qd = Request.Form["q5d"];
                qe = Request.Form["q5e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q5ans = qa;
                else if (qb != null)
                    q5ans = qb;
                else if (qc != null)
                    q5ans = qc;
                else if (qd != null)
                    q5ans = qd;
                else
                    q5ans = qe;
                //q6       
                qa = Request.Form["q6a"];
                qb = Request.Form["q6b"];
                qc = Request.Form["q6c"];
                qd = Request.Form["q6d"];
                qe = Request.Form["q6e"];
                //check which is not null and store it in column, if option is selected,storing null
                if (qa != null)
                    q6ans = qa;
                else if (qb != null)
                    q6ans = qb;
                else if (qc != null)
                    q6ans = qc;
                else if (qd != null)
                    q6ans = qd;
                else
                    q6ans = qe;

                //insert into database
                IntranetEntities ie = new IntranetEntities();
                string query = @" insert into indianamedicaidsurvey(imsQ1,imsQ2,imsQ3,imsQ4,imsQ5,imsQ6,imsName,
                            imsAddress,imsComment,imsEnteredDate,imsEnteredBy)
                              values('" + q1ans + "','" + q2ans + "','" + q3ans + "','" + q4ans + "','" + q5ans + "','" + q6ans + "','" +
                              name + "','" + address + "','" + comm + "','" + dt + "','" + u_addby + "')";

                

                int rowsinsert = ie.Database.ExecuteSqlCommand(query);
                string query2 = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',41,GETDATE())";
                int rowsinsert2 = ie.Database.ExecuteSqlCommand(query2);

                ViewBag.message = "Your Survey is Submitted. Thank you";
            }

            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }


        public ActionResult SurverResult()
        {
            SurverAnalysis surveyAnalysis = new SurverAnalysis();
            List<SurveyQestions> surveyReslist = new List<SurveyQestions>();


            for (int q = 1; q <= 6; q++)
            {
                SurveyQestions surveyRes = new SurveyQestions();
                IntranetEntities ie = new IntranetEntities();

                var query1 = @"
                                   select 
                                    imsQ" + q + " as Ques, count(*) as TotalCount from IndianaMedicaidSurvey    group by imsQ" + q + "  Order by imsQ" + q + " desc";





                var query = @" select  " +
" case " +
" when ey.ques='' then 'No Response' " +
" when  ey.ques  like '%NoOpinion%'  then  'No Opinion' " +
 " when ey.ques  like '%StronglyAgree%'  then  'Strongly Agree' " +
"  when ey.ques  like '%StronglyDisAgree%'  then  'Strongly Disagree' " +
 "   when ey.ques  like '%Agree%'  then  'Agree' " +
 "  when ey.ques  like '%DisAgree%'  then  'DisAgree' " +
" end as Ques, " +
" ey.TotalCount, " +
" ey.Percentage " +
" from " +
 " (select  " +
" imsQ"+q+ " as ques, count(*) as TotalCount ,  cast "+
 " ( cast((count(*) * 100) AS Decimal) "+
  " / (select count(*) from IndianaMedicaidSurvey) "+
      "  AS decimal(10, 2)) as Percentage " +
" from IndianaMedicaidSurvey  " +
  " group by imsQ" + q +
" ) ey " +
" order by  " +
 " case " +
 " when ey.ques='StronglyAgree' then '1' " +
 " when ey.ques='Agree' then '2' " +
 " when ey.ques='NoOpinion' then '3' " +
 " when ey.ques='DisAgree' then '4' " +
 " when ey.ques='StronglyDisAgree' then '5' " +
" when ey.ques='' then '6' end";






                ie.Database.CommandTimeout = 0;
                var list = ie.Database.SqlQuery<surveyAns>(query).ToList<surveyAns>();
                if (q == 1)
                    surveyRes.ques = "The quality of the product I am getting from my supplier meets my needs.";
                else if (q == 2)
                    surveyRes.ques = "The staff are responsive,courteous, and knowledgable in answering my questions.";
                else if (q == 3)
                    surveyRes.ques = "The phone system and/or interactive website is convenient and easy to use.";
                else if (q == 4)
                    surveyRes.ques = "I recieve my orders as expected.";
                else if (q == 5)
                    surveyRes.ques = "Any issues I may have are/were resolved to my satisfaction.";
                else if (q == 6)
                    surveyRes.ques = " Overall, I am satisfied with the services I am recieving and would recommend them to a friend, neighbor, or relativ";
                surveyRes.answer = list;

                if (surveyRes != null)
                {
                    surveyReslist.Add(surveyRes);


                }

                surveyAnalysis.surveyResults = surveyReslist;
                surveyAnalysis.TotalRecord = ie.IndianaMedicaidSurveys.Count();



            }

            IntranetEntities ie2 = new IntranetEntities();
            string query2 = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',42,GETDATE())";
            int rowsinsert = ie2.Database.ExecuteSqlCommand(query2);

            return View(surveyAnalysis);
        }

        public ActionResult SurverOptionInfo()
        {
            SuveryInformation serveyInformation = new SuveryInformation();

            List<SurveyInfo> surveyInfo = new List<SurveyInfo>();
            IntranetEntities ie = new IntranetEntities();
            surveyInfo = (from t in ie.IndianaMedicaidSurveys
                          select new SurveyInfo
                          {
                              Id = t.imsID,
                              Commnet = t.imsComment,
                              Name = t.imsName,
                              Address = t.imsAddress
                          }).OrderByDescending(t=>t.Id).ToList<SurveyInfo>();

            serveyInformation.info = surveyInfo;
            serveyInformation.TotalCount = ie.IndianaMedicaidSurveys.Count();
            string query = @"insert into Reports.dbo.tbl_ReportsAuditLine values('" + User.Identity.Name.Split('\\').Last().ToLower() + "',43,GETDATE())";
            int rowsinsert = ie.Database.ExecuteSqlCommand(query);
            return View(serveyInformation);
        }

        

        public ActionResult GetSurveyrecord(Int32? ID)
        {
            survey Survey = new survey();
            IList<surveyVM> surveyInfo = new List<surveyVM>();
            IntranetEntities ie = new IntranetEntities();
            surveyInfo = (from t in ie.IndianaMedicaidSurveys
                          where t.imsID == ID
                          select new surveyVM
                          {
                              imsID = t.imsID,
                              imsQ1 = t.imsQ1,
                              imsQ2 = t.imsQ2,
                              imsQ3 = t.imsQ3,
                              imsQ4 = t.imsQ4,
                              imsQ5 = t.imsQ5,
                              imsQ6 = t.imsQ6,
                              imsName = t.imsName,
                              imsComment = t.imsComment,
                              imsEnteredDate = t.imsEnteredDate,
                              imsEnterBy = t.imsEnteredBy

                          }).ToList<surveyVM>();


            Survey.surveyList = surveyInfo;

            return PartialView("_detailSurvey", Survey);
        }
        //_list.OrderBy(t => t.PostedDate).OrderBy(t=>t.Account);


    }
}