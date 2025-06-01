using AspNetCoreRateLimit;
using backend_blazor.Middlewares;

namespace backend_blazor.Extensions
{
    public static class AppExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>(); // Đăng ký middleware xử lý lỗi

            app.UseMiddleware<MaintenanceMiddleware>();

            // Cấu hình API lắng nghe trên tất cả IP trong mạng
            app.Urls.Add("http://0.0.0.0:5163");

            app.UseCors("AllowFlutter"); // Bật CORS
            app.UseHttpsRedirection();
            
            //  Kiểm tra token trước khi xác thực
            app.UseMiddleware<JwtBlacklistMiddleware>();
            
            // Xác thực & phân quyền
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseStaticFiles();  // Cho phép phục vụ file tĩnh

            app.UseIpRateLimiting();
            
            // Xử lý API
            app.MapControllers();
          //  app.MapHub<NotificationHub>("/notificationHub");

        }
    }
}