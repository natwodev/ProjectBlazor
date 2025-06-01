using Microsoft.Extensions.Caching.Distributed;

namespace backend_blazor.Middlewares;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;

    public JwtBlacklistMiddleware(RequestDelegate next, IDistributedCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var isRevoked = await _cache.GetStringAsync($"blacklist:{token}");
            if (!string.IsNullOrEmpty(isRevoked))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token has been revoked");
                return;
            }
        }

        await _next(context);
    }
}