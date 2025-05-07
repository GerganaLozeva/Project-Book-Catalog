// BookCatalog.Tests/BorrowingTests.cs
using Xunit;
using BookCatalog.Core.Data;
using BookCatalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Tests
{
    public class BorrowingTests
    {
        private readonly BookCatalogContext _context;

        public BorrowingTests()
        {
            var options = new DbContextOptionsBuilder<BookCatalogContext>()
                .UseMySQL("server=localhost;database=bookcatalog;user=root;password=Geriberi007;")
                .Options;

            _context = new BookCatalogContext(options);
        }

        [Fact]
        public void AddBorrowing_ShouldInsertToDatabase()
        {
            var borrowing = new Borrowing
            {
                BookId = 1, // Увери се, че BookId = 1 съществува
                ReaderId = 1, // ReaderId = 1 трябва също да съществува
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            _context.Borrowings.Add(borrowing);
            _context.SaveChanges();

            var found = _context.Borrowings.FirstOrDefault(b => b.BookId == 1 && b.ReaderId == 1);
            Assert.NotNull(found);
        }

        [Fact]
        public void DeleteBorrowing_ShouldRemoveFromDatabase()
        {
            var borrowing = _context.Borrowings.FirstOrDefault(b => b.BookId == 1 && b.ReaderId == 1);
            if (borrowing == null)
            {
                borrowing = new Borrowing
                {
                    BookId = 1,
                    ReaderId = 1,
                    BorrowDate = DateTime.Now
                };
                _context.Borrowings.Add(borrowing);
                _context.SaveChanges();
            }

            _context.Borrowings.Remove(borrowing);
            _context.SaveChanges();

            var deleted = _context.Borrowings.FirstOrDefault(b => b.Id == borrowing.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public void Borrowing_ShouldContainBookAndReader()
        {
            var borrowing = _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefault();

            Assert.NotNull(borrowing);
            Assert.NotNull(borrowing?.Book);
            Assert.NotNull(borrowing?.Reader);
        }
    }
}
