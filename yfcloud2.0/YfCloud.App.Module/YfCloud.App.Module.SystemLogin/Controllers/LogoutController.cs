using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.SystemLogin.Models.TSMUserAccount;
using YfCloud.App.Module.SystemLogin.Services;
using YfCloud.Models;
using YfCloud.Utilities.WebApi;
using YfCloud.DBModel;
using YfCloud.Framework;

namespace YfCloud.App.Module.SystemLogin.Controllers
{
    /// <summary>
    /// 系统退出
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly ILogoutService _service;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="service"></param>
        public LogoutController(ILogoutService service)
        {
            _service = service;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<bool>> LogOut()
        {
            var curent = TokenManager.GetCurentUserbyToken(Request.Headers);
            return await _service.LogoutAsync(curent);
        }
    }
}