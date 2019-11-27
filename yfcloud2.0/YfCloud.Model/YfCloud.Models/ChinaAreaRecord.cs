using System;
using System.Collections.Generic;
using System.Text;

namespace YfCloud.Models
{
    /// <summary>
    /// 地区
    /// </summary>
   public class ChinaAreaRecord
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Level { get; set; }
        
        /// <summary>
        /// 上级ID
        /// </summary>
        public virtual int ParentArea_id { get; set; }

    }

    public class ChinaAreaChildRecord
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public virtual int ParentArea_id { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public virtual List<ChinaAreaChildRecord> Children { get; set; }

    }
}
