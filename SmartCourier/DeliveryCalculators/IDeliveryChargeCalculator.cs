using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.DeliveryCalculators
{
    public interface IDeliveryChargeCalculator
    {
        decimal CalculateCharge(double ParcelWeight);
    }
}
