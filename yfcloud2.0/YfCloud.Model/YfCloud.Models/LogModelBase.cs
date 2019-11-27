using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 含有ID类的接口
    /// </summary>
   public  class LogModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID { get; set; }

        /// <summary>
        /// 日志名称
        /// </summary>
        public virtual string _LogName { get; set; }
    }
}
