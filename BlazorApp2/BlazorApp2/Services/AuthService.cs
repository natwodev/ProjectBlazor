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
                // Handle the case where the server returns only the token string within a JSON object
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    if (jsonDoc.RootElement.TryGetProperty("token", out var tokenElement) && tokenElement.ValueKind == JsonValueKind.String)
                    {
                        var token = tokenElement.GetString();
                        if (!string.IsNullOrEmpty(token))
                        {
                            // Lưu token vào localStorage
                            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
                            // Thêm token vào header mặc định cho các request tiếp theo
                            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                            OnAuthStateChanged?.Invoke();
                            // Return a successful AuthResultDto
                            return new AuthResultDto { IsSuccess = true, Token = token };
                        }
                    }

                    // If token property is not found or not a string, consider it a failure
                     return new AuthResultDto { IsSuccess = false, ErrorMessage = "Không thể đọc token từ server" };
                }
                catch (JsonException)
                {
                    // Handle cases where the response is not valid JSON
                    return new AuthResultDto { IsSuccess = false, ErrorMessage = "Phản hồi từ server không hợp lệ." };
                }
            }

            // Handle non-success status codes (assuming server might return AuthResultDto with error)
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
        // Xóa token khỏi localStorage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        // Xóa token khỏi header
        _httpClient.DefaultRequestHeaders.Authorization = null;
        OnAuthStateChanged?.Invoke();
        // Chuyển hướng về trang login
        _navigationManager.NavigateTo("/login");
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

            // Tìm claim 'role'
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");

            return roleClaim?.Value;
        }
        catch
        {
            // Xử lý lỗi khi đọc token
            return null;
        }
    }

    public async Task InitializeAuthState()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<bool> IsAdmin()
    {
        var role = await GetUserRoleFromToken();
        return role == "Admin";
    }
}