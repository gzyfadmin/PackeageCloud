using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YfCloud.Models.CacheModels
{
   public class ToDoModel
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 0 认证消息
        /// </summary>
        public MessageTypeEnum? MessageType { get; set; }

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
    }

    public enum MessageTypeEnum
    {
        [Description("认证消息")]
        ToApplytoComplany
    }
}
