using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_blazor.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public byte[]? Image { get; set; }

    // Foreign Key
    public int? CategoryId { get; set; }

    // Navigation
    public Category? Category { get; set; }

    public ICollection<OrderDetail>? OrderDetails { get; set; }
}