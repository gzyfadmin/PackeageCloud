using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace YfCloud.Utilities
{
    /// <summary>
    /// 登陆日志
    /// </summary>
   public class LoginLog
    {
        public LoginLog()
        {
            _id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        /// <summary>
        /// mangoDB id
        /// </summary>
        public object _id { get; set; }

        /// <summary>
        /// 当前登陆的ID
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登陆账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 日志描述
        /// </summary>
        public LoginTypeEum Description { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public LoginStatusEum Status { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public double Seconds { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public string TimeSpan { get; set; }
    }

    /// <summary>
    /// 登陆状态 登陆成功 0，登陆失败 1 登出2
    /// </summary>
    public enum LoginTypeEum
    {
        /// <summary>
        /// 登陆成功
        /// </summary>
        LoginSuccess =0,

        /// <summary>
        /// 登陆失败
        /// </summary>
        LoginFail=1,

        /// <summary>
        /// 登出
        /// </summary>
        Logout=2
    }

    /// <summary>
    /// 登陆状态 在线0 离线1
    /// </summary>
    public enum LoginStatusEum
    {  
        /// <summary>
       /// 在线
       /// </summary>
        Logining=0,

        /// <summary>
        /// 离线
        /// </summary>
        LogOut=1,

    }
}
