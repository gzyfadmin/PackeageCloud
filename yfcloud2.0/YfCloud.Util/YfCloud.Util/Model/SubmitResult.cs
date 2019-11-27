using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.Model
{
    /// <summary>
    /// 短信接口返回结果
    /// </summary>
   public class SubmitResult
    {
        /// <summary>
        /// 是否提交成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息编码供应商返回
        /// </summary>
        public string MessageNo { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
