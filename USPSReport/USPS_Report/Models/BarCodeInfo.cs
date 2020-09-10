using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class BarCodeInfo
    {
        public static int AddBarcodeAttempt(int account, string DocumentType)
        {
            int lastattempt = 0;
            using (ReportsEntities re = new ReportsEntities())
            {
                int maxattempt = re.BarCodeAttempts.Where(p => p.Account == account && p.DocumentType == DocumentType).OrderByDescending(p => p.Attempt).Select(p => p.Attempt).FirstOrDefault();
                BarCodeAttempt aobattempt = new BarCodeAttempt();
                aobattempt.Account = account;
                aobattempt.Attempt = maxattempt + 1;
                aobattempt.DocumentType = DocumentType;
                aobattempt.AttemptSentDate = DateTime.Now;
                re.BarCodeAttempts.Add(aobattempt);
                re.SaveChanges();
                lastattempt = aobattempt.Attempt;
            }
            return lastattempt;
        }
    }
}