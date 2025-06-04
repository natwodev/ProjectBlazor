namespace backend_blazor.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<OrderDetailDto>? OrderDetails { get; set; }
}

// DTO cho việc tạo mới order
public class CreateOrderDTO
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
}

// DTO cho việc cập nhật order
public class UpdateOrderDTO
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}