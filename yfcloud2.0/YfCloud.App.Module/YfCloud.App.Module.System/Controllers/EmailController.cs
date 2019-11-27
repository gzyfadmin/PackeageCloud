using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.System.Models.Verification;
using YfCloud.App.Module.System.Services;
using YfCloud.Models;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// 邮箱验证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailController : ControllerBase
    {
        private IVerificationService _verificationService;


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="verificationService"></param>
        public EmailController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        [HttpOptions]
        public ResponseObject<VerificationInputEmaiModel, string> Post(RequestObject<VerificationInputEmaiModel> requestObject)
        {
            return _verificationService.SendVerificationEmailCode(requestObject);
        }
    }
}