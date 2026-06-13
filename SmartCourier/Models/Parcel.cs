using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Models
{
    public class Parcel
    {
        public double ParcelWeight { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; } 
        public string NotificationType { get; set; }
    }
}
