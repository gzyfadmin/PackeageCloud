using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnString { get; set; }
        /// <summary>
        /// 是否自动关闭连接
        /// </summary>
        public bool IsAutoCloseConnection { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 程序启动地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件上传存放地址
        /// </summary>
        public string FileUploadDirectory { get; set; }
    }
}
