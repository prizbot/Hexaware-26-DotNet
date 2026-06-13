using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.DeliveryCalculators
{
    public class StandardDeliveryCalculator:IDeliveryChargeCalculator
    {
        public decimal CalculateCharge(double ParcelWeight)
        {
            return (decimal)(ParcelWeight * 50);
        }
    }
}
