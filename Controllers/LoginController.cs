using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace danh_gia_csharp.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kiểm tra thông tin đăng nhập (giả sử username = "test" và password = "123456")
            if (request.Username == "test" && request.Password == "123456")
            {
                // Tạo Claims
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                // Tạo ký JWT
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-strong-256-bit-long-secret-key-here234567890123456"));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "your-issuer",
                    audience: "your-audience",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials);

                // Trả về JWT
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }

        // Lớp để nhận dữ liệu login từ client
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        // Lớp Account để lấy thông tin tài khoản
        public class Account
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
