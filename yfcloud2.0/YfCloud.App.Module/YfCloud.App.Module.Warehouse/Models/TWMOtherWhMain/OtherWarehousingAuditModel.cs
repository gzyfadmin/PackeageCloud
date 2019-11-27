using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YfCloud.Models;

namespace YfCloud.App.Module.Warehouse.Models.TWMOtherWhMain
{
    /// <summary>
    /// 其他入库审核模型
    /// </summary>
    public class OtherWarehousingAuditModel 
    {
        /// <summary>
        /// 入库单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过 3撤销审核
        /// </summary>
        public byte AuditStatus { get;set; }
    }
}
