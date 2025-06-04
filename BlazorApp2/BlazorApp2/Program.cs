using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp2;
using BlazorApp2.Services;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CartItemService>();

// Đăng ký AuthHeaderHandler
builder.Services.AddTransient<AuthHeaderHandler>();

// Cấu hình HttpClient với AuthHeaderHandler
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler(); // Thêm inner handler
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5163/")
    };
});

var app = builder.Build();

// Khởi tạo trạng thái xác thực khi ứng dụng khởi động
var authService = app.Services.GetRequiredService<AuthService>();
await authService.InitializeAuthState();

await app.RunAsync();