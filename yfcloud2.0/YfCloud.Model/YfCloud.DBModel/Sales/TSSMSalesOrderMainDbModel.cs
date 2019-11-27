///////////////////////////////////////////////////////////////////////////////////////
// File: TSSMSalesOrderMain.cs
// Author: www.cloudyf.com
// Create Time:2019/8/22
// Copyright@2014-2019 www.cloudyf.com
///////////////////////////////////////////////////////////////////////////////////////

using System;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YfCloud.DBModel
{
    /// <summary>
    /// T_SSM_SalesOrderMain Model
    /// </summary>
    [SugarTable("T_SSM_SalesOrderMain")]
    public class TSSMSalesOrderMainDbModel
    {
        /// <summary>
        /// 主键自增长
        /// </summary>
        [SugarColumn(IsPrimaryKey = true , ColumnName = "ID" , IsIdentity = true)]
        [Required]
        public int ID { get; set; }
        
        /// <summary>
        /// 客户ID
        /// </summary>
        [SugarColumn(ColumnName = "CustomerId")]
        [Required]
        public int CustomerId { get; set; }
        
        /// <summary>
        /// 业务员ID
        /// </summary>
        [SugarColumn(ColumnName = "SalesmanId")]
        [Required]
        public int SalesmanId { get; set; }
        
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
        /// 收货地址
        /// </summary>
        [SugarColumn(ColumnName = "ReceiptAddress")]
        [StringLength(maximumLength:200)]
        public string ReceiptAddress { get; set; }
        
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
        /// 销售数量总计
        /// </summary>
        [SugarColumn(ColumnName = "SalesNum")]
        public decimal? SalesNum { get; set; } = 0;

        /// <summary>
        /// 销售金额总计
        /// </summary>
        [SugarColumn(ColumnName = "SalesAmount")]
        public decimal? SalesAmount { get; set; } = 0;
        
        /// <summary>
        /// 明细集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TSSMSalesOrderDetailDbModel> ChildList { get; set; }
        
        /// <summary>
        /// 转生产单状态
        /// </summary>
        [SugarColumn(ColumnName = "TransProdStatus")]
        public bool? TransProdStatus { get; set; } = false;

        /// <summary>
        /// 是否物料
        /// </summary>
        [SugarColumn(ColumnName = "IsMaterial")]
        [StringLength(maximumLength: 200)]
        public bool IsMaterial { get; set; } = false;
    }
}
