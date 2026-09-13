using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers;

[AllowAnonymous]
public class SearchController : Controller
{
    private readonly ApplicationDbContext _context;

    public SearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string? query,
        string category = "Books")
    {
        query = query?.Trim() ?? string.Empty;

        var model = new SearchViewModel
        {
            Query = query,
            Category = category
        };

        if (string.IsNullOrWhiteSpace(query))
        {
            return View(model);
        }

        switch (category)
        {
            case "Books":
                model.Books = await _context.Books
                    .AsNoTracking()
                    .Where(book =>
                        book.Name.Contains(query) ||
                        (book.Description != null &&
                         book.Description.Contains(query)) ||
                        book.Author.Contains(query) ||
                        book.Genre.Contains(query) ||
                        book.LibraryCode.Contains(query))
                    .ToListAsync();

                break;

            case "Music":
                model.Music = await _context.Music
                    .AsNoTracking()
                    .Where(music =>
                        music.Name.Contains(query) ||
                        (music.Description != null &&
                         music.Description.Contains(query)) ||
                        music.Artist.Contains(query) ||
                        music.Album.Contains(query) ||
                        music.LibraryCode.Contains(query))
                    .ToListAsync();

                break;

            case "Toys":
                model.Toys = await _context.Toys
                    .AsNoTracking()
                    .Where(toy =>
                        toy.Name.Contains(query) ||
                        (toy.Description != null &&
                         toy.Description.Contains(query)) ||
                        toy.Type.Contains(query) ||
                        toy.Manufacturer.Contains(query) ||
                        toy.LibraryCode.Contains(query))
                    .ToListAsync();

                break;

            default:
                model.Category = "Books";
                break;
        }

        return View(model);
    }
}
