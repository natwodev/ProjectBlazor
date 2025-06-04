
using Microsoft.AspNetCore.Identity;

namespace backend_blazor.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public ICollection<Order>? Orders { get; set; }
}