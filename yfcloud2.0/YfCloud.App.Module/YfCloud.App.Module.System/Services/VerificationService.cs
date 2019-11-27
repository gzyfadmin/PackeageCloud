using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YfCloud.App.Module.System.Models.Verification;
using YfCloud.Attributes;
using YfCloud.Models;
using YfCloud.Utilities;
using YfCloud.Utilities.Message;
using YfCloud.Utilities.Model;
using YfCloud.Utilities.WebApi;
using YfCloud.DBModel;
using YfCloud.Caches;
using SqlSugar;
using YfCloud.Utilities.SqlSugar;

namespace YfCloud.App.Module.System.Services
{
    /// <summary>
    /// 短信验证
    /// </summary>
    [UseDI(ServiceLifetime.Scoped, typeof(IVerificationService))]
    public class VerificationService : IVerificationService
    {
        /// <summary>
        /// 发送注册短信验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public ResponseObject<string> SendRegisterVerificationCode(RequestObject<VerificationInputModel> requestObject)
        {
            string result = string.Empty;

            if (requestObject.PostData == null)
                return ResponseUtil<string>.FailResult("PostData不能都为null");
            List<SingleReceiverMessage> messageList = new List<SingleReceiverMessage>();
            string templateId = @"1bb04f68ab6247ac904c2b8269420b32";
            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);
            result = randCode;
            string[] templateParas = { randCode };
            SingleReceiverMessage singleReceiverMessage = new SingleReceiverMessage(requestObject.PostData.Mobile, templateId, templateParas);
            messageList.Add(singleReceiverMessage);
            //发送短信
            Task.Factory.StartNew(() =>
            {
                try
                {
                    HuaWeiShortMessageProvider.SendDiffContentMessages(messageList);
                }
                catch { }
            });

            //写 登陆状态到redis 
            var redis = CacheFactory.Instance(CacheType.Redis);
            string redisKey = string.Format(CacheKeyString.RegisterMsgCode, requestObject.PostData.Mobile);
            redis.AddOrUpdateKey<string>(redisKey, result, 300);
            return ResponseUtil<string>.SuccessResult("发送成功");
        }


        /// <summary>
        /// 发送忘记密码短信验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public ResponseObject<string> SendForgetVerificationCode(RequestObject<VerificationInputModel> requestObject)
        {
            string result = string.Empty;

            if (requestObject.PostData == null)
                return ResponseUtil<string>.FailResult("PostData不能都为null");
            List<SingleReceiverMessage> messageList = new List<SingleReceiverMessage>();
            string templateId = @"595c42190cf847caadb0822b3685bcb6";
            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);
            result = randCode;
            string[] templateParas = { randCode };
            SingleReceiverMessage singleReceiverMessage = new SingleReceiverMessage(requestObject.PostData.Mobile, templateId, templateParas);
            messageList.Add(singleReceiverMessage);
            //发送短信
            Task.Factory.StartNew(() =>
            {
                try
                {
                    HuaWeiShortMessageProvider.SendDiffContentMessages(messageList);
                }
                catch { }
            });

