using backend_blazor.Models;

namespace backend_blazor.Authentication.Repositories
{
    public interface IAuthRepository
    {
        // Method to get user by ID
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        // Method to get permissions by user ID
    }
}