
namespace backend_blazor.Authentication.Services;

public interface IAuthService
{
    Task<AuthResultDTO> AuthenticateAsync(LoginModelDTO loginModel);
    Task LogoutAsync(string token);

}