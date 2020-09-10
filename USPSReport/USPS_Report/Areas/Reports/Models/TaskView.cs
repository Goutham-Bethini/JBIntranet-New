using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Areas.Reports.Models
{
    //public class Projection
    //{
    //    public string Title { get; set; }
    //    public DateTime? Start { get; set; }
    //    public DateTime? End { get; set; }
    //}

    //public class Events : ISchedulerEvent
    //{
    //    public int Id { get; set; }

    //    public string Description { get; set; }

    //    public DateTime End { get; set; }

    //    public string EndTimezone { get; set; }

    //    public bool IsAllDay { get; set; }

    //    public string RecurrenceException { get; set; }

    //    public string RecurrenceRule { get; set; }

    //    public DateTime Start { get; set; }

    //    public string StartTimezone { get; set; }

    //    public string Title { get; set; }
    //}

    public class TaskViewModel : ISchedulerEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public int? chk { get; set; }
    }



    public class ShippedOrderVM : ISchedulerEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public string Location { get; set; }
    }
}