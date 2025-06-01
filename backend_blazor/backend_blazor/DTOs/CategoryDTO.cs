namespace backend_blazor.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// DTO cho việc tạo mới category
public class CreateCategoryDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// DTO cho việc cập nhật category
public class UpdateCategoryDTO
{
    public string? Name { get; set; } 
    public string? Description { get; set; }
}