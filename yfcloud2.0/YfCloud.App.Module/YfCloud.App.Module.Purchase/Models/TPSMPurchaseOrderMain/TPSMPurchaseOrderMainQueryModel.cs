///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderDetail;

namespace YfCloud.App.Module.Purchase.Models.TPSMPurchaseOrderMain
{
    /// <summary>
    /// TPSMPurchaseOrderMain Query Model
    /// </summary>
    public class TPSMPurchaseOrderMainQueryModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        public int ID { get; set; }

      

        /// <summary>
        /// 业务员ID
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 采购员姓名
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderTypeId { get; set; }

        /// <summary>
        /// 订单类型名称
        /// </summary>
        public string OrderTypeName { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        public int? SettlementTypeId { get; set; }

        /// <summary>
        /// 结算方式名称
        /// </summary>
        public string SettlementTypeName { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        public byte? AuditStatus { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核员姓名
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 制单员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 制单员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 转单状态 true可转 false不可转
        /// </summary>
        public bool? TransferStatus { get; set; }

        /// <summary>
        /// 采购数量，即明细数量总计
        /// </summary>
        public decimal PurchaseNum { get; set; }

        /// <summary>
        /// 采购金额，即明细金额总计
        /// </summary>
        public decimal? PurchaseAmount { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// 源单号ID（生产订单ID）
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// 源单号（生产订单号）
        /// </summary>
        public string SourceNo { get; set; }
        
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<TPSMPurchaseOrderDetailQueryModel> ChildList { get; set; }
    }
}