            //写 登陆状态到redis 
            var redis = CacheFactory.Instance(CacheType.Redis);
            string redisKey = string.Format(CacheKeyString.ForgetMsgCode, requestObject.PostData.Mobile);
            redis.AddOrUpdateKey<string>(redisKey, result, 300);
            return ResponseUtil<string>.SuccessResult("发送成功");
        }

        /// <summary>
        /// 发送多次登录失败验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public ResponseObject<string> SendTimePassVerificationCode(RequestObject<VerificationInputModel> requestObject)
        {
            string result = string.Empty;

            if (requestObject.PostData == null)
                return ResponseUtil<string>.FailResult("PostData不能都为null");
            List<SingleReceiverMessage> messageList = new List<SingleReceiverMessage>();
            string templateId = @"1bb04f68ab6247ac904c2b8269420b32";
            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);
            result = randCode;
            string[] templateParas = { randCode };
            SingleReceiverMessage singleReceiverMessage = new SingleReceiverMessage(requestObject.PostData.Mobile, templateId, templateParas);
            messageList.Add(singleReceiverMessage);
            //发送短信
            Task.Factory.StartNew(() =>
            {
                try
                {
                    HuaWeiShortMessageProvider.SendDiffContentMessages(messageList);
                }
                catch { }
            });

            //写 登陆状态到redis 
            var redis = CacheFactory.Instance(CacheType.Redis);
            string redisKey = string.Format(CacheKeyString.TimePassMsgCode, requestObject.PostData.Mobile);
            redis.AddOrUpdateKey<string>(redisKey, result, 300);
            return ResponseUtil<string>.SuccessResult("发送成功");
        }

        /// <summary>
        /// 发送更改手机号码验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public ResponseObject<string> SendChangeMobileVerificationCode(RequestObject<VerificationInputModel> requestObject)
        {
            string result = string.Empty;
            if (requestObject.PostData == null)
                return ResponseUtil<string>.FailResult("PostData不能都为null");
            List<SingleReceiverMessage> messageList = new List<SingleReceiverMessage>();
            string templateId = @"1bb04f68ab6247ac904c2b8269420b32";
            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);
            result = randCode;
            string[] templateParas = { randCode };
            SingleReceiverMessage singleReceiverMessage = new SingleReceiverMessage(requestObject.PostData.Mobile, templateId, templateParas);
            messageList.Add(singleReceiverMessage);
            //发送短信
            Task.Factory.StartNew(() =>
            {
                try
                {
                    HuaWeiShortMessageProvider.SendDiffContentMessages(messageList);
                }
                catch { }
            }); 
            //写 登陆状态到redis 
            var redis = CacheFactory.Instance(CacheType.Redis);
            string redisKey = string.Format(CacheKeyString.ChangeMobileMsgCode, requestObject.PostData.Mobile);
            redis.AddOrUpdateKey<string>(redisKey, result, 300);
            return ResponseUtil<string>.SuccessResult("发送成功");
        }
         
        /// <summary>
        /// 验证注册短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<string>> GetVerificationCodeRegister(RequestGet requestObject)
        {
            try
            {
                String Mobile = "";
                String Code = "";
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "mobile")
                        {
                            Mobile = item.FieldValue;
                            continue;
                        }
                        if (item.FieldName.ToLower() == "code")
                        {
                            Code = item.FieldValue;
                            continue;
                        }
                    }
                }
                var redis = CacheFactory.Instance(CacheType.Redis);
                string redisKey = string.Format(CacheKeyString.RegisterMsgCode, Mobile);
                string RedisCode = redis.GetValueByKey<string>(redisKey);
                if (RedisCode == "")
                {
                    return ResponseUtil<string>.SuccessResult("0");
                }
                else if (RedisCode != Code)
                {
                    return ResponseUtil<string>.SuccessResult("2");
                }
                return ResponseUtil<string>.SuccessResult("1");
            }
            catch (Exception ex)
            {
                return ResponseUtil<string>
                    .FailResult(null, $"验证注册短信验证码合法性异常 {ex.Message}");
            }
        } 
        /// <summary>
        /// 验证忘记密码短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<string>> GetVerificationCodeForget(RequestGet requestObject)
        {
            try
            {
                String Mobile = "";
                String Code = "";
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "mobile")
                        {
                            Mobile = item.FieldValue;
                            continue;
                        }
                        if (item.FieldName.ToLower() == "code")
                        {
                            Code = item.FieldValue;
                            continue;
                        }
                    }
                }
                var redis = CacheFactory.Instance(CacheType.Redis);
                string redisKey = string.Format(CacheKeyString.ForgetMsgCode, Mobile);
                string RedisCode = redis.GetValueByKey<string>(redisKey);
                if (RedisCode == "")
                {
                    return ResponseUtil<string>.SuccessResult("0");
                }
                else if (RedisCode != Code)
                {
                    return ResponseUtil<string>.SuccessResult("2");
                }
                return ResponseUtil<string>.SuccessResult("1");
            }
            catch (Exception ex)
            {
                return ResponseUtil<string>
                    .FailResult(null, $"验证注册短信验证码合法性异常 {ex.Message}");
            }
        }
        /// <summary>
        /// 验证多次登录失败短信验证码合法性
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public async Task<ResponseObject<string>> GetVerificationCodeTimePass(RequestGet requestObject)
        {
            try
            {
                String Mobile = "";
                String Code = "";
                if (requestObject.QueryConditions != null && requestObject.QueryConditions.Count > 0)
                {
                    var conditionals = SqlSugarUtil.GetConditionalModels(requestObject.QueryConditions);
                    foreach (ConditionalModel item in conditionals)
                    {
                        if (item.FieldName.ToLower() == "mobile")
                        {
                            Mobile = item.FieldValue;
                            continue;
                        }
                        if (item.FieldName.ToLower() == "code")
                        {
                            Code = item.FieldValue;
                            continue;
                        }
                    }
                }
                var redis = CacheFactory.Instance(CacheType.Redis);
                string redisKey = string.Format(CacheKeyString.TimePassMsgCode, Mobile);
                string RedisCode = redis.GetValueByKey<string>(redisKey);
                if (RedisCode == "")
                {
                    return ResponseUtil<string>.SuccessResult("0");
                }
                else if (RedisCode != Code)
                {
                    return ResponseUtil<string>.SuccessResult("2");
                }
                return ResponseUtil<string>.SuccessResult("1");
            }
            catch (Exception ex)
            {
                return ResponseUtil<string>
                    .FailResult(null, $"验证注册短信验证码合法性异常 {ex.Message}");
            }
        }


        /// <summary>
        /// 发邮箱验证码
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>

        public ResponseObject<VerificationInputEmaiModel, string> SendVerificationEmailCode(RequestObject<VerificationInputEmaiModel> requestObject)
        {
            string result = string.Empty;

            string randCode = RandCodeCreate.GenerateRandomOnlyNumber(6);

            result = randCode;

            //预留发邮件的接口

            return ResponseUtil<VerificationInputEmaiModel, string>.SuccessResult(requestObject, result);
        }
    }
}
