using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace BlazorApp2.Services;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public AuthHeaderHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.RequestUri?.AbsolutePath.Contains("api/auth/login", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
} 