namespace BookCatalog.Core.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Nationality { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<Book> Books { get; set; }
}
