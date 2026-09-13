using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

public class Loan
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public Item? Item { get; set; }

    public int BorrowerId { get; set; }

    public Borrower? Borrower { get; set; }

    public DateTime BorrowedDate { get; set; } =
        DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    [Range(0, 100000)]
    public decimal FineAmount { get; set; }
}
