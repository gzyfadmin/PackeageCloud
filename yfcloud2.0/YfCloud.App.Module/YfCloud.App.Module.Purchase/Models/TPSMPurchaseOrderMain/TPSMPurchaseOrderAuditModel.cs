///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderMainEditModel.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderMain
{
    /// <summary>
    /// T_PSM_PurchaseOrderMain Audit Model
    /// </summary>
    public class TPSMPurchaseOrderAuditModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2通过
        /// </summary>
        [Required]
        public byte AuditStatus { get; set; }

    }
}
