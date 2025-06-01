namespace backend_blazor.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int? CategoryId { get; set; }
    public CategoryDTO? Category { get; set; }
}

// DTO cho việc tạo mới product
public class CreateProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int? CategoryId { get; set; }
}

// DTO cho việc cập nhật product
public class UpdateProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int? CategoryId { get; set; }
}