using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(USPS_Report.Startup))]
namespace USPS_Report
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
