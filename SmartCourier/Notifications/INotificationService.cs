using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCourier_demo.Notifications
{
    public interface INotificationService
    {
        void SendNotification(string message);
    }
}
