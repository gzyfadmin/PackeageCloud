using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 系统管理的当前登陆的用户信息
    /// </summary>
    public class SMUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string TelAccount { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set;}

        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAccount { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
