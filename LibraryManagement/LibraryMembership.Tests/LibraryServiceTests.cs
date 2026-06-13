using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using LibraryMembershipApp.Interfaces;
using LibraryMembershipApp.Models;
using LibraryMembershipApp.Services;
using Moq;

namespace LibraryMembershipApp.Tests
{
    [TestFixture]
    public class LibraryServiceTests
    {

        private Mock<IMemberRepository> _memberRepositoryMock;
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<INotificationService> _notificationServiceMock;
        private LibraryService _libraryService;
        [SetUp]
        public void SetUp()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _libraryService = new LibraryService(
                _memberRepositoryMock.Object,
                _bookRepositoryMock.Object,_notificationServiceMock.Object);
           

        }
        [Test]
        public void BorrowBook_WhenAllConditionsAreValid_ShouldReturnSuccessMessage()
        {
            var member = new Member
            {
                MemberId = 1,
                MemberName = "Harshu",
                Email = "harshu123@gmail.com",
                IsActive = true,
                BorrowedBookCount = 2
            };
            var book = new Book
            {
                BookId = 1,
                BookTitle = "Malgudi Days",
                IsAvailable = true
            };
            _memberRepositoryMock.Setup(x=>x.GetMemberById(1)).Returns(member);
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(book);

            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Book borrowed successfully."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(1), Times.Once);
            _memberRepositoryMock.Verify(x=>x.UpdateBorrowedBookCount(1), Times.Once);
            _notificationServiceMock.Verify(x=>x.SendBorrowNotification(member.Email,book.BookTitle), Times.Once);
            
        }
        [Test]
        public void BorrowBook_WhenMemberDoesNotExist_ShouldReturnMemberNotFound()
        {
            _memberRepositoryMock.Setup(x=>x.GetMemberById(1)).Returns((Member)null);
            var result= _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Member not found."));
            _bookRepositoryMock.Verify(x=>x.GetBookById(It.IsAny<int>()),Times.Never);
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x=>x.SendBorrowNotification(It.IsAny<string>(),It.IsAny<string>()), Times.Never);

            
        }
        [Test]
        public void BorrowBook_WhenMemberIsInactive_ShouldReturnMemberIsNotActive()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                MemberId=1,
                MemberName="Venkat",
                Email="whencutt123@gmail.com",
                IsActive = false,
                BorrowedBookCount=0
            });
            var result=_libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Member is not active."));
            _bookRepositoryMock.Verify(x => x.GetBookById(It.IsAny<int>()), Times.Never);
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);


        }
        [Test]
        public void BorrowBook_WhenBookDoesNotExist_ShouldReturnBookNotFound()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = true
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns((Book)null);
            var result=_libraryService.BorrowBook(1,1);
            Assert.That(result, Is.EqualTo("Book not found."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBookIsNotAvailable_ShouldReturnBookIsNotAvailable()
        {
            _memberRepositoryMock.Setup(x=>x.GetMemberById(1)).Returns(new Member { IsActive = true });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book { IsAvailable = false });
            var result=_libraryService.BorrowBook(1, 1);
            Assert.That(result, Is
                .EqualTo("Book is not available."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBorrowingLimitReached_ShouldReturnBorrowingLimitReached()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = true,
                BorrowedBookCount = 3
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                IsAvailable = true
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Borrowing limit reached."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);


        }
        [Test]
        public void BorrowBook_WhenMemberIdIsInvalid_ShouldReturnInvalidMemberId()
        {
            var result=_libraryService.BorrowBook(0, 1);
            Assert.That(result, Is.EqualTo("Invalid member id."));
            _memberRepositoryMock.Verify(x => x.GetMemberById(It.IsAny<int>()),Times.Never);

            _bookRepositoryMock.Verify(x => x.GetBookById(It.IsAny<int>()), Times.Never);
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBookIdIsInvalid_ShouldReturnInvalidBookId()
        {
            var result= _libraryService.BorrowBook(1, 0);
            Assert.That(result, Is.EqualTo("Invalid book id."));
            _memberRepositoryMock.Verify(x => x.GetMemberById(It.IsAny<int>()), Times.Never);
            _bookRepositoryMock.Verify(x => x.GetBookById(It.IsAny<int>()), Times.Never);
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenNormalMemberHasThreeBooks_ShouldReturnBorrowingLimitReached()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                MemberId = 1,
                IsActive = true,
                BorrowedBookCount = 3,
                IsPremiumMember = false
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                BookId = 1,
                BookTitle = "C sharp",
                IsAvailable = true
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Borrowing limit reached."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);


        }
        [Test]
        public void BorrowBook_WhenPremiumMemberHasThreeBooks_ShouldAllowBorrowing()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                MemberId = 1,
                Email="whencutt123@gmail.com",
                IsActive = true,
                BorrowedBookCount = 3,
                IsPremiumMember = true


            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                BookId = 1,
                BookTitle = "Full Stack development",
                IsAvailable = true
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Book borrowed successfully."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(1), Times.Once);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(1), Times.Once);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification("whencutt123@gmail.com", "Full Stack development"), Times.Once);

        }
        [Test]
        public void BorrowBook_WhenPremiumMemberHasFiveBooks_ShouldReturnBorrowingLimitReached()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                MemberId = 1,
                IsActive = true,
                BorrowedBookCount = 5,
                IsPremiumMember = true


            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                BookId = 1,
                BookTitle = "Advanced C# programs",
                IsAvailable = true
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Borrowing limit reached."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenNotificationSentCorrectly_RetunsCorrectValues()
        {
            var member = new Member
            {
                MemberId = 1,
                MemberName = "Shri",
                Email = "shri@gmail.com",
                IsActive = true,
                BorrowedBookCount = 2,
                IsPremiumMember = false
            };
            var book = new Book
            {
                BookId = 1,
                BookTitle = "Data Analytics",
                AuthorName = "Robert C Martin",
                IsAvailable = true,
            };
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(member);
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(book);
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Book borrowed successfully."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(1), Times.Once);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(1), Times.Once);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification("shri@gmail.com", "Data Analytics"), Times.Once);


        }
        [Test]
        public void BorrowBook_WhenMemberNotFound_ReturnsFailureMessage()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns((Member)null);
            var result= _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Member not found."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);


        }
        [Test]
        public void BorrowBook_WhenMemberIsNotActive_ReturnsFailureMessage()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = false
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Member is not active."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBookNotFound_ReturnsFailureMessage()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = true
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns((Book)null);
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Book not found."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBookNotAvailable_ReturnsFailureMessage()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = true
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                IsAvailable = false
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Book is not available."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBorrowingLimitReached_ReturnsFailureMessage()
        {
            _memberRepositoryMock.Setup(x => x.GetMemberById(1)).Returns(new Member
            {
                IsActive = true,
                BorrowedBookCount=3,
                IsPremiumMember=false
            });
            _bookRepositoryMock.Setup(x => x.GetBookById(1)).Returns(new Book
            {
                IsAvailable = true
            });
            var result = _libraryService.BorrowBook(1, 1);
            Assert.That(result, Is.EqualTo("Borrowing limit reached."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenMemberIdIsInvalid_ReturnsFailureMessage()
        {
            
            var result = _libraryService.BorrowBook(0, 1);
            Assert.That(result, Is.EqualTo("Invalid member id."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
        [Test]
        public void BorrowBook_WhenBookIdIsInvalid_ReturnsFailureMessage()
        {

            var result = _libraryService.BorrowBook(1, 0);
            Assert.That(result, Is.EqualTo("Invalid book id."));
            _bookRepositoryMock.Verify(x => x.MarkBookAsBorrowed(It.IsAny<int>()), Times.Never);
            _memberRepositoryMock.Verify(x => x.UpdateBorrowedBookCount(It.IsAny<int>()), Times.Never);
            _notificationServiceMock.Verify(x => x.SendBorrowNotification(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

    }
}
