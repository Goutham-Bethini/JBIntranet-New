using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USPSReport.Models
{
    [Table("USPS_POD_tbl")]
    public class USPSPod
    {
        [Key]
        public Guid uID { get; set; }
        public int? WorkOrder { get; set; }
        public string ConfirmationNum { get; set; }
        public string USPSReply { get; set; }
        public DateTime? PostDate { get; set; }
        public DateTime? Updated { get; set; }
        public int? Account { get; set; }
    }
}