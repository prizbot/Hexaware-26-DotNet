using System;
using SmartCourier_demo.Models;
using SmartCourier_demo.Services;
using SmartCourier_demo.DeliveryCalculators;
using SmartCourier_demo.Invoices;
using SmartCourier_demo.Notifications;

namespace SmartCourier_demo
{
    public class Program
    {
        public static void Main(string[]args)
        {
            Console.WriteLine("----------SMART COURIER APP---------");
            Customer customer = new Customer();
            Console.WriteLine("Enter Customer name: ");
            customer.CustomerName=Console.ReadLine();
            Console.WriteLine("Enter Customer emai;: ");
            customer.CustomerEmail = Console.ReadLine();
            Console.WriteLine("Enter Customer mobile number: ");
            customer.MobileNumber = Console.ReadLine();
            Parcel parcel = new Parcel();
            Console.WriteLine("Enter Parcel weight: ");
            parcel.ParcelWeight=Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter Source City: ");
            parcel.SourceCity = Console.ReadLine();
            Console.WriteLine("Enter Destination city: ");
            parcel.DestinationCity = Console.ReadLine();
            Console.WriteLine("\nDelivery Type: ");
            Console.WriteLine("1.Standard");
            Console.WriteLine("2.Express");
            Console.WriteLine("3.International");
            Console.WriteLine("Choose Delivery Type: ");
            int deliveryChoice=Convert.ToInt32(Console.ReadLine());
            IDeliveryChargeCalculator calculator = null;
            string deliveryType = "";
            switch (deliveryChoice)
            {
                case 1:
                    calculator = new StandardDeliveryCalculator();
                    deliveryType = "Standard Delivery";
                    break;
                case 2:
                    calculator = new ExpressDeliveryCalculator();
                    deliveryType = "Express Delivery";
                    break;
                case 3:
                    calculator = new InternationalDeliveryCalculator();
                    deliveryType = "International Delivery";
                    break;
                default:
                    Console.WriteLine("Invalid delivery type.");
                    return;

            }
            Console.WriteLine("\nNotification Type: ");
            Console.WriteLine("1.Email");
            Console.WriteLine("2.SMS");
            Console.WriteLine("3.Whatsapp");
            Console.WriteLine("Choose Notification Type: ");
            int notificationChoice=Convert.ToInt32(Console.ReadLine());
            INotificationService notificationService = null;
            switch(notificationChoice)
            {
                case 1:
                    notificationService = new EmailNotificationService();
                    break;
                case 2:
                    notificationService = new SmsNotificationService();
                    break;
                case 3:
                    notificationService = new WhatsappNotificationService();
                    break;
                default:
                    Console.WriteLine("Invalid Notification type.");
                    return;
            }
            CourierBooking booking = new CourierBooking
            {
                Customer = customer,
                Parcel = parcel,
                DeliveryType = deliveryType
            };
            IInvoiceGenerator invoiceGenerator = new ConsoleInvoiceGenerator();
            CourierBookingService courierBookingService = new CourierBookingService(calculator, notificationService, invoiceGenerator);
            courierBookingService.BookCourier(booking);
            Console.ReadLine();


        }
    }

}