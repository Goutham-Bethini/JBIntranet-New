using System.Web.Mvc;

namespace USPS_Report.Areas.ColdFusionReports
{
    public class ColdFusionReportsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ColdFusionReports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ColdFusionReports_default",
                "ColdFusionReports/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}