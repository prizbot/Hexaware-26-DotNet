using System;
using System.Collections.Generic;
using System.Text;
using SmartCourier_demo.Models;

namespace SmartCourier_demo.Invoices
{
    public interface IInvoiceGenerator
    {
        void GenerateInvoice(CourierBooking booking);
    }
}
