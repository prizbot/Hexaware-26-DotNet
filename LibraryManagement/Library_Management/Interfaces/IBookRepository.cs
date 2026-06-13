using System;
using System.Collections.Generic;
using System.Text;
using LibraryMembershipApp.Models;

namespace LibraryMembershipApp.Interfaces
{
    public interface IBookRepository
    {
        Book? GetBookById(int bookdId);
        void MarkBookAsBorrowed(int bookId);
    }
}
