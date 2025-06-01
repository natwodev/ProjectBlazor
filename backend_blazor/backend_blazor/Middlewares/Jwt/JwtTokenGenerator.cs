using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace backend_blazor.Middlewares.Jwt
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, IEnumerable<string> roles, IEnumerable<string> permissions)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:key"] ?? "default_secret_key");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            // Thêm role claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Thêm permission claims (custom claim type)
            claims.AddRange(permissions.Select(permission => new Claim("permission", permission)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}