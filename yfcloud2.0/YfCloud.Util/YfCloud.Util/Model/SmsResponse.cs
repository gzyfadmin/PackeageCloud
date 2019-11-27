using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Utilities.Model
{
    /// <summary>
    /// 短信响应
    /// </summary>
    public class SmsResponse
    {
        /// <summary>
        /// 结果
        /// </summary>
        public Result[] result { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string description { get; set; }
    }

    /// <summary>
    /// 相应主体
    /// </summary>
    public class Result
    {
        /// <summary>
        /// originTo
        /// </summary>
        public string originTo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 短信ID
        /// </summary>
        public string smsMsgId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
    }
}
