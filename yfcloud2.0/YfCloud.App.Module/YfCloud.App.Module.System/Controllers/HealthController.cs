using Microsoft.AspNetCore.Mvc;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "OK";
        }
    }
}