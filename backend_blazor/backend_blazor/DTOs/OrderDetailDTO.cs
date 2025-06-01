namespace backend_blazor.DTOs;

public class OrderDetailDTO
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public ProductDTO? Product { get; set; }
}

// DTO cho việc tạo mới order detail
public class CreateOrderDetailDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

// DTO cho việc cập nhật order detail
public class UpdateOrderDetailDTO
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}