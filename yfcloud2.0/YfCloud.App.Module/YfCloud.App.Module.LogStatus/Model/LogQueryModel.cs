using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;
using YfCloud.Utilities;

namespace YfCloud.App.Module.LogStatus.Model
{
    /// <summary>
    /// 日志查询类
    /// </summary>
    public class LogQueryModel
    {
        /// <summary>
        /// 菜单
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 操作时间开始
        /// </summary>
        public DateTime? CreateTimeBg { get; set; }

        /// <summary>
        /// 操作时间结束
        /// </summary>
        public DateTime? CreateTimeEd { get; set; }

        /// <summary>
        /// 操作类型 1 新增 2修改 3删除
        /// </summary>
        public OpetateEnum? OperateType { get; set; }

    }
}
