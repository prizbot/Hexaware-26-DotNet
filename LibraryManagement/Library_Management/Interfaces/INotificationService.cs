using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryMembershipApp.Interfaces
{
    public interface INotificationService
    {
        void SendBorrowNotification(string email, string bookTitle);
    }
}
