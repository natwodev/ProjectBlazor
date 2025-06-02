namespace BlazorApp1.DTOs;

public class LoginModelDTO
{
    public string UserName { get; set; }
    public string Password { get; set; } 
}

public class AuthResultDTO
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
} 