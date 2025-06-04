using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using BlazorApp2.DTOs;
using Microsoft.JSInterop;

namespace BlazorApp2.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";
    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient httpClient, NavigationManager navigationManager, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _jsRuntime = jsRuntime;
    }

    public async Task<AuthResultDto> Login(string username, string password)
    {
        try
        {
            var loginModel = new LoginModelDto
            {
                UserName = username,
                Password = password
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    if (jsonDoc.RootElement.TryGetProperty("token", out var tokenElement) && tokenElement.ValueKind == JsonValueKind.String)
                    {
                        var token = tokenElement.GetString();
                        if (!string.IsNullOrEmpty(token))
                        {
                            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
                            OnAuthStateChanged?.Invoke();
                            return new AuthResultDto { IsSuccess = true, Token = token };
                        }
                    }
                    return new AuthResultDto { IsSuccess = false, ErrorMessage = "Không thể đọc token từ server" };
                }
                catch (JsonException)
                {
                    return new AuthResultDto { IsSuccess = false, ErrorMessage = "Phản hồi từ server không hợp lệ." };
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResult = JsonSerializer.Deserialize<AuthResultDto>(errorContent);
                return errorResult ?? new AuthResultDto { IsSuccess = false, ErrorMessage = "Đăng nhập thất bại" };
            }
            catch
            {
                return new AuthResultDto { IsSuccess = false, ErrorMessage = "Đăng nhập thất bại" };
            }
        }
        catch (Exception ex)
        {
            return new AuthResultDto { IsSuccess = false, ErrorMessage = $"Lỗi: {ex.Message}" };
        }
    }

    public async Task Logout()
    {
        var response = await _httpClient.PostAsync("api/auth/logout", null);

        if (response.IsSuccessStatusCode)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);

            OnAuthStateChanged?.Invoke();

            _navigationManager.NavigateTo("/login");
        }
        else
        {
            Console.WriteLine("Logout failed: " + response.ReasonPhrase);
        }
    }


    public async Task<bool> IsAuthenticated()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetUserRoleFromToken()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return null;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            return roleClaim?.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task InitializeAuthState()
    {
        // Không cần thêm token vào header nữa vì đã có AuthHeaderHandler
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> IsAdmin()
    {
        var role = await GetUserRoleFromToken();
        return role == "Admin";
    }
}