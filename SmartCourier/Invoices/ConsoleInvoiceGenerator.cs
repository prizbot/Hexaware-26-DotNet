using System;
using System.Collections.Generic;
using System.Text;
using SmartCourier_demo.Models;

namespace SmartCourier_demo.Invoices
{
    public class ConsoleInvoiceGenerator:IInvoiceGenerator
    {
        public void GenerateInvoice(CourierBooking booking)
        {
            Console.WriteLine("\n------------------INVOICE------------------");
            Console.WriteLine($"Customer Name: {booking.Customer.CustomerName}");
            Console.WriteLine($"Source City : {booking.Parcel.SourceCity}");
            Console.WriteLine($"Destination City : {booking.Parcel.DestinationCity}");
            Console.WriteLine($"Parcel Weight : {booking.Parcel.ParcelWeight}");
            Console.WriteLine($"Delivery Type : {booking.DeliveryType}");
            Console.WriteLine($"Total Delivery Charge: Rs {booking.DeliveryCharge}");
            Console.WriteLine("\n--------------------------------------------");

        }

    }
}
