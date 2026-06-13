using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Models
{
    public class CourierBooking
    {
        public Customer Customer { get; set; }  
        public Parcel Parcel { get; set; }  
        public string DeliveryType { get; set; }
        public decimal DeliveryCharge { get; set; }
    }
}
