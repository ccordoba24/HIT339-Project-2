using LibraryApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items => Set<Item>();

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Music> Music => Set<Music>();

        public DbSet<Toy> Toys => Set<Toy>();

        public DbSet<Borrower> Borrowers => Set<Borrower>();

        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>()
                .HasIndex(item => item.LibraryCode)
                .IsUnique();

            builder.Entity<Item>()
                .Property(item => item.Status)
                .HasConversion<string>();

            builder.Entity<Loan>()
                .HasOne(loan => loan.Item)
                .WithMany()
                .HasForeignKey(loan => loan.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Loan>()
                .HasOne(loan => loan.Borrower)
                .WithMany(borrower => borrower.Loans)
                .HasForeignKey(loan => loan.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
