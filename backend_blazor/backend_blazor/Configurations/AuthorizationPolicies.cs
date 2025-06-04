using Microsoft.AspNetCore.Authorization;

namespace backend_blazor.Configurations
{
    public static class AuthorizationPolicies
    {
       // string[] roles = { "Admin", "Seller", "Customer", "Shipper", "Support" };

        public static void AddCustomPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
            
            options.AddPolicy("SellerOnly", policy =>
                policy.RequireRole("Seller"));
            
            options.AddPolicy("CustomerOnly", policy =>
                policy.RequireRole("Customer"));
            
            options.AddPolicy("CustomerOrAdmin", policy =>
                policy.RequireRole("Customer","Admin"));
        }
    }
}