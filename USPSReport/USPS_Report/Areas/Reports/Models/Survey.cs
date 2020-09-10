using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    public class SurveyQestions
    {
        public List<surveyAns> answer { get; set; }
        public string ques { get; set; }

      
    }


    public class SurverAnalysis
    {
        public List<SurveyQestions> surveyResults { get; set; }
        public int TotalRecord { get; set; }
    }
  
    

    public class surveyAns
    {
        public string Ques { get; set; }

        public int TotalCount { get; set; }

        public Decimal Percentage { get; set; }
    }

    public class SuveryInformation {

        public int TotalCount { get; set; }
        public List<SurveyInfo> info { get; set; }
    }
    public class SurveyInfo
    {
        public int Id { get; set; }
        public string Commnet { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class survey {
      public IList<surveyVM> surveyList { get; set; }
    }
    public class surveyVM
    {
        public int imsID { get; set; }
        public string imsQ1 { get; set; }
        public string imsQ2 { get; set; }
        public string imsQ3 { get; set; }
        public string imsQ4 { get; set; }
        public string imsQ5 { get; set; }
        public string imsQ6 { get; set; }
        public string imsName { get; set; }
        public string imsAddress { get; set; }
        public string imsComment { get; set; }
        public DateTime? imsEnteredDate { get; set; }
        public string imsEnterBy { get; set; }
    }
}