using Microsoft.AspNetCore.Mvc;
using danh_gia_csharp.ConnectDB;
using danh_gia_csharp.model;

namespace danh_gia_csharp.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckConnectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CheckConnectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("check")]
        public async Task<IActionResult> InsertClass()
        {
            // Khởi tạo MongoDbContext với IConfiguration
            var db = new MongoDbContext<Class>(_configuration);

            // Kiểm tra kết nối
            bool isConnected = db.CheckConnection();

            if (isConnected)
            {
                return Ok("Kết nối MongoDB thành công.");
            }
            else
            {
                return StatusCode(500, "Lỗi kết nối MongoDB.");
            }
        }
    }
}
