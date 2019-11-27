using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.Model
{
    internal class SmsErrors
    {
        private static IDictionary<string, string> CodeErrDic = new Dictionary<string, string> {
            { "E000000", "表示系统异常，一般是请求格式异常，短信平台无法解析。" },
            { "E000001", "HTTP消息头未找到Authorization字段。" },
            { "E000002", "Authorization字段中未找到realm属性。" },
            { "E000003", "Authorization字段中未找到profile属性。" },
            { "E000004", "Authorization中realm属性值应该为“SDP”。" },
            { "E000005", "Authorization中profile属性值应该为“UsernameToken”。" },
            { "E000006", "Authorization中type属性值应该为“Appkey”。" },
            { "E000007", "Authorization字段中未找到type属性。" },
            { "E000008", "Authorization中没有携带WSSE。" },
            { "E000020", "HTTP头未找到X-WSSE字段。" },
            { "E000021", "X-WSSE字段中未找到UserName属性。" },
            { "E000022", "X-WSSE字段中未找到Nonce属性。" },
            { "E000023", "X-WSSE字段中未找到Created属性。" },
            { "E000024", "X-WSSE字段中未找到PasswordDigest属性。" },
            { "E000025", "Created属性格式错误。" },
            { "E000026", "X-WSSE字段中未找到UsernameToken属性。" },
            { "E000027", "非法请求。" },
            { "E000040", "ContentType值应该为application/x-www-form-urlencoded。" },
            { "E000503", "参数格式错误。" },
            { "E200037", "短信发送失败，描述见参数status。" },
            {"E000101","鉴权失败。"},
            {"E000102","app_key无效。"},
            {"E000103","app_key不可用。"},
            {"E000104","app_secret无效。"},
            {"E000105","digest无效。"},
            {"E000106","app_key没有调用本API的权限。"},
            {"E000109","用户状态未激活。"},
            {"E000110","时间超出限制。"},
            {"E000111","用户名或密码错误。"},
            {"E000112","用户状态已冻结。" },
        };

        private static IDictionary<string, string> StatusErrDic = new Dictionary<string, string>
        {
            {"E200015","待发送短信数量太大。"},
            {"E200028","模板变量校验失败。"},
            {"E200029","模板类型校验失败。"},
            {"E200030","模板未激活。"},
            {"E200031","协议校验失败。"},
            {"E200033","模板类型不正确。"},
        };

        public static string GetCodeErrorMessage(string errCode)
        {
            string errMsg = string.Empty;

            if (!CodeErrDic.TryGetValue(errCode, out errMsg))
                errMsg = "未知错误，编码：" + errCode;

            return errMsg;
        }

        public static string GetStatusErrorMessage(string errCode)
        {
            string errMsg = string.Empty;

            if (!StatusErrDic.TryGetValue(errCode, out errMsg))
                errMsg = "未知错误，编码：" + errCode;

            return errMsg;
        }

    }
}
