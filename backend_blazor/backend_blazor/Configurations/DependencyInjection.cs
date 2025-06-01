using backend_blazor.Authentication.Repositories;
using backend_blazor.Authentication.Services;
using backend_blazor.Repositories.AuthRepository;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.AuthService;
using backend_blazor.Services.Interfaces;


namespace backend_blazor.Configurations
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
        
            // Register Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            
            // Register Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            
        }
    }
}