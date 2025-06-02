using System.Net.Http.Json;
using System.Text;
using BlazorApp1.DTOs;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public AuthService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public async Task<AuthResultDTO> Login(LoginModelDTO loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResultDTO>();
                if (result != null && result.IsSuccess)
                {
                    // Store the token in localStorage
                    await _httpClient.PostAsJsonAsync("api/auth/store-token", new { token = result.Token });
                    return result;
                }
            }
            
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return new AuthResultDTO 
            { 
                IsSuccess = false, 
                ErrorMessage = error?.Message ?? "Đăng nhập thất bại" 
            };
        }
        catch (Exception ex)
        {
            return new AuthResultDTO 
            { 
                IsSuccess = false, 
                ErrorMessage = "Có lỗi xảy ra khi đăng nhập" 
            };
        }
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
} 