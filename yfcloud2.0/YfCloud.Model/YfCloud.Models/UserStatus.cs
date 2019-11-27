using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 登陆状态
    /// </summary>
    public class UserStatus
    {
        /// <summary>
        /// guid
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 上次刷新时间
        /// </summary>
        public DateTime LastRefreshTime { get; set; }
    }
}
