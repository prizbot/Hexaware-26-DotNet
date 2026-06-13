using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Notifications
{
    public class WhatsappNotificationService:INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"Whatsapp message sent: {message}");
        }
    }
}
