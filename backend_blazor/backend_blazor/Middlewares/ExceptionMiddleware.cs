using System.Net;
using System.Text.Json;

namespace backend_blazor.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Tiếp tục xử lý yêu cầu
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Nếu có lỗi, bắt và xử lý
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            var response = new
            {
                statusCode,
                message = exception.Message,
                details = isDev ? exception.StackTrace : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}