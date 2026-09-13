namespace LibraryApp.Models;

public class Toy : Item
{
    public string Type { get; set; } = string.Empty;

    public int MinimumAge { get; set; }

    public string Manufacturer { get; set; } = string.Empty;
}