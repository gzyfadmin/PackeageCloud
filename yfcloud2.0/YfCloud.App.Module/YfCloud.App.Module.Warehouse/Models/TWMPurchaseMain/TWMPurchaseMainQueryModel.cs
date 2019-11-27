///////////////////////////////////////////////////////////////////////////////////////
// File: TWMPurchaseMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Warehouse.Models.TWMPurchaseDetail;

namespace YfCloud.App.Module.Warehouse.Models.TWMPurchaseMain
{
    /// <summary>
    /// TWMPurchaseMain Query Model
    /// </summary>
    public class TWMPurchaseMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 入库类型 0 采购入库
        /// </summary>
        public int WarehousingType { get; set; }
        
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime WarehousingDate { get; set; }
        
        /// <summary>
        /// 入库单号
        /// </summary>
        public string WarehousingOrderNo { get; set; }
        
        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 制单人ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单人名称
        /// </summary>
        public string  OperatorName { get; set; }

        /// <summary>
        /// 收货员ID
        /// </summary>
        public int? ReceiptId { get; set; }


        /// <summary>
        /// 收货员名称
        /// </summary>
        public string ReceiptName { get; set; }

        /// <summary>
        /// 仓管员ID
        /// </summary>
        public int? WhAdminId { get; set; }

        /// <summary>
        /// 仓管员名称
        /// </summary>
        public string WhAdminName { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员名称
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }
        
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
        
        /// <summary>
        /// 源单据ID
        /// </summary>
        public int SourceId { get; set; }
        
        /// <summary>
        /// 物料数量，明细总数量
        /// </summary>
        public decimal Number { get; set; }
        
        /// <summary>
        /// 金额，明细总金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrder { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool IsShowEdit { get; set; }


        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TWMPurchaseDetailQueryModel> ChildList { get; set; }
    }
}
