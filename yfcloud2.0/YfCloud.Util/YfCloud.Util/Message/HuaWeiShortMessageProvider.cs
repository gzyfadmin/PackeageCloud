using System;
using System.Collections.Generic;
using System.Text;
using YfCloud.Utilities.Model;
using System.Linq;
using System.Collections;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.Net.Security;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Collections.Specialized;
using System.Web;

namespace YfCloud.Utilities.Message
{
    /// <summary>
    /// 华为短信发送工具
    /// </summary>
   public class HuaWeiShortMessageProvider
    {
        /// <summary>
        /// 发送不同短信内容
        /// </summary>
        /// <param name="messages">短信数组</param>
        /// <returns></returns>
        public static SubmitResult SendDiffContentMessages(IEnumerable<SingleReceiverMessage> messages)
        {

            SubmitResult submitResult = new SubmitResult
            {
                Success = true
            };

            if (messages.Count() > 100)
                throw new  Exception("号码个数超过100个");


            HuaWeiShortMessageSetting msgSetting = HuaWeiShortMessageSetting.Default;

            var gatewayUrl = msgSetting.RootUrl;
            string username = msgSetting.Channel;//存储通道
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("短信网关通道不存在");
            }

            //必填,请参考"开发准备"获取如下数据,替换为实际值
            string apiAddress = gatewayUrl + msgSetting.SendDiffUrl; //APP接入地址+接口访问URI
            string appKey = msgSetting.AppKey; //APP_Key
            string appSecret = msgSetting.AppSecret; //APP_Secret




            ArrayList smsContent = new ArrayList();


            foreach (SingleReceiverMessage item in messages)
            {
                string[] receiver = { item.Mobile };

                smsContent.Add(InitDiffSms(receiver, item.templateId, item.templateParas, username));
            }


            try
            {
                string statusCallBack = "";
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //使用Tls1.2 = 3072
                ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | (SecurityProtocolType)3072;

                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(apiAddress);
                //请求方法
                myReq.Method = "POST";
                //请求Headers
                myReq.ContentType = "application/json";
                myReq.Headers.Add("Authorization", "WSSE realm=\"SDP\",profile=\"UsernameToken\",type=\"Appkey\"");
                myReq.Headers.Add("X-WSSE", BuildWSSEHeader(appKey, appSecret));
                //请求Body
                var body = new Dictionary<string, object>() {
                    {"from", username},
                    {"statusCallback", statusCallBack},
                    {"smsContent", smsContent}
                };

                string jsonData = JsonConvert.SerializeObject(body);

                //发送请求数据
                StreamWriter req = new StreamWriter(myReq.GetRequestStream(), Encoding.GetEncoding("utf-8"));
                req.Write(jsonData);
                req.Close();

                //获取响应数据
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                StreamReader resp = new StreamReader(myResp.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                string result = resp.ReadToEnd();
                myResp.Close();
                resp.Close();

                SmsResponse res = JsonConvert.DeserializeObject<SmsResponse>(result);

                if (res.code != "000000")
                {
                    submitResult.MessageNo = res.code;
                    submitResult.Success = false;
                    if (res.code == "E200037")
                    {
                        submitResult.ErrorMsg = SmsErrors.GetStatusErrorMessage(res.result[0].status);
                    }
                    else
                    {
                        submitResult.ErrorMsg = SmsErrors.GetCodeErrorMessage(res.code);
                    }
                }

                return submitResult;



            }
            catch (Exception ex)
            {
                throw new Exception("短信通道连接失败，请稍后重试！");
            }

        }


        /// <summary>
        /// 构造smsContent参数值
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="templateId"></param>
        /// <param name="templateParas"></param>
        /// <param name="signature">签名名称,使用国内短信通用模板时填写</param>
        /// <returns></returns>
        static Dictionary<string, object> InitDiffSms(string[] receiver, string templateId, string[] templateParas, string signature)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                {"to", receiver},
                {"templateId", templateId},
                {"templateParas", templateParas}
            };
            if (!signature.Equals(null) && signature.Length > 0)
            {
                dic.Add("signature", signature);
            }

            return dic;
        }

        /// <summary>
        /// 构造X-WSSE参数值
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        static string BuildWSSEHeader(string appKey, string appSecret)
        {
            string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); //Created
            string nonce = Guid.NewGuid().ToString().Replace("-", ""); //Nonce

            byte[] material = Encoding.UTF8.GetBytes(nonce + now + appSecret);
            byte[] hashed = SHA256Managed.Create().ComputeHash(material);
            string hexdigest = BitConverter.ToString(hashed).Replace("-", "");
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexdigest)); //PasswordDigest

            return String.Format("UsernameToken Username=\"{0}\",PasswordDigest=\"{1}\",Nonce=\"{2}\",Created=\"{3}\"",
                            appKey, base64, nonce, now);
        }

        /// <summary>
        /// 构造请求body
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        static string BuildQueryString(NameValueCollection keyValues)
        {
            StringBuilder temp = new StringBuilder();
            foreach (string item in keyValues.Keys)
            {
                temp.Append(item).Append("=").Append(HttpUtility.UrlEncode(keyValues.Get(item))).Append("&");
            }
            return temp.Remove(temp.Length - 1, 1).ToString();
        }

    }
}
