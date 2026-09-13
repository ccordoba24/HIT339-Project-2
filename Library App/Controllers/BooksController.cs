using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Defines the books controller and provides access to the database
// for authorized users.

namespace LibraryApp.Controllers;

[Authorize(Roles = "Admin,Receptionist,Manager")]

public class BooksController : Controller
{
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Books
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books
            .AsNoTracking()
            .ToListAsync();

        return View(books);
    }

    // GET: /Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: /Books/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Author,Genre,PageCount,Name,Description,LibraryCode")]
        Book book)
    {
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: /Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Author,Genre,PageCount,Name,Description,LibraryCode")]
        Book submittedBook)
    {
        if (id != submittedBook.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(submittedBook);
        }

        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        book.Author = submittedBook.Author;
        book.Genre = submittedBook.Genre;
        book.PageCount = submittedBook.PageCount;
        book.Name = submittedBook.Name;
        book.Description = submittedBook.Description;
        book.LibraryCode = submittedBook.LibraryCode;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: /Books/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
