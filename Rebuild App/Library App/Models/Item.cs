using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public enum ItemStatus
    {
        Available,
        Borrowed,
        Damaged,
        Destroyed
    }

    public abstract class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string LibraryCode { get; set; } = string.Empty;

        public ItemStatus Status { get; set; } = ItemStatus.Available;
    }
}
