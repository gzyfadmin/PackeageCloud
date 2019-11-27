using System;
using System.Collections.Generic;
using System.Text;
using YfCloud.Utilities.Model;

namespace YfCloud.Utilities
{
    /// <summary>
    /// 单个接收手机号的消息体
    /// </summary>
    public class SingleReceiverMessage 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="templateId"></param>
        /// <param name="templateParas"></param>
        public SingleReceiverMessage(string Mobile, string templateId, string[] templateParas)
        {
            this.Mobile = Mobile;
            this.templateId = templateId;
            this.templateParas = templateParas;
        }

        /// <summary>
        /// templateId
        /// </summary>
        public string templateId
        {
            get;
            set;
        }

        /// <summary>
        ///templateParas
        /// </summary>
        public string[] templateParas
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}
