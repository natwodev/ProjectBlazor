using System.IdentityModel.Tokens.Jwt;
using backend_blazor.Authentication.Repositories;
using backend_blazor.Middlewares.Jwt;
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace backend_blazor.Authentication.Services
   {
       public class AuthService : IAuthService
       {
           private readonly IAuthRepository _authRepository;
           private readonly JwtTokenGenerator _jwtTokenGenerator;
           private readonly SignInManager<ApplicationUser> _signInManager;
           private readonly RoleManager<IdentityRole> _roleManager;
           private readonly IDistributedCache _cache;
           private readonly IUserRepository _userRepository;


           public AuthService(IAuthRepository authRepository, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,RoleManager<IdentityRole> roleManager,IDistributedCache cache,IUserRepository userRepository)
           {
               _authRepository = authRepository;
               _signInManager = signInManager;
               _jwtTokenGenerator = new JwtTokenGenerator(configuration); // Khởi tạo JwtTokenGenerator
               _roleManager = roleManager;
               _cache = cache;
               _userRepository = userRepository;
           }
   
           // Phương thức xác thực
          public async Task<AuthResultDTO> AuthenticateAsync(LoginModelDTO loginModel)
          {
              // Tìm kiếm người dùng theo tên đăng nhập
              var user = await _authRepository.GetUserByUsernameAsync(loginModel.UserName);

              if (user == null)
              {
                  return new AuthResultDTO
                  {
                      IsSuccess = false,
                      ErrorMessage = "Tài khoản không tồn tại"
                  };
              }
              
              // Kiểm tra tài khoản có bị khóa không
              if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
              {
                  return new AuthResultDTO
                  {
                      IsSuccess = false,
                      ErrorMessage = "Tài khoản đã bị khóa. Vui lòng liên hệ phòng đào tạo để biết thêm chi tiết!"
                  };
              }
              // Kiểm tra mật khẩu với SignInManager (bạn có thể bỏ qua phần này nếu không cần)
              var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
              if (!signInResult.Succeeded)
              {
                  return new AuthResultDTO
                  {
                      IsSuccess = false,
                      ErrorMessage = "Sai tài khoản hoặc mật khẩu."
                  };
              }
              
              // ✅ Lấy role và permission giống như login thường
              var roles = await _userRepository.GetUserRolesAsync(user);
              var permissions = await _userRepository.GetUserPermissionsAsync(user);
              // Tạo JWT với thông tin userName, roles và permissions
              var tokenString = _jwtTokenGenerator.GenerateJwtToken(user.Id, roles, permissions);
              return new AuthResultDTO
              {
                  IsSuccess = true,
                  Token = tokenString
              };
          }

          public async Task LogoutAsync(string token)
          {
              var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

              if (jwtToken == null)
              {
                  throw new ArgumentException("Invalid token");
              }

              var expirationTime = jwtToken.ValidTo;
              var timeToExpire = expirationTime - DateTime.UtcNow;

              if (timeToExpire.TotalSeconds > 0)
              {
                  await _cache.SetStringAsync(
                      $"blacklist:{token}",
                      "revoked",
                      new DistributedCacheEntryOptions
                      {
                          AbsoluteExpirationRelativeToNow = timeToExpire
                      }
                  );
              }
          }
       }
   }
   