using System.Web;
using System.Web.Optimization;

namespace USPS_Report
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     //   "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

         //   bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                   
                     //  "~/Scripts/jquery-ui-1.10.1.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Script/bootstrap-table.js",
                       "~/Scripts/wizard/jquery.snippet.min.js",
                     "~/Scripts/wizard/jquery.easyWizard.js",
                         "~/Scripts/wizard/scripts.js",
                         "~/Scripts/sortable/sortable.min.js",
                        "~/Scripts/sortable/sortable.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Highcharts/js/highcharts.js",
                       "~/Scripts/Highcharts/js/highcharts-more.js"
                      
                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/bootstrap-table.css",
                    "~/Content/bootstrap-responsive.css",
                     "~/Content/style.css",
                      "~/Content/style-responsive.css",
                      "~/Content/site.css",
                       "~/Scripts/sortable/sortable-theme-bootstrap.css",
                        "~/Content/animate.css"));

          //  bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
          //  "~/Scripts/kendo/kendo.all.min.js",
            // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
           // "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            "~/Content/kendo/kendo.common-bootstrap.min.css",
            "~/Content/kendo/kendo.bootstrap.min.css",
            "~/Content/kendo/kendo.common.min.css",
            "~/Content/kendo/kendo.default.min.css"));
          



            bundles.IgnoreList.Clear();


        }
    }
}
