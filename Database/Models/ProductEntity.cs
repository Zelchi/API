using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class ProductEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }

    public AccountEntity Account { get; set; }

    public DateTime DeletedAt { get; set; } = DateTime.MinValue;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
}