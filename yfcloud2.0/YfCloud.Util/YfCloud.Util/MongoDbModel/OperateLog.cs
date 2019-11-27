using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace YfCloud.Utilities
{
    public class OperateLog
    {
        public OperateLog()
        {
            _id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }

        /// <summary>
        /// mangoDB id
        /// </summary>
        public object _id { get; set; }

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
        /// 操作时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 操作类型 1 新增 2修改 3删除
        /// </summary>
        public OpetateEnum OperateType { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string OperateDeatail { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OpetateEnum
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}
