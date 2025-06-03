using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public AuthService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public async Task<(bool success, string token, string error)> Login(string username, string password)
    {
        try
        {
            var payload = new Dictionary<string, string>
            {
                ["UserName"] = username,
                ["Password"] = password
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return (true, token, "");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                var json = JsonDocument.Parse(responseContent);
                var message = json.RootElement.GetProperty("message").GetString();
                return (false, "", message ?? "Đăng nhập thất bại");
            }
            catch
            {
                return (false, "", "Đăng nhập thất bại");
            }
        }
        catch (Exception ex)
        {
            return (false, "", $"Lỗi: {ex.Message}");
        }
    }
}