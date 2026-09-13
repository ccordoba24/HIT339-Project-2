namespace LibraryApp.Models;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;

    public string Category { get; set; } = "Books";

    public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();

    public IEnumerable<Music> Music { get; set; } = Enumerable.Empty<Music>();

    public IEnumerable<Toy> Toys { get; set; } = Enumerable.Empty<Toy>();
}
