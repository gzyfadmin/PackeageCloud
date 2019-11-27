using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.Utilities;

namespace YfCloud.App.Module.LogStatus.Model
{
    /// <summary>
    /// 登录日志查询实体
    /// </summary>
    public class LoginLogQuery
    {
        /// <summary>
        /// 当前登陆的ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 登陆开始时间
        /// </summary>
        public DateTime? LoginTimeBg { get; set; }

        /// <summary>
        /// 登陆结束时间
        /// </summary>
        public DateTime? LoginTimeEd { get; set; }

        /// <summary>
        /// 登陆账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 日志描述 登陆成功 0，登陆失败 1 登出2
        /// </summary>
        public LoginTypeEum? Description { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }


        /// <summary>
        /// 状态 在线0 离线1
        /// </summary>
        public LoginStatusEum? Status { get; set; }


    }
}
