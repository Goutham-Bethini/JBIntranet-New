using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS_Report.Models
{
    public class Tracker
    {
    }
    public class PumpTrackerVM
    {

        public int id { get; set; }
        public int Account { get; set; }
        public DateTime? Created { get; set; } //Date first added to call log
        public DateTime? Modified { get; set; } //Date last updated in call log

        public string Manufacturer { get; set; }
        public string OrderStatus { get; set; } // OrderStatus
        public string Supplies1 { get; set; }
        public string Supplies2 { get; set; }
        public string SuppliesOther { get; set; }// Type of Supplies from New Account / Restart 
        public string Model { get; set; } // Insulin pump assessment; New/Replacement pump name/product code
        public string NewReplacement { get; set; } //Insulin pump assessment; new or replacement
        public string ReceiverProductCode { get; set; }  //Call log - insulin pump assessment; NEEDS TO BE ADDED TO CALL LOG
        public string ReceiverSerial { get; set; } // Call Log - Insulin pump assessment; Receiver Serial #
        public string TransmitterProductCode { get; set; } // Call log - insulin pump assessment; NEEDS TO BE ADDED TO CALL LOG
        public string TransmitterSerial { get; set; } //Call log - insulin pump assessment; transmitter serial #
        public bool? SentRequestPurchasing { get; set; } //Call Log - box checked for sent request to purchasing under order status
        public DateTime? ShipDate { get; set; } //Call Log - Order status; ADD TEXT BOX FOR DATE TO BE ENTERED NEXT TO ORDER SHIPPED
        public string InNeedOf { get; set; } // Call Log - documentation text box last entry
        public string AdditionalInformation { get; set; } // Call Log - order status text box last entry
        public OrderDetail detailOrder { get; set; }
    }


    public class OrderDetail
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PrimaryIns { get; set; }
        public string InsType { get; set; }
        public string PhysicianFN { get; set; }
        public string PhysicianLN { get; set; }
        public string PhysicianAddress { get; set; }
        public string PhysicianCity { get; set; }
        public string PhysicianState { get; set; }
        public string PhysicianZip { get; set; }
        public string PhysicianNPI { get; set; }

        public int Account { get; set; }
      


    }
}