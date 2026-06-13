using System;
using System.Collections.Generic;
using System.Text;

namespace orders_ecommerce_demo.Service
{
    public class OrderBillingService
    {
        public decimal CalculateSubTotal(decimal price,int quantity)
        {
            if(price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }
            
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity must be greated than 0");
            }
            return price*quantity;
        }
        public decimal CalculateDiscount( decimal subtotal)
        {
            if(subtotal >=5000)
            {
                return subtotal * 0.10m;
            }
            else if(subtotal >=2000&&subtotal<=4999)
            {
                return subtotal * 0.05m;
            }
            return 0;
            

        }
        public decimal CalculateDeliveryCharge(decimal amountAfterDelivery)
        {
            return amountAfterDelivery<1000 ? 100 : 0;
        }
        public decimal CalculateFinalAmount(decimal productPrice,int quantity)
        {
            decimal subtotal = CalculateSubTotal(productPrice, quantity);
            decimal discount= CalculateDiscount(subtotal);
            decimal amountAfterDelivery = subtotal - discount;
            decimal deliveryCharge= CalculateDeliveryCharge(amountAfterDelivery);
            return amountAfterDelivery + deliveryCharge;

        }
    }
}
