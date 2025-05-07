using BookCatalog.Core.Data;
using BookCatalog.Core.Models;
using Microsoft.EntityFrameworkCore;

using var context = new BookCatalogContext();

while (true)
{
    Console.WriteLine("\n📚 Book Catalog Menu");
    Console.WriteLine("1. Show all books");
    Console.WriteLine("2. Add new book");
    Console.WriteLine("3. Update book");
    Console.WriteLine("4. Delete book");
    Console.WriteLine("5. Show all borrowings");
    Console.WriteLine("0. Exit");
    Console.Write("Choose: ");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            ShowBooks(context);
            break;
        case "2":
            AddBook(context);
            break;
        case "3":
            UpdateBook(context);
            break;
        case "4":
            DeleteBook(context);
            break;
        case "5":
            ShowBorrowings(context);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

void ShowBooks(BookCatalogContext db)
{
    var books = db.Books
        .Include(b => b.Author)
        .Include(b => b.Genre)
        .ToList();

    Console.WriteLine("\n📚 BOOKS IN CATALOG:\n");

    foreach (var book in books)
    {
        Console.WriteLine($"{book.Id}. {book.Title} ({book.Year})");
        Console.WriteLine($"   Author: {book.Author?.Name}");
        Console.WriteLine($"   Genre:  {book.Genre?.Name}");
    }
}

void AddBook(BookCatalogContext db)
{
    string? title;
    do
    {
        Console.Write("Title: ");
        title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("❌ Title is required and cannot be empty.");
        }
    } while (string.IsNullOrWhiteSpace(title));

    Console.Write("Year: ");
    var year = int.Parse(Console.ReadLine()!);

    Console.WriteLine("\n📚 Available Authors:");
    foreach (var a in db.Authors.ToList())
        Console.WriteLine($"{a.Id}. {a.Name}");

    Console.Write("Author ID: ");
    var authorId = int.Parse(Console.ReadLine()!);

    Console.WriteLine("\n📘 Available Genres:");
    foreach (var g in db.Genres.ToList())
        Console.WriteLine($"{g.Id}. {g.Name}");

    Console.Write("Genre ID: ");
    var genreId = int.Parse(Console.ReadLine()!);

    var newBook = new Book
    {
        Title = title!,
        Year = year,
        AuthorId = authorId,
        GenreId = genreId
    };

    db.Books.Add(newBook);
    db.SaveChanges();

    Console.WriteLine("✅ Book added!");
}

void UpdateBook(BookCatalogContext db)
{
    Console.Write("Enter book ID to update: ");
    var id = int.Parse(Console.ReadLine()!);

    var book = db.Books.Find(id);
    if (book == null)
    {
        Console.WriteLine("❌ Book not found.");
        return;
    }

    Console.Write("New Title (leave empty to keep current): ");
    var newTitle = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newTitle)) book.Title = newTitle;

    Console.Write("New Year (leave empty to keep current): ");
    var yearInput = Console.ReadLine();
    if (int.TryParse(yearInput, out var newYear)) book.Year = newYear;

    Console.Write("New Author ID (leave empty to keep current): ");
    var authorInput = Console.ReadLine();
    if (int.TryParse(authorInput, out var newAuthorId)) book.AuthorId = newAuthorId;

    Console.Write("New Genre ID (leave empty to keep current): ");
    var genreInput = Console.ReadLine();
    if (int.TryParse(genreInput, out var newGenreId)) book.GenreId = newGenreId;

    db.SaveChanges();
    Console.WriteLine("✅ Book updated!");
}

void DeleteBook(BookCatalogContext db)
{
    Console.Write("Enter book ID to delete: ");
    var id = int.Parse(Console.ReadLine()!);

    var book = db.Books.Find(id);
    if (book == null)
    {
        Console.WriteLine("❌ Book not found.");
        return;
    }

    db.Books.Remove(book);
    db.SaveChanges();
    Console.WriteLine("🗑️ Book deleted.");
}

void ShowBorrowings(BookCatalogContext db)
{
    var borrows = db.Borrowings
        .Include(b => b.Book)
        .ThenInclude(b => b.Author)
        .Include(b => b.Reader)
        .ToList();

    Console.WriteLine("\n📚 BORROWINGS:\n");

    foreach (var b in borrows)
    {
        Console.WriteLine($"Borrowing ID: {b.Id}");
        Console.WriteLine($"   Book:   {b.Book?.Title} by {b.Book?.Author?.Name}");
        Console.WriteLine($"   Reader: {b.Reader?.Name}");
        Console.WriteLine($"   Borrowed: {b.BorrowDate.ToShortDateString()}");
        Console.WriteLine($"   Returned: {(b.ReturnDate?.ToShortDateString() ?? "Not yet")}");
        Console.WriteLine();
    }
}
