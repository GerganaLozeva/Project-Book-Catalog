using Microsoft.EntityFrameworkCore;
using BookCatalog.Core.Models;

namespace BookCatalog.Core.Data;

public class BookCatalogContext : DbContext
{
    public BookCatalogContext() { }

    public BookCatalogContext(DbContextOptions<BookCatalogContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Reader> Readers => Set<Reader>();
    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=localhost;user=root;password=Geriberi007;database=bookcatalog";
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
