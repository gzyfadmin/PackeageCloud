using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.App.Module.System.Models.Verification;
using YfCloud.Models;
using YfCloud.DBModel;

namespace YfCloud.App.Module.System.Services
{
    /// <summary>
    /// 短信验证接口
    /// </summary>
    public interface IVerificationService
    {
        /// <summary>
        /// 发送注册手机验证短信
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        ResponseObject<string> SendRegisterVerificationCode(RequestObject<VerificationInputModel> requestObject);

        /// <summary>
        /// 发送忘记密码短信验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        ResponseObject<string> SendForgetVerificationCode(RequestObject<VerificationInputModel> requestObject);

        /// <summary>
        /// 发送多次登录失败验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        ResponseObject<string> SendTimePassVerificationCode(RequestObject<VerificationInputModel> requestObject);

        /// <summary>
        /// 发送更改手机号码验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        ResponseObject<string> SendChangeMobileVerificationCode(RequestObject<VerificationInputModel> requestObject);



        /// <summary>
        /// 验证注册短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<string>> GetVerificationCodeRegister(RequestGet requestObject);


        /// <summary>
        /// 验证忘记密码短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<string>> GetVerificationCodeForget(RequestGet requestObject);

        /// <summary>
        /// 验证多次登录失败短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        Task<ResponseObject<string>> GetVerificationCodeTimePass(RequestGet requestObject);

        /// <summary>
        /// 发送邮箱验证
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        ResponseObject<VerificationInputEmaiModel, string> SendVerificationEmailCode(RequestObject<VerificationInputEmaiModel> requestObject);

    }
}
