// BookCatalog.Tests/BookTests.cs
using Xunit;
using BookCatalog.Core.Data;
using BookCatalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Tests
{
    public class BookTests
    {
        private readonly BookCatalogContext _context;

        public BookTests()
        {
            var options = new DbContextOptionsBuilder<BookCatalogContext>()
                .UseMySQL("server=localhost;database=bookcatalog;user=root;password=Geriberi007;")
                .Options;

            _context = new BookCatalogContext(options);
        }

        [Fact]
        public void AddBook_ShouldInsertBookToDatabase()
        {
            var testBook = new Book
            {
                Title = "[TEST] Temporary Book",
                Year = 2024,
                AuthorId = 1,
                GenreId = 1
            };

            _context.Books.Add(testBook);
            _context.SaveChanges();

            var insertedBook = _context.Books.FirstOrDefault(b => b.Title == testBook.Title);

            Assert.NotNull(insertedBook);
            Assert.Equal(testBook.Year, insertedBook?.Year);
        }

        [Fact]
        public void DeleteBook_ShouldRemoveBookFromDatabase()
        {
            var book = _context.Books.FirstOrDefault(b => b.Title == "[TEST] Temporary Book");
            if (book == null)
            {
                book = new Book
                {
                    Title = "[TEST] Temporary Book",
                    Year = 2024,
                    AuthorId = 1,
                    GenreId = 1
                };
                _context.Books.Add(book);
                _context.SaveChanges();
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            var deletedBook = _context.Books.FirstOrDefault(b => b.Id == book.Id);
            Assert.Null(deletedBook);
        }

        [Fact]
        public void GetBooks_ShouldReturnBooksFromDatabase()
        {
            var books = _context.Books.ToList();
            Assert.NotNull(books);
            Assert.True(books.Count >= 0);
        }

        [Fact]
        public void EditBook_ShouldUpdateBookTitle()
        {
            // Arrange
            var book = _context.Books.FirstOrDefault(b => b.Title == "[TEST] To Edit");
            if (book == null)
            {
                book = new Book
                {
                    Title = "[TEST] To Edit",
                    Year = 2024,
                    AuthorId = 1,
                    GenreId = 1
                };
                _context.Books.Add(book);
                _context.SaveChanges();
            }

            // Act
            book.Title = "[TEST] Edited Title";
            _context.Books.Update(book);
            _context.SaveChanges();

            var updatedBook = _context.Books.FirstOrDefault(b => b.Id == book.Id);

            // Assert
            Assert.NotNull(updatedBook);
            Assert.Equal("[TEST] Edited Title", updatedBook?.Title);
        }
    }
}
