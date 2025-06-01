using backend_blazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace backend_blazor.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> GetUserRolesAsync(string userId);
        // Phương thức RoleClaim
        Task<ApplicationUser?> FindByEmailAsync(string email);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, ExternalLoginInfo info);
        Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey);
        Task<List<string>> GetUserPermissionsAsync(ApplicationUser user);

    }
}
