using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.DeliveryCalculators
{
    public class ExpressDeliveryCalculator:IDeliveryChargeCalculator
    {
        public decimal CalculateCharge(double ParcelWeight)
        {
            return (decimal)(ParcelWeight * 80 + 100);
        }
    }
}
