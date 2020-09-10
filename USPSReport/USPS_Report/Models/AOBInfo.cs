using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class AOBInfo
    {
        public static int AddAOBAttempt(int account)
        {
            int lastattempt = 0;
            using (ReportsEntities re = new ReportsEntities())
            {
                int maxattempt = re.AOBAttempts.Where(p => p.Account == account).OrderByDescending(p => p.Attempt).Select(p => p.Attempt).FirstOrDefault();
                AOBAttempt aobattempt = new AOBAttempt();
                aobattempt.Account = account;
                aobattempt.Attempt = maxattempt + 1;
                aobattempt.AttemptSentDate = DateTime.Now;
                re.AOBAttempts.Add(aobattempt);
                re.SaveChanges();
                lastattempt = aobattempt.Attempt;
            }
            return lastattempt;
        }
    }
}