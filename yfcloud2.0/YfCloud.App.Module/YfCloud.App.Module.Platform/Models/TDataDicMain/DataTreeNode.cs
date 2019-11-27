using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.App.Module.Platform.Models.TDataDicMain
{
    /// <summary>
    /// 数据字典树
    /// </summary>
    public class DataTreeNode
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型 1表示DataMain 2表示DicDetailModel
        /// </summary> 
        public int Type { get; set; }

        /// <summary>
        /// 后代节点
        /// </summary>
        public List<DataTreeNode> Children { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// 上级ID，顶级为-1
        /// </summary>
        public int ParentID { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 创建人
        ///// </summary>
        //public int CreateId { get; set; }
    }
}
