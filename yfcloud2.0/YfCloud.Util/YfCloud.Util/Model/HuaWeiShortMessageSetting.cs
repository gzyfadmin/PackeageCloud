using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.Model
{
    /// <summary>
    /// 华为短信账户信息
    /// </summary>
    public class HuaWeiShortMessageSetting 
    {
        /// <summary>
        ///AppKey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 发送相同短信接口路由
        /// </summary>
        public string SendSameUrl { get; set; }

        /// <summary>
        /// 发送不通短信接口路由
        /// </summary>
        public string SendDiffUrl { get; set; }

        /// <summary>
        /// 接入地址
        /// </summary>
        public string RootUrl { get; set; }

        /// <summary>
        /// 发送通道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        public string MsgPrefix { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string DomainUrl { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public static HuaWeiShortMessageSetting Default
        {
            get
            {
                return new HuaWeiShortMessageSetting
                {
                    RootUrl= @"https://api.rtc.huaweicloud.com:10443",
                    AppKey = "N401K8o0etqB4JCjhAwT2mZpfO7X",
                    AppSecret = "5253097BO6ct9jyk33POs96N7C91",
                    SendSameUrl = "/sms/batchSendSms/v1",
                    SendDiffUrl = "/sms/batchSendDiffSms/v1",
                    DomainUrl = "",
                    MsgPrefix= "云飞云",
                    Channel= "8819070534606"
                };
            }
        }
    }
}
