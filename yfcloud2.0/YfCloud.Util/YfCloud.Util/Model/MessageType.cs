using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YfCloud.Utilities.Model
{
    /// <summary>
    /// 短信类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 标准短信
        /// </summary>
        [Description("标准短信")]
        Standard,

        /// <summary>
        /// 营销短信
        /// </summary>
        [Description("营销短信")]
        Marketing,
    }
}
