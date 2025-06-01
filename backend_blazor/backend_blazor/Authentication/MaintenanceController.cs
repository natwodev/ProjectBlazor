using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace backend_blazor.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;

        public MaintenanceController(IDistributedCache cache, IConfiguration config)
        {
            _cache = cache;
            _config = config;
        }

        [HttpPost("maintenance")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ToggleMaintenance([FromQuery] string key, [FromQuery] bool enable)
        {
            var secretKey = _config["SecretAccess:SecretLoginKey"];

            if (key != secretKey)
                return Forbid("Bạn không có quyền thay đổi chế độ bảo trì.");

            // Xóa key cũ trong Redis trước khi thiết lập lại giá trị
            await _cache.RemoveAsync("maintenance_mode");

            // Dùng StringSet thay vì SetStringAsync
            await _cache.SetStringAsync("maintenance_mode", enable ? "true" : "false");

            return Ok(new { message = enable ? "Chế độ bảo trì đã được bật." : "Chế độ bảo trì đã được tắt." });
        }



        [HttpGet("maintenance")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CheckMaintenance()
        {
            var status = await _cache.GetStringAsync("maintenance_mode");
            return Ok(new { maintenance = status == "true" });
        }
    }
}

// POST: api/maintenance/maintenance?key=xxx&enable=true    → Bật/tắt chế độ bảo trì (cần key và quyền Admin)
// GET: api/maintenance/maintenance                         → Kiểm tra trạng thái chế độ bảo trì (cần quyền Admin)
