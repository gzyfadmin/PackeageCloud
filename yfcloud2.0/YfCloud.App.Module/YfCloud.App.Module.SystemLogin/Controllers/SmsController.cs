using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YfCloud.App.Module.SystemLogin.Models.Verification;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.Message;
using YfCloud.Utilities.Model;
using YfCloud.Utilities.WebApi;

namespace YfCloud.App.Module.SystemLogin.Controllers
{
    /// <summary>
    /// 短信
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="requestObject">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        [HttpOptions]
        public SubmitResult Post(RequestObject<VerificationEditModel> requestObject)
        {
            List<SingleReceiverMessage> messageList = new List<SingleReceiverMessage>();

            string templateId = @"1bb04f68ab6247ac904c2b8269420b32";
            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);
            string[] templateParas = { randCode };
            SingleReceiverMessage singleReceiverMessage = new SingleReceiverMessage(requestObject.PostData.Mobile, templateId, templateParas);

            messageList.Add(singleReceiverMessage);

            //发送短信
            SubmitResult result = HuaWeiShortMessageProvider.SendDiffContentMessages(messageList);

            return result;
        }


    }
}