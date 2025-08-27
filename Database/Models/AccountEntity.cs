using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Models;

public class AccountEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100)]
    public string Username { get; set; }

    [Required]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Password { get; set; }

    [Required]
    [StringLength(30)]
    public string Role { get; set; }

    public Collection<ProductEntity> Products { get; set; }

    public DateTime DeletedAt { get; set; } = DateTime.MinValue;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
}