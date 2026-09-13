using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Allows authorized users to manage loans, view loan records, and create new loans.
[Authorize(Roles = "Admin,Receptionist,Manager")]

public class LoansController : Controller
{
    private readonly ApplicationDbContext _context;

    private const decimal FinePerDay = 1.00m;

    public LoansController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var loans = await _context.Loans
            .Include(loan => loan.Item)
            .Include(loan => loan.Borrower)
            .OrderByDescending(loan => loan.BorrowedDate)
            .ToListAsync();

        return View(loans);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Borrowers = await _context.Borrowers
            .OrderBy(borrower => borrower.LastName)
            .ThenBy(borrower => borrower.FirstName)
            .ToListAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string libraryCode,
        int borrowerId)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(item =>
                item.LibraryCode == libraryCode);

        var borrower = await _context.Borrowers
            .FindAsync(borrowerId);

        if (item == null)
        {
            ModelState.AddModelError(
                "libraryCode",
                "No item was found with that library code.");
        }

        if (borrower == null)
        {
            ModelState.AddModelError(
                "borrowerId",
                "Borrower was not found.");
        }

        if (item != null &&
            item.Status != ItemStatus.Available)
        {
            ModelState.AddModelError(
                "libraryCode",
                "This item is not available.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Borrowers = await _context.Borrowers
                .OrderBy(b => b.LastName)
                .ThenBy(b => b.FirstName)
                .ToListAsync();

            return View();
        }

        item!.Status = ItemStatus.Borrowed;

        var loan = new Loan
        {
            ItemId = item.Id,
            BorrowerId = borrower!.Id,
            BorrowedDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14)
        };

        _context.Loans.Add(loan);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _context.Loans
            .Include(loan => loan.Item)
            .FirstOrDefaultAsync(loan => loan.Id == id);

        if (loan == null)
        {
            return NotFound();
        }

        if (loan.ReturnedDate != null)
        {
            return BadRequest(
                "This loan has already been returned.");
        }

        var returnedDate = DateTime.UtcNow;

        var lateDays = Math.Max(
            0,
            (returnedDate.Date - loan.DueDate.Date).Days);

        loan.ReturnedDate = returnedDate;
        loan.FineAmount = lateDays * FinePerDay;

        if (loan.Item != null)
        {
            loan.Item.Status = ItemStatus.Available;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
