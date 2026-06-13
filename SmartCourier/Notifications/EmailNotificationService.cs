using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Notifications
{
    public class EmailNotificationService:INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"Email sent: {message}");
        }
    }
}
