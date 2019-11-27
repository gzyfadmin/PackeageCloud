using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace YfCloud.Framework.OrderGenerator
{
    /// <summary>
    /// 单号枚举
    /// </summary>
    public enum OrderEnum
    {
        /// <summary>
        /// 其他入库单号
        /// </summary>
        [Description("OWR")]
        OWR = 0,
        /// <summary>
        /// 其他出库单号
        /// </summary>
        [Description("OWS")]
        OWS = 1,
        /// <summary>
        /// 盘盈入库单号
        /// </summary>
        [Description("IR")]
        IR = 2,
        /// <summary>
        /// 盘亏出库单号
        /// </summary>
        [Description("IS")]
        IS = 3,
        /// <summary>
        /// 盘点单
        /// </summary>
        [Description("IC")]
        IC = 4,
        /// <summary>
        /// 销售出库单
        /// </summary>
        [Description("SOWR")]
        SOWR = 5,
        /// <summary>
        /// 销售单号
        /// </summary>
        [Description("SO")]
        SO = 6,
        /// <summary>
        /// 采购单号
        /// </summary>
        [Description("PO")]
        PO = 7,

        /// <summary>
        /// 采购入库单
        /// </summary>
        [Description("WO")]
        WO =8,

        /// <summary>
        /// 生产单号
        /// </summary>
        [Description("MO")]
        MO =9,

        /// <summary>
        /// 生产领料单
        /// </summary>
        [Description("PMR")]
        PMR =10,

        /// <summary>
        /// 生产入库
        /// </summary>
        [Description("PDR")]
        PDR=11,

        /// <summary>
        /// 生产采购
        /// </summary>
        [Description("PR")]
        PR =12,

        /// <summary>
        /// 领料申请
        /// </summary>
        [Description("MR")]
        MR = 13,

        /// <summary>
        /// 生产出库
        /// </summary>
        [Description("PDC")]
        PDC = 14,
    }
}
