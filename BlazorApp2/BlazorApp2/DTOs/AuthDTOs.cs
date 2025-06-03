namespace BlazorApp2.DTOs;

public class LoginModelDto
{
    public string UserName { get; set; }
    public string Password { get; set; } 
}

public class AuthResultDto
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
} 