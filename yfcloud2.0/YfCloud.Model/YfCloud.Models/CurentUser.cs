using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUser
    {
        public string ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 用户账户
        /// </summary>
        public string UserName { get; set; }
    }
}
