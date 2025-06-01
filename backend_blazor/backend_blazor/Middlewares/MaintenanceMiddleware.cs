using Microsoft.Extensions.Caching.Distributed;

namespace backend_blazor.Middlewares;

public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _cache;
    private readonly ILogger<MaintenanceMiddleware> _logger;

    public MaintenanceMiddleware(RequestDelegate next, IDistributedCache cache, ILogger<MaintenanceMiddleware> logger)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? maintenanceFlag = "false"; // Default is no maintenance

        try
        {
            maintenanceFlag = await _cache.GetStringAsync("maintenance_mode");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while checking maintenance mode from Redis");
        }

        if (string.IsNullOrEmpty(maintenanceFlag))
        {
            maintenanceFlag = "false"; // Default to false if no value is retrieved
        }

        if (maintenanceFlag == "true" && 
            !context.Request.Path.StartsWithSegments("/api/auth/secret-login") && 
            !context.Request.Path.StartsWithSegments("/api/admin/maintenance"))
        {
            _logger.LogWarning("System is in maintenance mode, request denied: {Path}", context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsJsonAsync(new {
                message = "Hệ thống đang bảo trì, vui lòng quay lại sau."
            });
            return;
        }

        await _next(context);
    }
}