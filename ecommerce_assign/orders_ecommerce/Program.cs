using System;
using orders_ecommerce_demo.Model;
using orders_ecommerce_demo.Service;

namespace orders_ecommerce_demo
{
    public class Program
    {
        public static void Main(string[]args)
        {
            Order order = new Order
            {
                Price = 5000,
                Quantity = 5
            };
            
            OrderBillingService service = new OrderBillingService();
            decimal subtotal=service.CalculateSubTotal(order.Price, order.Quantity);
            decimal discount = service.CalculateDiscount(subtotal);
            decimal amountafterdelivery = subtotal - discount;
            decimal deliveryCharge = service.CalculateDeliveryCharge(amountafterdelivery);
            decimal finalAmount = service.CalculateFinalAmount(order.Price, order.Quantity);
            Console.WriteLine("--------------ORDERS ECOMMERCE CONSOLE APP---------------");
            Console.WriteLine($"Product Price: {order.Price}");
            Console.WriteLine($"Product Quantity: {order.Quantity}");
            Console.WriteLine($"Subtotal: {subtotal}");
            Console.WriteLine($"Discount: {discount}");
            Console.WriteLine($"DeliveryCharge: {deliveryCharge}");
            Console.WriteLine($"Final Amount: {finalAmount}");
            Console.WriteLine("----------------------------------------------------------");
            Console.ReadLine();

        }
    }
}