// Defines a borrower model with contact details, registration information,
// and associated loans.
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

public class Borrower
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; } =
        DateTime.UtcNow;

    public ICollection<Loan> Loans { get; set; } =
        new List<Loan>();
}
