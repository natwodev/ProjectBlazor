using BlazorApp2.DTOs;

namespace backend_blazor.DTOs;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductDto? Product { get; set; }
}

// DTO cho việc tạo mới order detail
public class CreateOrderDetailDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

// DTO cho việc cập nhật order detail
public class UpdateOrderDetailDto
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}