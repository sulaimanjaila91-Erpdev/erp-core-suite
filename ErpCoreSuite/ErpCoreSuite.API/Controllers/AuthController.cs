// ErpCoreSuite.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ErpCoreSuite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // In production: validate against Users table with hashed password
            // This is a demo — replace with DB lookup
            var validUsers = new Dictionary<string, (string Password, string Role)>
            {
                { "admin",      ("Admin@123", "Admin") },
                { "hruser",     ("Hr@123",    "HR") },
                { "accountant", ("Acc@123",   "Accountant") }
            };

            if (!validUsers.TryGetValue(request.Username.ToLower(), out var user)
                || user.Password != request.Password)
            {
                return Unauthorized(new { success = false, message = "Invalid username or password" });
            }

            var token = GenerateJwt(request.Username, user.Role);

            return Ok(new
            {
                success  = true,
                token    = token,
                role     = user.Role,
                username = request.Username,
                expiry   = DateTime.UtcNow.AddHours(8)
            });
        }

        private string GenerateJwt(string username, string role)
        {
            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims  = new[]
            {
                new Claim(ClaimTypes.Name,  username),
                new Claim(ClaimTypes.Role,  role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             _config["Jwt:Issuer"],
                audience:           _config["Jwt:Audience"],
                claims:             claims,
                expires:            DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
