
using backend_blazor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace backend_blazor.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<ApplicationUser?> FindByEmailAsync(string email);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
        Task<IdentityResult> AddLoginAsync(ApplicationUser user, ExternalLoginInfo info);
        Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey);
        Task<List<string>> GetUserPermissionsAsync(ApplicationUser user);
    }
}
