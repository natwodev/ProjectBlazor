using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_blazor.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [MaxLength(450)]
    [ForeignKey("ApplicationUser")]
    public string? UserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}