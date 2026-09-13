using LibraryApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Allows manager to view library statistics about borrowing activity and item statuses.
[Authorize(Roles = "Manager")]
public class StatisticsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatisticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Borrowing()
    {
        var rows = await _context.Loans
            .GroupBy(loan => loan.BorrowedDate.Date)
            .Select(group => new
            {
                Date = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(row => row.Date)
            .ToListAsync();

        return View(rows);
    }

    public async Task<IActionResult> ItemStatus()
    {
        var rows = await _context.Items
            .GroupBy(item => item.Status)
            .Select(group => new
            {
                Status = group.Key,
                Count = group.Count()
            })
            .OrderBy(row => row.Status)
            .ToListAsync();

        return View(rows);
    }

    public async Task<IActionResult> Fines()
    {
        var rows = await _context.Loans
            .Include(loan => loan.Borrower)
            .Include(loan => loan.Item)
            .Where(loan => loan.FineAmount > 0)
            .OrderByDescending(loan => loan.FineAmount)
            .ToListAsync();

        ViewBag.TotalFines = rows.Sum(loan => loan.FineAmount);

        return View(rows);
    }
}
