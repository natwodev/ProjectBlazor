using System.Security.Claims;
using backend_blazor.Authentication.Services;
using backend_blazor.Middlewares.Jwt;
using backend_blazor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend_blazor.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        
        public AuthController(IAuthService authService, IConfiguration configuration,IUserService userService)
        {
            _authService = authService;
            _configuration = configuration;
            _userService = userService;
            _jwtTokenGenerator = new JwtTokenGenerator(configuration); // Khởi tạo JwtTokenGenerator
        }
        
        // Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO loginModel)
        {
            try
            {
                var authResult = await _authService.AuthenticateAsync(loginModel);

                if (!authResult.IsSuccess)
                {
                    return BadRequest(new { message = authResult.ErrorMessage });
                }

                return Ok(new
                {
                    token = authResult.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau." });
            }
        }
       

        
        // Đăng nhập bí mật
        [HttpPost("secret-login")]
        public async Task<IActionResult> SecretLogin([FromQuery] string key, [FromBody] LoginModelDTO loginModel)
        {
            var secretKey = _configuration["SecretAccess:SecretLoginKey"];

            if (key != secretKey)
            {
                return Forbid("Bạn không có quyền sử dụng đường dẫn này.");
            }

            try
            {
                var authResult = await _authService.AuthenticateAsync(loginModel);

                if (!authResult.IsSuccess)
                {
                    return BadRequest(new { message = authResult.ErrorMessage });
                }

                return Ok(new
                {
                    token = authResult.Token
                });
            }
            catch
            {
                return StatusCode(500, new { message = "Đăng nhập thất bại. Vui lòng thử lại sau!" });
            }
        }
        
        // Đăng xuất
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"]
                .FirstOrDefault()
                ?.Split(" ")
                .Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }
        
            try
            {
                await _authService.LogoutAsync(token);
                return Ok(new { message = "Successfully logged out" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _userService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
                return BadRequest($"Lỗi xác thực: {remoteError}");

            var info = await _userService.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            // 1. Kiểm tra Google login đã được liên kết chưa
            var user = await _userService.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                // 2. Nếu chưa liên kết, lấy email từ Google
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Không thể lấy email từ tài khoản Google.");

                // 3. Tìm user nội bộ có email này
                user = await _userService.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest("Tài khoản Google chưa được liên kết và email không tồn tại trong hệ thống.");
                }

                // 4. Nếu user tồn tại → thêm liên kết Google vào bảng AspNetUserLogins
                var result = await _userService.AddLoginAsync(user, info);
                if (!result.Succeeded)
                {
                    return BadRequest("Không thể liên kết tài khoản Google với người dùng.");
                }
            }

            // 5. Lấy roles & permissions như login thường
            var roles = await _userService.GetUserRolesAsync(user.Id);
            var permissions = await _userService.GetUserPermissionsAsync(user);

            // 6. Tạo JWT token
            var token = _jwtTokenGenerator.GenerateJwtToken(user.UserName, roles, permissions);

            return Ok(new { token });
        }

    }
}

// POST: api/auth/login          → Đăng nhập, trả về token
// POST: api/auth/secret-login   → Đăng nhập bí mật (cần key), trả về token
// POST: api/auth/logout         → Đăng xuất, hủy token hiện tại
