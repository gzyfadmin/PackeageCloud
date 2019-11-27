using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.LogStatus.Service;
using YfCloud.Caches;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.LogStatus.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ICheckLoginStatusService _checkLoginStatusService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="checkLoginStatusService"></param>
        public HealthController(ICheckLoginStatusService checkLoginStatusService)
        {
            _checkLoginStatusService = checkLoginStatusService;
        }


        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "OK";
        }

        /// <summary>
        /// 在线检查
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Post()
        {
            //
            var currentUser  = TokenManager.GetIDbyToken(Request.Headers);
            _checkLoginStatusService.RefresUserStatus(currentUser);
            return "OK";
        }
    }
}