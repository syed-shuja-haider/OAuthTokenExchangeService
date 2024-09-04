using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OAuthTokenExchangeService.Controler
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult CreateToken()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "client_id"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var secretKey = Encoding.UTF8.GetBytes("your-256-bit-secret-key-goes-here-ensure-this-is-long-enough");
            var signingKey = new SymmetricSecurityKey(secretKey);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "https://localhost:7017/",
                "https://localhost:7017/",
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { access_token = tokenString });
        }
    }
}
