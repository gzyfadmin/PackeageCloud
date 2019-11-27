using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YfCloud.App.Module.System.Models.Verification;
using YfCloud.App.Module.System.Services;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.Message;
using YfCloud.Utilities.Model;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.System.Controllers
{
    /// <summary>
    /// 短信验证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SmsController : ControllerBase
    {
        private IVerificationService _verificationService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="verificationService"></param>
        public SmsController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        /// <summary>
        /// 发送注册短信验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public ResponseObject<string> PostRegister(RequestObject<VerificationInputModel> requestObject)
        {
            return _verificationService.SendRegisterVerificationCode(requestObject);
        }

        /// <summary>
        /// 发送忘记密码短信验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public ResponseObject<string> PostForget(RequestObject<VerificationInputModel> requestObject)
        {
            return _verificationService.SendForgetVerificationCode(requestObject);
        }
        /// <summary>
        /// 发送多次登录失败验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public ResponseObject<string> PostTimePass(RequestObject<VerificationInputModel> requestObject)
        {
            return _verificationService.SendTimePassVerificationCode(requestObject);
        }

        /// <summary>
        /// 发送更改手机号码验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpPost]
        [HttpOptions]
        [Route("[action]")]
        public ResponseObject<string> PostChangeMobile(RequestObject<VerificationInputModel> requestObject)
        {
            return _verificationService.SendChangeMobileVerificationCode(requestObject);
        }

        /// <summary>
        /// 验证注册短信验证码合法性[0=失效,1=有效,2=验证码不正确]
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpGet]
        [Route("[action]")]
        public Task<ResponseObject<string>> GetVerificationCodeRegister(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return _verificationService.GetVerificationCodeRegister(request);
        }
        /// <summary>
        /// 验证忘记密码短信验证码合法性[0=失效,1=有效,2=验证码不正确]
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpGet]
        [Route("[action]")]
        public Task<ResponseObject<string>> GetVerificationCodeForget(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return _verificationService.GetVerificationCodeForget(request);
        }
        /// <summary>
        /// 验证多次登录失败短信验证码合法性[0=失效,1=有效,2=验证码不正确]
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns>返回验证码</returns>
        [HttpGet]
        [Route("[action]")]
        public Task<ResponseObject<string>> GetVerificationCodeTimePass(string requestObject)
        {
            var request = JsonConvert.DeserializeObject<RequestGet>(requestObject);
            return _verificationService.GetVerificationCodeTimePass(request);
        }

        //// <summary>
        ///// 发送验证码
        ///// </summary>
        ///// <param name="requestObject">输入参数</param>
        ///// <returns>返回验证码</returns>
        //[HttpPost]
        //[HttpOptions]
        //[Route("[action]")]
        //public ResponseObject<VerificationInputEmaiModel, string> Send(RequestObject<VerificationInputEmaiModel> requestObject)
        //{
        //    return _verificationService.SendVerificationEmailCode(requestObject);
        //}
    }
}