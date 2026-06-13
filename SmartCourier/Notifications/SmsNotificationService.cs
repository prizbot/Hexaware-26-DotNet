using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Notifications
{
    public class SmsNotificationService:INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"SMS sent: {message}");
        }
    }
}
