using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YfCloud.Utilities.MongoDbModel
{
    /// <summary>
    /// 未处理事件
    /// </summary>
   public class ToDoMgModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ToDoMgModel()
        {
            _id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        /// <summary>
        /// mangoDB id
        /// </summary>
        public object _id { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        public string ToDoId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// 0 认证消息
        /// </summary>
        public MessageTypeMgEnum? MessageType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否读取
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 消息接收人
        /// </summary>
        public int To { get; set; }
    }

    public enum MessageTypeMgEnum
    {
        [Description("认证消息")]
        ToApplytoComplany
    }
}
