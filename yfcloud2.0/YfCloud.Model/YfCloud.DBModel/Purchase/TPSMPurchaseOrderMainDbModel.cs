///////////////////////////////////////////////////////////////////////////////////////
// File: TPSMPurchaseOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/26
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_PSM_PurchaseOrderMain Model
    /// </summary>
    [SugarTable("T_PSM_PurchaseOrderMain")]
    public class TPSMPurchaseOrderMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        
        /// <summary>
        /// 业务员ID
        /// </summary>
        [SugarColumn(ColumnName = "BuyerId")]
        [Required]
        public int BuyerId { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        [SugarColumn(ColumnName = "OrderNo")]
        [Required]
        [StringLength(maximumLength:20)]
        public string OrderNo { get; set; }
        
        /// <summary>
        /// 订单类型
        /// </summary>
        [SugarColumn(ColumnName = "OrderTypeId")]
        [Required]
        public int OrderTypeId { get; set; }
        
        /// <summary>
        /// 结算方式
        /// </summary>
        [SugarColumn(ColumnName = "SettlementTypeId")]
        public int? SettlementTypeId { get; set; }
        
        /// <summary>
        /// 币种
        /// </summary>
        [SugarColumn(ColumnName = "Currency")]
        [StringLength(maximumLength:10)]
        public string Currency { get; set; }
        
        /// <summary>
        /// 订单日期
        /// </summary>
        [SugarColumn(ColumnName = "OrderDate")]
        [Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 审核状态 0待审核 1未通过 2已通过
        /// </summary>
        [SugarColumn(ColumnName = "AuditStatus")]
        public byte? AuditStatus { get; set; } = 0;
        
        /// <summary>
        /// 审核人ID
        /// </summary>
        [SugarColumn(ColumnName = "AuditId")]
        public int? AuditId { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        [SugarColumn(ColumnName = "AuditTime")]
        public DateTime? AuditTime { get; set; }
        
        /// <summary>
        /// 制单员ID
        /// </summary>
        [SugarColumn(ColumnName = "OperatorId")]
        [Required]
        public int OperatorId { get; set; }
        
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [SugarColumn(ColumnName = "ContactName")]
        [StringLength(maximumLength:10)]
        public string ContactName { get; set; }
        
        /// <summary>
        /// 联系人电话
        /// </summary>
        [SugarColumn(ColumnName = "ContactNumber")]
        [StringLength(maximumLength:20)]
        public string ContactNumber { get; set; }
        
        /// <summary>
        /// 企业ID
        /// </summary>
        [SugarColumn(ColumnName = "CompanyId")]
        [Required]
        public int CompanyId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [SugarColumn(ColumnName = "DeleteFlag")]
        [Required]
        public bool DeleteFlag { get; set; } = false;

        /// <summary>
        /// 转单状态 true可转 false不可转
        /// </summary>
        [SugarColumn(ColumnName = "TransferStatus")]
        public bool? TransferStatus { get; set; } = false;
        
        /// <summary>
        /// 采购数量，即明细数量总计
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseNum")]
        [Required]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal PurchaseNum { get; set; }
        
        /// <summary>
        /// 采购金额，即明细金额总计
        /// </summary>
        [SugarColumn(ColumnName = "PurchaseAmount")]
        [RegularExpression(pattern: "^(-)?\\d{1,14}(\\.\\d{1,4})?$", ErrorMessage = "整数位须小于等于14位，小数位须小于等于4位")]
        public decimal? PurchaseAmount { get; set; }

        /// <summary>
        /// 源单号
        /// </summary>
        [SugarColumn(ColumnName = "SourceId")]
        public int? SourceId { get; set; }
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TPSMPurchaseOrderDetailDbModel> ChildList { get; set; }
    }
}
