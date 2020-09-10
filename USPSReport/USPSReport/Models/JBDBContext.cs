using System.Data.Entity;

namespace USPSReport.Models
{
    public class JBDBContext:DbContext
    {
        public JBDBContext()
            : base("name=ReportsEntities")
        {
            Database.SetInitializer<JBDBContext>(null);//Disable initializer
        }
        public virtual DbSet<USPSPod> USPSPod { get; set; }
    }
}