using System;
using System.Collections.Generic;
using System.Text;
using SmartCourier_demo.DeliveryCalculators;
using SmartCourier_demo.Invoices;
using SmartCourier_demo.Models;
using SmartCourier_demo.Notifications;

namespace SmartCourier_demo.Services
{
    public class CourierBookingService
    {
        private readonly IDeliveryChargeCalculator _chargeCalculator;
        private readonly IInvoiceGenerator _invoiceGenerator;
        private readonly INotificationService _notificationService;
        public CourierBookingService(IDeliveryChargeCalculator calculator,INotificationService notificationService,IInvoiceGenerator invoiceGenerator)
        {
            _chargeCalculator = calculator;
            _invoiceGenerator = invoiceGenerator;
            _notificationService = notificationService;
        }
        public void BookCourier(CourierBooking booking)
        {
            booking.DeliveryCharge = _chargeCalculator.CalculateCharge(booking.Parcel.ParcelWeight);
            _notificationService.SendNotification($"Courier booked successfully. Charge: Rs{booking.DeliveryCharge}");
            _invoiceGenerator.GenerateInvoice(booking);
        }



    }
}
