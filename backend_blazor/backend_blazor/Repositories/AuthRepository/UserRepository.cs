
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend_blazor.Repositories.AuthRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        
        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IdentityResult> AddLoginAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            return await _userManager.AddLoginAsync(user, info);
        }

        public async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return await _userManager.FindByLoginAsync(loginProvider, providerKey);
        }

        public async Task<List<string>> GetUserPermissionsAsync(ApplicationUser user)
        {
            var permissions = new List<string>();

            // Claims gắn trực tiếp với user
            var userClaims = await _userManager.GetClaimsAsync(user);
            permissions.AddRange(userClaims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value));

            // Claims từ các role
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    permissions.AddRange(roleClaims
                        .Where(c => c.Type == "permission")
                        .Select(c => c.Value));
                }
            }

            // Loại bỏ trùng lặp
            return permissions.Distinct().ToList();
        }

    }
}
