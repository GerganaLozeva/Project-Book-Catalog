namespace BookCatalog.Core.Models;

public class Reader
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public int? PreferredGenreId { get; set; }
    public Genre PreferredGenre { get; set; }

    public ICollection<Borrowing> Borrowings { get; set; }
}
