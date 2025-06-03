using System.Net.Http.Json;
using BlazorApp1.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<AuthResultDTO> Login([FromBody] LoginModelDTO loginModel)
    {
        try
        {

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                return new AuthResultDTO
                {
                    IsSuccess = true,
                    Token = token,
                    Role = "", 
                };
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
                ErrorMessage = $"Có lỗi xảy ra khi đăng nhập: {ex.Message}"
            };
        }
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
}