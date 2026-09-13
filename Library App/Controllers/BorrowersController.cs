// Imports the data models, authorization, MVC, and Entity Framework Core features
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Allows authorized users to manage borrowers and display
// or create borrower records.

[Authorize(Roles = "Admin,Receptionist,Manager")]

public class BorrowersController : Controller
{
    private readonly ApplicationDbContext _context;

    public BorrowersController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var borrowers = await _context.Borrowers
            .OrderBy(borrower => borrower.LastName)
            .ToListAsync();

        return View(borrowers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Borrower borrower)
    {
        if (!ModelState.IsValid)
        {
            return View(borrower);
        }

        _context.Borrowers.Add(borrower);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrower = await _context.Borrowers.FindAsync(id);

        if (borrower == null)
        {
            return NotFound();
        }

        return View(borrower);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Borrower borrower)
    {
        if (id != borrower.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(borrower);
        }

        _context.Borrowers.Update(borrower);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var borrower = await _context.Borrowers.FindAsync(id);

        if (borrower == null)
        {
            return NotFound();
        }

        return View(borrower);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(
        int id)
    {
        var borrower = await _context.Borrowers.FindAsync(id);

        if (borrower == null)
        {
            return NotFound();
        }

        var hasLoans = await _context.Loans
            .AnyAsync(loan => loan.BorrowerId == id);

        if (hasLoans)
        {
            ModelState.AddModelError(
                "",
                "Borrowers with loan history cannot be deleted.");

            return View("Delete", borrower);
        }

        _context.Borrowers.Remove(borrower);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
