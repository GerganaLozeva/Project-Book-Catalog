namespace BookCatalog.Core.Models;

public class Borrowing
{
    public int Id { get; set; }

    public int ReaderId { get; set; }
    public Reader Reader { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }

    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
