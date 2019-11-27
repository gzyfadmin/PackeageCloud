using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.System.Models.TSMUserAccount;
using YfCloud.App.Module.System.Service;
using YfCloud.Framework;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// 账户验证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CheckAccountController : ControllerBase
    {
        private readonly ITSMUserAccountService _service;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CheckAccountController(ITSMUserAccountService service)
        {
            _service = service;
        }

        /// <summary>
        /// 验证手机号、邮箱是否已经存在
        /// </summary>
        /// <param name="requestObject">返回响应结果</param>
        [HttpPost]
        [HttpOptions]
        public async Task<ResponseObject<TSMUserAccountInputModel, bool>> Post(RequestObject<TSMUserAccountInputModel> requestObject)
        {
            return await _service.CheckIsExistsAsync(requestObject);
        }
    }
}