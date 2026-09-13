namespace LibraryApp.Models;

public class Music : Item
{
    public string Artist { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Album { get; set; } = string.Empty;
}