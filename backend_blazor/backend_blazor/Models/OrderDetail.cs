using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_blazor.Models;

public class OrderDetail
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign Keys
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    // Navigation
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}