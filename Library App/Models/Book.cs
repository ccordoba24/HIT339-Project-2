// Defines a book model with author, genre, and page count information.
namespace LibraryApp.Models;

public class Book : Item
{
    public string Author { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int PageCount { get; set; }
}