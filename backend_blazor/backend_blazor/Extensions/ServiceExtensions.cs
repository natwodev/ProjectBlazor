using AspNetCoreRateLimit;
using backend_blazor.Configurations;
using backend_blazor.Data;
using backend_blazor.Mappings;
using backend_blazor.Middlewares.Jwt;
using backend_blazor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend_blazor.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /*
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");

                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];

                    // Optional: cấu hình đường callback nếu bạn dùng route tùy chỉnh
                    // googleOptions.CallbackPath = new PathString("/api/auth/external-login-callback");
                });
                */
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddSignalR();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
            });
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<IdentityOptions>(options =>
            {
                // Cấu hình số lần đăng nhập sai tối đa trước khi bị khóa
                options.Lockout.MaxFailedAccessAttempts = 10; // ví dụ cho phép 10 lần
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // khóa 15 phút
                options.Lockout.AllowedForNewUsers = true; // áp dụng cho cả người dùng mới
            });


            // Đăng ký DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFlutter", policy =>
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });


            // Đăng ký các service khác
            services.ConfigureDependencies();

            // Đăng ký xác thực JWT (được tách riêng)
            services.ConfigureJwt(configuration);

            // Đăng ký các service cần thiết cho API Controllers
            services.AddControllers();
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; // Thay bằng địa chỉ Redis của bạn
            });

            //đăng ký tạo policy phân quyền
            services.AddAuthorization(options =>
            {
                AuthorizationPolicies.AddCustomPolicies(options);
            });
            
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("RateLimit"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        }
    }
}