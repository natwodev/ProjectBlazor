using backend_blazor.DTOs;

namespace BlazorApp2.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; } = string.Empty;
    public ICollection<OrderDetailDto>? OrderDetails { get; set; }
}

// DTO cho việc tạo mới order
public class CreateOrderDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
}

// DTO cho việc cập nhật order
public class UpdateOrderDto
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}